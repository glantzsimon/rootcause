﻿using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Models;
using K9.Base.Globalisation;
using K9.DataAccessLayer.Enums;
using K9.SharedLibrary.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K9.DataAccessLayer.Models
{
    [AutoGenerateName]
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.EmailQueueItems, PluralName = Globalisation.Strings.Names.EmailQueueItems, Name = Globalisation.Strings.Names.EmailQueueItem)]
    public class EmailQueueItem : ObjectBase
    {
        [Required]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.EmailTypeLabel)]
        public EEmailType Type { get; set; }

        [Required]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.EmailTemplateLabel)]
        [UIHint("EmailTemplate")]
        [ForeignKey("EmailTemplate")]
        public int EmailTemplateId { get; set; }

        public virtual EmailTemplate EmailTemplate { get; set; }

        [Display(ResourceType = typeof(K9.Globalisation.Dictionary), Name = K9.Globalisation.Strings.Labels.EmailTemplateLabel)]
        [LinkedColumn(LinkedTableName = "EmailTemplate", LinkedColumnName = "Name")]
        public string EmailTemplateName { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ContactLabel)]
        [UIHint("Contact")]
        [ForeignKey("Contact")]
        public int? ContactId { get; set; }

        public virtual Client Client { get; set; }

        [Display(ResourceType = typeof(K9.Globalisation.Dictionary), Name = K9.Globalisation.Strings.Names.Contact)]
        [LinkedColumn(LinkedTableName = "Contact", LinkedColumnName = "FullName")]
        public string ContactName { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.User)]
        [UIHint("User")]
        [ForeignKey("User")]
        public int? UserId { get; set; }
        
        public virtual User User { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.UserNameLabel)]
        [LinkedColumn(LinkedTableName = "User", LinkedColumnName = "FullName")]
        public string UserName { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.EmailAddressLabel)]
        public string RecipientEmailAddress => User != null ? User.EmailAddress :
            Client != null ? Client.EmailAddress : string.Empty;

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.NameLabel)]
        [StringLength(128)]
        public string RecipientName => User != null ? User.FullName:
            Client != null ? Client.FullName : string.Empty;

        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SubjectLabel)]
        public string Subject { get; set; }

        [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.BodyLabel)]
        public string Body { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ScheduledOnLabel)]
        public DateTime? ScheduledOn { get; set; }

        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SentOnLabel)]
        public DateTime? SentOn { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.IsProcessedLabel)]
        public bool IsProcessed { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ResultLabel)]
        public string Result { get; set; }

        public bool IsSent => SentOn.HasValue;
    }
}
