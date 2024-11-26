using VipTest.Transactions.models;
using VipTest.Users.Models;
using VipTest.Utlity.Basic;

namespace VipTest.Wallets.Model;

public class Wallet:BaseEntity<Guid>
{
    
    public int? WalletCode { get; set; }
    
    public string? Name { get; set; }
    
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    public decimal Balance { get; set; } = 0;
    public decimal TotalIncome { get; set; } = 0;
    public decimal TotalExpense { get; set; } = 0;
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    
    public void AddTransaction(Transaction transaction)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        if (transaction.Amount < 0)
        {
            TotalExpense += transaction.Amount;
        }
        else
        {
            TotalIncome += transaction.Amount;
        }

        Balance += transaction.Amount;
        Transactions.Add(transaction);
    }
    
}