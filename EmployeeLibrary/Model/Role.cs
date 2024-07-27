using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeLibrary.Model
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [StringLength(20)]
        public string? RoleName { get; set; }
        public decimal? RoleSalary { get; set; }

    }
}