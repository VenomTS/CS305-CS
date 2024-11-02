namespace BankingSystem;

class Program
{
    static void Main(string[] args)
    {
        var database = new JsonDatabase("Database.json");

        string[][] optionSets =
        [
            ["Create Account", "Login", "View All Accounts", "Exit"],
            ["(Un)Freeze Account", "Deposit/Withdraw/Transfer Funds", "View Transaction History", "Back"], // After Login
            ["Deposit Funds", "Withdraw Funds", "Transfer Funds", "Back"] // Deposit / Withdraw Funds
        ];

        var currOptionSet = 0;

        Account? selectedAccount = null;

        while (true)
        {
            var option = SelectOption(optionSets[currOptionSet]);

            if (option == 3)
            {
                currOptionSet -= 1;
                if (currOptionSet < 0) break;
            }
            else if (option == 0) // Create Account, Freeze Account or Deposit Funds - DONE
            {
                if (currOptionSet == 0) // Create Account
                    CreateAccount(database);
                
                else if (currOptionSet == 1) // Freeze / Unfreeze Account
                    FreezeAccount(selectedAccount);
                
                else if (currOptionSet == 2) // Deposit Funds
                    DepositFunds(selectedAccount);
            }
            else if (option == 1) // Login, Deposit/Withdraw/Transfer Funds or Withdraw Funds - DONE
            {
                if (currOptionSet == 0) // Login
                {
                    selectedAccount = Login(database);
                    if(selectedAccount == null) continue;
                    currOptionSet += 1;
                }
                
                else if (currOptionSet == 1) // Deposit/Withdraw/Transfer
                {
                    currOptionSet += 1;
                }
                
                else if (currOptionSet == 2) // Withdraw Funds
                {
                    WithdrawFunds(selectedAccount);
                }
            }
            else if (option == 2) // View All Accounts, View Transaction History or Transfer Funds
            {
                if (currOptionSet == 0) // View All Accounts
                    DisplayAllAccounts(database);
                else if (currOptionSet == 1) // View Transaction History
                    ViewTransactionHistory(selectedAccount);
                else if (currOptionSet == 2) // Transfer Funds
                {
                    TransferFunds(selectedAccount, database);
                }
            }
        }
        
        database.SaveAccounts();
    }

    private static void CreateAccount(IDatabase database)
    {
        Console.Clear();
        Console.WriteLine("You chose to create a new account!\nIf you want to cancel, write 'cancel' as your name!");
        Console.Write("Write your Full Name: ");
        var fullName = Console.ReadLine();
        if (fullName != null && fullName.ToLower().Equals("cancel")) return;
        Console.Write("Write your E-Mail Address: ");
        var mail = Console.ReadLine();
        Console.Write("Enter your desired Pin: ");
        var pin = Console.ReadLine();

        if (pin is null || !IsPinValid(pin))
        {
            Console.WriteLine("Invalid PIN");
            GoBack();
            return;
        }

        if (fullName is null || mail is null)
        {
            Console.WriteLine("Name and Mail must be valid!");
            GoBack();
            return;
        }
        
        var account = database.CreateAccount(fullName, mail, pin);

        if (account == null)
        {
            Console.WriteLine($"There is already account under the name of {fullName}");
            GoBack();
            return;
        }
        
        Console.WriteLine($"Thank you {fullName}!\nYou successfully created an account\nYour account number is: {account.AccountNumber}");
        database.SaveAccounts();
        GoBack();
    }

    private static Account? Login(IDatabase database)
    {
        Console.Clear();
        Console.Write("Please enter your account number: ");
        var accountNumber = Console.ReadLine();
        Console.Write("Please enter your pin: ");
        var pin = Console.ReadLine();

        if (accountNumber is null || pin is null)
        {
            Console.WriteLine("Name and Pin must be valid!");
            GoBack();
            return null;
        }

        var account = database.GetAccount(accountNumber, pin);

        if (account is null)
        {
            Console.WriteLine("There is no account matching your account number and pin!");
            GoBack();
            return null;
        }
        
        Console.WriteLine("You have successfully logged in!");
        var str = $"Owner: {account.FullName} | Account Number: {account.AccountNumber} | Funds: {account.Funds}";
        Console.WriteLine(str);
        GoBack();
        return account;
    }

