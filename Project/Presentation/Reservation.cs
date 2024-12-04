using Microsoft.VisualBasic;
using Project.Logic.Account;
using Project.Presentation;

public static class Reservation
{
    private const string DATEFORMAT = "dd-MM-yyyy HH:mm:ss";
    private static readonly ReservationsLogic _reservationsLogic = new();

    // Remove during code factoring bc of bad code practice
    private static readonly ShowingsLogic _showingsLogic = new ();

    private static readonly MoviesLogic _moviesLogic = new ();

    public static void Make()
    {
        Console.Clear();

        Console.WriteLine("\nUpcoming showings:\n");
        Showings.ShowUpcoming(makingReservation: true);

        Console.WriteLine("Select a showing (Enter the number in front of the showing to continue)");

        var id = Console.ReadLine();
        var showingId = Convert.ToInt32(id);

        string confirmSeats = "";
        List<string> selectedSeats;

        do        
        {
            selectedSeats = SeatingPresentation.Present(showingId);
            System.Console.WriteLine("Enter Y to confirm your seats.");
            System.Console.WriteLine("Enter any other key to select restart the seat selection.");
            confirmSeats = Console.ReadLine();
        } while(confirmSeats.ToLower().Trim() != "y");

        Console.WriteLine("Would you like to order extra's?"); 
        Console.WriteLine("Enter Y for Yes/N for No");
        string decision = Console.ReadLine();
        if(decision.Equals("Y", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("These are the food choices:");
            Console.WriteLine("1. Gourmet Truffle Cheeseburger");
            Console.WriteLine("2. Italian Style Pizza");
            Console.WriteLine("3. Cheeseboard");
          
            Console.WriteLine("Please enter the number of your choice (1-3):");
            string foodChoice = Console.ReadLine();
           
            switch (foodChoice)
            {
                case "1":
                    Console.WriteLine("You have selected Gourmet Truffle Cheeseburger.");
                    break;
                case "2":
                    Console.WriteLine("You have selected Italian Style Pizza.");
                    break;
                case "3":
                    Console.WriteLine("You have selected Cheeseboard.");
                    break;
                default:
                    Console.WriteLine("Invalid choice, please choose a number between 1 and 3.");
                    return;  
            }

            Console.WriteLine("Would you like anything to drink?");
            Console.WriteLine("These are the drink choices:");
            Console.WriteLine("1. Red Wine");
            Console.WriteLine("2. White Wine");
            Console.WriteLine("3. Vitamin Water");
            Console.WriteLine("4. Sparkling Water");
            Console.WriteLine("5. Orange Juice");
   
            Console.WriteLine("Please enter the number of your drink choice (1-5):");
            string drinkChoice = Console.ReadLine();
          
            switch (drinkChoice)
            {
                case "1":
                    Console.WriteLine("You have selected Red Wine.");
                    break;
                case "2":
                    Console.WriteLine("You have selected White Wine.");
                    break;
                case "3":
                    Console.WriteLine("You have selected Vitamin Water.");
                    break;
                case "4":
                    Console.WriteLine("You have selected Sparkling Water.");
                    break;
                case "5":
                    Console.WriteLine("You have selected Orange Juice.");
                    break;
                default:
                    Console.WriteLine("Invalid choice, please choose a number between 1 and 5.");
                    return;  // Exit if invalid drink choice
            }

            Console.WriteLine("Thank you for your order! Your food and drink will be prepared.");
        }
        else if (decision.Equals("N", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("No extras ordered. Thank you for your response.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter Y for Yes or N for No.");
        }
        
        string payment = "X";
        while (payment != "")
        {
            Console.WriteLine("\nBank details:");
            payment = _reservationsLogic.ValidateBankDetails(Console.ReadLine()!);
            Console.WriteLine(payment);
        }
    
        _reservationsLogic.AddReservation(AccountsLogic.CurrentAccount.Id, showingId, string.Join(",", selectedSeats), true);

        Console.WriteLine("\nYou have successfully booked your tickets!\n");
        MenuHelper.WaitForKey(Menus.LoggedInMenu);
    }

    public static void Adjust(int userId)
    {
        List<ReservationModel> reservations = _reservationsLogic.Reservations.Where(r => r.UserId == userId).ToList();
        
        if (reservations.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("You have no reservations!");
            Thread.Sleep(1500);
            MenuHelper.WaitForKey(Menus.LoggedInMenu);
            return;
        }

        List<string> reservationStrings = new();
        List<Action> actions = new();

        foreach (ReservationModel res in reservations)
        {
            ShowingModel s = _showingsLogic.FindShowingByIdReturnShowing(res.ShowingId);
            MovieModel m = _moviesLogic.GetMovieById(s.MovieId);
            reservationStrings.Add($"{m.Title}: {s.Date.ToString(DATEFORMAT)}");
            actions.Add(() => AdjustmentMenu(res, m.Title));
        }

        reservationStrings.Add("Return");
        actions.Add(Menus.LoggedInMenu);

        MenuHelper.NewMenu(reservationStrings, actions, "Your reservations");   
    }

    private static void AdjustmentMenu(ReservationModel reservation, string showing)
    {   
        List<string> options = new List<string>() 
        {
            "Change or add seats (NOT YET IMPLEMENTED!!!)",
            "Change date",
            "Cancel reservation",
            "Cancel"
        };

        List<Action> actions = new List<Action>() 
        {
            () => {
                Console.Clear();
                Console.WriteLine("(NOT YET IMPLEMENTED!!!)");
                MenuHelper.WaitForKey(() => AdjustmentMenu(reservation, showing));
                },
            () => ChangeReservationDate(reservation, showing),
            () => {
                if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, "Are you sure you want to cancel your reservation?"))
                    _reservationsLogic.RemoveReservation(reservation);
                Adjust(reservation.UserId);
            },
            () => Adjust(reservation.UserId)
        };

        MenuHelper.NewMenu(options, actions);
    }

