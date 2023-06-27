﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Resources.Models.Requests
{
    public class RegisterRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