    private static void WithdrawFunds(Account? account)
    {
        if (account is null) return; // Impossible but to make IDE happy
        Console.Clear();

        if (account.IsFrozen)
        {
            Console.WriteLine("Your account is frozen, thus you cannot withdraw funds!");
            GoBack();
            return;
        }
        
        Console.Write("Enter the amount you want to withdraw (Format: XX.XX): ");
        var amount = Console.ReadLine();
        if (amount is null)
        {
            Console.WriteLine("Incorrect amount!");
            GoBack();
            return;
        }
        try
        {
            var decimalAmount = decimal.Parse(amount);
            if (!account.Withdraw(decimalAmount))
            {
                Console.WriteLine($"You do not have enough funds in your account ({account.Funds}$)");
                GoBack();
                return;
            }
            Console.WriteLine($"You successfully withdrew {decimalAmount}$ from your account!\n" +
                              $"Your new account balance is: {account.Funds}$");
            GoBack();
        }
        catch (FormatException)
        {
            Console.WriteLine("You need to enter the amount in form XX.XX!");
            GoBack();
        }
    }

    private static void DepositFunds(Account? account)
    {
        if (account is null) return; // STOP CRYING
        Console.Clear();
        
        if (account.IsFrozen)
        {
            Console.WriteLine("Your account is frozen, thus you cannot deposit funds!");
            GoBack();
            return;
        }
        
        Console.Write("Enter the amount you want to deposit (Format: XX.XX): ");
        var amount = Console.ReadLine();
        if (amount is null)
        {
            Console.WriteLine("Incorrect amount!");
            GoBack();
            return;
        }
        try
        {
            var decimalAmount = decimal.Parse(amount);
            account.Deposit(decimalAmount);
            Console.WriteLine($"You successfully deposited {decimalAmount}$ to your account!\n" +
                              $"Your new account balance is: {account.Funds}$");
            GoBack();
        }
        catch (FormatException)
        {
            Console.WriteLine("You need to enter the amount in form XX.XX!");
            GoBack();
        }
    }

    private static void TransferFunds(Account? account, IDatabase database)
    {
        if (account is null) return; // I ran out of jokes here
        Console.Clear();
        
        if (account.IsFrozen)
        {
            Console.WriteLine("Your account is frozen, thus you cannot transfer funds!");
            GoBack();
            return;
        }
        
        Console.Write("Enter the Account Number of the person who you want to send money to: ");
        var receiver = Console.ReadLine();
        if (receiver is null)
        {
            Console.WriteLine("Invalid Account Number for receiver!");
            GoBack();
            return;
        }

        var receiverAccount = database.GetAccount(receiver);

        if (receiverAccount is null || receiverAccount.IsFrozen)
        {
            Console.WriteLine("Account Number you entered is either frozen or invalid!");
            GoBack();
            return;
        }
        
        Console.Write("Enter the amount you want to transfer (Format: XX.XX): ");
        var amount = Console.ReadLine();
        if (amount is null)
        {
            Console.WriteLine("Incorrect amount!");
            GoBack();
            return;
        }
        try
        {
            var decimalAmount = decimal.Parse(amount);
            
            if(!account.Transfer(receiverAccount, decimalAmount))
            {
                Console.WriteLine($"You do not have enough funds in your account ({account.Funds}$)");
                GoBack();
                return;
            }

            Console.WriteLine($"You successfully transferred {decimalAmount}$ from your account to {receiver}!\n" +
                              $"Your new account balance is: {account.Funds}$");
            GoBack();
        }
        catch (FormatException)
        {
            Console.WriteLine("You need to enter the amount in form XX.XX!");
            GoBack();
        }
        
    }

