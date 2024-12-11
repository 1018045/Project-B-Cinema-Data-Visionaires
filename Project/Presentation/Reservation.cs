using System.Globalization;
using Microsoft.VisualBasic;
using Project.Helpers;
using Project.Logic.Account;
using Project.Presentation;

public static class Reservation
{
    private const string DATEFORMAT = "dd-MM-yyyy HH:mm:ss";
    private const string EXTENDEDDATEFORMAT = "dddd d MMMM yyyy";
    private static readonly ReservationsLogic _reservationsLogic = new();

    private static readonly ShowingsLogic _showingsLogic = new ();

    private static readonly MoviesLogic _moviesLogic = new ();

    private static readonly AccountsLogic _accountsLogic = new();

    public static void Make(ShowingModel showing)
    {
        Console.Clear();
        
        while (AccountsLogic.CurrentAccount == null)
        {
            System.Console.WriteLine("Please login to continue making your reservation.");
            System.Console.WriteLine("You are being redirected to the login screen.");
            Thread.Sleep(2500);
            Menus.Login(() => Make(showing), acceptOnlyCustomerLogin: true);
        }

        bool confirmSeats = false;
        List<string> selectedSeats;
        do        
        {  
            selectedSeats = SeatingPresentation.Present(showing.Id);
            confirmSeats = MenuHelper.NewMenu(
                new List<string> {"Confirm", "Retry"},
                new List<bool> {true, false}, 
                subtext: "You have selected the following seats: " +
                $"{SeatSelectionHelpers.PositionsToRowSeatString(SeatSelectionHelpers.StringToPositions(selectedSeats))}"
            );
        } while(!confirmSeats);

        if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: "Would you like to order extra's?")) 
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
            string name;
            switch (drinkChoice)
            {
                case "1":
                    Console.WriteLine("You have selected Red Wine.");
                    name = "Red Wine";
                    break;
                case "2":
                    Console.WriteLine("You have selected White Wine.");
                    name = "White Wine";
                    break;
                case "3":
                    Console.WriteLine("You have selected Vitamin Water.");
                    name = "Vitamin Water";
                    break;

                case "4":
                    Console.WriteLine("You have selected Sparkling Water.");
                    name = "Sparkling Water";
                    break;

                case "5":
                    Console.WriteLine("You have selected Orange Juice.");
                    name = "Orange Juice";

                    break;
                default:
                    Console.WriteLine("Invalid choice, please choose a number between 1 and 5.");
                    return;  // Exit if invalid drink choice
            }

            Console.WriteLine("Thank you for your order! Your food and drink will be prepared.");
        }
        else
        {
            Console.WriteLine("No extras ordered. Thank you for your response.");
        }
        
        string payment = "X";
        while (payment != "")
        {
            Console.Clear();
            Console.WriteLine("Please enter your bank details:");
            payment = _reservationsLogic.ValidateBankDetails(Console.ReadLine()!);
            Console.WriteLine(payment);
        }
    
        double basePrice = 10.00; 
        double specialPrice = 0.00; 

        if (showing.Special == "Premier")
        {
            specialPrice += 5.00;
        }
        else if (showing.Special == "Dolby")
        {
            specialPrice += 3.50;
        }
  
        double totalPrice = (basePrice + specialPrice) * selectedSeats.Count;

        FakeProcessingPayment(5000);
        ReservationModel reservation = _reservationsLogic.AddReservation(AccountsLogic.CurrentAccount.Id, showing.Id, string.Join(",", selectedSeats), true, totalPrice);

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("You have successfully booked your ticket(s)!\n");
        Console.ResetColor();
        Console.WriteLine($"Your unique reservation code is {reservation.Id}.");
        MenuHelper.WaitForKey(Menus.LoggedInMenu);
    }

    public static void ChooseShowing(MovieModel movie)
    {
        Console.Clear();
        List<object> showings = _showingsLogic.FindShowingsByMovieId(movie.Id).ToList<object>();
        if (showings.Count() == 0)
        {
            System.Console.WriteLine("There are no upcoming showings for this movie");
            Thread.Sleep(1000);
            MenuHelper.WaitForKey(() => Movies.MoviesBrowser());
            return;
        }
        List<string> options = showings.Cast<ShowingModel>().Select(s => s.Date.ToString(DATEFORMAT)).ToList();
        options.Add("Return");
        showings.Add(Menus.LoggedInMenu);

        var show = MenuHelper.NewMenu(options, showings, movie.Title, "Select a showing to start the reservation progress:");
        ShowingModel selectedShowing = (ShowingModel)show;
        Make(selectedShowing);
    }

    private static void FakeProcessingPayment(int lengthInMilliSeconds)
    {

        for (int i = 0; i < lengthInMilliSeconds/400; i++)
        {
            Console.Clear();
            string dots = new string('.', (i % 3) + 1);
            System.Console.WriteLine("Payment processing" + dots);
            Thread.Sleep(400);
        }
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
            "Return"
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
        if (upcomingShowings.Count == 1)
        {
            Console.WriteLine("There are no available shows planned, please cancel your reservation if you are unavailable at that time.");
        }
        else
        {
            Console.WriteLine("Which date do you want to change your reservation to?");
            Console.WriteLine("We charge a fee of 5 euros for changing a reservation");
            int counter = 1;
            foreach (ShowingModel showing in upcomingShowings.Where(s => s.Date != _showingsLogic.FindShowingByIdReturnShowing(oldReservation.ShowingId).Date))
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
            _reservationsLogic.AddReservation(oldReservation.UserId, newShowing.Id, oldReservation.Seats, true, oldReservation.Price + 5);
            Console.WriteLine($"The date of your reservation has been succesfully changed to: {newShowing.Date.ToString(DATEFORMAT)}");
        }

        MenuHelper.WaitForKey(() => Adjust(oldReservation.UserId));
    }

    public static void SelectDate()
    {
        // print less than 2 weeks of showings if it doesn't fit
        int howManyDatesFitOnScreen = Console.WindowHeight - 4;
        int actualAmountOfDatesShown = Math.Min(howManyDatesFitOnScreen, 14);

        List<DateTime> dates = GetDateTimeList(actualAmountOfDatesShown);
        List<Action> actions = new();

        List<string> dateOptions = dates.Select(d => d.ToString(EXTENDEDDATEFORMAT)).ToList();
        dateOptions.Add("[Select a different date]");
        dateOptions.Add("Return");

        dates.ForEach(d => actions.Add(() => SelectMovieOnDate(d)));
        actions.Add(() => SelectMovieOnDate(AskAndParseDate()));
        actions.Add(AccountsLogic.CurrentAccount == null ? Menus.GuestMenu : Menus.LoggedInMenu);

        MenuHelper.NewMenu(dateOptions, actions, "Select a date:");
    }

    private static void SelectMovieOnDate(DateTime date)
    {
        List<Action> actions = new();
        _moviesLogic.Movies.Where(m => _moviesLogic.HasUpcomingShowingsOnDate(_showingsLogic, m, date)).ToList()
                            .ForEach(m => actions.Add(() => SelectShowingOnDate(m, date)));

        if (actions.Count == 0)
        {
            Console.Clear();
            System.Console.WriteLine($"No movies found on {date.ToString(EXTENDEDDATEFORMAT)}");
            MenuHelper.WaitForKey(AccountsLogic.CurrentAccount == null ? Menus.GuestMenu : Menus.LoggedInMenu);
            return;
        }
        
        List<string> movieOptions = _moviesLogic.Movies.Where(m => _moviesLogic.HasUpcomingShowingsOnDate(_showingsLogic, m, date))
                                                        .Select(m => m.Title).ToList();

        actions.Add(SelectDate);
        movieOptions.Add("Return");
        MenuHelper.NewMenu(movieOptions, actions, $"Movies on {date.ToString(EXTENDEDDATEFORMAT)}");
    }

    private static void SelectShowingOnDate(MovieModel movie, DateTime date)
    {
        List<ShowingModel> showings = _showingsLogic.FindShowingsByMovieId(movie.Id).Where(s => s.Date.Date == date.Date).ToList();
        List<string> showingOptions = showings.Select(s => $"Room {s.Room}: {s.Date.ToString("HH:mm")}").ToList();

        Make(MenuHelper.NewMenu(showingOptions, showings, $"Showings of {movie.Title} on {date.ToString(EXTENDEDDATEFORMAT)}"));
    }

    private static List<DateTime> GetDateTimeList(int amount)
    {
        List<DateTime> dates = new();
        for (int i = 0; i < amount; i++) dates.Add(DateTime.Now.Date.AddDays(i));
        return dates;
    }

    private static DateTime AskAndParseDate()
    {  
        string dateInput = "";
        do
        {
            Console.Clear();
            System.Console.WriteLine("Please enter a future date in this format 'dd-MM-yyyy'");
            dateInput = Console.ReadLine();
        }
        while(!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) && DateTime.Now.Date > date.Date);             
        return DateTime.ParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture);
    }
}