public static class Reservation
{
    static private ReservationsLogic _reservationsLogic = new();

    public static void Make()
    {
        System.Console.WriteLine("\nUpcoming showings:");
        Showings.ShowUpcoming();

        System.Console.WriteLine("Select a showing (Enter the number in front of the showing to continue)");
        string id = Console.ReadLine();
        System.Console.WriteLine("\nWhich seats do you want?");
        // communicate with the seating layer to recieve, print, and then send back the seating config
        string seats = Console.ReadLine();
        string payment = "X";
        while (payment != "")
        {
            System.Console.WriteLine("\nBank details:");
            payment = _reservationsLogic.ValidateBankDetails(Console.ReadLine());
        }

        _reservationsLogic.AddReservation(1, Convert.ToInt32(id), seats, true);
        System.Console.WriteLine("\nYou have succesfully booked your tickets!\n");
        Menu.Start();
    }
}