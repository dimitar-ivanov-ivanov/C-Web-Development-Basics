﻿namespace SimpleMvc.Framework.Attributes.Property
{
    using System.Text.RegularExpressions;

    public class RegexAttribute : PropertyAttribute
    {
        private readonly string pattern;

        public RegexAttribute(string pattern)
        {
            this.pattern = "^" + pattern + "$";
        }

        public override bool IsValid(object value) =>
            Regex.IsMatch(value.ToString(), pattern);
    }
}
