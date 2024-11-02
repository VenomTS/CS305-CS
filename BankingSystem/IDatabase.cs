namespace BankingSystem;

public interface IDatabase
{
    List<Account> GetAccounts();
    Account? GetAccount(string accountNumber, string pin);
    Account? GetAccount(string accountNumber);
    Account? CreateAccount(string fullName, string mail, string pin);
    
    void SaveAccounts();
}