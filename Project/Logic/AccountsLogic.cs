using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Security;


//This class is not static so later on we can use inheritance and interfaces
public class AccountsLogic
{
    public static AccountsLogic Logic { get; } = new ();
    private List<AccountModel> _accounts;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    static public AccountModel? CurrentAccount { get; private set; }

    public AccountsLogic()
    {
        _accounts = AccountsAccess.LoadAll();
    }

    public static bool CheckForExistingEmail(string email)
    {
        foreach(AccountModel account in Logic._accounts)
        {
            if(account.EmailAddress == email)
            {
                return true;
            }
        }
        return false;
    }

    public AccountModel UpdateList(string email, string password, string fullname, int age)
    {
        
        AccountModel acc = new UserModel(FindFirstAvailableID(), email, password, fullname, age);
        //Find if there is already an model with the same id
        int index = _accounts.FindIndex(s => s.Id == acc.Id);

        if (index != -1)
        {
            //update existing model
            _accounts[index] = acc;
        }
        else
        {
            //add new model
            _accounts.Add(acc);
        }
        
        AccountsAccess.WriteAll(_accounts);

        return acc;

    }

    public AccountModel? GetById(int id)
    {
        return _accounts.Find(i => i.Id == id);
    }

    public AccountModel? CheckLogin(string? email, string? password)
    {
        if (email == null || password == null)
        {
            return null;
        }
        CurrentAccount = _accounts.Find(i => i.EmailAddress == email && i.Password == password);
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
        List<AccountModel> tempList = _accounts.OrderBy(a => a.Id).ToList<AccountModel>();
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
        var userToRemove = _accounts.FirstOrDefault(a => a.EmailAddress.ToLower() == email.ToLower());
        
        if (userToRemove != null)
        {
            _accounts.Remove(userToRemove);
            AccountsAccess.WriteAll(_accounts);
            return true;
        }
        return false;
    }
    
}
   





