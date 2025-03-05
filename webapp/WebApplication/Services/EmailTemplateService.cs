﻿using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Enums;
using K9.DataAccessLayer.Models;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Helpers;
using K9.SharedLibrary.Models;
using K9.WebApplication.Packages;
using System;
using System.Linq;

namespace K9.WebApplication.Services
{
    public class EmailTemplateService : BaseService, IEmailTemplateService
    {
        private readonly IRepository<EmailTemplate> _emailTemplatesRepository;

        public EmailTemplateService(IRepository<EmailTemplate> emailTemplatesRepository, IServiceBasePackage my)
        : base(my)
        {
            _emailTemplatesRepository = emailTemplatesRepository;
        }

        public EmailTemplate Find(int id)
        {
            return _emailTemplatesRepository.Find(id);
        }

        public EmailTemplate FindSystemTemplate(ESystemEmailTemplate systemEmailTemplate)
        {
            return _emailTemplatesRepository.Find(e => e.SystemEmailTemplate == systemEmailTemplate).FirstOrDefault();
        }

        public string ParseForUser(int emailTemplateId, User user, object data)
        {
            if (user == null)
            {
                throw new Exception("User is null");
            }

            return Parse(emailTemplateId, user.FirstName, My.UrlHelper.AbsoluteAction("UnsubscribeUser", "Account", new { externalId = user.Name }), data);
        }

        public string ParseForUser(EmailTemplate emailTemplate, User user, object data)
        {
            if (user == null)
            {
                throw new Exception("User is null");
            }

            return Parse(emailTemplate, user.FirstName, My.UrlHelper.AbsoluteAction("UnsubscribeUser", "Account", new { externalId = user.Name }), data);
        }

        public string ParseForContact(int emailTemplateId, Client client, object data)
        {
            if (client == null)
            {
                throw new Exception("Contact is null");
            }

            return Parse(emailTemplateId, client.FirstName, My.UrlHelper.AbsoluteAction("UnsubscribeContact", "Account", new { externalId = client.Name }), data);
        }

        public string ParseForContact(EmailTemplate emailTemplate, Client client, object data)
        {
            if (client == null)
            {
                throw new Exception("Contact is null");
            }

            return Parse(emailTemplate, client.FirstName, My.UrlHelper.AbsoluteAction("UnsubscribeContact", "Account", new { externalId = client.Name }), data);
        }

        public string ParseForUser(string title, string body, User user, object data)
        {
            if (user == null)
            {
                throw new Exception("User is null");
            }

            return Parse(title, body, user.FirstName, My.UrlHelper.AbsoluteAction("UnsubscribeUser", "Account", new { externalId = user.Name }), data);
        }

        public string ParseForContact(string title, string body, Client client, object data)
        {
            if (client == null)
            {
                throw new Exception("Contact is null");
            }

            return Parse(title, body, client.FirstName, My.UrlHelper.AbsoluteAction("UnsubscribeContact", "Account", new { externalId = client.Name }), data);
        }

        public string Parse(int emailTemplateId, string recipientFirstName, string unsubscribeLink, object data)
        {
            var template = Find(emailTemplateId);
            if (template == null)
            {
                throw new Exception($"Email Template {emailTemplateId} was not found");
            }

            return Parse(template, recipientFirstName, unsubscribeLink, data);
        }
        
        private string Parse(EmailTemplate emailTemplate, string recipientFirstName, string unsubscribeLink, object data)
        {


            return Parse(emailTemplate.Subject, emailTemplate.HtmlBody, recipientFirstName, unsubscribeLink, data);
        }

        private string Parse(string title, string body, string recipientFirstName, string unsubscribeLink, object data)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new Exception("Title cannot be empty");
            }

            if (string.IsNullOrEmpty(body))
            {
                throw new Exception("Body cannot be empty");
            }

            if (string.IsNullOrEmpty(unsubscribeLink))
            {
                throw new Exception("Unsubscribe link cannot be empty");
            }

            if (data != null)
                body = TemplateParser.Parse(body, data);

            body = TemplateParser.Parse(body, new
            {
                FirstName = recipientFirstName
            });

            return TemplateParser.Parse(Globalisation.Dictionary.BaseEmailTemplate, new
            {
                FirstName = recipientFirstName,
                Title = title,
                Body = body,
                PrivacyPolicyLink = My.UrlHelper.AbsoluteAction("PrivacyPolicy",
                "Home"),
                TermsOfServiceLink = My.UrlHelper.AbsoluteAction("TermsOfService", "Home"),
                UnsubscribeLink = unsubscribeLink,
                DateTime.Now.Year,
                My.WebsiteConfiguration.CompanyName,
                My.DefaultValuesConfiguration.CompanyAddress,
                My.DefaultValuesConfiguration.SiteBaseUrl,
                HeaderImageSrc = $"{My.DefaultValuesConfiguration.BaseEmailTemplateImagesPath}/emailtemplates/email-template-header.jpg",
                CompanyLogoSrc = $"{My.DefaultValuesConfiguration.BaseEmailTemplateImagesPath}/company/logo-small.png",
            });
        }
    }
}
