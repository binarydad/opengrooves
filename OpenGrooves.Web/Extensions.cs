using OpenGrooves.Core;
using OpenGrooves.Core.Extensions;
using OpenGrooves.Core.Helpers;
using OpenGrooves.Services.Configuration;
using OpenGrooves.Web.Models;
using StructureMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace OpenGrooves.Web.Extensions
{
    public static class Extensions
    {
        #region Html/Url Helpers
        public static MvcHtmlString LineItem(this HtmlHelper html, string data)
        {
            if (!data.IsNullOrWhiteSpace())
            {
                var tag = new TagBuilder("div");
                tag.InnerHtml = data;
                return MvcHtmlString.Create(tag.ToString());
            }

            return MvcHtmlString.Create(data);
        }

        public static string FeedAge(this HtmlHelper h, DateTime dt)
        {
            var timeDiff = DateTime.Now.Subtract(dt);

            if (timeDiff.TotalMinutes == 0) return "just now";
            else if (timeDiff.TotalMinutes < 10) return "a moment ago";
            else if (timeDiff.TotalMinutes < 60) return Math.Ceiling(timeDiff.TotalMinutes) + "m ago";
            else if (timeDiff.TotalHours < 1.5) return "about an hour ago";
            else if (timeDiff.TotalHours < 12) return Math.Ceiling(timeDiff.TotalHours) + " hours ago";
            else if (timeDiff.TotalHours <= 24) return "earlier today";
            else if (timeDiff.TotalDays <= 2) return "yesterday";
            else if (timeDiff.TotalDays <= 30) return "about " + Math.Ceiling(timeDiff.TotalDays) + " days ago";
            else if (timeDiff.TotalDays <= 60) return "over a month ago";
            else if (timeDiff.TotalDays <= 365) return "about " + Math.Ceiling(timeDiff.TotalDays / 30) + " months ago";
            else return "about " + Math.Ceiling(timeDiff.TotalDays / 365) + " years ago";

        }

        public static MvcHtmlString FormFieldFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool showValidation = true)
        {
            return CreateFormField<TModel, TValue>(html, expression, null, showValidation: showValidation);
        }

        public static MvcHtmlString FormFieldFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object divHtmlAttributes, bool showValidation = true)
        {
            return CreateFormField<TModel, TValue>(html, expression, divHtmlAttributes, showValidation: showValidation);
        }

        public static MvcHtmlString ReadOnlyFieldFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return CreateFormField<TModel, TValue>(html, expression, null, true);
        }

        public static MvcHtmlString ReadOnlyFieldFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object divHtmlAttributes)
        {
            return CreateFormField<TModel, TValue>(html, expression, divHtmlAttributes, true);
        }

        private static MvcHtmlString CreateFormField<TModel, TValue>(HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object divHtmlAttributes, bool readOnly = false, bool showValidation = true)
        {
            if (html == null) throw new ArgumentNullException("html");

            var metadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var fieldName = metadata.DisplayName ?? (metadata.PropertyName ?? htmlFieldName.Split('.').Last());

            if (string.IsNullOrEmpty(fieldName)) fieldName = string.Empty;

            var tb = new TagBuilder("label");
            if (metadata.IsRequired) tb.AddCssClass("required");
            tb.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            tb.SetInnerText(fieldName);

            var label = tb.ToString(TagRenderMode.Normal);

            string display;

            if (readOnly)
            {
                var value = html.DisplayFor(expression).ToHtmlString();

                if (value.IsNullOrWhiteSpace())
                {
                    return MvcHtmlString.Empty;
                }

                display = "<span>" + value.NL2BR() + "</span>";
            }
            else
            {
                display = html.EditorFor(expression).ToHtmlString();
            }

            MvcHtmlString validator = null;

            if (showValidation)
            {
                validator = html.ValidationMessageFor(expression);
            }

            tb = new TagBuilder("div");
            tb.Attributes["class"] = "field";
            if (divHtmlAttributes != null) tb.MergeAttributes<string, object>(new RouteValueDictionary(divHtmlAttributes));
            tb.InnerHtml = label + display + validator;

            return MvcHtmlString.Create(tb.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            if (html == null) throw new ArgumentNullException("html");

            //Get the expression's metadata, then obtain its display name.
            var metadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
            string displayName = metadata.DisplayName;

            //Rather than returning null, we return an empty string.
            if (String.IsNullOrEmpty(displayName)) displayName = "";

            return MvcHtmlString.Create(displayName);
        }

        public static MvcHtmlString StateListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            if (html == null) throw new ArgumentNullException("html");

            var statesSelect = Data.Static.States.Select(s => new SelectListItem { Text = s.Key, Value = s.Value });

            return html.DropDownListFor(expression, statesSelect);
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool showRequired)
        {
            var metadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
            var htmlFieldName = ExpressionHelper.GetExpressionText(expression);

            string str = metadata.DisplayName ?? (metadata.PropertyName ?? htmlFieldName.Split(new char[] { '.' }).Last<string>());
            if (string.IsNullOrEmpty(str))
            {
                return MvcHtmlString.Empty;
            }
            TagBuilder builder = new TagBuilder("label");
            if (showRequired && metadata.IsRequired) builder.AddCssClass("required");
            builder.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
            builder.SetInnerText(str);
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ScriptInclude(this HtmlHelper html, string path)
        {
            string include = String.Format("<script type=\"text/javascript\" lang=\"javascript\" src=\"{0}\"></script>", System.Web.VirtualPathUtility.ToAbsolute(path));
            return MvcHtmlString.Create(include);
        }

        public static MvcHtmlString CssInclude(this HtmlHelper html, string path)
        {
            string include = String.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" />", System.Web.VirtualPathUtility.ToAbsolute(path));
            return MvcHtmlString.Create(include);
        }

        public static MvcHtmlString BandProfileUrl(this HtmlHelper html, BandModel band)
        {
            return BandProfileUrl(html, band.UrlName, band.Name);
        }
        
        public static MvcHtmlString BandProfileUrl(this HtmlHelper html, string bandUrl, string name)
        {
            if (!name.IsNullOrWhiteSpace() && !bandUrl.IsNullOrWhiteSpace())
            {
                return html.RouteLink(name, "band", new RouteValueDictionary(new { action = "profile", name = bandUrl }), new Dictionary<string, object> { { "data-popup-type", "band" }, { "data-popup-data", bandUrl } });
            }

            return MvcHtmlString.Create(name);
        }

        public static MvcHtmlString UserProfileUrl(this HtmlHelper html, UserModel user)
        {
            return UserProfileUrl(html, user.UserName, user.RealName);
        }

        public static MvcHtmlString UserProfileUrl(this HtmlHelper html, string username, string name)
        {
            if (!name.IsNullOrWhiteSpace() && !username.IsNullOrWhiteSpace())
            {
                return html.RouteLink(name, "user", new RouteValueDictionary(new { action = "profile", username = username.ToLower() }), new Dictionary<string, object> { { "data-popup-type", "user" }, { "data-popup-data", username } });
            }

            return MvcHtmlString.Create(name);
        }

        public static MvcHtmlString EventUrl(this HtmlHelper html, EventModel ev)
        {
            return EventUrl(html, ev.UrlName, ev.Name);
        }

        public static MvcHtmlString EventUrl(this HtmlHelper html, string eventUrl, string name)
        {
            if (!eventUrl.IsNullOrWhiteSpace() && !name.IsNullOrWhiteSpace())
            {
                return html.RouteLink(name, "event", new RouteValueDictionary(new { action = "index", name = eventUrl }), new Dictionary<string, object> { { "data-popup-type", "event" }, { "data-popup-data", eventUrl } });
            }

            return MvcHtmlString.Create(name);
        }

        public static MvcHtmlString FacebookUrl(this HtmlHelper html, string name, int truncate = 50)
        {
            TagBuilder fb = new TagBuilder("a");
            fb.Attributes["href"] = name.Contains("facebook.com") ? name.CheckProtocol() : ("http://facebook.com/" + name);
            fb.Attributes["class"] = "facebook-link";
            fb.Attributes["target"] = "_blank";
            fb.SetInnerText(name.Length > truncate ? "View Facebook profile" : name);

            return MvcHtmlString.Create(fb.ToString());
        }

        public static MvcHtmlString TwitterUrl(this HtmlHelper html, string name, int truncate = 50)
        {
            TagBuilder tw = new TagBuilder("a");
            tw.Attributes["href"] = name.Contains("twitter.com") ? name.CheckProtocol() : ("http://twitter.com/" + name);
            tw.Attributes["class"] = "twitter-link";
            tw.Attributes["target"] = "_blank";
            tw.SetInnerText(name.Length > truncate ? "View Twitter profile" : name);

            return MvcHtmlString.Create(tw.ToString());
        }

        public static MvcHtmlString WebsiteUrl(this HtmlHelper html, string url, int truncate = 50)
        {
            TagBuilder tw = new TagBuilder("a");
            tw.Attributes["href"] = url.CheckProtocol();
            tw.Attributes["target"] = "_blank";
            tw.SetInnerText(url.Length > truncate ? "View website" : url);

            return MvcHtmlString.Create(tw.ToString());
        }

        public static string CheckProtocol(this string url)
        {
            if (url.Contains("http://") || url.Contains("https://"))
            {
                return url;
            }

            return String.Format("http://{0}", url);
        }

        public static MvcHtmlString LetterBrowser(this HtmlHelper h)
        {
            // TODO: add number support
            var alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var formatted = alpha.ToList().Select(c =>
            {
                return String.Format("<a href=\"?letter={0}#letter-browser\">{0}</a>", c);
            });

            return MvcHtmlString.Create(String.Format("<a name=\"letter-browser\"></thu><div class=\"letter-browser\">{0}<div class=\"clear\"></div></div>", String.Join(" &nbsp;", formatted)));
        }

        public static MvcHtmlString LocationSearchUrl(this HtmlHelper html, Location location, string action = "nearby")
        {
            return LocationSearchUrl(html, location.City, location.State, location.Zip, action);
        }

        public static MvcHtmlString LocationSearchUrl(this HtmlHelper html, string city, string state, string zip, string action = "nearby")
        {
            string location, displayText;

            if ((city.IsNullOrWhiteSpace() || state.IsNullOrWhiteSpace()) && !zip.IsNullOrWhiteSpace())
            {
                displayText = zip;
                location = zip;
            }
            else
            {
                displayText = LocationHelper.CityStateOrZip(city, state); // we dont want to display zip, it looks cluttered
                location = LocationHelper.CityStateOrZip(city, state, zip);
            }

            if (!location.IsNullOrWhiteSpace())
            {
                return html.RouteLink(displayText, "browse", new { action = action, address = location });
            }

            return MvcHtmlString.Empty;
        }
        #endregion

        public static MvcHtmlString Uploadify(this HtmlHelper html, string uploadUrl)
        {
            return Uploadify(html, uploadUrl, null);
        }

        public static MvcHtmlString Uploadify(this HtmlHelper html, string uploadUrl, string callbackJs)
        {
            return html.Action("Uploadify", "Upload", new { uploadUrl = uploadUrl, callbackJs = callbackJs});
        }

        #region Image Processing
        public static string GenerateUserFileName(this HttpContextBase ctx, string filename)
        {
            return String.Format("{0}_{1}_{2}{3}", ctx.User.Identity.Name, Path.GetFileNameWithoutExtension(filename), Guid.NewGuid(), Path.GetExtension(filename));
        }

        public static MvcHtmlString AvatarImage(this HtmlHelper html, string filename, bool useThumbnail = true)
        {
            string tag = null;

            if (!String.IsNullOrWhiteSpace(filename))
            {
                var thumbConfig = useThumbnail ? "ThumbPath" : "MediumImagePath";

                var config = ObjectFactory.GetInstance<IConfig>();
                var imagePath = VirtualPathUtility.Combine(VirtualPathUtility.ToAbsolute(config.GetSetting<string>("LargeImagePath")), filename);
                var thumbPath = VirtualPathUtility.Combine(VirtualPathUtility.ToAbsolute(config.GetSetting<string>(thumbConfig)), filename);
                tag = String.Format("<a class=\"fancy\" href=\"{0}\"><img class=\"avatar\" src=\"{1}\" /></a>", html.AttributeEncode(imagePath), html.AttributeEncode(thumbPath));
            }
            else
            {
                tag = String.Format("<img class=\"avatar\" src=\"{0}\" />", System.Web.VirtualPathUtility.ToAbsolute("~/content/images/no-image.gif"));
            }

            return MvcHtmlString.Create(tag);
        }

        public static MvcHtmlString GetImageLink(this HtmlHelper html, ImageModel image, bool useThumbnail, object imageAttributes = null, object anchorAttributes = null)
        {
            if (image == null || image.Url.IsNullOrWhiteSpace())
            {
                return MvcHtmlString.Create(String.Format("<img class=\"avatar\" src=\"{0}\" />", System.Web.VirtualPathUtility.ToAbsolute("~/content/images/no-image.gif")));
            }

            var thumbConfig = useThumbnail ? "ThumbPath" : "MediumImagePath";

            var config = ObjectFactory.GetInstance<IConfig>();
            var imagePath = VirtualPathUtility.Combine(VirtualPathUtility.ToAbsolute(config.GetSetting<string>("LargeImagePath")), image.Url);
            var thumbPath = VirtualPathUtility.Combine(VirtualPathUtility.ToAbsolute(config.GetSetting<string>(thumbConfig)), image.Url);

            var anchorTag = new TagBuilder("a");
            var imageTag = new TagBuilder("img");

            if (anchorAttributes != null)
            {
                GetAnonTypeValues(anchorAttributes).ForEach(a =>
                {
                    anchorTag.Attributes.Add(a);
                });
            }

            if (imageAttributes != null)
            {
                GetAnonTypeValues(imageAttributes).ForEach(a =>
                {
                    imageTag.Attributes.Add(a);
                });
            }

            anchorTag.Attributes["href"] = html.AttributeEncode(imagePath);
            imageTag.Attributes["src"] = html.AttributeEncode(thumbPath);
            imageTag.Attributes["alt"] = image.Caption;

            anchorTag.InnerHtml = imageTag.ToString();

            return MvcHtmlString.Create(anchorTag.ToString());

            //var tag = String.Format("<a href=\"{0}\"><img src=\"{1}\" /></a>", html.AttributeEncode(imagePath), html.AttributeEncode(thumbPath));

            //return MvcHtmlString.Create(tag);
        }

        private static IDictionary<string, string> GetAnonTypeValues(object obj)
        {
            return TypeDescriptor.GetProperties(obj).Cast<PropertyDescriptor>().Select(d => new KeyValuePair<string, string>(d.Name, d.GetValue(obj).ToString())).ToDictionary(d => d.Key, d => d.Value);
        }

        public static string SaveAudio(this HttpContextBase ctx, HttpPostedFileBase file)
        {
            var config = ObjectFactory.GetInstance<IConfig>();

            var filename = ctx.GenerateUserFileName(file.FileName.Replace(" ", String.Empty));

            var audioPath = Path.Combine(ctx.Server.MapPath(config.GetSetting<string>("AudioPath")), filename);

            file.SaveAs(audioPath);

            return filename;
        }

        public static string SaveImage(this HttpContextBase ctx, HttpPostedFileBase file, bool createThumb = true, bool createMedium = false)
        {
            var config = ObjectFactory.GetInstance<IConfig>();

            var filename = ctx.GenerateUserFileName(file.FileName);

            var imagesPath = Path.Combine(ctx.Server.MapPath(config.GetSetting<string>("LargeImagePath")), filename);
            int size = config.GetSetting<int>("ThumbSize");

            var image = System.Drawing.Image.FromStream(file.InputStream);
            var resizedImage = ResizeImage(image, 800, 800);

            resizedImage.Save(imagesPath);

            if (createThumb)
            {
                var thumbPath = Path.Combine(ctx.Server.MapPath(config.GetSetting<string>("ThumbPath")), filename);
                var thumb = image.GetThumbnailImage(size, size, null, IntPtr.Zero);
                thumb.Save(thumbPath, System.Drawing.Imaging.ImageFormat.Png);
            }

            if (createMedium)
            {
                var mediumPath = Path.Combine(ctx.Server.MapPath(config.GetSetting<string>("MediumImagePath")), filename);
                var mediumImage = ResizeImage(image, 200, 200);
                mediumImage.Save(mediumPath);
            }

            return filename;
        }

        private static Image ResizeImage(Image imgToResize, int width, int height)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)width / (float)sourceWidth);
            nPercentH = ((float)height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            var b = new Bitmap(destWidth, destHeight);
            using (var g = Graphics.FromImage((Image)b))
            {
                g.InterpolationMode = InterpolationMode.Default;
                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            }

            return (Image)b;
        }
        #endregion
    }
}
