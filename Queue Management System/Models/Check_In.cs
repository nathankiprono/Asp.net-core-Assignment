using System.ComponentModel.DataAnnotations;

namespace Queue_Management_System.Models
{
    public class Check_In
    {
        [Key]
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Servicetypes { get; set; }
        public string? description { get; set; }
        public DateTime CheckinTime { get; set; }
        public string? UserId { get; set; }
        public Check_In()
        {
            CheckinTime = DateTime.UtcNow;
        }

        public string TicketNo { get; set; }
        public int Status { get; set; }
        public bool ischecked { get; set; } 

     

    }
}
