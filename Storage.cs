using Arowolo_Project_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arowolo_Project_2
{
    public class Storage
    {
        private static Storage _storage;


        public static Storage GetInstance()
        {
            if (_storage == null)
            {
                _storage = new Storage();
            }

            return _storage;
        }

        private List<User> Users { get; }
        private User? ActiveUser { get; set; }

        private Storage()
        {
            Users = new List<User>();
        }

        public void AddUser(User user)
        {
            if (Users.Any(x => x.Email.ToLower() == user.Email.ToLower()))
            {
                throw new ArgumentException("There are already user with same email");
            }

            Users.Add(user);
        }

        public User FindUserByEmail(string email)
        {
            var user = Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {email} does not found!");
            }

            return user;
        }

        public void SetActiveUser(string email)
        {
            var user = Users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {email} does not found!");
            }

            ActiveUser = user;
        }

        public User? GetActiveUser()
        {
            return ActiveUser;
        }


        public void LogOut()
        {
            ActiveUser = null;
        }

        private static Wallet _activeWallet { get; set; } = null!;

        public static Wallet ActiveWallet
        {
            get
            {
                if (_activeWallet is not null)
                {
                    return _activeWallet;
                }

                else
                {
                    throw new InvalidOperationException("Active wallet not set");
                }
            }
        }

        public Wallet[] GetWallet()
        {
            return ActiveUser.Wallets.ToArray();
        }


        public void SetActiveWallet(Wallet wallet)
        {
            var wallets = GetWallet();
            if (_activeWallet == null)
            {
                _activeWallet = wallets.FirstOrDefault(w => w.WalletName == w.WalletName);
            }

        }
    }
}
