using System.Runtime.InteropServices;
using Arowolo_Project_2.Services;
using Arowolo_Project_2;
using System.Globalization;
using Arowolo_Project_2.Enums;

while (true)
{
    //var wallet = new Wallet();
    //var walletService = new WalletService(wallet);

    Console.ForegroundColor = ConsoleColor.White;
    IBusinessLogicService service = new BusinessLogicService();
    Storage storage = Storage.GetInstance();
    var activeUser = storage.GetActiveUser();
    Console.Clear();
    Console.WriteLine("Main menu options:");
    if (activeUser == null)
    {
        Console.WriteLine("Nobody is logged in!" + Environment.NewLine);
        Console.WriteLine("1 > Register");
        Console.WriteLine("2 > Login");
    }
    else
    {
        Console.WriteLine($"Hello: {activeUser.Email}");

        if (activeUser.ActiveWallet == null)
        {

        }
        else
        {
            Console.WriteLine($"Active Wallet: {activeUser.ActiveWallet.WalletName}");
        }
        Console.WriteLine("1 > Create Wallet");
        Console.WriteLine("2 > Choose Wallet");
        Console.WriteLine("3 > Show my wallets");
        Console.WriteLine("4 > Delete Wallet");

        if (activeUser.ActiveWallet is not null)
        {
            Console.WriteLine("5 > Add operation to Wallets");
            Console.WriteLine("6 > Check statistics");
        }
        Console.WriteLine("100 > Logout");
    }

    Console.WriteLine("7 > Quit");

    Console.WriteLine(Environment.NewLine + "Choose menu:");

    var userInput = Console.ReadLine();
    if (int.TryParse(userInput, out var output))
    {
        switch (output)
        {
            case 1:
                if (activeUser == null)
                {
                    Register(service);
                }
                else
                {
                    CreateWallet(service);
                }
                break;

            // login part
            case 2:
                if (activeUser is null)
                    Login(service);

                else
                {
                    ChooseWallet(service);
                }

                break;

            case 3:
                if (activeUser is not null)
                {
                    ShowWallets(service);
                }
                break;

            case 4:
                if (activeUser is not null)
                {
                    DeleteWallet(service);
                }
                break;

            case 5:
                if (activeUser is not null)
                {
                    AddOperation(service);
                }
                break;

            case 6:
                if (activeUser is not null)
                {
                    CheckStatistics(service);
                }

                break;

            case 100:
                if (activeUser is not null)
                {
                    storage.LogOut();
                }
                break;

            case 7:
                return;
            default:

                break;
        }
    }
    else
    {
        Console.WriteLine("You must type a number only!");
        Console.ReadLine();
    }
}


static void Register(IBusinessLogicService service)
{
    Console.Clear();

    Console.WriteLine("Enter your Full Name:");
    var name = Console.ReadLine();
    Console.WriteLine("Type your email:");
    var email = Console.ReadLine();
    Console.WriteLine("Type your password:");
    var pass = Console.ReadLine();
    /*Console.WriteLine("Type your birthdate in format 'dd.mm.yyyy':");
    var birthDate = Console.ReadLine();*/
    var result = service.Register(email, pass, name);
    Console.ForegroundColor = result.Item1 ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine(result.Item2);
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadLine();
}


static void Login(IBusinessLogicService service)
{
    Console.Clear();

    Console.WriteLine("Type your Email:");
    var Email = Console.ReadLine();

    Console.WriteLine("Enter your password");
    var Password = Console.ReadLine();

    var login = service.Login(Email, Password);

    Console.ForegroundColor = login.Item1 ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine(login.Item2);
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.ReadLine();
}


static void CreateWallet(IBusinessLogicService service)
{
    Console.Clear();

    Console.WriteLine("Input your wallet name: ");
    var WalletName = Console.ReadLine();

    Console.WriteLine("Enter the Index of your currency choice: {1. USD, 2. EUR, 3. RUB}");
    var currency = Console.ReadLine();

    Currency currency1;
    Enum.TryParse<Currency>(currency, out currency1);

    Console.WriteLine("Input the start amount: [10.0]");
    var Amount = Console.ReadLine();

    Money money = new Money(Amount, currency1);

    var createWallet = service.CreateWallet(WalletName!, money);

    Console.ForegroundColor = createWallet.Item1 ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine(createWallet.Item2);
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.ReadLine();
}


static void ShowWallets(IBusinessLogicService service)
{
    Console.Clear();

    var list = service.GetWallets();

    if (list.Any())
    {
        Console.WriteLine("List of Wallet");
    }

    int walletId = 0;
    foreach (var item in list)
    {
        walletId++;
        string money = $"{item.Money.GetIntegerPart()}.{item.Money.GetFractionalPart()}";
        Console.WriteLine(walletId + " " + item.WalletName + " " + item.Currency.ToString() + " " + money);
    }
    Console.ReadLine();
}


