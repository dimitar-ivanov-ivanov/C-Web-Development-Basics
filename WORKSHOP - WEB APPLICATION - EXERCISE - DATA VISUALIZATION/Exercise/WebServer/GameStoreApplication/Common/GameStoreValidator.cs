namespace HTTPServer.GameStoreApplication.Common
{
    public class GameStoreValidator
    {
        public static bool IsNull(object text)
        {
            if (text == null)
            {
                return true;
            }

            return false;
        }

        public static bool IsNullOrEmpty(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }

            return false;
        }

        public static bool AreEqual(object obj1, object obj2)
        {
            if (obj1.Equals(obj2))
            {
                return true;
            }

            return false;
        }

        public static bool IsEqualOrLongerThan(string text, int length)
        {
            return text.Length >= length;
        }

        public static bool IsEqualOrBiggerThan(decimal num, decimal val)
        {
            return num >= val;
        }

        public static bool IsEqualOrLesserThan(decimal num, decimal val)
        {
            return num <= val;
        }

        public static bool IsPositive(decimal num)
        {
            return num > 0;
        }

        public static bool CheckStringPrecision(decimal num,int targetPrecision)
        {
            var text = num.ToString();

            var index = text.IndexOf('.');

            if(index == -1)
            {
                return false;
            }

            var numberPrecision = text.Length - index - 1;

            return numberPrecision == targetPrecision;
        }

        public static bool StringLengthIsEqualTo(string text,int length)
        {
            return text.Length == length;
        }

        public static bool StringStartsWithAny(string text,params string[] startTexts)
        {
            foreach (var starter in startTexts)
            {
                if (text.StartsWith(starter))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
