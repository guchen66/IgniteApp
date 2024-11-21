using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Interfaces
{
    public interface IMenuItem
    {
        int Id { get; set; }
        string MenuName { get; set; }
       
    }

    public class MenuChildItem : IMenuItem
    {
        public int Id { get; set; }

        public string MenuName { get; set; }
    }
}
