using System;
using System.Collections.Generic;
using System.Text;

namespace SyntacticAnalysis
{
    public static class Extensions
    {
        public static int AsInteger(this double value)
        {
            return Convert.ToInt32(value);
        }

        public static long AsLong(this string value, long defaultVal = 0)
        {
            long integer = 0;

            if (Int64.TryParse(value, out integer))
                return integer;

            return defaultVal;
        }

        public static int AsInteger(this string value, int defaultVal = 0)
        {
            int integer = 0;

            if (Int32.TryParse(value, out integer))
                return integer;

            return defaultVal;
        }

        public static double AsDouble(this string value)
        {
            double doubleValue = 0;
            Double.TryParse(value, out doubleValue);
            return doubleValue;
        }

        public static bool AsBool(this string value)
        {
            bool boolean;
            Boolean.TryParse(value, out boolean);
            return boolean;
        }
    }
}
