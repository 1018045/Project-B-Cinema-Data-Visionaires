using System.Globalization;
using System.Runtime.InteropServices;
static class LoginMenu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {

        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to create an account");
        Console.WriteLine("Enter 3 to show upcoming movie showings");
        Console.WriteLine("Enter 4 to show upcoming movie showings on a specific date");
        Console.WriteLine("Enter 5 to exit program");

        string input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if( input == "2")
        {
            AccountCreation.ChooseAccount();
        }
        else if( input == "3")
        {
            Showings.ShowUpcoming();
            Start();
        }
        else if( input == "4")
        {
            Showings.ShowUpcomingOnDate();
            Start();
        }
        else if (input == "5")
        {
            Environment.Exit(0);
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
    }
}