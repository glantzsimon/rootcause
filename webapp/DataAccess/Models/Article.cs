using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Models;
using K9.Base.Globalisation;
using K9.SharedLibrary.Attributes;
using K9.SharedLibrary.Enums;
using K9.SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace K9.DataAccessLayer.Models
{
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.Articles, PluralName = Globalisation.Strings.Names.Articles, Name = Globalisation.Strings.Names.Article)]
    public class Article : ObjectBase
	{
	    [Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
        [ForeignKey("User")]
	    public int UserId { get; set; }

	    public virtual User User { get; set; }

        [UIHint("ArticleCategory")]
	    [ForeignKey("ArticleCategory")]
        [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.CategoryLabel)]
	    public int ArticleCategoryId { get; set; }

	    public virtual ArticleCategory ArticleCategory { get; set; }

	    public virtual IEnumerable<ArticleSection> ArticleSections { get; set; }

	    [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.PublishedOnLabel)]
		[Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
		public DateTime PublishedOn { get; set; }

		[Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.PublishedByLabel)]
		public string PublishedBy { get; set; }
        
		[Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SubjectLabel)]
		[Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
		[StringLength(256)]
		public string Subject { get; set; }
		
		[Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.BodyLabel)]
		[Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
		[StringLength(int.MaxValue)]
		[DataType(DataType.Html)]
		[AllowHtml]
		public string Body { get; set; }

	    [FileSourceInfo("upload/articles", Filter = EFilesSourceFilter.Images)]
		[Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UploadImages)]
		public FileSource ImageFileSource { get; set; }

	    [FileSourceInfo("upload/articles", Filter = EFilesSourceFilter.Videos)]
	    [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UploadVideo)]
	    public FileSource VideoFileSource { get; set; }
        
	    [StringLength(512)]
	    [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ImageUrlLabel)]
	    public string ImageUrl { get; set; }

	    [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.AdditionalCssClassesLabel)]
	    public string AdditionalCssClasses { get; set; }

	    [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.SeoFriendlyIdLabel)]
	    public string SeoFriendlyId { get; set; }

	    [StringLength(512)]
	    [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.VideoUrlLabel)]
	    public string VideoUrl { get; set; }

	    [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.UserNameLabel)]
	    [LinkedColumn(LinkedTableName = "User", LinkedColumnName = "Username")]
	    public string UserName { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.ArticleCategory)]
	    [LinkedColumn(LinkedTableName = "ArticleCategory", LinkedColumnName = "Name")]
	    public string ArticleCategoryName { get; set; }

	}
}
