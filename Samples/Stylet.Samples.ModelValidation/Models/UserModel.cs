using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stylet.Samples.ModelValidation.Models
{
    public class UserModel : PropertyChangedBase
    {
        private string _userName;
        private string _email;
        private string _password;
        private string _passwordConfirmation;
        private Stringable<int> _age;

        public string UserName
        {
            get => _userName;
            set => SetAndNotify(ref _userName, value);
        }

        public string Email
        {
            get => _email;
            set => SetAndNotify(ref _email, value);
        }

        public string Password
        {
            get => _password; set => SetAndNotify(ref _password, value);
        }
        public string PasswordConfirmation
        {
            get => _passwordConfirmation;
            set => SetAndNotify(ref _passwordConfirmation, value);
        }
        public Stringable<int> Age
        {
            get => _age;
            set => SetAndNotify(ref _age, value);
        }
    }
}