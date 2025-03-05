using K9.Base.WebApplication.Constants.Html;
using K9.Base.WebApplication.Enums;
using K9.Base.WebApplication.Helpers;
using K9.DataAccessLayer.Models;
using K9.Globalisation;
using K9.WebApplication.Controllers;
using K9.WebApplication.Models;
using K9.WebApplication.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using K9.Base.DataAccessLayer.Attributes;
using K9.Base.WebApplication.Constants;
using K9.Base.WebApplication.Options;
using K9.SharedLibrary.Extensions;
using K9.SharedLibrary.Models;
using K9.WebApplication.Enums;
using K9.WebApplication.ViewModels;
using WebMatrix.WebData;
using NineStarKiModel = K9.WebApplication.Models.NineStarKiModel;

namespace K9.WebApplication.Helpers
{
    public static partial class HtmlHelpers
    {
        public static string ActionWithBookmark(this UrlHelper helper, string actionName, string controllerName, string bookmark)
        {
            return helper.RequestContext.RouteData.Values["action"].ToString().ToLower() == actionName.ToLower() &&
                   helper.RequestContext.RouteData.Values["controller"].ToString().ToLower() == controllerName.ToLower()
                ? $"#{bookmark}"
                : $"{helper.Action(actionName, controllerName)}#{bookmark}";
        }

        public static MvcHtmlString CollapsiblePanel(this HtmlHelper html, CollapsiblePanelOptions options)
        {
            return html.Partial("Controls/_CollapsiblePanel", options);
        }

        public static MvcHtmlString CollapsiblePanel(this HtmlHelper html, string title, string body, bool expanded = false, string footer = "", string id = "", string imageSrc = "", EPanelImageSize imageSize = EPanelImageSize.Default, EPanelImageLayout imageLayout = EPanelImageLayout.Cover)
        {
            return html.Partial("Controls/_CollapsiblePanel", new CollapsiblePanelOptions
            {
                Id = id,
                Title = title,
                Body = body,
                Expaded = expanded,
                Footer = footer,
                ImageSrc = string.IsNullOrEmpty(imageSrc) ? string.Empty : new UrlHelper(html.ViewContext.RequestContext).Content(imageSrc),
                ImageLayout = imageLayout,
                ImageSize = imageSize
            });
        }

        public static MvcHtmlString Panel(this HtmlHelper html, PanelOptions options)
        {
            return html.Partial("Controls/_Panel", options);
        }

        public static MvcHtmlString Panel(this HtmlHelper html, string title, string body, string id = "", string imageSrc = "", EPanelImageSize imageSize = EPanelImageSize.Default, EPanelImageLayout imageLayout = EPanelImageLayout.Cover)
        {
            return html.Partial("Controls/_Panel", new PanelOptions
            {
                Id = id,
                Title = title,
                Body = body,
                ImageSrc = string.IsNullOrEmpty(imageSrc) ? string.Empty : new UrlHelper(html.ViewContext.RequestContext).Content(imageSrc),
                ImageSize = imageSize,
                ImageLayout = imageLayout
            });
        }

        public static MvcHtmlString ImagePanel(this HtmlHelper html, PanelOptions options)
        {
            return html.Partial("Controls/_ImagePanel", options);
        }

        public static MvcHtmlString ImagePanel(this HtmlHelper html, string title, string imageSrc = "", EPanelImageSize imageSize = EPanelImageSize.Default, EPanelImageLayout imageLayout = EPanelImageLayout.Cover)
        {
            return html.Partial("Controls/_ImagePanel", new PanelOptions
            {
                Title = title,
                ImageSrc = string.IsNullOrEmpty(imageSrc) ? string.Empty : new UrlHelper(html.ViewContext.RequestContext).Content(imageSrc),
                ImageSize = imageSize,
                ImageLayout = imageLayout
            });
        }

        public static MvcHtmlString CollapsibleDiv(this HtmlHelper html, string body)
        {
            return html.Partial("Controls/_CollapsibleDiv", body);
        }

        public static string GetLabelTooltipFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var description = metaData.Description;
            if (!string.IsNullOrEmpty(description))
            {
                return $"<i data-toggle=\"tooltip\" title=\"{description}\" class=\"input-tooltip fa fa-question-circle\"></i>";
            }
            return string.Empty;
        }

