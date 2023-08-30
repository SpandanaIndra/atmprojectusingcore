namespace AtmProject_Core.Models
{
    public class Transactions
    {
        public int TransactionsId { get; set; }

        public int AccountNum { get; set; }
        public DateTime DateOfTransaction { get; set; }

        public string TransactionType { get; set;}
        public double Amount { get; set; }
        public double  AvailableBalance { get; set; }
    }
}
