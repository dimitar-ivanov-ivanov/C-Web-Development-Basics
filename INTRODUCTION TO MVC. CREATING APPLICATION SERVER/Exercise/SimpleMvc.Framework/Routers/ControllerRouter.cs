namespace SimpleMvc.Framework.Routers
{
    using SimpleMvc.Common;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Controllers;
    using SimpleMvc.Framework.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using WebServer.Contracts;
    using WebServer.Enums;
    using WebServer.Http.Contracts;
    using WebServer.Http.Response;

    public class ControllerRouter : IHandleable
    {
        private IDictionary<string, string> getParams;
        private IDictionary<string, string> postParams;
        private string requestMethod;
        private string controllerName;
        private string actionName;
        private object[] methodParams;

        public IHttpResponse Handle(IHttpRequest request)
        {
            this.GetRequestData(request);

            this.GetControllerAndActionNames(request);

            MethodInfo method = this.GetMethod();

            if (method == null)
            {
                return new NotFoundResponse();
            }

            this.MapMethodParameters(method);
            return this.PrepareResponse(method);
        }

        private void GetRequestData(IHttpRequest request)
        {
            this.getParams = request.UrlParameters;
            this.postParams = request.FormData;
            this.requestMethod = request.Method.ToString();
        }

        private IHttpResponse PrepareResponse(MethodInfo method)
        {
            var actionResult = (IInvocable)method.Invoke(this.GetController(), this.methodParams);

            var content = actionResult.Invoke();

            var response = new ContentResponse(HttpStatusCode.Ok, content);

            return response;
        }

        private void GetControllerAndActionNames(IHttpRequest request)
        {
            var invocationParameters = request.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (invocationParameters.Length != 2)
            {
                throw new InvalidOperationException("Invalid URL.");
            }

            this.controllerName = invocationParameters[0].CapitalizeFirstLetter() +
                MvcContext.Get.ControllerSuffix;
            this.actionName = invocationParameters[1].CapitalizeFirstLetter();
        }

        private void MapMethodParameters(MethodInfo method)
        {
            var parameterDescriptions = method.GetParameters();
            this.methodParams = new object[parameterDescriptions.Length];

            for (int i = 0; i < parameterDescriptions.Length; i++)
            {
                var param = parameterDescriptions[i];

                if (param.ParameterType.IsPrimitive ||
                   param.ParameterType == typeof(string))
                {
                    //Get requet primitive types
                    object value = this.getParams[param.Name];
                    this.methodParams[i] = Convert.ChangeType(
                        value,
                        param.ParameterType);
                }
                else
                {
                    //Get request non primitive
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

                    this.methodParams[i] = Convert.ChangeType(
                        modelInstance,
                        modelType);
                }
            }
        }

        private MethodInfo GetMethod()
        {
            MethodInfo method = null;

            foreach (var methodInfo in this.GetSuitableMethods())
            {
                var attributes = methodInfo
                    .GetCustomAttributes()
                    .Where(m => m is HttpMethodAttribute)
                    .Cast<HttpMethodAttribute>();

                if (!attributes.Any() && this.requestMethod == "GET")
                {
                    return methodInfo;
                }

                foreach (var attribute in attributes)
                {
                    if (attribute.IsValid(this.requestMethod))
                    {
                        return methodInfo;
                    }
                }
            }

            return method;
        }

        private ICollection<MethodInfo> GetSuitableMethods()
        {
            var controller = this.GetController();

            if (controller == null)
            {
                return new MethodInfo[0];
            }

            return
                controller
                .GetType()
                .GetMethods()
                .Where(m => m.Name == actionName)
                .ToList();
        }

        private Controller GetController()
        {
            var controllerTypeName = string.Format(
                "{0}.{1}.{2}, {0}",
                MvcContext.Get.AssemblyName,
                MvcContext.Get.ControllersFolders,
                this.controllerName);

            Type type = Type.GetType(controllerTypeName);

            if (type == null)
            {
                return null;
            }

            var controller = (Controller)Activator.CreateInstance(type);

            return controller;
        }
    }
}