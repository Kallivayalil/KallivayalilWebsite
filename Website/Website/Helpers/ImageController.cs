using System.Web.Mvc;

namespace Website.Helpers
{
    public class ImageController : Controller
    {
        protected internal virtual ThumbnailResult Thumbnail(int width, int height, string filename)
        {
            return new ThumbnailResult(width, height, filename);
        }
    }
}