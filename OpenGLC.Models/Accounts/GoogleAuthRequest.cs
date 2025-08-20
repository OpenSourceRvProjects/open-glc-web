using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLC.Models.Accounts
{
    public class GoogleAuthRequest
    {
        public string IdToken { get; set; }
    }
    public class GoogleUserInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string DisplayName { get; set; }
    }
}
