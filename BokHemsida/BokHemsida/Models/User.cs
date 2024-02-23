using Microsoft.AspNetCore.Identity;

namespace BokHemsida.Models
{
    public class User : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public override string UserName { get; set; } //Gör UserName mer flexibelt att använda
        public bool Private { get; set; }
        //public virtual IEnumerable<Message_User> SentMessages { get; set; }
        //public virtual IEnumerable<Message_User> ReceivedMessages { get; set; }
        public virtual IEnumerable<UserBook> Books { get; set; }

    }
}
