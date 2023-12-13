
using Arowolo_Project_2.Enums;
using Arowolo_Project_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Arowolo_Project_2
{
    public class Wallet
    {
        // the wallet field name
        public string WalletName { get; set; }
        // accessing the money class according to the class diagram showing association to the wallet
        public Money Money { get; set; }
        // accessing the enum
        public Currency Currency { get; set; }
        // aggregation of the walet
        private List<Operation> Operations { get; set; }

        public Wallet(string walletName, Money money, Currency currency)
        {
            Operations = new List<Operation>();
            WalletName = walletName;
            Money = money;
            Currency = currency;
        }


        public void AddOperation(Operation operation)
        {
            Operations.Add(operation);
        }

        public void AddOperation(Income value)
        {
            Operations.Add(value);
        }

        public void AddOperation(Expense value)
        {
            Operations.Add(value);
        }


        public (List<(Money Amount, DateTime Date, IncomeType Type)> Incomes, List<(Money Amount, DateTime Date, ExpenseType Type)> Expenses) CheckStatistics(DateTime from, DateTime to)
        {
            var incomesList = Operations
                .Where(x => x.Date >= from && x.Date.Date <= to && x is Income)
                .Cast<Income>()
                .Select(income => (income.Value, income.Date.Date, income.IncomeType))
                .ToList();

            var expensesList = Operations
                .Where(x => x.Date >= from && x.Date.Date <= to && x is Expense)
                .Cast<Expense>()
                .Select(expense => (expense.Value, expense.Date.Date, expense.ExpenseType))
                .ToList();

            return (incomesList, expensesList);
        }

    }

    public class Operation
    {
        public DateTime Date { get; set; }

        public Money Value { get; set; }

        public Operation(Money value, DateTime date)
        {
            Value = value;
            Date = date;
        }
    }

    public class Expense : Operation
    {
        public ExpenseType ExpenseType { get; set; }

        public Expense(Money value, DateTime date) : base(value, date)
        {
            this.ExpenseType = ExpenseType;
        }
    }
    public class Income : Operation
    {
        public IncomeType IncomeType { get; set; }

        public Income(Money value, DateTime date) : base(value, date)
        {
            this.IncomeType = IncomeType;
        }
    }
}