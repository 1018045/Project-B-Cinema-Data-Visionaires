using Microsoft.VisualBasic;
using Project.Presentation;

public static class Reservation
{
    private static readonly ReservationsLogic ReservationsLogic = new();

    // Remove during code factoring bc of bad code practice
    private static readonly ShowingsLogic ShowingsLogic = new ();

    public static void Make()
    {
        Console.WriteLine("\nUpcoming showings:\n");
        Showings.ShowUpcoming(makingReservation: true);

        Console.WriteLine("Select a showing (Enter the number in front of the showing to continue)");

        var id = Console.ReadLine();
        var showingId = Convert.ToInt32(id);

        var selectedSeats = SeatingPresentation.Present(showingId);
        
        string payment = "X";
        while (payment != "")
        {
            Console.WriteLine("\nBank details:");
            payment = ReservationsLogic.ValidateBankDetails(Console.ReadLine()!);
            Console.WriteLine(payment);
        }

        //add the all the reserved seats
        ReservationsLogic.AddReservation(AccountsLogic.CurrentAccount.Id, showingId, string.Join(",", selectedSeats), true);

        Console.WriteLine("\nYou have successfully booked your tickets!\n");
        Menus.LoggedInMenu();
    }

    public static void Show(int userId)
    {
        Console.WriteLine("These are your current reservations:");
        List<ReservationModel> reservations = ReservationsLogic.ShowAllUserReservations(userId);
        int counter = 1;
        if (reservations.Count == 0)
        {
            Console.WriteLine("You have 0 reservations!");
            
        }
        else
        {
            foreach (ReservationModel reservation in reservations)
            {
                Console.WriteLine($"{counter++}. {ShowingsLogic.FindShowingById(reservation.ShowingId)}");
            }
        }
        Menus.LoggedInMenu();
    }

    public static void Adjust(int userId)
    {
        Console.WriteLine("These are your current reservations:");
        List<ReservationModel> reservations = ReservationsLogic.ShowAllUserReservations(userId);

        if (reservations.Count == 0)
        {
            Console.WriteLine("You have 0 reservations!");
            Menus.LoggedInMenu();
        }
        else
        {
            int counter = 1;
            foreach (ReservationModel reservation in reservations)
            {
                Console.WriteLine($"{counter++}. {ShowingsLogic.FindShowingById(reservation.ShowingId)}");
            }
            Console.WriteLine("Which reservation would you like to change?");
            string userChoice;
            do
            {
                userChoice = Console.ReadLine();
            } while(!AccountsLogic.IsInt(userChoice) || AccountsLogic.ParseInt(userChoice) > counter - 1 || AccountsLogic.ParseInt(userChoice) < 1);

            ShowingModel showing = ShowingsLogic.FindShowingByIdReturnShowing(reservations[AccountsLogic.ParseInt(userChoice) - 1].ShowingId);
            // Console.WriteLine($"Reservation:\n{showing.Id}");
            AdjustmentMenu(reservations[AccountsLogic.ParseInt(userChoice) - 1], showing.Title);
        }
    }

    private static void AdjustmentMenu(ReservationModel reservation, string showing)
    {
        Console.WriteLine("What would you like to adjust?");
        Console.WriteLine("1. Change or add seats (NOT YET IMPLEMENTED!!!)"); // Implement after youri's part
        Console.WriteLine("2. Change date");
        // System.Console.WriteLine("3. Add extra's");
        Console.WriteLine("4. Remove reservation");
        Console.WriteLine("5. Cancel");
        
        string userChoice = Console.ReadLine();

        switch(userChoice)
        {
            case "1":
                Console.WriteLine("(NOT YET IMPLEMENTED!!!)");
                break;
            case "2":
                ChangeReservationDate(reservation, showing);
                break;
            case "3":
                Console.WriteLine("(NOT YET IMPLEMENTED!!!)");
                break;
            case "4":
                ReservationsLogic.RemoveReservation(reservation);
                Console.WriteLine("Your reservation has been removed.");
                break;
            case "5":
                Console.WriteLine("Editing your reservation has been cancelled.");
                break;
            default:
                Console.WriteLine("Invalid input, try again!");
                AdjustmentMenu(reservation, showing);
                break;
        }
        Menus.LoggedInMenu();
    }


    // CHANGE SEAT IMPLEMENTATION AFTER YOURI IS DONE
    private static void ChangeReservationDate(ReservationModel oldReservation, string movieTitle)
    {
        List<ShowingModel> upcomingShowings = ShowingsLogic.GetUpcomingShowingsOfMovie(movieTitle);
        if (upcomingShowings.Count == 0)
        {
            Console.WriteLine("There are no available shows planned, please cancel your reservation if you are unavailable at that time.");
        }
        else
        {
            Console.WriteLine("Which date do you want to change your reservation to?");
            int counter = 1;
            foreach (ShowingModel showing in upcomingShowings)
            {
                Console.WriteLine($"{counter++}: {showing.Date}");
            }
            string userChoice;
            do
            {
                userChoice = Console.ReadLine();
            } while(!AccountsLogic.IsInt(userChoice) || AccountsLogic.ParseInt(userChoice) > counter - 1 || AccountsLogic.ParseInt(userChoice) < 1);
            

            ReservationsLogic.RemoveReservation(oldReservation);
            ShowingModel newShowing = upcomingShowings[AccountsLogic.ParseInt(userChoice) - 1];
            ReservationsLogic.AddReservation(oldReservation.UserId, newShowing.Id, oldReservation.Seats, true);
            Console.WriteLine($"The date of your reservation has been succesfully changed to: {newShowing.Date}");
        }

        Menus.LoggedInMenu();
    }
}