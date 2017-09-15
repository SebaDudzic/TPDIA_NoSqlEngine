using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlEngineConsoleApp
{
    static class Utilities
    {
        public static int ParseToInt(string toParse)
        {
            try
            {
                return int.Parse(toParse);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public static float ParseToFloat(string toParse)
        {
            try
            {
                return float.Parse(toParse);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
