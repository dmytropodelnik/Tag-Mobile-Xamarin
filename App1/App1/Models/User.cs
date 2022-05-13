using System;
using System.Collections.Generic;

namespace App1.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Result> Results { get; set; } = new List<Result>();
    }
}