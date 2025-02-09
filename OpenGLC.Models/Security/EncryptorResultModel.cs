using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Models.Security
{
    public class EncryptorResultModel
    {
        public string PasswordHash { get; set; }
        public string SaltValue { get; set; }
    }
}
