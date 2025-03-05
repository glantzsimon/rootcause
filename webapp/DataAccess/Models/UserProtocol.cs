using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Models;
using K9.SharedLibrary.Attributes;
using K9.SharedLibrary.Authentication;
using K9.SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using K9.Base.Globalisation;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.UserProtocol, PluralName = Globalisation.Strings.Names.UserProtocols, ListName = Globalisation.Strings.Names.UserProtocols)]
    [DefaultPermissions(Role = RoleNames.DefaultUsers)]
    public class UserProtocol : ObjectBase, IUserData
    {
        [UIHint("User")]
        [Required]
        [ForeignKey("User")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.User)]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [LinkedColumn(LinkedTableName = "User", LinkedColumnName = "Username")]
        public string UserName { get; set; }

        [UIHint("Protocol")]
        [Required]
        [ForeignKey("Protocol")]
        public int ProtocolId { get; set; }

        public virtual Protocol Protocol { get; set; }

        [LinkedColumn(LinkedTableName = "Protocol", LinkedColumnName = "Name")]
        public string ProtocolName { get; set; }
    }
}
