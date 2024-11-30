using Microsoft.VisualBasic;
using Project.Logic.Account;
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

        string confirmSeats = "";
        List<string> selectedSeats;

        do        
        {
            selectedSeats = SeatingPresentation.Present(showingId);
            System.Console.WriteLine("Enter Y to confirm your seats.");
            System.Console.WriteLine("Enter any other key to change your seat selection, or the amount of seats you want to book.");
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
            payment = ReservationsLogic.ValidateBankDetails(Console.ReadLine()!);
            Console.WriteLine(payment);
        }
    
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
        MoviesLogic moviesLogic = new();

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
            AdjustmentMenu(reservations[AccountsLogic.ParseInt(userChoice) - 1], moviesLogic.GetMovieById(showing.MovieId).Title);
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
            Console.WriteLine("We charge a fee of 25 euros for changing a reservation");
            int counter = 1;
            foreach (ShowingModel showing in upcomingShowings)
            {
                Console.WriteLine($"{counter++}: {showing.Date.ToString("dd-MM-yyyy HH:mm:ss")}");
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
                payment = ReservationsLogic.ValidateBankDetails(Console.ReadLine()!);
                Console.WriteLine(payment);
            }


            ReservationsLogic.RemoveReservation(oldReservation);
            ShowingModel newShowing = upcomingShowings[AccountsLogic.ParseInt(userChoice) - 1];
            ReservationsLogic.AddReservation(oldReservation.UserId, newShowing.Id, oldReservation.Seats, true);
            Console.WriteLine($"The date of your reservation has been succesfully changed to: {newShowing.Date.ToString("dd-MM-yyyy HH:mm:ss")}");
        }

        Menus.LoggedInMenu();
    }
}