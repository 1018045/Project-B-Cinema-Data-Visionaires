using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;


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

    public AccountModel GetById(int id)
    {
        return _accounts.Find(i => i.Id == id);
    }

    public AccountModel CheckLogin(string email, string password)
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
        if(password.Length < 8)
        {
            return false;
        }
        return true;


    } 

    public static bool VerifyEmail(string email)
    {
        var emailValidation = new EmailAddressAttribute();
        
        return emailValidation.IsValid(email);
    }

    // public static bool CheckForValidInput(int input)
    // {
    //     int userinput;
    //     bool validinput = false; 
    //     while(validinput == false || input != 1)
    //     {
    //       AccountCreation.ForFaultyInput();
    //       try
    //       {
    //         userinput = Convert.ToInt32(input);
    //       }
    //       catch
    //       {
    //         AccountCreation.ForFaultyInput();
    //         validinput = false;
    //       }
    //     }
    //     return validinput;
        
    // }
//    public static int ParseMethod(string userinput)
//    {
//         bool parseResult; 

//         int number; 

//         parseResult = int.TryParse(userinput, out number);

//         return number;

//    } 

     public static bool ParseAge(string userinput)
   {
        bool parseResult; 

        int number; 

        parseResult = int.TryParse(userinput, out number);

        return parseResult;
   } 
    
}
   
//  System.Console.WriteLine("Invalid input. Try Again");
           // userinput = Convert.ToInt32(Console.ReadLine());





