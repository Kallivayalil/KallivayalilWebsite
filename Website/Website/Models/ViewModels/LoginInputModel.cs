using System;

namespace Website.Models.ViewModels
{
    [Serializable]
    public class LoginInputModel
    {
        public virtual string Password { get; set; }
        public virtual bool IsAdmin { get; set; }
        public  string CreatedBy { get; set; }
        public  DateTime CreatedDateTime { get; set; }
        public  int Id { get; set; }

    }
}