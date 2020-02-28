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
        public string ConnectionString { get; set; }
        public string Table { get; set; }
        public Group[] Groups { get; set; }
        public PropertyValue[] PropertyValues { get; set; }
    }

    public class Group
    {
        public Criteria[] Criterias { get; set; }
        public string Concatenator { get; set; }
    }

    public class Criteria
    {
        public string Field { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
        public string Concatenator { get; set; }
    }

    public class PropertyValue
    {
        public string Column { get; set; }
        public string Value { get; set; }
    }

}
