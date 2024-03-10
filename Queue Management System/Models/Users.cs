using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Queue_Management_System.Models
{
    public class Users
    {
        [Key]

        public int Id { get; set; }

        [DisplayName("Names")]

        public string Names { get; set; }

        [DisplayName("User Group")]

        public string UserGroup { get; set; }
        [DisplayName("Username")]

        public string UserName { get; set; }
        [DisplayName("Password")]

        public string Password { get; set; }
    }
}
