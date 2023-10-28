using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class LeaveTypeVM : CreateLeaveTypeVM
    {
        public int Id { get; set; }
    }

    public class CreateLeaveTypeVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Default Number of days")]
        public int DefaultDays { get; set; }
    }
}