    private static void FreezeAccount(Account? account)
    {
        if (account is null) return; // Happy Compiler, Happy Life
        Console.Clear();
        var str = $"Your account is currently {(account.IsFrozen ? "Frozen" : "Active")}\n" +
                  $"Do you want to {(account.IsFrozen ? "re-activate" : "freeze")} it?\n" +
                  $"Write yes to confirm / no to deny";
        Console.WriteLine(str);
        var answer = Console.ReadLine();
        if (answer is null || answer.ToLower().Equals("no")) return;
        account.IsFrozen = !account.IsFrozen;
        Console.WriteLine($"You successfully {(account.IsFrozen ? "froze" : "re-activated")} your account!");
        GoBack();
    }

    private static void ViewTransactionHistory(Account? account)
    {
        if (account is null) return; // PLEASE STOP
        Console.Clear();
        var transactionHistory = account.TransactionHistory;
        foreach (var transaction in transactionHistory)
        {
            Console.WriteLine(transaction);
        }
        GoBack();
    }

    private static void DisplayAllAccounts(IDatabase database)
    {
        Console.Clear();
        var accounts = database.GetAccounts();
        var str = accounts.Aggregate("", (current, acc) => current + (acc + "\n"));
        if (str.Length == 0)
        {
            Console.WriteLine("There are no created accounts!");
            GoBack();
            return;
        }
        Console.WriteLine($"List of ALL ACCOUNTS: \n{str}");
        GoBack();
    }

    private static int SelectOption(string[] options)
    {
        options = EvenlySpaceOptions(options);
        var selectedOption = 0;
        
        Console.Clear();
        PrintOptions(selectedOption, options);
        
        while (true)
        {
            var pressedKey = Console.ReadKey(true).Key;

            if (pressedKey == ConsoleKey.LeftArrow)
                selectedOption = selectedOption > 0 ? selectedOption - 1 : 3;
            else if (pressedKey == ConsoleKey.RightArrow)
                selectedOption = (selectedOption + 1) % 4;
            else if (pressedKey == ConsoleKey.Enter)
                return selectedOption;
            else continue;
            
            Console.Clear();
            PrintOptions(selectedOption, options);
        }
    }

    private static void PrintOptions(int selectedOption, string[] options)
    {
        for (var row = 0; row < 5; row++)
        {
            PrintRow(row, options[0], selectedOption == 0);
            PrintRow(row, options[1], selectedOption == 1);
            PrintRow(row, options[2], selectedOption == 2);
            PrintRow(row, options[3], selectedOption == 3);
            Console.WriteLine("");
        }
    }

    private static void PrintRow(int row, string option, bool isSelected)
    {
        var selectedChar = '#';
        var optionLength = option.Length;

        Console.ForegroundColor = ConsoleColor.Red;
        if (isSelected) Console.ForegroundColor = ConsoleColor.Green;
        
        switch (row)
        {
            // First and Last Row
            case 0 or 4:
            {
                var str = "";
                for (var col = 0; col < optionLength + 6; col++)
                    str += selectedChar;
                Console.Write(str + " ");
                break;
            }
            // Empty Rows
            case 1 or 3:
            {
                var str = selectedChar + "  ";
                for (var i = 0; i < option.Length; i++)
                    str += " ";
                str += "  " + selectedChar;
                Console.Write(str + " ");
                break;
            }
            default:
                Console.Write(selectedChar + "  " + option + "  " + selectedChar + " ");
                break;
        }

        Console.ForegroundColor = ConsoleColor.White;
    }

    private static string[] EvenlySpaceOptions(string[] options)
    {
        var longest = options.Select(option => option.Length).Max();

        var lastOp = false; // false means left side, true means right side

        var currOp = 0;

        while (currOp < 4)
        {
            while (options[currOp].Length < longest)
            {
                if (!lastOp) options[currOp] = " " + options[currOp];
                else options[currOp] += " ";
                lastOp = !lastOp;
            }

            currOp += 1;
            lastOp = false;
        }
        
        return options;
    }

    private static bool IsPinValid(string pin)
    {
        if (pin.Length != 4) return false;
        char[] allowedChars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
        return pin.All(letter => allowedChars.Contains(letter));
    }

    private static void GoBack()
    {
        Console.WriteLine("Press any key to go back to the menu");
        Thread.Sleep(1000); // Sleep one second just for security
        Console.ReadKey(true);
    }
}