﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Application.DTO
{
    public class AuthenticateUserDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
