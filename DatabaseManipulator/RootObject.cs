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
        public CriteriaOperator[] CriteriaOperators { get; set; }
        public Column[] Columns { get; set; }
    }

    public class Entitymessage
    {
        public string Table { get; set; }
        public string Column { get; set; }
        public string Value { get; set; }
    }

    public class CriteriaOperator
    {
        public string Operator { get; set; }
    }

    public class Column
    {
        public dynamic Values { get; set; }
    }
}
