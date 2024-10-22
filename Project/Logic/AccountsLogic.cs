using System.ComponentModel.DataAnnotations;


//This class is not static so later on we can use inheritance and interfaces
public class AccountsLogic
{
    private List<AccountModel> _accounts;

    

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    static public AccountModel? CurrentAccount { get; private set; }

    public AccountsLogic()
    {
        _accounts = AccountsAccess.LoadAll();
    }

    public void UpdateList(string email, string password, string fullname, int age)
    {
        int id = 13;
        AccountModel acc = new AccountModel(13, email, password, fullname, age);
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
        return password.Length >= 8;
    } 

    public static bool VerifyEmail(string email)
    {
        var emailValidation = new EmailAddressAttribute();
        return emailValidation.IsValid(email);
    }

    //helper method to test if the age is valid
    public static bool IsInt(string userinput)
    {
        return int.TryParse(userinput, out _);
   }

   //returns -1 if the result is not a success
   public static int ParseInt(string input)
   {
        var success = int.TryParse(input, out var number);
        return !success ? -1 : number;
   }
    
}
   





