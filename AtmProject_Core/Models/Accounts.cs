using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace AtmProject_Core.Models
{
    public class Accounts
    {
        public Accounts()
        {
           Pin=PinGenerator.GenerateRandomPin();
        }

      

        [Key]
        [Required(ErrorMessage = "Account Number is must")]

        public int AccountsId { get; set; }
        [Required]
        public string CustomerName { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a valid 10-digit mobile number.")]
        public string MobileNumber { get; set; }

        [Required]
       // [Range(1, double.MaxValue, ErrorMessage = "Minimum balance must be at least 1.")]
        public double Balance { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must be a 4-digit number.")]
        public string Pin { get; set; }


    }
}
