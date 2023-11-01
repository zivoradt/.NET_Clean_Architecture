﻿using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class RegisterVM
    {
        [Required]
        public string FirstName { get; set; }

        [Required] public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }
    }
}