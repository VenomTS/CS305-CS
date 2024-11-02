using System.Text.Json.Serialization;

namespace BankingSystem;

[method: JsonConstructor]
public class Account(
    uint id,
    string fullName,
    string accountNumber,
    string pin,
    string mail,
    DateTime creationTime,
    decimal funds,
    bool isFrozen)
{
    private const decimal MaxAllowedDebt = 500; // Max amount that user can go into minus 

    public uint Id { get; set; } = id; // Represents the ID under which all information is stored
    
    public string FullName { get; set; } = fullName; // Name + Surname of the user
    public string Mail { get; set; } = mail;
    public string AccountNumber { get; set; } = accountNumber; // Represents 16-digit account number
    
    public string Pin { get; set; } = pin;

    public List<string> TransactionHistory { get; set; } = [];

    public decimal Funds { get; set; } = funds; // Available funds

    public bool IsFrozen { get; set; } = isFrozen; // If account is frozen, funds cannot be transfered to / from

    public DateTime CreationTime { get; set; } = creationTime;

    public Account(Account account) : this(account.Id, account.FullName, account.AccountNumber, account.Pin, account.Mail, account.CreationTime, account.Funds, account.IsFrozen)
    {
    }

    private bool CanWithdraw(decimal amount)
    {
        return amount <= Funds + MaxAllowedDebt;
    }

    public bool Withdraw(decimal amount)
    {
        if (!CanWithdraw(amount)) return false;
        TransactionHistory.Add($"Date: {DateTime.Now} | Operation: Withdraw | Amount: {amount}$");
        Funds -= amount;
        return true;
    }

    public bool Transfer(Account receiver, decimal amount)
    {
        if (!CanWithdraw(amount)) return false;
        TransactionHistory.Add($"Date: {DateTime.Now} | Operation: Transfer | Amount: {amount}$ | Destination: {receiver.AccountNumber}");
        receiver.TransactionHistory.Add($"Date: {DateTime.Now} | Operation: Transfer | Amount {amount}$ | From: {AccountNumber}");
        Funds -= amount;
        receiver.Funds += amount;
        return true;
    }

    public void Deposit(decimal amount)
    {
        TransactionHistory.Add($"Date: {DateTime.Now} | Operation: Deposit | Amount: {amount}$");
        Funds += amount;
    }

    public override string ToString()
    {
        return $"Owner: {FullName} | Mail: {Mail} | Account Number: {AccountNumber}";
    }
}