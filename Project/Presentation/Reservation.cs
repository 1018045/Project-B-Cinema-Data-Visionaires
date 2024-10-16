using Microsoft.VisualBasic;

public static class Reservation
{
    static private ReservationsLogic _reservationsLogic = new();

    public static void Make()
    {
        System.Console.WriteLine("Welcome to the reservation screen!");
        System.Console.WriteLine("Choose a showing from the list:");
        Showings.ShowAll();

        System.Console.WriteLine("Enter the id-number your showing of choice:");
        string id = Console.ReadLine();
        System.Console.WriteLine("Which seats do you want?");
        string seats = Console.ReadLine();
        System.Console.WriteLine("Enter your bank details:");
        bool paymentSuccesfull = _reservationsLogic.MakePayment(Console.ReadLine());

        _reservationsLogic.AddReservation(1, Convert.ToInt32(id), seats, paymentSuccesfull);
        
        Menu.Start();
    }
}