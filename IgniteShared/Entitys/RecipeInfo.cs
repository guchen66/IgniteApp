using IgniteShared.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteShared.Entitys
{
    [Table("RecipeInfo")]
    public class RecipeInfo:EntityBase
    {
        public string RecipeName {  get; set; }
        public string ViewName {  get; set; }
        public bool IsEnabled { get; set; }
    }
}
