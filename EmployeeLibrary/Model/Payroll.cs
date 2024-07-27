using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeLibrary.Model
{
    public class Payroll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayrollId { get; set; }
        [ForeignKey("UserRole")]
        public int UserRoleId { get; set; }
        public decimal? Salary { get; set; }
        [ForeignKey("UserRoleId")]
        public UserRole? UserRole { get; set; }
        public DateTime Date { get; set; }
    }
}
