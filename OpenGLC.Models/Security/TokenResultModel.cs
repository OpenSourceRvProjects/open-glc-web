﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Models.Security
{
    public class TokenResultModel
    {
        public string Token { get; set; }
        public Guid UserID { get; set; }
    }
}