static void ChooseWallet(IBusinessLogicService service)
{
    Console.Clear();

    var list = service.GetWallets();

    if (list.Any()) { }

    Console.WriteLine("List of wallets");
    int walletId = 0;
    foreach (var item in list)
    {
        walletId++;
        Console.WriteLine(walletId + " " + item.WalletName);

    }

    Console.WriteLine("Enter Wallet Index");
    var index = Console.ReadLine();

    int convertId = Convert.ToInt32(index);

    var walletSelected = service.ChooseWallet(convertId - 1);

    Console.ForegroundColor = walletSelected.Item1 ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine(walletSelected.Item2);
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadLine();
}


static void DeleteWallet(IBusinessLogicService service)
{
    Console.Clear();
    var list = service.GetWallets();
    if (list.Any())
    {

    }
    int i = 1;
    foreach (var item in service.GetWallets())
    {
        Console.WriteLine(i + "  " + item.WalletName);
        i++;
    }
    Console.WriteLine("choose your Wallet index:");
    var index = Console.ReadLine();
    var result = service.DeleteWallet(Convert.ToInt32(index) - 1);
    Console.ForegroundColor = result.Item1 ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine(result.Item2);
    Console.ForegroundColor = ConsoleColor.White;
    Console.ReadLine();
}

static void CheckStatistics(IBusinessLogicService service)
{
    Console.Clear();

    Console.WriteLine("Check your staistics collection here: ");
    Console.WriteLine("Enter the FROM date in format 'dd.MM.yyyy': ");
    var from = Console.ReadLine();

    Console.WriteLine("Enter the TO date in format 'dd.MM.yyyy': ");
    var to = Console.ReadLine();

    var checkStats = service.CheckStatistics(from, to);

    Console.ForegroundColor = checkStats.Item1 ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine(checkStats.Item2);
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.ReadLine();
}


static void AddOperation(IBusinessLogicService service)
{
    Console.Clear();
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Operation Types.");
        Console.WriteLine("1 > Income");
        Console.WriteLine("2 > Expenditure");
        Console.WriteLine("3 > Exit");
        Console.WriteLine("Choose The Index Of Operation");

        var IndexString = Console.ReadLine();

        (bool, string) result = (false, "");

        var IndexInt = int.TryParse(IndexString, out int convertedStringIndex);

        //Console.WriteLine(typeof(IndexInt));
        if (IndexInt)
        {
            switch (convertedStringIndex)
            {
                case 1:
                    Console.WriteLine("Type amount value to be Added:[1.50]");
                    var value = Console.ReadLine();
                    Console.WriteLine("Choose category:");
                    Console.WriteLine("1 > Salary");
                    Console.WriteLine("2 > Scholarship");
                    Console.WriteLine("3 > Other");
                    Console.WriteLine("Select the category you want to add:");
                    var category = Console.ReadLine();
                    Console.WriteLine("choose date in format 'dd.mm.yyyy':");
                    var date = Console.ReadLine();

                    DateTime checkIncomeDate;
                    DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out checkIncomeDate);

                    IncomeType incomeCategory;
                    Enum.TryParse<IncomeType>(category, out incomeCategory);

                    Money money = new Money(value, 0);
                    result = service.AddOperation(new Income(money, checkIncomeDate), value, incomeCategory, date);
                    Console.ForegroundColor = result.Item1 ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine(result.Item2);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    break;

                case 2:
                    Console.WriteLine("Type value amount to be Added:[1.50]");
                    var expValue = Console.ReadLine();
                    Console.WriteLine("Choose category:");
                    Console.WriteLine("1 > Food");
                    Console.WriteLine("2 > Restaurant");
                    Console.WriteLine("3 > Medicine");
                    Console.WriteLine("4 > Sport");
                    Console.WriteLine("5 > Taxi");
                    Console.WriteLine("6 > Rent");
                    Console.WriteLine("7 > Inverstments");
                    Console.WriteLine("8 > Clothes");
                    Console.WriteLine("9 > Fan");
                    Console.WriteLine("10 > other");
                    Console.WriteLine("Select the category Index: ");
                    var expCategory = Console.ReadLine();
                    Console.WriteLine("choose date in format 'dd.mm.yyyy':");
                    var expdate = Console.ReadLine();

                    ExpenseType expenseCategory;
                    Enum.TryParse<ExpenseType>(expCategory, out expenseCategory);

                    DateTime checkDate;
                    DateTime.TryParseExact(expdate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out checkDate);

                    Money money2 = new Money(expValue, 0);
                    result = service.AddOperation(new Expense(money2, checkDate), expValue, expenseCategory, expdate);
                    Console.ForegroundColor = result.Item1 ? ConsoleColor.Green : ConsoleColor.Red;
                    Console.WriteLine(result.Item2);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();

                    break;

                case 3:
                    return;
                default:
                    break;
            }
        }
    }

}
