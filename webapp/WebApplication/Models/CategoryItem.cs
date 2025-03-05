using System.ComponentModel.DataAnnotations;

namespace K9.WebApplication.Models
{
    public class CategoryItem
    {
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CategoryLabel)]
        public string Name { get; set; }
    }
}