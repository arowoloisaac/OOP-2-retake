using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arowolo_Project_2.Services
{
    public interface IBusinessLogicService
    {
        (bool, string) Register(string email, string password, string name);
        (bool, string) Login(string email, string password);
        (bool, string) CreateWallet(string walletname, Money money);
        (bool, string) ChooseWallet(int index);
        List<Wallet> GetWallets();
        (bool, string) CheckStatistics(string from, string to);
        (bool, string) AddOperation(Operation operation, string value, Enum category, string date);
        (bool, string) DeleteWallet(int index);
    }
}
