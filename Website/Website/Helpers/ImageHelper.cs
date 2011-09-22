using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Website.Helpers
{
    public static class ImageHelper
    {
        //0 - Image Source
        //1 - Alt Tag
        //2 - Styles
        //3 - Class
        private const string ImageTag = "<img src=\"{0}\" alt=\"{1}\" {2} {3} />";

        public static string Thumbnail(this HtmlHelper helper,string controllername,string action,int width,int height,string file)
        {
            var imagelocation = ThumbnailLocation(helper, controllername, action, width, height, file);

            //WC3 - ALT tag always needed so when not set then set to the filename.
            return string.Format(ImageTag, imagelocation, file, null, null);
        }

        public static string Thumbnail(this HtmlHelper helper,string controllername,string action,int width,int height,string file,string alt)
        {
            var imagelocation = ThumbnailLocation(helper, controllername, action, width, height, file);

            return string.Format(ImageTag, imagelocation, alt, null, null);
        }

        public static string Thumbnail(this HtmlHelper helper,string controllername,string action,int width,int height,string file,string alt,IDictionary<string, object> styleAttributes)
        {
            var imagelocation = string.Format("/{0}/{1}/{2}/{3}/{4}", controllername, action, width, height, file);

            var builder = new StringBuilder();
            var classs = string.Empty;

            if (styleAttributes != null && styleAttributes.Count > 0)
            {
                builder = new StringBuilder("style=\"");
                foreach (KeyValuePair<string, object> styleAttribute in styleAttributes)
                {
                    if (styleAttribute.Key == "class")
                    {
                        classs = string.Format("class=\"{0}\"", styleAttribute.Value);
                    }
                    else
                    {
                        builder.Append(styleAttribute.Key + ":" + styleAttribute.Value + ";");
                    }
                }
                builder.Append("\"");
            }

            return string.Format(ImageTag, imagelocation, alt, builder, classs);
        }

        public static string ThumbnailLocation(this HtmlHelper helper,string controllername,string action,int width,int height,string file)
        {
            return string.Format("/{0}/{1}/{2}/{3}/{4}", controllername, action, width, height, file);
        }
    }
}