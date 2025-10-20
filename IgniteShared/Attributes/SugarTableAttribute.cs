using IT.Tangdao.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Attributes
{
    public class SugarTableAttribute : Attribute
    {
        public string TableName { get; set; }

        public SugarTableAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}