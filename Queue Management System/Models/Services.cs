using System.ComponentModel.DataAnnotations;

namespace Queue_Management_System.Models
{
    public class Services
    {
        [Key]
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string Service_Providers { get; set; }
        public string OpeningHours { get; set; }


    }
}
