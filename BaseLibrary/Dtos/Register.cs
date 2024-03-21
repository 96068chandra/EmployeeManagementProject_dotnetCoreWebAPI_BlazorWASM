using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Dtos
{
    public class Register:AccountBase
    {
        [Required]
        [MaxLength(100)]
        [MinLength(10)]
        public string? FullName { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Required]
        public string? ConfirmPassword { get; set; }
    }
}
