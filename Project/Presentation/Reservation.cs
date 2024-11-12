public static class Reservation
{
    static private ReservationsLogic _reservationsLogic = new();

    // Remove during code factoring bc of bad code practice
    static private ShowingsLogic _showingsLogic = new ShowingsLogic();

    public static void Make()
    {
        System.Console.WriteLine("\nUpcoming showings:\n");
        Showings.ShowUpcoming(makingReservation: true);

        System.Console.WriteLine("Select a showing (Enter the number in front of the showing to continue)");

        string id = Console.ReadLine();
        System.Console.WriteLine("\nWhich seats do you want?");
        // communicate with the seating layer to recieve, print, and then send back the seating config
        string seats = Console.ReadLine();
        // communicate with the seating layer to recieve, print, and then send back the seating config
        
        string payment = "X";
        while (payment != "")
        {
            System.Console.WriteLine("\nBank details:");
            payment = _reservationsLogic.ValidateBankDetails(Console.ReadLine());
            Console.WriteLine(payment);
        }

        _reservationsLogic.AddReservation(1, Convert.ToInt32(id), seats, true);
        System.Console.WriteLine("\nYou have succesfully booked your tickets!\n");
        Menu.Start();
    }

    public static void Show(int userId)
    {
        System.Console.WriteLine("These are your current reservations:");
        List<ReservationModel> reservations = _reservationsLogic.ShowAllUserReservations(userId);
        int counter = 1;
        if (reservations.Count == 0)
        {
            System.Console.WriteLine("You have 0 reservations!");
            
        }
        else
        {
            foreach (ReservationModel reservation in reservations)
            {
                System.Console.WriteLine($"{counter++}. {_showingsLogic.FindShowingById(reservation.ShowingId)}");
            }
        }
        Menu.Start();
    }

    public static void Adjust(int userId)
    {
        System.Console.WriteLine("These are your current reservations:");
        List<ReservationModel> reservations = _reservationsLogic.ShowAllUserReservations(userId);

        if (reservations.Count == 0)
        {
            System.Console.WriteLine("You have 0 reservations!");
            Menu.Start();
        }
        else
        {
            int counter = 1;
            foreach (ReservationModel reservation in reservations)
            {
                System.Console.WriteLine($"{counter++}. {_showingsLogic.FindShowingById(reservation.ShowingId)}");
            }
            System.Console.WriteLine("Which reservation would you like to change?");
            string userChoice;
            do
            {
                userChoice = Console.ReadLine();
            } while(!AccountsLogic.IsInt(userChoice) || AccountsLogic.ParseInt(userChoice) > counter - 1 || AccountsLogic.ParseInt(userChoice) < 1);

            ShowingModel showing = _showingsLogic.FindShowingByIdReturnShowing(reservations[AccountsLogic.ParseInt(userChoice) - 1].ShowingId);
            // Console.WriteLine($"Reservation:\n{showing.Id}");
            AdjustmentMenu(reservations[AccountsLogic.ParseInt(userChoice) - 1], showing.Title);
        }
    }

    private static void AdjustmentMenu(ReservationModel reservation, string showing)
    {
        System.Console.WriteLine("What would you like to adjust?");
        System.Console.WriteLine("1. Change or add seats (NOT YET IMPLEMENTED!!!)"); // Implement after youri's part
        System.Console.WriteLine("2. Change date");
        // System.Console.WriteLine("3. Add extra's");
        System.Console.WriteLine("4. Remove reservation");
        System.Console.WriteLine("5. Cancel");
        
        string userChoice = Console.ReadLine();

        switch(userChoice)
        {
            case "1":
                System.Console.WriteLine("(NOT YET IMPLEMENTED!!!)");
                break;
            case "2":
                ChangeReservationDate(reservation, showing);
                break;
            case "3":
                System.Console.WriteLine("(NOT YET IMPLEMENTED!!!)");
                break;
            case "4":
                _reservationsLogic.RemoveReservation(reservation);
                System.Console.WriteLine("Your reservation has been removed.");
                break;
            case "5":
                System.Console.WriteLine("Editing your reservation has been cancelled.");
                break;
            default:
                System.Console.WriteLine("Invalid input, try again!");
                AdjustmentMenu(reservation, showing);
                break;
        }
        Menu.Start();
    }


    // CHANGE SEAT IMPLEMENTATION AFTER YOURI IS DONE
    private static void ChangeReservationDate(ReservationModel oldReservation, string movieTitle)
    {
        List<ShowingModel> upcomingShowings = _showingsLogic.GetUpcomingShowingsOfMovie(movieTitle);
        if (upcomingShowings.Count == 0)
        {
            System.Console.WriteLine("There are no available shows planned, please cancel your reservation if you are unavailable at that time.");
        }
        else
        {
            System.Console.WriteLine("Which date do you want to change your reservation to?");
            int counter = 1;
            foreach (ShowingModel showing in upcomingShowings)
            {
                System.Console.WriteLine($"{counter++}: {showing.Date}");
            }
            string userChoice;
            do
            {
                userChoice = Console.ReadLine();
            } while(!AccountsLogic.IsInt(userChoice) || AccountsLogic.ParseInt(userChoice) > counter - 1 || AccountsLogic.ParseInt(userChoice) < 1);
            

            _reservationsLogic.RemoveReservation(oldReservation);
            ShowingModel newShowing = upcomingShowings[AccountsLogic.ParseInt(userChoice) - 1];
            _reservationsLogic.AddReservation(oldReservation.UserId, newShowing.Id, oldReservation.Seats, true);
            System.Console.WriteLine($"The date of your reservation has been succesfully changed to: {newShowing.Date}");
        }

        Menu.Start();
    }
}