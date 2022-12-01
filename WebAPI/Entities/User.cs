﻿using System.ComponentModel.DataAnnotations;

namespace WebAPI.Db_Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
    }
}
