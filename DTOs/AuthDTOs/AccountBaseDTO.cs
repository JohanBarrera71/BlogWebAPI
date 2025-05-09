﻿using System.ComponentModel.DataAnnotations;

namespace DemoLinkedInApi.DTOs
{
    public class AccountBaseDTO
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }

    }
}
