namespace Website.Models
{
    public class Login : Entity
    {
        public virtual int Id { get; set; }

        public virtual Email Email { get; set; }
        public virtual string Password { get; set; }
        public virtual bool IsAdmin { get; set; }


    }
}