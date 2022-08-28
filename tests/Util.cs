using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tests
{
    public static class Util
    {
        public static string GetTempPath()
        {
            return Path.Combine(Path.GetTempPath(), "werkbank_tests", Guid.NewGuid().ToString());
        }
    }
}
