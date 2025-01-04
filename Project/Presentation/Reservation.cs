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

    private const double BASE_TICKET_PRICE = 10.00;

    // Prijzen voor eten en drinken
    private const double BURGER_PRICE = 18.50;
    private const double PIZZA_PRICE = 15.95;
    private const double CHEESE_PRICE = 12.50;
    private const double WINE_PRICE = 7.50;
    private const double VITAMIN_WATER_PRICE = 4.95;
    private const double WATER_PRICE = 3.95;
    private const double JUICE_PRICE = 3.95;

    public static void Make(ShowingModel showing)
    {
        Console.Clear();
        MovieModel movie = _moviesLogic.GetMovieById(showing.MovieId);
        
        while (AccountsLogic.CurrentAccount == null)
        {
            System.Console.WriteLine("Please login to continue making your reservation.");
            System.Console.WriteLine("You are being redirected to the login screen.");
            Thread.Sleep(2000);
            Menus.Login(() => Make(showing), acceptOnlyCustomerLogin: true);
        }

        if (!_accountsLogic.IsOldEnough(movie.MinimumAge))
        {
            System.Console.WriteLine("You are not old enough to watch this movie.");
            System.Console.WriteLine("You are being redirected to the menu.");
            Thread.Sleep(2000);
            MenuHelper.WaitForKey(Menus.LoggedInMenu);
            return;
        }
        else
        {
            if (!MenuHelper.NewMenu(new List<string>() {"Yes", "No"}, new List<bool>() {true, false}, subtext: $"Is everyone in your party at the age of {movie.MinimumAge} or above?"))
            {
                System.Console.WriteLine("You are being redirected to the menu.");
                Thread.Sleep(2000);
                MenuHelper.WaitForKey(Menus.LoggedInMenu);
                return;
            }
        }
        Console.Clear();

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

        string? selectedFood = null;
        string? selectedDrink = null;

        if (MenuHelper.NewMenu(new List<string> { "Yes", "No" }, new List<bool> { true, false }, subtext: "Would you like to order extra's?"))
        {
            Console.WriteLine("These are the food choices:");
            Console.WriteLine($"1. Gourmet Truffle Cheeseburger (€{BURGER_PRICE:F2})");
            Console.WriteLine($"2. Italian Style Pizza (€{PIZZA_PRICE:F2})");
            Console.WriteLine($"3. Cheeseboard (€{CHEESE_PRICE:F2})");

            string foodChoice = Console.ReadLine();
            switch (foodChoice)
            {
                case "1":
                    selectedFood = "Gourmet Truffle Cheeseburger";
                    break;
                case "2":
                    selectedFood = "Italian Style Pizza";
                    break;
                case "3":
                    selectedFood = "Cheeseboard";
                    break;
                default:
                    Console.WriteLine("Invalid choice, please choose a number between 1 and 3.");
                    return;
            }

            Console.WriteLine("\nWould you like anything to drink?");
            Console.WriteLine($"1. Red Wine (€{WINE_PRICE:F2})");
            Console.WriteLine($"2. White Wine (€{WINE_PRICE:F2})");
            Console.WriteLine($"3. Vitamin Water (€{VITAMIN_WATER_PRICE:F2})");
            Console.WriteLine($"4. Sparkling Water (€{WATER_PRICE:F2})");
            Console.WriteLine($"5. Orange Juice (€{JUICE_PRICE:F2})");

            string drinkChoice = Console.ReadLine();
            switch (drinkChoice)
            {
                case "1":
                    selectedDrink = "Red Wine";
                    break;
                case "2":
                    selectedDrink = "White Wine";
                    break;
                case "3":
                    selectedDrink = "Vitamin Water";
                    break;
                case "4":
                    selectedDrink = "Sparkling Water";
                    break;
                case "5":
                    selectedDrink = "Orange Juice";
                    break;
                default:
                    Console.WriteLine("Invalid choice, please choose a number between 1 and 5.");
                    return;
            }
        }

        int numberOfTickets = selectedSeats.Count; // Aantal geselecteerde tickets

        // Toon de rekening
        ShowBill(selectedFood, selectedDrink, numberOfTickets);
        
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

    private static void ShowBill(string? foodChoice, string? drinkChoice, int numberOfTickets)
    {
        Console.Clear();
        double totalPrice = BASE_TICKET_PRICE * numberOfTickets; // Basis ticketprijs

        Console.WriteLine("========== UW BESTELLING ==========");
        Console.WriteLine($"Tickets: {numberOfTickets} x €{BASE_TICKET_PRICE:F2} = €{totalPrice:F2}");

        if (foodChoice != null)
        {
            double foodPrice = foodChoice switch
            {
                "Gourmet Truffle Cheeseburger" => BURGER_PRICE,
                "Italian Style Pizza" => PIZZA_PRICE,
                "Cheeseboard" => CHEESE_PRICE,
                _ => 0
            };
            Console.WriteLine($"Eten: {foodChoice} - €{foodPrice:F2}");
            totalPrice += foodPrice;
        }

        if (drinkChoice != null)
        {
            double drinkPrice = drinkChoice switch
            {
                "Red Wine" => WINE_PRICE,
                "White Wine" => WINE_PRICE,
                "Vitamin Water" => VITAMIN_WATER_PRICE,
                "Sparkling Water" => WATER_PRICE,
                "Orange Juice" => JUICE_PRICE,
                _ => 0
            };
            Console.WriteLine($"Drinken: {drinkChoice} - €{drinkPrice:F2}");
            totalPrice += drinkPrice;
        }

        Console.WriteLine($"Totaal te betalen: €{totalPrice:F2}");
        Console.WriteLine("=================================");
      
        if (AccountsLogic.CurrentAccount != null)
        {
            AccountsLogic.CurrentAccount.TotalSpent += totalPrice;
            
        }
      

        Console.WriteLine("\nDruk op een toets om door te gaan met betalen...");
        Console.ReadKey();

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
        actions.Add(() => SelectMovieOnDate(AskAndParseFutureDate()));
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

    private static DateTime AskAndParseFutureDate()
    {  
        string dateInput = "";
        do
        {
            Console.Clear();
            System.Console.WriteLine("Please enter a future date in this format 'dd-MM-yyyy'");
            dateInput = Console.ReadLine();
        }
        while(!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) || DateTime.Now.Date > date.Date);             
        return DateTime.ParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture);
    }

    public static DateTime AskAndParsePastDate()
    {  
        string dateInput;
        do
        {
            Console.Clear();
            System.Console.WriteLine("Please enter a future date in this format 'dd-MM-yyyy'");
            dateInput = Console.ReadLine();
        }
        while(!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) || DateTime.Now.Date < date.Date);             
        return DateTime.ParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture);
    }
}