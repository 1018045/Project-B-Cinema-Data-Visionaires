using System.Security;
public class UserLogin
{
    


    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");

        SecureString pass = AccountsLogic.MaskInputstring();  
        string Password = new System.Net.NetworkCredential(string.Empty, pass).Password; 

        AccountModel acc = AccountsLogic.Logic.CheckLogin(email, Password);
        if (acc != null)
        {
            Console.WriteLine("Welcome back " + acc.FullName);
            Console.WriteLine("Your email is " + acc.EmailAddress);

            //Write some code to go back to the menu
            Menu.Start();
        }
        else
        {
            Console.WriteLine("No account found with that email and password");
            LoginMenu.Start();
        }
    }
}