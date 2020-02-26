using Newtonsoft.Json;
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
        public string ConnectionString { get; set; }
        public string Table { get; set; }
        public string Criteria { get; set; }
        public string CriteriaValue { get; set; }
        public string CriteriaOperators { get; set; }
        public string[] Properties { get; set; }
        public string[] Values { get; set; }
    }




}
