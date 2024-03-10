using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Queue_Management_System.Models
{
    public class serviceproviders
    {
        [Key]

        public int Id { get; set; }
        public string Service_Providers { get; set; }

    }
}
