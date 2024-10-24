static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine();
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        if (acc != null)
        {
            System.Console.WriteLine("\n");
            Console.WriteLine(new string('-', Console.WindowWidth));
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