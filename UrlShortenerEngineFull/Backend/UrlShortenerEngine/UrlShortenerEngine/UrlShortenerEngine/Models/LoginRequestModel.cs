﻿using System.ComponentModel.DataAnnotations;

namespace UrlShortenerEngine.Models
{
    public class LoginRequestModel
    {
        [Required]
        public string  Email { get; set; }
        [Required] 
        public string Password { get; set; }
    }
}
