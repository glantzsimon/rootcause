using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Models;
using K9.Base.Globalisation;
using K9.DataAccessLayer.Enums;
using K9.SharedLibrary.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(K9.Globalisation.Dictionary), ListName = Globalisation.Strings.Names.Clients, PluralName = Globalisation.Strings.Names.Clients, Name = Globalisation.Strings.Names.Client)]
    public class Client : ObjectBase
    {
        [UIHint("PriceTier")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.PriceTierLabel)]
        public EPriceTier PriceTier { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ClientDiscount)]
        [DataType(DataType.Currency)]
        public double ClientDiscount { get; set; }

        [UIHint("Client")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ClientLabel)]
        public int ClientId => Id;

        [UIHint("User")]
        [ForeignKey("User")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.User)]
        public int? UserId { get; set; }

        public virtual User User { get; set; }

        [LinkedColumn(LinkedTableName = "User", LinkedColumnName = "Username")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = K9.Globalisation.Strings.Labels.StripeCustomerIdLabel)]
        public string StripeCustomerId { get; set; }

        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.NameLabel)]
        [StringLength(128)]
        public string FullName { get; set; }

        [Index(IsUnique = true)]
        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.InvalidEmailAddress)]
        [EmailAddress(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.InvalidEmailAddress)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.EmailAddressLabel)]
        [StringLength(255)]
        public string EmailAddress { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.PhoneNumberLabel)]
        [StringLength(255)]
        public string PhoneNumber { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.AddressLabel)]
        [StringLength(1024)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        public string GetAddressHtmlString() => string.IsNullOrEmpty(Address)
            ? string.Empty
            : string.Join(Environment.NewLine, Regex.Split(Address, Environment.NewLine).Select(e => e.Trim())
                .Where(e => !string.IsNullOrEmpty(e)).Select(e => $"<p>{e}</p>"));

        [UIHint("Country")]
        [ForeignKey("Country")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.CountryLabel)]
        public int? CountryId { get; set; }

        public virtual Country Country { get; set; }

        [LinkedColumn(LinkedTableName = "Country", LinkedColumnName = "Name")]
        public string CountryName { get; set; }

        [Display(ResourceType = typeof(K9.Globalisation.Dictionary), Name = K9.Globalisation.Strings.Labels.CompanyLabel)]
        [StringLength(255)]
        public string CompanyName { get; set; }

        [UIHint("Percentage")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ShopCommissionLabel)]
        public double? ShopCommission { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.NotesLabel)]
        [StringLength(243)]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string Notes { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.IsUnsubscribedLabel)]
        public bool IsUnsubscribed { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Labels.AreMarketingEmailsAllowedLabel)]
        public bool AreMarketingEmailsAllowed => !IsUnsubscribed;

        private string _firstName;
        public string FirstName
        {
            get
            {
                if (string.IsNullOrEmpty(_firstName))
                {
                    _firstName = GetFirstName();
                }
                return _firstName;
            }
        }

        private string GetFirstName() => FullName.Split(' ').FirstOrDefault();
    }
}
