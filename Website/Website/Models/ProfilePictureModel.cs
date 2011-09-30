using System.Web;

namespace Website.Models
{
    public class ProfilePictureModel
    {
        public HttpPostedFileBase imageLoad2 { get; set; }
        public int x1 { get; set; }
        public int y1 { get; set; }
        public int x2 { get; set; }
        public int y2 { get; set; }
    }
}