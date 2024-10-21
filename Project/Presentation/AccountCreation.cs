using System.Runtime.CompilerServices;

public class AccountCreation
{
    
    public static void ChooseAccount()
    {
        System.Console.WriteLine("What type of account would you like to create?");
        System.Console.WriteLine("Choose the from the options below");
        System.Console.WriteLine("1)User");
        int userinput = Convert.ToInt32(Console.ReadLine());
        while(userinput != 1)
        {
            System.Console.WriteLine("Input was incorrect. Try again");
            userinput = Convert.ToInt32(Console.ReadLine());
        }
   
        
            System.Console.WriteLine("Enter the information below");
            System.Console.WriteLine("Email: ");
            string userEmail = Console.ReadLine();
            while(AccountsLogic.VerifyEmail(userEmail) == false)
            {
                
                System.Console.WriteLine("Enter the information below");
                System.Console.WriteLine("Email: ");
                userEmail = Console.ReadLine();
            }
            System.Console.WriteLine("Password");
            string userPassword = Console.ReadLine();
            while(AccountsLogic.VerifyPassword(userPassword) == false)
            {
                System.Console.WriteLine("Password needs to be atleast 8 characters. Try again.");
                userPassword = Console.ReadLine();
            }
            System.Console.WriteLine("Your fullname: ");
            string fullName = Console.ReadLine();
            System.Console.WriteLine("Your age");
            string userAge = Console.ReadLine();
            while(AccountsLogic.ParseAge(userAge)== false)
            {
                System.Console.WriteLine("Input is not valid. Please enter a number");
                userAge = Console.ReadLine();
            }

            AccountsLogic accountsLogic = new();
            accountsLogic.UpdateList(userEmail, userPassword, fullName,Convert.ToInt32(userAge));
            // accountsLogic.UpdateList(new AccountModel(userEmail, userPassword, fullName));
            
        
        //    System.Console.WriteLine("2)Admin");
        //    System.Console.WriteLine("3)Finance");
        //    System.Console.WriteLine("4)Employee");
    }

    


}

