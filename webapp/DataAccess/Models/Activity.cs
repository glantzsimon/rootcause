﻿using K9.Base.DataAccessLayer.Attributes;
using K9.Base.Globalisation;
using K9.SharedLibrary.Attributes;
using K9.SharedLibrary.Enums;
using K9.SharedLibrary.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace K9.DataAccessLayer.Models
{
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.Activities, PluralName = Globalisation.Strings.Names.Activities, Name = Globalisation.Strings.Names.Activity)]
    public class Activity : GenoTypeBase
    {
        public Guid ExternalId { get; set; }

        [NotMapped]
        [UIHint("Activity")]
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ActivityLabel)]
        public int ActivityId => Id;
        
        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.SummaryLabel)]
        [StringLength(int.MaxValue)]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string ShortDescription { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.DetailsLabel)]
        [StringLength(int.MaxValue)]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string Body { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.BenefitsLabel)]
        [StringLength(int.MaxValue)]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string Benefits { get; set; }

        [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.RecommendationsLabel)]
        [StringLength(int.MaxValue)]
        [DataType(DataType.Html)]
        [AllowHtml]
        public string Recommendations { get; set; }

        [FileSourceInfo("upload/activities", Filter = EFilesSourceFilter.Images)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UploadImages)]
        public FileSource ImageFileSource { get; set; }

        [FileSourceInfo("upload/activities", Filter = EFilesSourceFilter.Videos)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UploadVideo)]
        public FileSource VideoFileSource { get; set; }

        [StringLength(512)]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ImageUrlLabel)]
        public string ImageUrl { get; set; }
    }
}
