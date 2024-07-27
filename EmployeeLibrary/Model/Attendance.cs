using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeLibrary.Model
{
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendanceId { get; set; }
        [ForeignKey("User")]
        [DisplayName("User")]
        [Required(ErrorMessage = "User Id is required")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Date { get; set; }

        public string Status { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime CheckIn { get; set; }
        [DisplayFormat(DataFormatString = "{0:HH:mm}")]
        public DateTime CheckOut { get; set; }
        public int WorkingHours { get; set; }
        public User? User { get; set; }

    }
}
