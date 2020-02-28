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
        public Criterion[] Criteria { get; set; }
        public Criteriaoperator[] CriteriaOperators { get; set; }
        public Propertyvalue[] PropertyValues { get; set; }
    }

    public class Criterion
    {

    }

    public class Criteriaoperator
    {

    }

    public class Propertyvalue
    {

    }


}
