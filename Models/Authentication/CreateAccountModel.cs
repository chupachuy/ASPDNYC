using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DNyC.Models.Authentication
{
    public class CreateAccountModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        [Required]
        public string UserName
        {
            get { return Email; }
            set { Email = value; }
        }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Cedula { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        public string SocialId { get; set; }
        public string SocialProvider { get; set; }
    }
}