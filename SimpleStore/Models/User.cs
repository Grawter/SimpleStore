using Microsoft.AspNetCore.Identity;

namespace SimpleStore.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public int Year { get; set; }
        public int Mount { get; set; }
        public int Day { get; set; }
    }
}