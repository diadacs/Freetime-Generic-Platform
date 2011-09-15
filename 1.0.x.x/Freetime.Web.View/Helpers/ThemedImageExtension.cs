using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Freetime.Web.Context;

namespace Freetime.Web.View.Helpers
{
    public static class ThemedImageExtensions
    {
        public static string ThemedImage(this HtmlHelper helper, string imageName, object htmlAttributes)
        {
            return ThemedImage(helper, UserHandle.CurrentUser.DefaultTheme, imageName, htmlAttributes);
        }
        public static string ThemedImage(this HtmlHelper helper, string theme, string imageName,
            object htmlAttributes)
        {          
            TagBuilder builder = new TagBuilder("img");
            AttributeHelper.MergeAttribute(builder, htmlAttributes);            
            string imageLocationFormat = "http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/Themes/{0}/{1}/{2}";
            builder.MergeAttribute("src", string.Format(imageLocationFormat, theme, "images", imageName));
            return builder.ToString();
        }

        public static string ThemedImage(this HtmlHelper helper, string theme, string imageName)
        {            
            TagBuilder builder = new TagBuilder("img");
            string imageLocationFormat = "http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/Themes/{0}/{1}/{2}";
            builder.MergeAttribute("src", string.Format(imageLocationFormat, theme, "images", imageName));
            return builder.ToString();
        }
    }
}
