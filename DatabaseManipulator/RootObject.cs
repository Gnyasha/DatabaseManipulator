using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseManipulator
{
    public class RootObject
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public Entitymessage[] EntityMessage { get; set; }
    }

    public class Entitymessage
    {
        public string Database { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public string Value { get; set; }
    }

}