    // CHANGE SEAT IMPLEMENTATION AFTER YOURI IS DONE
    private static void ChangeReservationDate(ReservationModel oldReservation, string movieTitle)
    {
        List<ShowingModel> upcomingShowings = _showingsLogic.GetUpcomingShowingsOfMovie(movieTitle);
        if (upcomingShowings.Count == 0)
        {
            Console.WriteLine("There are no available shows planned, please cancel your reservation if you are unavailable at that time.");
        }
        else
        {
            Console.WriteLine("Which date do you want to change your reservation to?");
            Console.WriteLine("We charge a fee of 5 euros for changing a reservation");
            int counter = 1;
            foreach (ShowingModel showing in upcomingShowings)
            {
                Console.WriteLine($"{counter++}: {showing.Date.ToString(DATEFORMAT)}");
            }
            string userChoice;
            do
            {
                userChoice = Console.ReadLine();
            } while(!AccountsLogic.IsInt(userChoice) || AccountsLogic.ParseInt(userChoice) > counter - 1 || AccountsLogic.ParseInt(userChoice) < 1);
            
            string payment = "X";        
            while (payment != "")
            {
                Console.WriteLine("\nBank details:");
                payment = _reservationsLogic.ValidateBankDetails(Console.ReadLine()!);
                Console.WriteLine(payment);
            }


            _reservationsLogic.RemoveReservation(oldReservation);
            ShowingModel newShowing = upcomingShowings[AccountsLogic.ParseInt(userChoice) - 1];
            _reservationsLogic.AddReservation(oldReservation.UserId, newShowing.Id, oldReservation.Seats, true);
            Console.WriteLine($"The date of your reservation has been succesfully changed to: {newShowing.Date.ToString(DATEFORMAT)}");
        }

        MenuHelper.WaitForKey(() => Adjust(oldReservation.UserId));
    }
}