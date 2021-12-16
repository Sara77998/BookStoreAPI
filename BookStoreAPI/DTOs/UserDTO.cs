using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.DTOs
{
    public class UserDTO
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Password is limited to {2} to {1} characters",MinimumLength = 6)]
        public string Password { get; set; }
    }
}
