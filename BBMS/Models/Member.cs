using System.ComponentModel.DataAnnotations;

namespace BBMS.Models
{
    public class Member
    {
        private string password;

        public int Id { get; set; }
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /* [Required]
         [DataType(DataType.Password)]
         [Compare("Password", ErrorMessage = "Passwords do not match")]
         public string ConfirmPassword { get; set; }*/
    }
}