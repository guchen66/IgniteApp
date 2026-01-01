using System;

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