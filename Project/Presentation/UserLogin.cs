public class UserLogin
{
    


    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine();
        AccountModel acc = AccountsLogic.Logic.CheckLogin(email, password);
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