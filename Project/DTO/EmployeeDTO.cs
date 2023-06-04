using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.DTO
{
    [Table("Employee")]
    public class EmployeeDTO
    {

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }

        [Required]
        [MaxLength()]
        public string Image { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        public int HomeNo { get; set; }

        [MaxLength(50)]
        public string Street { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        public DateTime BirthDate { get; set; }

        [Required]
        [MaxLength()]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; }
    }
}
