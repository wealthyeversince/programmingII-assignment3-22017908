using System;
using System.Collections.Generic;

namespace DCIT318.Assignment3
{
    // QUESTION 1: Finance Management System

    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"Bank Transfer processed: Amount = ${transaction.Amount:F2}, Category = {transaction.Category}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"Mobile Money processed: Amount = ${transaction.Amount:F2}, Category = {transaction.Category}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine(
                $"Crypto Wallet processed: Amount = ${transaction.Amount:F2}, Category = {transaction.Category}");
        }
    }

    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;

            Console.WriteLine(
                $"Transaction applied successfully. Updated balance: ${Balance:F2}");
        }
    }

    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance)
        {
        }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
            }
            else
            {
                Balance -= transaction.Amount;

                Console.WriteLine(
                    $"Transaction applied successfully. Updated balance: ${Balance:F2}");
            }
        }
    }

    public class FinanceApp
    {
        private List<Transaction> _transactions = new List<Transaction>();

        public void Run()
        {
            SavingsAccount account = new SavingsAccount("ACC001", 1000m);

            Transaction t1 = new Transaction(
                1, DateTime.Now, 150m, "Groceries");

            Transaction t2 = new Transaction(
                2, DateTime.Now, 200m, "Utilities");

            Transaction t3 = new Transaction(
                3, DateTime.Now, 100m, "Entertainment");

            Dictionary<string, ITransactionProcessor> processors =
                new Dictionary<string, ITransactionProcessor>
                {
                    { "Groceries", new MobileMoneyProcessor() },
                    { "Utilities", new BankTransferProcessor() },
                    { "Entertainment", new CryptoWalletProcessor() }
                };

            processors[t1.Category].Process(t1);
            processors[t2.Category].Process(t2);
            processors[t3.Category].Process(t3);

            account.ApplyTransaction(t1);
            account.ApplyTransaction(t2);
            account.ApplyTransaction(t3);

            _transactions.Add(t1);
            _transactions.Add(t2);
            _transactions.Add(t3);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            FinanceApp app = new FinanceApp();
            app.Run();
        }
    }
}