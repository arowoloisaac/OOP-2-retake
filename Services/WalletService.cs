using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arowolo_Project_2
{
    internal class WalletService
    {
        private Wallet _wallet;
        public WalletService(Wallet wallet)
        {
            _wallet = wallet;
        }

        public bool AddOperation(Operation operation, Money value, int categoryNumber)
        {
            char sign = value.GetSign();
            uint integerPart = value.GetIntegerPart();
            ushort fractionalPart = value.GetFractionalPart();

            if (integerPart == 0 && fractionalPart == 0)
            {
                return false;
            }


            operation.Value = value;
            if (operation is Expense expense)
            {
                // validation for expenses and choose the categoryNumber from the expenses enum
                _wallet.AddOperation(expense);
            }
            else if (operation is Income income)
            {
                // validation for income and choose the categoryNumber from the income enum
                _wallet.AddOperation(income);
            }

            return true;
        }
    }
}
