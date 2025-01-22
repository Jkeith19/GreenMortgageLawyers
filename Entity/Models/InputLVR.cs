using System.ComponentModel.DataAnnotations;

namespace Entity.Models
{
    public class InputLVR
    {
        [Required]
        public decimal LoanAmount { get; set; }
        [Required]
        public decimal PropertyValue { get; set; }
    }
}
