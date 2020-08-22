using Microsoft.AspNetCore.Identity;

namespace SimpleStore.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string DateBirth { get; set; }

    }
}