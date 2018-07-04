namespace Validate_URL
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Startup
    {
        public static void Main(string[] args)
        {
            Execute();
        }

        private static void Execute()
        {
            var regex = new Regex(@"((?<protocol>http|https):\/(?<host>\/[0-9a-zA-Z\-\.]+)(?<port>\:\d+)?)(?<path>\/[0-9a-zA-Z\-\.\/]+)?(?<query>\?[0-9a-zA-Z\-\.\=\+\&_]+)*(?<fragments>\#[0-9a-zA-Z\-\.]+)?");
            var groupsToCheck = new string[] { "protocol", "host", "port", "path", "query", "fragments" };

            var url = Console.ReadLine();

            if (!regex.IsMatch(url))
            {
                Console.WriteLine("Invalid URL");
                return;
            }

            var groups = regex.Match(url).Groups;
            var output = new StringBuilder();
            var protocol = string.Empty;

            for (int i = 0; i < groupsToCheck.Length; i++)
            {
                var group = groups[groupsToCheck[i]];
                if (group.Success)
                {
                    var text = group.Value;

                    if (groupsToCheck[i] == "protocol")
                    {
                        protocol = text;
                    }

                    if (groupsToCheck[i] != "path" && groupsToCheck[i] != "protocol")
                    {
                        text = new string(text.ToCharArray().Skip(1).ToArray());
                    }

                    if (groupsToCheck[i] == "port")
                    {
                        if ((protocol == "https" && !text.StartsWith("4")) ||
                            (protocol == "http" && !text.StartsWith("8")))
                        {
                            Console.WriteLine("Invalid URL");
                            return;
                        }
                    }

                    var title = groupsToCheck[i];
                    var upper = title.ToCharArray();
                    upper[0] = upper[0].ToString().ToUpper()[0];
                    title = new string(upper);

                    output.AppendLine($"{title}: {text}");
                }
                else if (groupsToCheck[i] == "port")
                {
                    if (protocol == "https")
                    {
                        output.AppendLine("Port: 443");
                    }
                    else if (protocol == "http")
                    {
                        output.AppendLine("Port: 80");
                    }
                }
                else if (groupsToCheck[i] == "path")
                {
                    output.AppendLine("Path: /");
                }
            }

            Console.Write(output);
        }
    }
}
