namespace Request_Parser
{
    using System;
    using System.Collections.Generic;

    public class Startup
    {
        public static void Main(string[] args)
        {
            Execute();
        }

        private static void Execute()
        {
            var methodsAndPaths = new Dictionary<string, HashSet<string>>();
            methodsAndPaths.Add("GET", new HashSet<string>());
            methodsAndPaths.Add("POST", new HashSet<string>());

            GetInput(methodsAndPaths);
            PerformRequest(methodsAndPaths);
        }

        private static void PerformRequest(Dictionary<string, HashSet<string>> methodsAndPaths)
        {
            var args = Console.ReadLine().Split(new[] { '/', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var method = args[0];
            var path = args[1];
            var statusCode = 0;
            var statusMessage = string.Empty;

            if (!methodsAndPaths.ContainsKey(method) ||
                !methodsAndPaths[method].Contains(path))
            {
                statusCode = 404;
                statusMessage = "Not Found";
            }
            else
            {
                statusCode = 200;
                statusMessage = "OK";
            }

            Console.WriteLine($"HTTP/1.1 {statusCode} {statusMessage}");
            Console.WriteLine($"Content-Length: {statusMessage.Length}");
            Console.WriteLine("Content-Type: text/plain\n");
            Console.WriteLine(statusMessage);
        }

        private static void GetInput(Dictionary<string, HashSet<string>> methodsAndPaths)
        {
            var input = Console.ReadLine();

            while (input != "END")
            {
                var args = input.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var method = args[1];
                var path = args[0];
                methodsAndPaths[method.ToUpper()].Add(path);

                input = Console.ReadLine();
            }
        }
    }
}
