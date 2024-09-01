using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Libray.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [DisplayName("Full name")]
        [Required(ErrorMessage = "{0} is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Address 1")]
        public string Address1 { get; set; }
        
        [DisplayName("Address 2")]
        public string? Address2 { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("City")]
        public string City { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("State")]
        public string State { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Pin Code")]
        public string PinCode { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "{0}'s length should be between {2} and {1}.")]
        [DisplayName("Password")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Date of Birth")]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Gender")]
        public string Gender { get; set; }

        public Boolean IsActive { get; set; }
        public Boolean IsAdmin { get; set; }
        public DateTime CreatedOn { get; set; }


    }
}
