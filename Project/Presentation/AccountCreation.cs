public class AccountCreation
{
    
    public static void ChooseAccount()
    {
        Console.WriteLine("What type of account would you like to create?");
        Console.WriteLine("Choose the from the options below");
        Console.WriteLine("1)User");

        var userinput = Console.ReadLine();
        while(AccountsLogic.ParseInt(userinput) != 1)
        {
            Console.WriteLine("Input was incorrect. Try again");
            userinput = Console.ReadLine();
        }

        Console.WriteLine("Enter the information below");
        Console.WriteLine("Email: ");
        var userEmail = Console.ReadLine();
        while(AccountsLogic.VerifyEmail(userEmail) == false)
        {
                
            Console.WriteLine("Enter the information below");
            Console.WriteLine("Email: ");
            userEmail = Console.ReadLine();
        }
        Console.WriteLine("Enter your password. It must contain atleast 8 characters which consists of 1 capital letter, 1 number and 1 special character e.g. $,#,% etc.");
        var userPassword = Console.ReadLine();
        while(AccountsLogic.VerifyPassword(userPassword) == false)
        {
            Console.WriteLine("Password was not valid. Try again.");
            userPassword = Console.ReadLine();
        }
        Console.WriteLine("Your fullname: ");
        var fullName = Console.ReadLine();
        Console.WriteLine("Your age");
        var userAge = Console.ReadLine();
        while(AccountsLogic.IsInt(userAge) == false)
        {
            Console.WriteLine("Input is not valid. Please enter a number");
            userAge = Console.ReadLine();
        }

        AccountsLogic accountsLogic = new();
        accountsLogic.UpdateList(userEmail, userPassword, fullName, Convert.ToInt32(userAge));

        Console.WriteLine($"\nSuccessfully created your account, welcome {fullName}!");

        accountsLogic.CheckLogin(userEmail, userPassword);

        //wait so that it is more clear
        Thread.Sleep(1500);

        Console.WriteLine("\n");
        Menu.Start();
        // accountsLogic.UpdateList(new AccountModel(userEmail, userPassword, fullName));
            
        
        //    System.Console.WriteLine("2)Admin");
        //    System.Console.WriteLine("3)Finance");
        //    System.Console.WriteLine("4)Employee");
    }

    


}

