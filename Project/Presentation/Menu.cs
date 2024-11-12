static class Menu
{
    
    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.WriteLine("\n" + new string('-', Console.WindowWidth));
        System.Console.WriteLine($"You are logged in as {AccountsLogic.CurrentAccount.FullName}");
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine("Enter 1 to make a reservation");
        Console.WriteLine("Enter 2 to show your reservations");
        Console.WriteLine("Enter 3 to adjust your reservations");
        Console.WriteLine("Enter 4 to log out");

        string input = Console.ReadLine().Trim();
        if (input == "1")
        {
            Showings.ShowUpcoming();
            Start();
        }
        else if (input == "2")
        {
            Reservation.Show(AccountsLogic.CurrentAccount.Id);
        }
        else if (input == "3")
        {
            Reservation.Adjust(AccountsLogic.CurrentAccount.Id);
        }
        else if (input == "4")
        {
            AccountsLogic.LogOut();
            System.Console.WriteLine("\nYou are now logged out\n");
            LoginMenu.Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
    }
}