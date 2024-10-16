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
            System.Console.WriteLine("Invalid input. Try Again");
            break;
        }
        
            System.Console.WriteLine("Enter the information below");
            System.Console.WriteLine("Email: ");
            string userEmail = Console.ReadLine();
            System.Console.WriteLine("Password");
            string userPassword = Console.ReadLine();
            System.Console.WriteLine("Your fullname: ");
            string fullName = Console.ReadLine();
            System.Console.WriteLine("Your age");
            int userAge = Convert.ToInt32(Console.ReadLine());

            AccountsLogic accountsLogic = new();
            accountsLogic.UpdateList(userEmail, userPassword, fullName,userAge);
            // accountsLogic.UpdateList(new AccountModel(userEmail, userPassword, fullName));
            
        
        //    System.Console.WriteLine("2)Admin");
        //    System.Console.WriteLine("3)Finance");
        //    System.Console.WriteLine("4)Employee");
    }
}
