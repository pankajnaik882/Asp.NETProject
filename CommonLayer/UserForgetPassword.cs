using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer
{
    public class UserForgetPassword
    {
        [Required]
        public string Email { get; set; }
    }
}
