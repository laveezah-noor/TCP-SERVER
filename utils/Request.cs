using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace utils
{
    public class LoginRequest
        {
            public string username;
            public string password;

            public LoginRequest(string username, string password)
            {
                this.username = username;
                this.password = password;
            }
        }
    public class RegisterRequest
    {
        public string username;
        public string name;
        public string password;
        public string email;
        public string phone;

        public RegisterRequest(string username, string name, string password, string email, string phone)
        {
            this.username = username;
            this.name = name;
            this.password = password;
            this.email = email;
            this.phone = phone;
        }
    }
}
