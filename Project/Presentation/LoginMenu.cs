static class LoginMenu
{
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login as User");
        Console.WriteLine("Enter 2 to login as Admin");
        Console.WriteLine("Enter 3 to create an account");
        Console.WriteLine("Enter 4 to login as Accountant");
        Console.WriteLine("Enter 5 to exit program");

        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                UserLogin.Start();
                break;
            case "2":
                AdminLogin.Start();
                break;
            case "3":
                AccountCreation.ChooseAccount();
                break;
            case "4":
                AccountantMenu.Start();
                break;
            case "5":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid input");
                Start();
                break;
        }
    }
}