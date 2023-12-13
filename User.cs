using Arowolo_Project_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arowolo_Project_2
{
    public class User
    {
        public string Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        //used for the account creation
        public DateTime BirthDate { get; set; }
        public string PasswordHash { get; set; }

        public List<Wallet> Wallets { get; set; }
        public Wallet? ActiveWallet { get; set; }


        public User()
        {
            Wallets = new List<Wallet>();
        }
    }
}
