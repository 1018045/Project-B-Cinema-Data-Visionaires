using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Text.RegularExpressions;


//This class is not static so later on we can use inheritance and interfaces
namespace Project.Logic.Account;

public class AccountsLogic
{
    public static AccountsLogic Logic { get; } = new ();
    protected List<AccountModel> Accounts { get; init; }

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    static public AccountModel? CurrentAccount { get; private set; }

    public AccountsLogic()
    {
        Accounts = AccountsAccess.LoadAll();
    }

    public static bool CheckForExistingEmail(string email) => Logic.Accounts.Any(account => account.EmailAddress == email);

    public AccountModel UpdateList(string email, string password, string fullname, int age)
    {

        AccountModel acc = new UserModel(FindFirstAvailableID(), email, password, fullname, age);
        //Find if there is already an model with the same id
        int index = Accounts.FindIndex(s => s.Id == acc.Id);

        if (index != -1)
        {
            //update existing model
            Accounts[index] = acc;
        }
        else
        {
            //add new model
            Accounts.Add(acc);
        }

        AccountsAccess.WriteAll(Accounts);

        return acc;

    }

    public AccountModel? GetById(int id)
    {
        return Accounts.Find(i => i.Id == id);
    }

    public AccountModel? CheckLogin(string? email, string? password)
    {
        Accounts.Clear();
        Accounts.AddRange(AccountsAccess.LoadAll());

        if (email == null || password == null)
        {
            return null;
        }
        CurrentAccount = Accounts.Find(i => i.EmailAddress == email && i.Password == password);
        return CurrentAccount;
    }

    public static bool VerifyPassword(string password)
    {
        if (password.Length < 8)
        {
            return false;
        }

        string pattern = @"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";


        return Regex.IsMatch(password, pattern);
    }

    public static bool VerifyEmail(string email)
    {
        var emailValidation = new EmailAddressAttribute();
        return emailValidation.IsValid(email);
    }

    //helper method to test if the age is valid
    public static bool IsInt(string userinput)
    {
        if (int.TryParse(userinput, out int age))
        {
            if(age >= 0 && age <= 166)
            {
                return true;
            }
        }
        return false;
    }

    //returns -1 if the result is not a success
    public static int ParseInt(string input)
    {
        var success = int.TryParse(input, out var number);
        return !success ? -1 : number;
    }

    private int FindFirstAvailableID()
    {
        int pointer = 0;
        List<AccountModel> tempList = Accounts.OrderBy(a => a.Id).ToList();
        foreach (AccountModel account in tempList)
        {
            if (pointer != account.Id)
            {
                return pointer;
            }
            pointer++;
        }
        return pointer;
    }

    public static void LogOut()
    {
        CurrentAccount = null;
    }

    public static SecureString MaskInputstring()
    {
        SecureString pass = new SecureString();
        ConsoleKeyInfo keyInfo;

        do
        {
            keyInfo = Console.ReadKey(true);


            if (!char.IsControl(keyInfo.KeyChar))
            {
                pass.AppendChar(keyInfo.KeyChar);
                Console.Write("*");
            }

            else if (keyInfo.Key == ConsoleKey.Backspace && pass.Length > 0)
            {

                pass.RemoveAt(pass.Length - 1);
                Console.Write("\b \b");
            }

        } while (keyInfo.Key != ConsoleKey.Enter);

        Console.WriteLine();
        return pass;
    }

    public bool RemoveUser(string email)
    {
        var userToRemove = Accounts.FirstOrDefault(a =>
            string.Equals(a.EmailAddress, email, StringComparison.OrdinalIgnoreCase));

        if (userToRemove == null)
            return false;

        Accounts.Remove(userToRemove);
        AccountsAccess.WriteAll(Accounts);
        return true;
    }

}