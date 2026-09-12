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
        {HealthSystemApp healthApp = new HealthSystemApp();

healthApp.SeedData();
healthApp.BuildPrescriptionMap();

Console.WriteLine("\n--- All Patients ---");
healthApp.PrintAllPatients();

Console.WriteLine("\n--- Prescriptions for Patient 1 ---");
healthApp.PrintPrescriptionsForPatient(1);
            FinanceApp app = new FinanceApp();
            app.Run();
        }
        
    }
}
// QUESTION 2: Healthcare System

public class Repository<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
    }

    public List<T> GetAll()
    {
         return items;
    }

    public T? GetById(Func<T, bool> predicate)
    {
       return items.FirstOrDefault(predicate);
    }

    public bool Remove(Func<T, bool> predicate)
    {
       T? item = items.FirstOrDefault(predicate);

if (item == null)
    return false;

items.Remove(item);
return true;
    }
}
public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }

    public Patient(int id, string name, int age, string gender)
    {
       Id = id;
Name = name;
Age = age;
Gender = gender;
    }
}
public class Prescription
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string MedicationName { get; set; }
    public DateTime DateIssued { get; set; }

    public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
    {
    Id = id;
PatientId = patientId;
MedicationName = medicationName;
DateIssued = dateIssued;
    }
}
public class HealthSystemApp
{
    private Repository<Patient> _patientRepo = new Repository<Patient>();
    private Repository<Prescription> _prescriptionRepo = new Repository<Prescription>();

    private Dictionary<int, List<Prescription>> _prescriptionMap =
        new Dictionary<int, List<Prescription>>();

    public void SeedData()
    {
        _patientRepo.Add(new Patient(1, "John Mensah", 25, "Male"));
        _patientRepo.Add(new Patient(2, "Ama Boateng", 30, "Female"));
        _patientRepo.Add(new Patient(3, "Kofi Asante", 40, "Male"));

        _prescriptionRepo.Add(
            new Prescription(1, 1, "Paracetamol", DateTime.Now));

        _prescriptionRepo.Add(
            new Prescription(2, 1, "Amoxicillin", DateTime.Now));

        _prescriptionRepo.Add(
            new Prescription(3, 2, "Ibuprofen", DateTime.Now));

        _prescriptionRepo.Add(
            new Prescription(4, 3, "Vitamin C", DateTime.Now));
    }

    public void BuildPrescriptionMap()
    {
        foreach (Prescription prescription in _prescriptionRepo.GetAll())
        {
            if (!_prescriptionMap.ContainsKey(prescription.PatientId))
            {
                _prescriptionMap[prescription.PatientId] =
                    new List<Prescription>();
            }

            _prescriptionMap[prescription.PatientId].Add(prescription);
        }
    }

    public List<Prescription> GetPrescriptionsByPatientId(int id)
    {
        if (_prescriptionMap.ContainsKey(id))
        {
            return _prescriptionMap[id];
        }

        return new List<Prescription>();
    }

    public void PrintAllPatients()
    {
        foreach (Patient patient in _patientRepo.GetAll())
        {
            Console.WriteLine(
                $"ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}");
        }
    }

    public void PrintPrescriptionsForPatient(int id)
    {
        List<Prescription> prescriptions =
            GetPrescriptionsByPatientId(id);

        foreach (Prescription prescription in prescriptions)
        {
            Console.WriteLine(
                $"Prescription ID: {prescription.Id}, Medication: {prescription.MedicationName}, Date: {prescription.DateIssued}");
        }
        WareHouseManager warehouse = new WareHouseManager();
warehouse.RunDemo();
    }
}
public interface IInventoryItem
{
    int Id { get; }
    string Name { get; }
    int Quantity { get; set; }
}

public class ElectronicItem : IInventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string Brand { get; set; }
    public int WarrantyMonths { get; set; }

    public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        Brand = brand;
        WarrantyMonths = warrantyMonths;
    }
}

public class GroceryItem : IInventoryItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }

    public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
    {
        Id = id;
        Name = name;
        Quantity = quantity;
        ExpiryDate = expiryDate;
    }
}public class DuplicateItemException : Exception
{
    public DuplicateItemException(string message) : base(message)
    {
    }
}

public class ItemNotFoundException : Exception
{
    public ItemNotFoundException(string message) : base(message)
    {
    }
}

public class InvalidQuantityException : Exception
{
    public InvalidQuantityException(string message) : base(message)
    {
    }
}
public class InventoryRepository<T> where T : IInventoryItem
{
    private Dictionary<int, T> _items = new Dictionary<int, T>();

    public void AddItem(T item)
    {
        if (_items.ContainsKey(item.Id))
            throw new DuplicateItemException("Item already exists.");

        _items.Add(item.Id, item);
    }

    public T GetItemById(int id)
    {
        if (!_items.ContainsKey(id))
            throw new ItemNotFoundException("Item not found.");

        return _items[id];
    }

    public void RemoveItem(int id)
    {
        if (!_items.ContainsKey(id))
            throw new ItemNotFoundException("Item not found.");

        _items.Remove(id);
    }

    public List<T> GetAllItems()
    {
        return new List<T>(_items.Values);
    }

    public void UpdateQuantity(int id, int quantity)
    {
        if (quantity < 0)
            throw new InvalidQuantityException("Quantity cannot be negative.");

        T item = GetItemById(id);
        item.Quantity = quantity;
    }
}
public class WareHouseManager
{
    private InventoryRepository<ElectronicItem> _electronics =
        new InventoryRepository<ElectronicItem>();

    private InventoryRepository<GroceryItem> _groceries =
        new InventoryRepository<GroceryItem>();

    public void SeedData()
    {
        _electronics.AddItem(
            new ElectronicItem(1, "Laptop", 10, "Dell", 24));

        _electronics.AddItem(
            new ElectronicItem(2, "Phone", 15, "Samsung", 12));

        _groceries.AddItem(
            new GroceryItem(3, "Milk", 20, DateTime.Now.AddDays(7)));

        _groceries.AddItem(
            new GroceryItem(4, "Bread", 30, DateTime.Now.AddDays(5)));
    }

    public void PrintAllItems<T>(InventoryRepository<T> repository)
        where T : IInventoryItem
    {
        foreach (T item in repository.GetAllItems())
        {
            Console.WriteLine(
                $"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}");
        }
    }

    public void IncreaseStock<T>(
        InventoryRepository<T> repository, int id, int amount)
        where T : IInventoryItem
    {
        T item = repository.GetItemById(id);
        item.Quantity += amount;
    }

    public void RemoveItemById<T>(
        InventoryRepository<T> repository, int id)
        where T : IInventoryItem
    {
        repository.RemoveItem(id);
    }

    public void RunDemo()
    {
        SeedData();

        Console.WriteLine("\n--- Electronics ---");
        PrintAllItems(_electronics);

        Console.WriteLine("\n--- Groceries ---");
        PrintAllItems(_groceries);

        Console.WriteLine("\n--- After Increasing Laptop Stock ---");
        IncreaseStock(_electronics, 1, 5);
        PrintAllItems(_electronics);

        try
        {
            _electronics.AddItem(
                new ElectronicItem(1, "Another Laptop", 5, "HP", 12));
        }
        catch (DuplicateItemException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        try
        {
            _groceries.RemoveItem(999);
        }
        catch (ItemNotFoundException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        try
        {
            _electronics.UpdateQuantity(1, -5);
        }
        catch (InvalidQuantityException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}