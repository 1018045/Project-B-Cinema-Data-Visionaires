using System.Globalization;
using System.Text.Json;
using Project.Helpers;
using Project.Logic.Account;
using Project.Presentation;

public class Reservation
{
    private const string DATEFORMAT = "dd-MM-yyyy HH:mm:ss";
    private const string EXTENDEDDATEFORMAT = "dddd d MMMM yyyy";

    private const double BASE_TICKET_PRICE = 10.00;

    // Prijzen voor eten en drinken
    private const double BURGER_PRICE = 18.50;
    private const double PIZZA_PRICE = 15.95;
    private const double CHEESE_PRICE = 12.50;
    private const double WINE_PRICE = 7.50;
    private const double VITAMIN_WATER_PRICE = 4.95;
    private const double WATER_PRICE = 3.95;
    private const double JUICE_PRICE = 3.95;

    private LogicManager _logicManager;
    private MenuManager _menuManager;

    public Reservation(LogicManager logicManager, MenuManager menuManager)
    {
        _logicManager = logicManager;
        _menuManager = menuManager;
    }

    public void Make(ShowingModel showing, bool makeForGuest, int customerId = -1)
    {
        Console.Clear();
        ReservationsLogic reservationsLogic = _logicManager.ReservationsLogic;
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
        MovieModel movie = moviesLogic.GetMovieById(showing.MovieId);

        if (AccountsLogic.CurrentAccount == null)
        {
            int choice  = MenuHelper.NewMenu(new List<string> {"Continue as guest", "Log in", "Create Account"}, new List<int> {0,1,2}, subtext: "You are currently not logged in, what do you want to do?");
            if (choice == 1)
            {
                System.Console.WriteLine("You are being redirected to the login screen.");
                Thread.Sleep(1500);
                _menuManager.AccountPresentation.Login(() => Make(showing, makeForGuest), acceptOnlyCustomerLogin: true);
                return;
            }
            else if (choice == 2)
            {
                System.Console.WriteLine("You are being redirected to the login screen.");
                Thread.Sleep(1500);
                _menuManager.AccountPresentation.CreateAccount(() => Make(showing, makeForGuest));
                return;
            }
        }
        Console.Clear();
        if (!_logicManager.AccountsLogic.IsOldEnough(movie.MinimumAge) && AccountsLogic.CurrentAccount != null && customerId == -1 && !makeForGuest)
        {
            System.Console.WriteLine("You are not old enough to watch this movie.");
            System.Console.WriteLine("You are being redirected to the menu.");
            Thread.Sleep(2000);
            MenuHelper.WaitForKey(_menuManager.MainMenus.LoggedInMenu);
            return;
        }
        else
        {
            if (customerId == -1)
            {
                if (!MenuHelper.NewMenu(new List<string>() { "Yes", "No" }, new List<bool>() { true, false },
                        subtext: $"Is everyone in your party at the age of {movie.MinimumAge} or above?"))
                {
                    System.Console.WriteLine("You are being redirected to the menu.");
                    Thread.Sleep(2000);
                    MenuHelper.WaitForKey(customerId != -1 || makeForGuest ? _menuManager.MainMenus.StaffMenu : AccountsLogic.CurrentAccount != null ? _menuManager.MainMenus.LoggedInMenu : _menuManager.MainMenus.GuestMenu);
                    return;
                }
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

        List<string> selectedFoods = new List<string>();
        List<string> selectedDrinks = new List<string>();
        double totalFoodPrice = 0;
        double totalDrinkPrice = 0;

       
        if (MenuHelper.NewMenu(new List<string> { "Yes", "No" }, new List<bool> { true, false }, subtext: "Would you like to order food?"))
        {
            while (true)
            {
                Console.WriteLine("=== Food Menu ===");
                Console.WriteLine($"1. Burger:{BURGER_PRICE}");
                Console.WriteLine($"2. Pizza: {PIZZA_PRICE}");
                Console.WriteLine($"3. Cheeseboard: {CHEESE_PRICE}");
                Console.WriteLine($"4. Done");
              
                Console.Write("Please select your food (1-4): ");
                string foodChoice = Console.ReadLine();
                switch (foodChoice)
                {
                    case "1":
                        selectedFoods.Add($"Gourmet Truffle Cheeseburger: {BURGER_PRICE}");
                        totalFoodPrice += BURGER_PRICE;
                        break;
                    case "2":
                        selectedFoods.Add($"Italian Style Pizza: {PIZZA_PRICE}");
                        totalFoodPrice += PIZZA_PRICE;
                        break;
                    case "3":
                        selectedFoods.Add($"Cheeseboard: {CHEESE_PRICE}");
                        totalFoodPrice += CHEESE_PRICE;
                        break;
                    case "4":
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please choose a valid option.");
                        continue;
                }
                if (foodChoice == "4") break; 
            }
        }

        
        if (MenuHelper.NewMenu(new List<string> { "Yes", "No" }, new List<bool> { true, false }, subtext: "Would you like to order drinks?"))
        {
            while (true)
            {
                Console.WriteLine("=== Drinks Menu ===");
                Console.WriteLine($"1. Red Wine: {WINE_PRICE}");
                Console.WriteLine($"2. White Wine: {WINE_PRICE}");
                Console.WriteLine($"3. Vitamin Water: {VITAMIN_WATER_PRICE}");
                Console.WriteLine($"4. Sparkling Water: {WATER_PRICE}");
                Console.WriteLine($"5. Orange Juice: {JUICE_PRICE}");
                Console.WriteLine($"6. Done");
                string drinkChoice = Console.ReadLine();
                switch (drinkChoice)
                {
                    case "1":
                        selectedDrinks.Add($"Red Wine: {WINE_PRICE}");
                        totalDrinkPrice += WINE_PRICE;
                        break;
                    case "2":
                        selectedDrinks.Add($"White Wine: {WINE_PRICE}");
                        totalDrinkPrice += WINE_PRICE;
                        break;
                    case "3":
                        selectedDrinks.Add($"Vitamin Water: {VITAMIN_WATER_PRICE}");
                        totalDrinkPrice += VITAMIN_WATER_PRICE;
                        break;
                    case "4":
                        selectedDrinks.Add($"Sparkling Water: {WATER_PRICE}");
                        totalDrinkPrice += WATER_PRICE;
                        break;
                    case "5":
                        selectedDrinks.Add($"Orange Juice: {JUICE_PRICE}");
                        totalDrinkPrice += JUICE_PRICE;
                        break;
                    case "6":
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please choose a valid option.");
                        continue;
                }
                if (drinkChoice == "6") break; 
            }
        }

        List<ExtraModel> selectedExtras = new List<ExtraModel>();
        double extrasPrice = 0.0;

        if (showing.Is3D)
        {
            if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: $"This showing of {movie.Title} is in 3D. Would you like to add {selectedSeats.Count + 1} 3D glasses ($7.50 each) to your order? (You can also bring your own)"))
            {
                for (int i = 0; i < selectedSeats.Count; i++)
                {
                    selectedExtras.Add(new ExtraModel("3D glasses", (decimal)7.50, false));
                    extrasPrice += 7.50;
                }
            }
        }

        foreach (var extra in showing.Extras)
        {
            if (extra.IsMandatory)
            {
                for (int i = 0; i < selectedSeats.Count; i++)
                {
                    extrasPrice += (double)extra.Price;
                    selectedExtras.Add(extra);
                }
            }
            else
            {
                if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: $"Would you like to add a(n) {extra.Name} for {extra.Price:C} per selected seat?"))
                {
                    for (int i = 0; i < selectedSeats.Count; i++)
                    {
                        selectedExtras.Add(extra);
                        extrasPrice += (double)extra.Price;
                    }
                }
            }
        }

        double basePrice = 10.00; 
        double specialPrice = 0.00; 

        if (showing.Special == "Premier")
        {
            specialPrice += 5.00;
        }
        if (showing.Special == "Dolby")
        {
            specialPrice += 3.50;
        }
  
        double totalPrice = (basePrice + specialPrice) * selectedSeats.Count + totalFoodPrice + totalDrinkPrice + extrasPrice;       
        ShowBill(showing, selectedFoods, selectedDrinks, selectedExtras, selectedSeats.Count, totalPrice);
        
        string payment;        
        do
        {
            Console.Clear();
            ShowBill(showing, selectedFoods, selectedDrinks, selectedExtras, selectedSeats.Count, totalPrice);
            Console.WriteLine("Please enter your bank details: (example NL91ABNA0417164300)");
            Console.Write("IBAN: ");
            payment = reservationsLogic.ValidateBankDetails(Console.ReadLine());
            Console.WriteLine(payment);
        } while (payment != "");

        FakeProcessingPayment(5000);

        AccountantLogic accountantLogic = new();
        BillModel bill;
        if (AccountsLogic.CurrentAccount != null)
        {    
            bill = new BillModel(
                accountantLogic.FindFirstAvailableID(),
                AccountsLogic.CurrentAccount.Id,
                true,
                totalPrice,
                DateTime.Now
            );
        }
        else
        {
            bill = new BillModel(
                accountantLogic.FindFirstAvailableID(),
                -1,
                true,
                totalPrice,
                DateTime.Now
            );
        }
        accountantLogic.AddBill(bill);
        // ID == -1 ? guest user... ID == -1 && makeForGuest ? guest staff
        int userId;
        Action returnMenu;
        if (customerId != -1)
        {
            // staff makes reservation for existing account
            userId = customerId;
            returnMenu = _menuManager.MainMenus.StaffMenu;
        }
        else if (AccountsLogic.CurrentAccount == null)
        {
            // user makes reservation as guest
            userId = -1;
            returnMenu = _menuManager.MainMenus.GuestMenu;
        }
        else if (makeForGuest)
        {
            // staff makes reservation for guest
            userId = -1;
            returnMenu = _menuManager.MainMenus.StaffMenu;
        }
        else if (AccountsLogic.CurrentAccount != null)
        {
            // logged in user makes reservation
            userId = AccountsLogic.CurrentAccount.Id;
            returnMenu = _menuManager.MainMenus.LoggedInMenu;
        }
        else
        {
            // you shouldn't end up here but alas
            Console.Clear();
            System.Console.WriteLine("How did you get here?");
            Thread.Sleep(1000);
            userId = -1;
            returnMenu = _menuManager.MainMenus.GuestMenu;
        }
        
        userId = customerId == -1 ? (AccountsLogic.CurrentAccount != null ? AccountsLogic.CurrentAccount.Id : -1): customerId;
        ReservationModel reservation;

        reservation = reservationsLogic.AddReservation(userId, showing.Id, string.Join(",", selectedSeats), true, totalPrice, selectedExtras);
        
        reservation.SetBillId(bill.ID);
        reservationsLogic.UpdateReservation(reservation);

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("You have successfully booked your ticket(s)!\n");
        Console.WriteLine($"Your unique reservation code is {reservation.Id}.\n");
        if (AccountsLogic.CurrentAccount == null || makeForGuest)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("IMPORTANT: Remember this unique reservation code and print your bill! Since you booked as a guest these are your only proof of reservation!\n");
            Console.ResetColor();
            ShowBill(showing, selectedFoods, selectedDrinks, selectedExtras, selectedSeats.Count, totalPrice, clear: false);
        }
        Console.ResetColor();
        Console.WriteLine($"Your unique reservation code is {reservation.Id}.");

        MenuHelper.WaitForKey(returnMenu);
    }

    public void MakeCustomerReservation(bool makeForGuest)
    {
        AccountModel? customer = null;

        if (!makeForGuest)
        {
            while (customer == null)
            {
                Console.Clear();
                Console.WriteLine("Please enter the email of the customer: ");
                var email = Console.ReadLine()!;

                var accounts = AccountsAccess.LoadAll();
                customer = accounts.Find(a => a.EmailAddress.ToLower().Equals(email.ToLower()));
            }
            _menuManager.Movies.MoviesBrowser(makeForGuest, customerId: customer.Id);
        }
        else
        {
            _menuManager.Movies.MoviesBrowser(makeForGuest);
        }
        
    }

    private void ShowBill(ShowingModel showing, List<string> selectedFoods, List<string> selectedDrinks, List<ExtraModel> selectedExtras, int numberOfTickets, double totalPrice, bool clear = true)
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
        if (clear) Console.Clear();

        Console.WriteLine("===== Order Summary =====\n");
    
        Console.WriteLine($"Tickets: {numberOfTickets} x {moviesLogic.GetMovieById(showing.MovieId).Title} on {showing.Date.ToString(DATEFORMAT)} x €{BASE_TICKET_PRICE:F2}");
     
        if (selectedFoods.Count > 0)
        {
            Console.WriteLine("\nFood:");
            foreach (var food in selectedFoods)
            {
                Console.WriteLine($"  - {food}");
            }
        }
      
        if (selectedDrinks.Count > 0)
        {
            Console.WriteLine("\nDrinks:");
            foreach (var drink in selectedDrinks)
            {
                Console.WriteLine($"  - {drink}");
            }
        }

        if (selectedExtras.Count > 0)
        {
            Console.WriteLine("\nExtras:");
            foreach (ExtraModel extra in selectedExtras)
            {
                Console.WriteLine($"  - {extra.Name}: {extra.Price}");
            }
        }

        Console.WriteLine($"\nTotal to pay: €{totalPrice:F2}");
        Console.WriteLine("\n=========================");
    }

    public void ChooseShowing(MovieModel movie)
    {
        ShowingsLogic showingsLogic = _logicManager.ShowingsLogic;
        Console.Clear();
        if (CinemaLogic.CurrentCinema == null)
        {
            Console.Clear();
            System.Console.WriteLine("Please select a cinema before continuing.");
            Thread.Sleep(2000);
            _menuManager.CinemaLocations.ChooseCinema(() => ChooseShowing(movie), () => 
            {
                if (AccountsLogic.CurrentAccount == null) _menuManager.MainMenus.GuestMenu();
                else _menuManager.MainMenus.LoggedInMenu();
            });
            return;
        }
        
        List<object> showings = showingsLogic.FindShowingsByMovieId(movie.Id, CinemaLogic.CurrentCinema.Id).ToList<object>();
        if (showings.Count() == 0)
        {
            System.Console.WriteLine("There are no upcoming showings for this movie");
            Thread.Sleep(1000);
            MenuHelper.WaitForKey(() => _menuManager.Movies.MoviesBrowser());
            return;
        }
        List<string> options = showings.Cast<ShowingModel>().Select(s => s.Date.ToString(EXTENDEDDATEFORMAT)).ToList();
        
        // adding 3D to text
        for (int i = 0; i < options.Count; i++)
        {
            ShowingModel s = (ShowingModel)showings[i];
            if (s.Is3D) options[i] += " in 3D";
        }
        
        options.Add("Return");
        showings.Add(AccountsLogic.CurrentAccount == null ? _menuManager.MainMenus.GuestMenu : _menuManager.MainMenus.LoggedInMenu);

        var show = MenuHelper.NewMenu(options, showings, movie.Title, "Select a showing to start the reservation progress");
        ShowingModel selectedShowing = (ShowingModel)show;
        Make(selectedShowing, false);
    }

    private void FakeProcessingPayment(int lengthInMilliSeconds)
    {

        for (int i = 0; i < lengthInMilliSeconds/400; i++)
        {
            Console.Clear();
            string dots = new string('.', (i % 3) + 1);
            System.Console.WriteLine("Payment processing" + dots);
            Thread.Sleep(400);
        }
    }

    public void Adjust(int userId)
    {
        ReservationsLogic reservationsLogic = _logicManager.ReservationsLogic;
        List<ReservationModel> reservations = reservationsLogic.Reservations.Where(r => r.UserId == userId).ToList();
        
        if (reservations.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("You have no reservations!");
            Thread.Sleep(1500);
            MenuHelper.WaitForKey(_menuManager.MainMenus.LoggedInMenu);
            return;
        }

        List<string> reservationStrings = new();
        List<Action> actions = new();

        foreach (ReservationModel res in reservations)
        {
            ShowingModel s = _logicManager.ShowingsLogic.FindShowingByIdReturnShowing(res.ShowingId);
            MovieModel m = _logicManager.MoviesLogic.GetMovieById(s.MovieId);
            reservationStrings.Add($"{m.Title}: {s.Date.ToString(DATEFORMAT)}");
            actions.Add(() => AdjustmentMenu(res, m.Title));
        }

        reservationStrings.Add("Return");
        actions.Add(_menuManager.MainMenus.LoggedInMenu);

        MenuHelper.NewMenu(reservationStrings, actions, "Your reservations");   
    }

    private void AdjustmentMenu(ReservationModel reservation, string showing)
    {   
        ReservationsLogic reservationsLogic = _logicManager.ReservationsLogic;
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
                    reservationsLogic.RemoveReservation(reservation);
                Adjust(reservation.UserId);
            },
            () => Adjust(reservation.UserId)
        };

        MenuHelper.NewMenu(options, actions);
    }

    // CHANGE SEAT IMPLEMENTATION AFTER YOURI IS DONE
    private void ChangeReservationDate(ReservationModel oldReservation, string movieTitle)
    {
        ShowingsLogic showingsLogic = _logicManager.ShowingsLogic;
        ReservationsLogic reservationsLogic = _logicManager.ReservationsLogic;

        List<ShowingModel> upcomingShowings = showingsLogic.GetUpcomingShowingsOfMovie(movieTitle, CinemaLogic.CurrentCinema.Id);
        if (upcomingShowings.Count == 1)
        {
            Console.WriteLine("There are no available shows planned, please cancel your reservation if you are unavailable at that time.");
        }
        else
        {
            Console.WriteLine("Which date do you want to change your reservation to?");
            Console.WriteLine("We charge a fee of 5 euros for changing a reservation");
            int counter = 1;
            foreach (ShowingModel showing in upcomingShowings.Where(s => s.Date != showingsLogic.FindShowingByIdReturnShowing(oldReservation.ShowingId).Date))
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
                payment = reservationsLogic.ValidateBankDetails(Console.ReadLine()!);
                Console.WriteLine(payment);
            }

            reservationsLogic.RemoveReservation(oldReservation);
            ShowingModel newShowing = upcomingShowings[AccountsLogic.ParseInt(userChoice) - 1];
            reservationsLogic.AddReservation(oldReservation.UserId, newShowing.Id, oldReservation.Seats, true, oldReservation.Price + 5,oldReservation.SelectedExtras);
            Console.WriteLine($"The date of your reservation has been succesfully changed to: {newShowing.Date.ToString(DATEFORMAT)}");
        }

        MenuHelper.WaitForKey(() => Adjust(oldReservation.UserId));
    }

    public void SelectDate()
    {
        if (CinemaLogic.CurrentCinema == null)
        {
            Console.Clear();
            System.Console.WriteLine("Please select a cinema before browsing movies.");
            _menuManager.CinemaLocations.ChooseCinema(SelectDate, () => 
            {
                if (AccountsLogic.CurrentAccount == null) _menuManager.MainMenus.GuestMenu();
                else _menuManager.MainMenus.LoggedInMenu();
            });
            return;
        }
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
        actions.Add(AccountsLogic.CurrentAccount == null ? _menuManager.MainMenus.GuestMenu : _menuManager.MainMenus.LoggedInMenu);

        MenuHelper.NewMenu(dateOptions, actions, "Select a date:");
    }

    private void SelectMovieOnDate(DateTime date)
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
        ShowingsLogic showingsLogic = _logicManager.ShowingsLogic;
        List<Action> actions = new();
        moviesLogic.Movies.Where(m => moviesLogic.HasUpcomingShowingsOnDate(showingsLogic, m, date)).ToList()
                            .ForEach(m => actions.Add(() => SelectShowingOnDate(m, date)));

        if (actions.Count == 0)
        {
            Console.Clear();
            System.Console.WriteLine($"No movies found on {date.ToString(EXTENDEDDATEFORMAT)}");
            MenuHelper.WaitForKey(AccountsLogic.CurrentAccount == null ? _menuManager.MainMenus.GuestMenu : _menuManager.MainMenus.LoggedInMenu);
            return;
        }
        
        List<string> movieOptions = moviesLogic.Movies.Where(m => moviesLogic.HasUpcomingShowingsOnDate(showingsLogic, m, date))
                                                        .Select(m => m.Title).ToList();

        actions.Add(SelectDate);
        movieOptions.Add("Return");
        MenuHelper.NewMenu(movieOptions, actions, $"Movies on {date.ToString(EXTENDEDDATEFORMAT)}");
    }

    private void SelectShowingOnDate(MovieModel movie, DateTime date)
    {
        List<ShowingModel> showings = _logicManager.ShowingsLogic.FindShowingsByMovieId(movie.Id, CinemaLogic.CurrentCinema.Id).Where(s => s.Date.Date == date.Date).ToList();
        List<string> showingOptions = showings.Select(s => $"Room {s.Room}: {s.Date.ToString("HH:mm")}").ToList();

        Make(MenuHelper.NewMenu(showingOptions, showings, $"Showings of {movie.Title} on {date.ToString(EXTENDEDDATEFORMAT)}"), false);
    }

    private List<DateTime> GetDateTimeList(int amount)
    {
        List<DateTime> dates = new();
        for (int i = 0; i < amount; i++) dates.Add(DateTime.Now.Date.AddDays(i));
        return dates;
    }

    private DateTime AskAndParseFutureDate()
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

    public DateTime AskAndParsePastDate()
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