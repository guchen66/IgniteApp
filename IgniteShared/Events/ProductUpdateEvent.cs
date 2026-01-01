using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Events
{
    public class ProductUpdateEvent
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public string Remark { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}