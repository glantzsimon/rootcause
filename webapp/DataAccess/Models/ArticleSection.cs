using K9.Base.DataAccessLayer.Attributes;
using K9.Base.DataAccessLayer.Enums;
using K9.Base.DataAccessLayer.Models;
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
    [AutoGenerateName]
    [Name(ResourceType = typeof(Globalisation.Dictionary), ListName = Globalisation.Strings.Names.ArticleSections, PluralName = Globalisation.Strings.Names.ArticleSections, Name = Globalisation.Strings.Names.ArticleSection)]
    public class ArticleSection : ObjectBase
	{
	    [ForeignKey("Article")]
	    public int ArticleId { get; set; }

	    public virtual Article Article { get; set; }
        
		[Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.TitleLabel)]
		[Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
		[StringLength(256)]
		public string Title { get; set; }
		
		[Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.BodyLabel)]
		[Required(ErrorMessageResourceType = typeof(Dictionary), ErrorMessageResourceName = Strings.ErrorMessages.FieldIsRequired)]
		[StringLength(int.MaxValue)]
		[DataType(DataType.Html)]
		[AllowHtml]
		public string Body { get; set; }

	    [Display(ResourceType = typeof(Globalisation.Dictionary), Name = Globalisation.Strings.Labels.OrderLabel)]
	    public int? Order { get; set; }

	    [FileSourceInfo("upload/newsitems", Filter = EFilesSourceFilter.Images)]
		[Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UploadImages)]
		public FileSource ImageFileSource { get; set; }

	    [FileSourceInfo("upload/newsitems", Filter = EFilesSourceFilter.Videos)]
	    [Display(ResourceType = typeof(Dictionary), Name = Strings.Names.UploadVideo)]
	    public FileSource VideoFileSource { get; set; }

	    [StringLength(512)]
	    [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.VideoUrlLabel)]
	    public string VideoUrl { get; set; }
        
	    [StringLength(512)]
	    [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.ImageUrlLabel)]
	    public string ImageUrl { get; set; }

	    [Display(ResourceType = typeof(Dictionary), Name = Strings.Labels.AdditionalCssClassesLabel)]
	    public string AdditionalCssClasses { get; set; }
	}
}
