using System.Runtime.CompilerServices;

public class AccountCreation
{
    
    public static void ChooseAccount()
    {
        System.Console.WriteLine("What type of account would you like to create?");
        System.Console.WriteLine("Choose the from the options below");
        System.Console.WriteLine("1)User");
        int userinput = Convert.ToInt32(Console.ReadLine());
        // AccountsLogic.CheckForValidInput(userinput);
        
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
            System.Console.WriteLine("Your fullname: ");
            string fullName = Console.ReadLine();
            System.Console.WriteLine("Your age");
            string userAge = Console.ReadLine();
            
            // int AgeConverted = AccountsLogic.ParseMethod(userAge);
           

            AccountsLogic accountsLogic = new();
            // accountsLogic.UpdateList(userEmail, userPassword, fullName,userAge);
            // accountsLogic.UpdateList(new AccountModel(userEmail, userPassword, fullName));
            
        
        //    System.Console.WriteLine("2)Admin");
        //    System.Console.WriteLine("3)Finance");
        //    System.Console.WriteLine("4)Employee");
    }

    // public static int ForFaultyInput(bool result)
    // {
    //     if(result == false)
    //     {
    //         System.Console.WriteLine("Invalid input. Try Again");
    //         int userinput = Convert.ToInt32(Console.ReadLine());
    //         return userinput;
    //     }
    // }

    public void PasswordMessage()
{
    System.Console.WriteLine("Password must contain atleast 8 characters");
}

}

