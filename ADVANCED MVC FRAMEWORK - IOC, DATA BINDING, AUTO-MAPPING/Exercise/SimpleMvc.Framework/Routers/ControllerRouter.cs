namespace SimpleMvc.Framework.Routers
{
    using SimpleMvc.Common;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Controllers;
    using SimpleMvc.Framework.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using WebServer.Contracts;
    using WebServer.Enums;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    public class ControllerRouter : IHandleable
    {
        private IEnumerable<Type> controllersType;

        public IHttpResponse Handle(IHttpRequest request)
        {
            //FindControllersType();

            var getParams = request.UrlParameters;
            var postParams = request.FormData;

            var requestMethod = request.Method.ToString();

            var invocationParameters = request.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (invocationParameters.Length != 2)
            {
                throw new InvalidOperationException("Invalid URL.");
            }

            var controllerName = invocationParameters[0].CapitalizeFirstLetter() + MvcContext.Get.ControllerSuffix;
            var actionName = invocationParameters[1].CapitalizeFirstLetter();

            var controller = this.GetController(controllerName, request);

            MethodInfo method = this.GetMethod(controller, actionName, requestMethod);

            if (method == null)
            {
                return new NotFoundResponse();
            }

            var parameters = MapMethodParameters(method, getParams, postParams);

            try
            {
                return this.PrepareResponse(method, controller, parameters);
            }
            catch (Exception e)
            {
                return new InternalServerErrorResponse(e);
            }
        }

        private void FindControllersType()
        {
            var assemblyPath = Directory.GetCurrentDirectory() + "\\" + MvcContext.Get.AssemblyName + MvcContext.Get.AssemblyNameFileSuffix;
            var assembly = Assembly.LoadFile(assemblyPath);

            controllersType = assembly
              .GetTypes()
              .Where(t => !t.IsAbstract &&
              (typeof(Controller)).IsAssignableFrom(t) &&
              t.Name.EndsWith(MvcContext.Get.ControllerSuffix))
              .ToList();
        }

        private IHttpResponse PrepareResponse(MethodInfo method, Controller controller, object[] parameters)
        {
            var actionResult = method.Invoke(controller, parameters);

            IHttpResponse response = null;

            if (actionResult is IViewable)
            {
                var content = ((IViewable)actionResult).Invoke();

                response = new ContentResponse(HttpStatusCode.Ok, content);
            }
            else if (actionResult is IRedirectable)
            {
                var redirectUrl = ((IRedirectable)actionResult).Invoke();

                response = new RedirectResponse(redirectUrl);
            }
            else
            {
                throw new InvalidOperationException("The view result is not supported.");
            }

            return response;
        }

        private object[] MapMethodParameters(MethodInfo method, IDictionary<string, string> getParams, IDictionary<string, string> postParams)
        {
            var parameterDescriptions = method.GetParameters();
            var parameters = new object[parameterDescriptions.Length];

            for (int i = 0; i < parameterDescriptions.Length; i++)
            {
                var param = parameterDescriptions[i];

                if (param.ParameterType.IsPrimitive ||
                   param.ParameterType == typeof(string))
                {
                    //Get requet primitive types
                    parameters[i] = GetPrimitiveParameters(getParams, param);
                }
                else
                {
                    //Get request non primitive
                    parameters[i] = GetComplexParameters(postParams, param);
                }
            }
            //Cannot load assembly 'D:\Vsichki  Programi\Softuni\C# Web\ADVANCED MVC FRAMEWORK - IOC, DATA BINDING, AUTO-MAPPING\Exercise\SimpleMvc.App\bin\Debug\netcoreapp2.0\Notes.dll'.
            return parameters;
        }

        private static object GetComplexParameters(IDictionary<string, string> postParams, ParameterInfo param)
        {
            var modelType = param.ParameterType;
            var modelInstance = Activator.CreateInstance(modelType);

            var modelProperties = modelType.GetProperties();

            foreach (var property in modelProperties)
            {
                var value = postParams[property.Name];

                property.SetValue(
                    modelInstance,
                    Convert.ChangeType(
                        value, property.PropertyType));
            }

            return Convert.ChangeType(
                modelInstance,
                modelType);
        }

        private static object GetPrimitiveParameters(IDictionary<string, string> getParams, ParameterInfo param)
        {
            object value = getParams[param.Name];
            return Convert.ChangeType(
                value,
                param.ParameterType);
        }

        private MethodInfo GetMethod(Controller controller, string actionName, string requestMethod)
        {
            MethodInfo method = null;

            foreach (var methodInfo in this.GetSuitableMethods(controller, actionName))
            {
                var attributes = methodInfo
                    .GetCustomAttributes()
                    .Where(m => m is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();

                if (!attributes.Any() && requestMethod.ToUpper() == "GET")
                {
                    return methodInfo;
                }

                foreach (var attribute in attributes)
                {
                    if (attribute.IsValid(requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return method;
        }

        private ICollection<MethodInfo> GetSuitableMethods(Controller controller, string actionName)
        {
            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return
                controller
                .GetType()
                .GetMethods()
                .Where(m => m.Name.ToLower() == actionName.ToLower())
                .ToList();
        }

        private Controller GetController(string controllerName, IHttpRequest request)
        {
            var controllerTypeName = string.Format(
                "{0}.{1}.{2}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ControllersFolders,
                controllerName);

            Type type = Type.GetType(controllerTypeName);

            //type = controllersType
                //.FirstOrDefault(ct => ct.Name == controllerName);

            if (type == null)
            {
                return null;
            }

            var controller = (Controller)Activator.CreateInstance(type);

            if (controller != null)
            {
                controller.Request = request;
                controller.InitializeUser();
            }

            return controller;
        }
    }
}