using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetable
{
    static class Transformer
    {
        static public Dictionary<int, Teacher> Teachers { get; set; }

        static public Dictionary<int, string> Disciplines { get; set; }

        static public Dictionary<int, string> Time { get; set; }
        static public Dictionary<int, string> Groups { get; set; }
    }
}
