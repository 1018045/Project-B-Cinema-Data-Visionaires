public class CinemaLocations
{
    private LogicManager _logicManager;
    private MenuManager _menuManager;


    public CinemaLocations(LogicManager logicManager, MenuManager menuManager)
    {
        _logicManager = logicManager;
        _menuManager = menuManager;
    }
    
    public void ChooseCinema(Action originMenu, Action cancellationMenu)
    {
        CinemaLogic cinemaLogic = _logicManager.CinemaLogic;
        List<string> options = cinemaLogic.Cinemas.Select(c => c.Name).ToList();
        List<Action> actions = new();
        foreach (CinemaModel cinema in cinemaLogic.Cinemas)
        {
            actions.Add(() => {
                cinemaLogic.ChangeCinema(cinema);
                originMenu.Invoke();
            });
        }
        options.Add("Cancel");
        actions.Add(cancellationMenu);
        MenuHelper.NewMenu(options, actions, "Please select a cinema location");
    }
    
    public void AboutContact(Action returnMenu)
    {
        Console.Clear();

        if (CinemaLogic.CurrentCinema == null)
        {
            System.Console.WriteLine("Please first select a cinema!");
            Thread.Sleep(2000);
            ChooseCinema(() => AboutContact(returnMenu), returnMenu);
            return;
        }
        
        Console.WriteLine($"=== About Cine&Dine {CinemaLogic.CurrentCinema.Name} ===\n");
        Console.WriteLine($"Welcome to Cine&Dine {CinemaLogic.CurrentCinema.Name} - where film and culinary delight come together!");
        Console.WriteLine("We offer a unique cinema experience where you can enjoy the latest movies");
        Console.WriteLine("while being pampered with delicious dishes and drinks.\n");
        
        Console.WriteLine("=== Contact Information ===");
        Console.WriteLine($"{CinemaLogic.CurrentCinema.Address}, {CinemaLogic.CurrentCinema.City} {CinemaLogic.CurrentCinema.PostalCode}");
        
        Console.WriteLine($"Phone: {CinemaLogic.CurrentCinema.PhoneNumber}");
        Console.WriteLine($"Email: info-{CinemaLogic.CurrentCinema.Name}@cineanddine.nl\n");
        
        Console.WriteLine("=== Opening Hours ===");
        Console.WriteLine("Monday through Sunday: 12:00 - 00:00\n");
        
        MenuHelper.WaitForKey(returnMenu);
    }

    public void ChooseCinemaLocationToManage()
    {
        CinemaLogic cinemaLogic = _logicManager.CinemaLogic;
        List<string> options = cinemaLogic.Cinemas.Select(c => c.Name).ToList();
        options.Add("[Add new location]");
        options.Add("Return");

        List<Action> actions = new List<Action>();
        foreach (CinemaModel cinema in cinemaLogic.Cinemas)
        {
            actions.Add(() => ManageCinemaLocation(cinema));
        }
        actions.Add(() => AddCinemaLocation());
        actions.Add(_menuManager.MainMenus.AdminMenu);
        MenuHelper.NewMenu(options, actions, subtext: "Which do you want to manage?");
    }

    private void ManageCinemaLocation(CinemaModel cinema)
    {
        List<string> options = new List<string> {"Edit location name", "edit location address", "Close location (DANGER)", "Return"};
        List<Action> actions = new List<Action> 
        {
            () => ChangeLocationName(cinema),
            () => ChangeLocationAddress(cinema),
            () => RemoveCinemaLocation(cinema),
            ChooseCinemaLocationToManage
        };

        MenuHelper.NewMenu(options, actions, $"Cinema {cinema.Name}", subtext: $"{cinema.City}, {cinema.Address} {cinema.PostalCode}");
    }

    private void ChangeLocationAddress(CinemaModel cinema)
    {
        Console.Clear();

        System.Console.WriteLine("Please enter the new city of the cinema:");
        string city = Console.ReadLine();

        System.Console.WriteLine("Please enter the new street and number of the cinema:");
        string address = Console.ReadLine();

        string postal;
        do
        {
            System.Console.WriteLine("Please enter the new postal code of the cinema: (format: 0000AA)");
            postal = Console.ReadLine().Trim();
        }while (!CinemaLogic.VerifyPostalCode(postal));

        if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: $"Are you sure you want to change the location of {cinema.Name} to {address} {postal} in {city}?"))
        {
            _logicManager.CinemaLogic.EditCinemaAddress(cinema, city, address, postal);
        }

        MenuHelper.WaitForKey(() => ManageCinemaLocation(cinema));
    }

    private void ChangeLocationName(CinemaModel cinema)
    {
        Console.Clear();

        System.Console.WriteLine("Please enter the new name of the cinema location:");
        string name = Console.ReadLine();
        if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: $"Are you sure you want to change the name of {cinema.Name} to {name}?"))
        {
            _logicManager.CinemaLogic.EditCinemaName(cinema, name);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine($"Succesfully renamed cinema location to {cinema.Name}!");
        }
        else
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("Canceled renaming cinema location!");
        }
        Console.ResetColor();

        MenuHelper.WaitForKey(() => ManageCinemaLocation(cinema));
    }

    private void AddCinemaLocation()
    {
        Console.Clear();

        System.Console.WriteLine("Please enter the name of the new cinema location:");
        string name = Console.ReadLine();
        System.Console.WriteLine("Please enter the city of the new cinema location:");
        string city = Console.ReadLine();
        System.Console.WriteLine("Please enter the address of the new cinema location:");
        string address = Console.ReadLine();
        string postal;
        do
        {
            System.Console.WriteLine("Please enter the postal code of the new cinema location: (format: 0000AA)");
            postal = Console.ReadLine().Trim();
        }while (!CinemaLogic.VerifyPostalCode(postal));

        System.Console.WriteLine("Please enter the phone number that the cinema will be reachable on:");
        string phoneNumber = Console.ReadLine();
        
        if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: $"Are you sure you want to add cinema {name} in {city} at {address} {postal}?"))
        {
            Console.Clear();
            _logicManager.CinemaLogic.AddCinema(name, city, address, postal, phoneNumber);
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Succesfully added cinema location!");
        }
        else
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("Canceled adding cinema location!");
        }
        Console.ResetColor();

        Thread.Sleep(1000);
        MenuHelper.WaitForKey(ChooseCinemaLocationToManage);
    }

    private void RemoveCinemaLocation(CinemaModel cinema)
    {
        if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: $"Are you sure you want to delete location {cinema.Name}?"))
        {
            Console.Clear();
            _logicManager.CinemaLogic.RemoveCinema(cinema);
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("Succesfully removed cinema location!");
        }
        else
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("Canceled removing cinema location!");
        }
        Console.ResetColor();

        Thread.Sleep(1000);
        MenuHelper.WaitForKey(ChooseCinemaLocationToManage);
    }
}