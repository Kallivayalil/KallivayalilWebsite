using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Website.Models
{
    [KnownType(typeof(UploadModel))]
    public class UploadModel 
    {

        [ScaffoldColumn(false)]
        public virtual int Id { get; set; }

        [DisplayName("Description")]
        public virtual string Description { get; set; }

        [DisplayName("Name")]
        public virtual  Constituent Constituent { get; set; }
        
        [UIHint("Attachment")]
        public virtual string Name { get; set; }
    }

    public class UploadFilesModel : List<UploadModel> { }
}