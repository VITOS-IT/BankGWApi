﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankingGWService.Models
{
    public class User
    {
        [Key]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool EmailConfirmed { get; set; } = false;
    }
}
