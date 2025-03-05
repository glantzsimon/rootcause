﻿using K9.Base.DataAccessLayer.Models;
using K9.DataAccessLayer.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace K9.WebApplication.ViewModels
{
    public class MyAccountViewModel
    {
        public User User { get; set; }

        public Client Client { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary),
            Name = Globalisation.Strings.Names.AllowMarketingEmails)]
        public bool AllowMarketingEmails { get; set; }

        public UserMembership Membership { get; set; }

        public List<UserConsultation> Consultations { get; set; }

        public List<Protocol> Protocols { get; set; }
        
        public List<Protocol> SuggestedProtocols { get; set; }
        
        public HealthQuestionnaire HealthQuestionnaire { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Names.RedeemPromoCode)]
        public string PromoCode { get; set; }
    }
}