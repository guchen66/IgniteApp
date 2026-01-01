using System.ComponentModel.DataAnnotations.Schema;

namespace IgniteShared.Entitys
{
    [Table("RecipeInfo")]
    public class RecipeInfo : EntityBase
    {
        public string RecipeName { get; set; }
        public string ViewName { get; set; }
        public bool IsEnabled { get; set; }
    }
}
