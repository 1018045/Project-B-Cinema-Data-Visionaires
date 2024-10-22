static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {

        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 3 to make a reservation");
        Console.WriteLine("Enter 2 To create an account");

        string input = Console.ReadLine();
        if (input == "1")
        {
            UserLogin.Start();
        }
        else if (input == "3")
        {
            Reservation.Make();
            AccountCreation.ChooseAccount();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }
    }
}