using System.ComponentModel.DataAnnotations;

namespace K9.WebApplication.ViewModels
{
    public class ImpersonateViewModel
    {
        [UIHint("User")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.User)]
        public int UserId { get; set; }
        
        public string UserName { get; set; }
    }
}