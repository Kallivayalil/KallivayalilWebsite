using System.Web;

namespace Website.Models
{
    public class MediaAssetUploadModel
    {
        public HttpPostedFileBase fileData { get; set; }
        public string SecurityToken { get; set; }
        public string Filename { get; set; }
    }
}