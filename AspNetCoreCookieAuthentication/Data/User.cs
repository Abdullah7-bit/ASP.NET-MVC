using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace AspNetCoreCookieAuthentication.Data
{
    public class User: IdentityUser
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

    }
}