        public static MvcHtmlString BootstrapEditorwithInfoFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, EditorOptions options = null)
        {
            var sb = new StringBuilder();
            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var modelType = modelMetadata.ModelType;
            var propertyName = modelMetadata.PropertyName;

            // Get additional view data for the control
            var viewDataDictionary = new ViewDataDictionary();
            options = options ?? new EditorOptions();
            viewDataDictionary.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Class, options.InputSize.ToCssClass());
            viewDataDictionary.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Class, options.InputWidth.ToCssClass());
            viewDataDictionary.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.CultureInfo, options.CultureInfo);

            if (modelType != typeof(bool))
            {
                viewDataDictionary.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Class, Bootstrap.Classes.FormControl);
            }

            viewDataDictionary.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.PlaceHolder, options.PlaceHolder);
            viewDataDictionary.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Title, string.Empty);
            viewDataDictionary.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Label, options.Label);

            // Get container div
            var div = new TagBuilder(Tags.Div);
            if (options.IsHidden)
            {
                div.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Style, "display: none;");
            }
            var attributes = new Dictionary<string, object>();
            attributes.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Class, !options.IsReadOnly && modelType == typeof(bool) ? Bootstrap.Classes.Checkbox : Bootstrap.Classes.FormGroup);
            attributes.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.DataInputId, propertyName);
            if (html.GetModelErrorsFor(expression).Any())
            {
                attributes.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Class, Bootstrap.Classes.HasError);
            }

            div.MergeAttributes(attributes);
            sb.AppendLine(div.ToString(TagRenderMode.StartTag));

            var hideLabelForTypes = new List<Type> { typeof(bool), typeof(FileSource) };
            if (!hideLabelForTypes.Contains(modelType))
            {
                sb.AppendLine(html.LabelFor(expression, options.Label).ToString());
                sb.Append(html.GetLabelTooltipFor(expression));
            }

            if (options.IsReadOnly)
            {
                sb.AppendLine(html.DisplayFor(expression, new { viewDataDictionary }).ToString());
            }
            else
            {
                sb.AppendLine(html.EditorFor(expression, new { viewDataDictionary }).ToString());
                sb.AppendLine(html.ValidationMessageFor(expression).ToString());
            }
            sb.AppendLine(div.ToString(TagRenderMode.EndTag));

            return MvcHtmlString.Create(sb.ToString());
        }

        public static string GetEnergySpecificDisplayNameFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, string energyName)
        {
            return $"{energyName} {html.GetDisplayNameFor(expression)}";
        }

        public static IDisposable PayWall(this HtmlHelper html, ESection section, MembershipOption.ESubscriptionType subscriptionType = MembershipOption.ESubscriptionType.WeeklyPlatinum, bool silent = false, string displayHtml = "")
        {
            var baseController = html.ViewContext.Controller as BaseRootController;
            var activeUserMembership = baseController?.GetActiveUserMembership();
            return html.PayWall<NineStarKiModel>(section, null,
                () => activeUserMembership?.MembershipOption?.SubscriptionType >= subscriptionType, silent, displayHtml);
        }

        public static IDisposable PayWall(this HtmlHelper html, ESection section, Func<bool?> condition, bool silent = false, string displayHtml = "")
        {
            return html.PayWall<NineStarKiModel>(section, null, condition, silent, displayHtml);
        }

        public static IDisposable PayWall<T>(this HtmlHelper html, ESection section, T model, bool silent = false,
            string displayHtml = "", bool hidePadlock = false)
        {
            var baseController = html.ViewContext.Controller as BaseRootController;
            var activeUserMembership = baseController?.GetActiveUserMembership();
            return html.PayWall<NineStarKiModel>(section, null, () => (activeUserMembership != null && activeUserMembership?.IsAuthorisedToViewPaidContent() == true) || SessionHelper.CurrentUserIsAdmin(), silent, displayHtml, hidePadlock);
        }

        public static IDisposable PayWall<T>(this HtmlHelper html, ESection section, T model, Func<bool?> condition, bool silent = false, string displayHtml = "", bool hidePadlock = false)
        {
            var baseController = html.ViewContext.Controller as BaseRootController;
            var activeUserMembership = baseController?.GetActiveUserMembership();
            var isAuthorised = activeUserMembership != null && (condition?.Invoke().Value ?? true) || activeUserMembership?.IsAuthorisedToViewPaidContent() == true;
            var isProfile = typeof(T) == typeof(NineStarKiModel);
            var isCompatibility = typeof(T) == typeof(CompatibilityModel);
            var div = new TagBuilder(Tags.Div);

            if (!(WebSecurity.IsAuthenticated && isAuthorised))
            {
                div.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Style, "display: none !important;");
                div.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Class, "paywall-remove-element");
            }

            if (!silent && !isAuthorised)
            {
                var retrieveLast = GetSectionCode(section);
                var centerDiv = new TagBuilder(Tags.Div);
                centerDiv.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Class,
                    "upgrade-container center-block");
                html.ViewContext.Writer.WriteLine(centerDiv.ToString(TagRenderMode.StartTag));
                if (WebSecurity.IsAuthenticated)
                {
                    if (hidePadlock)
                    {
                        html.ViewContext.Writer.Write(html.Partial("UpgradePromptNoPadlock", retrieveLast));
                    }
                    else
                    {
                        html.ViewContext.Writer.Write(html.Partial("UpgradePrompt", retrieveLast));
                    }
                }
                else
                {
                    if (hidePadlock)
                    {
                        html.ViewContext.Writer.Write(html.Partial("LoginPromptNoPadlock", retrieveLast));
                    }
                    else
                    {
                        html.ViewContext.Writer.Write(html.Partial("LoginPrompt", retrieveLast));
                    }
                }

                if (!string.IsNullOrEmpty(displayHtml))
                {
                    html.ViewContext.Writer.WriteLine(displayHtml);
                }

                html.ViewContext.Writer.WriteLine(centerDiv.ToString(TagRenderMode.EndTag));
            }

            html.ViewContext.Writer.WriteLine(div.ToString(TagRenderMode.StartTag));
            return new TagCloser(html, Tags.Div);
        }

        public static MvcHtmlString PayWallContent(this HtmlHelper html, ESection section, string content, bool showPadlock = false)
        {
            return html.PayWallContent<NineStarKiModel>(section, null, content, showPadlock);
        }

        public static MvcHtmlString PayWallContent<T>(this HtmlHelper html, ESection section, T model, string content, bool showPadlock = false, MembershipOption.ESubscriptionType subscriptionType = MembershipOption.ESubscriptionType.WeeklyPlatinum)
        {
            var baseController = html.ViewContext.Controller as BaseRootController;
            var activeUserMembership = baseController?.GetActiveUserMembership();
            var isAuthorised = (activeUserMembership != null && (activeUserMembership.IsAuthorisedToViewPaidContent() || 
                                activeUserMembership.MembershipOption.SubscriptionType >= subscriptionType)) ||
                                SessionHelper.CurrentUserIsAdmin();

            if (!(WebSecurity.IsAuthenticated && isAuthorised))
            {
                SetLast(section, model);
            }

            if (!isAuthorised)
            {
                using (StringWriter writer = new StringWriter())
                {
                    var retrieveLast = GetSectionCode(section);
                    var centerDiv = new TagBuilder(Tags.Div);
                    centerDiv.MergeAttribute(Base.WebApplication.Constants.Html.Attributes.Class,
                        "upgrade-container center-block");
                    writer.WriteLine(centerDiv.ToString(TagRenderMode.StartTag));

                    if (showPadlock)
                    {
                        if (WebSecurity.IsAuthenticated)
                        {
                            writer.WriteLine(html.Partial("UpgradePrompt", retrieveLast));
                        }
                        else
                        {
                            writer.WriteLine(html.Partial("LoginPrompt", retrieveLast));
                        }
                    }
                    else
                    {
                        var viewModel = new PromptViewModel
                        {
                            Content = content,
                            RetrieveLast = retrieveLast
                        };

                        if (WebSecurity.IsAuthenticated)
                        {
                            writer.WriteLine(html.Partial("UpgradePromptGentle", viewModel));
                        }
                        else
                        {
                            writer.WriteLine(html.Partial("LoginPromptGentle", viewModel));
                        }
                    }

                    writer.WriteLine(centerDiv.ToString(TagRenderMode.EndTag));

                    content = writer.ToString();
                }
            }

            return new MvcHtmlString(content);
        }

        private static string GetSectionCode(ESection section)
        {
            return section.GetAttribute<EnumDescriptionAttribute>().CultureCode;
        }

        private static void SetLast(ESection section, object model)
        {
            if (model != null)
            {
                switch (section)
                {
                    case ESection.Profile:
                        SessionHelper.SetLastProfile(model as NineStarKiModel);
                        break;

                    case ESection.Compatibility:
                        SessionHelper.SetLastCompatibility(model as CompatibilityModel);
                        break;

                    case ESection.Predictions:
                        SessionHelper.SetLastPrediction(model as NineStarKiModel);
                        break;

                    case ESection.Biorhythms:
                        SessionHelper.SetLastBiorhythm(model as NineStarKiModel);
                        break;

                    case ESection.KnowledgeBase:
                        SessionHelper.SetLastKnowledgeBase(model as string);
                        break;
                }
            }
        }
    }
}