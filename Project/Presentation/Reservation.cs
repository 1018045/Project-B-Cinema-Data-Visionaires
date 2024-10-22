public static class Reservation
{
    static private ReservationsLogic _reservationsLogic = new();

    public static void Make()
    {
        System.Console.WriteLine("Upcoming showings:");
        Showings.ShowUpcoming();

        System.Console.WriteLine("Upcoming showings (Enter the number in front to continue):");
        string id = Console.ReadLine();
        System.Console.WriteLine("Which seats do you want?");
        // communicate with the seating layer to recieve, print, and then send back the seating config
        string seats = Console.ReadLine();
        bool paymentSuccesfull = false;
        while (!paymentSuccesfull)
        {
            System.Console.WriteLine("Bank details:");
            paymentSuccesfull = _reservationsLogic.ValidateBankDetails(Console.ReadLine());
        }

        _reservationsLogic.AddReservation(1, Convert.ToInt32(id), seats, paymentSuccesfull);
        
        Menu.Start();
    }
}