static class LoginMenu
{
    // This shows the menu. You can call back to this method to show the menu again
    // after another presentation method is completed.
    // You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login as User");
        Console.WriteLine("Enter 2 to login as Admin");
        Console.WriteLine("Enter 3 to create an account");
        Console.WriteLine("Enter 4 to exit program");

        string input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start(); // Login als gebruiker
        }
        else if (input == "2")
        {
            AdminLogin.Start(); // Login als admin
        }
        else if (input == "3")
        {
            AccountCreation.ChooseAccount(); // Account aanmaken
        }
        else if (input == "4")
        {
            Environment.Exit(0); // Programma afsluiten
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start(); // Herstart het menu
        }
    }
}