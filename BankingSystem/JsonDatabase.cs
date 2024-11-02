using System.Text.Json;

namespace BankingSystem;

public class JsonDatabase : IDatabase
{
    private List<Account> _accounts;

    private readonly string _filePath;

    private const decimal NewAccountStartingFunds = 0;

    private const bool NewAccountStartsFrozen = false;

    public JsonDatabase(string filePath)
    {
        _accounts = [];
        _filePath = filePath;
        CreateDatabase(_filePath);
        PopulateAccounts(_filePath);
    }

    private static void CreateDatabase(string filePath)
    {
        if (File.Exists(filePath)) return;
        File.WriteAllText(filePath, "[]");
    }

    private void PopulateAccounts(string filePath)
    {
        var jsonData = File.ReadAllText(filePath);
        var data = JsonSerializer.Deserialize<List<Account>>(jsonData);
        _accounts = data ?? throw new ApplicationException("Error deserializing data from Json file!");
    }
    
    public uint GetFirstAvailableId()
    {
        var allIds = new HashSet<uint>(_accounts.Select(user => user.Id));

        uint fId = 1;
        while (allIds.Contains(fId)) 
            fId += 1;
        
        // Can be done using LINQ, however limit is INT (Max Value = 2^31-1) - UINT (Max Value = 2^32)
        
        return fId;
    }

    public bool IsAccountNumberAvailable(string accountNumber)
    {
        return _accounts.All(account => account.AccountNumber != accountNumber);
    }

    public List<Account> GetAccounts()
    {
        return _accounts.Select(account => new Account(account)).ToList();
    }

    public Account? CreateAccount(string fullName, string mail, string pin)
    {
        
        if (_accounts.Any(acc => acc.FullName.Equals(fullName))) return null;
        
        var id = GetFirstAvailableId();
        var accountNumber = AccountNumberGenerator.GenerateAccountNumber();
        while (!IsAccountNumberAvailable(accountNumber))
            accountNumber = AccountNumberGenerator.GenerateAccountNumber();
        var currTime = DateTime.Now;
        
        var account = new Account(id, fullName, accountNumber, pin, mail, currTime, NewAccountStartingFunds, NewAccountStartsFrozen);
        
        _accounts.Add(account);

        return account;
    }

    public void SaveAccounts()
    {
        var data = JsonSerializer.Serialize(_accounts);
        File.WriteAllText(_filePath, data);
    }

    public Account? GetAccount(string accountNumber)
    {
        return _accounts.FirstOrDefault(account => account.AccountNumber == accountNumber);
    }
    
    public Account? GetAccount(string accountNumber, string pin)
    {
        return _accounts.FirstOrDefault(account => account.AccountNumber == accountNumber && account.Pin == pin);
    }
}