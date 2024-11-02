namespace BankingSystem;

public static class AccountNumberGenerator
{
    public static string GenerateAccountNumber()
    {

        var random = new Random();
        
        var result = "";
        result += random.Next(1, 10);
        for (int i = 1; i < 16; i++)
        {
            result += random.Next(0, 10);
        }

        return result;
    }
    
}