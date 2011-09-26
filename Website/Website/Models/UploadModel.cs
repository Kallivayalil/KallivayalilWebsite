using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class UploadModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [UIHint("Attachment")]
        public string Picture { get; set; }
    }
}