﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Models.Request
{
    public class PasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

    }
}