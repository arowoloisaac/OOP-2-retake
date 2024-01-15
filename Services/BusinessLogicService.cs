using Arowolo_Project_2;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Arowolo_Project_2.Enums;

namespace Arowolo_Project_2.Services
{
    public class BusinessLogicService : IBusinessLogicService
    {
        private Storage Storage { get; }

        public BusinessLogicService()
        {
            Storage = Storage.GetInstance();
        }

        // For register
        public (bool, string) Register(string email, string password, string name)
        {
            try
            {
                var mail = new MailAddress(email);
            }
            catch (FormatException)
            {
                return (false, "Email is incorrect!");
            }

            var passwordScore = PasswordAdvisor.CheckStrength(password);
            if (passwordScore < PasswordScore.Medium)
            {
                return (false, "Your password is too week!");
            }

            try
            {
                Storage.AddUser(new User
                {
                    Name = name,
                    Email = email,
                    Password = password
                });
            }
            catch (ArgumentException)
            {
                return (false, "There are already user with same email!");
            }

            // Login
            Storage.SetActiveUser(email);

            return (true, "Successfull registration! You will be automatically logged in!");
        }


        //For Login
        public (bool, string) Login(string email, string password)
        {
            try
            {
                var user = Storage.FindUserByEmail(email);
                if (user.Password == password)
                {
                    Storage.SetActiveUser(email);
                    return (true, "Successfull login!");
                }

                return (false, "Password is incorrect!");
            }
            catch (KeyNotFoundException)
            {
                return (false, "User with provided email is not found!");
            }
        }

        //For Creating Wallet
        public (bool, string) CreateWallet(string walletname, Money money)
        {
            try
            {
                var user = Storage.GetActiveUser();
                if (user == null)
                {
                    return (false, "Not active user");
                }

                else
                {
                    user.Wallets.Add(new Wallet(walletname, money, money.currency));
                    return (true, "Successfully Created Wallet");
                }
            }

            catch (KeyNotFoundException)
            {
                return (false, "Failed to Create wallet");
            }
        }

        //get wallet list
        public List<Wallet> GetWallets()
        {
            var user = Storage.GetActiveUser();

            if (user == null)
            {
                return null;
            }

            return user.Wallets;
        }

        //for choose wallet
        public (bool, string) ChooseWallet(int index)
        {
            try
            {
                var user = Storage.GetActiveUser();

                if (user == null)
                {
                    return (false, "There is no active user");
                }

                user.ActiveWallet = user.Wallets[index];
                return (true, "Selection successful");
            }
            catch (KeyNotFoundException)
            {
                return (false, "No wallet with such index");
            }
        }


        public (bool, string) CheckStatistics(string from, string to)
        {
            var user = Storage.GetActiveUser();
            if (user == null)
            {
                return (false, "User is not logged in!");
            }

            if (user.ActiveWallet == null)
            {
                return (false, "User has not active wallet!");
            }

            if (!DateTime.TryParseExact(from, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fromParsed))
            {
                return (false, "from has incorrect format!");
            }

            if (!DateTime.TryParseExact(to, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var toParsed))
            {
                return (false, "to has incorrect format!");
            }

            var statistics = user.ActiveWallet.CheckStatistics(fromParsed, toParsed);
            //Console.WriteLine(statistics);
            var combinedTransactions = new List<(Money Amount, DateTime Date, string Type, string Category)>();

             combinedTransactions.AddRange(statistics.Incomes.Select(income =>
                (income.Amount, income.Date, income.Type.ToString(), "Income")));

            combinedTransactions.AddRange(statistics.Expenses.Select(expense =>
                (expense.Amount, expense.Date, expense.Type.ToString(), "Expense")));

            
            var result = new StringBuilder();

            result.AppendLine("History:");

            var firstTwentyTransact = combinedTransactions.Take(20);

            foreach (var transaction in firstTwentyTransact)
            {
                result.AppendLine($"Category: {transaction.Category}, Amount: {transaction.Amount}, Date: {transaction.Date}, Type: {transaction.Type}");
            }

            return (true, result.ToString());
        }


        // to add operation for money income and expenditure
        public (bool, string) AddOperation(Operation operation, string value, Enum category, string date)
        {
            var user = Storage.GetActiveUser();

            if (user is null)
            {
                return (false, "There is no active user, Login to perform this operation");
            }

            if (user.ActiveWallet is null)
            {
                return (false, "No Active wallet, create a wallet");
            }

            if (!DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateParsed))
            {
                return (false, "incorrect date format");
            }

            var currency = user.ActiveWallet.Currency;

            Money money = new Money(value, currency);

            operation.Value = money;

            operation.Date = dateParsed;

            switch (operation)
            {
                case Expense expense when category is ExpenseType expenseType:
                    expense.ExpenseType = expenseType;
                    user.ActiveWallet.AddOperation(expense);
                    break;

                case Income income when category is IncomeType incomeType:
                    income.IncomeType = incomeType;
                    user.ActiveWallet.AddOperation(income);
                    break;

                default:
                    return (false, "Invalid operation type or category.");
            }


            return (true, "Operation successfully added");
        }

        public (bool, string) DeleteWallet(int index)
        {
            try
            {
                var user = Storage.GetActiveUser();

                if (user is null)
                {
                    return (false, "No active user");
                }

                else
                {
                    user.Wallets.RemoveAt(index);

                    return (true, "Wallet deleted");
                }
            }

            catch (KeyNotFoundException)
            {
                return (false, "Unable to delete");
            }
        }
    }
}
