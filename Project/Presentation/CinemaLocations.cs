public static class CinemaLocations
{
    private static CinemaLogic _cinemaLogic = new();
    
    
    public static void ChooseCinemaLocationToManage()
    {
        CinemaLogic cinemaLogic = new();
        List<string> options = cinemaLogic.Cinemas.Select(c => c.Name).ToList();
        options.Add("[Add new location]");
        options.Add("Return");

        List<Action> actions = new List<Action>();
        foreach (CinemaModel cinema in cinemaLogic.Cinemas)
        {
            actions.Add(() => ManageCinemaLocation(cinema));
        }
        actions.Add(() => AddCinemaLocation(cinemaLogic));
        actions.Add(Menus.AdminMenu);
        MenuHelper.NewMenu(options, actions, subtext: "Which do you want to manage?");
    }

    private static void ManageCinemaLocation(CinemaModel cinema)
    {
        List<string> options = new List<string> {"Edit location name", "edit location address", "Close location (DANGER)", "Return"};
        List<Action> actions = new List<Action> 
        {
            () => ChangeLocationName(cinema),
            () => ChangeLocationAddress(cinema),
            () => 
        };

        MenuHelper.NewMenu(options, actions, "What do you want to manage?");
    }

    private static void ChangeLocationName(CinemaModel cinema)
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
        }while (postal.Length != 6 && postal.Substring(0,4).All(char.IsDigit) && postal.Substring(4,2).All(char.IsLetter));

        if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: $"Are you sure you want to change the location of {cinema.Name} to {address} {postal} in {city}?"))
        {
            _cinemaLogic.EditCinemaAddress(cinema, city, address, postal);
        }

        MenuHelper.WaitForKey(() => ManageCinemaLocation(cinema));
    }

    private static void ChangeLocationAddress(CinemaModel cinema)
    {
        Console.Clear();

        System.Console.WriteLine("Please enter the new name of the cinema location:");
        string name = Console.ReadLine();
        if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: $"Are you sure you want to change the name of {cinema.Name} to {name}?"))
        {
            _cinemaLogic.EditCinemaName(cinema, name);
        }

        MenuHelper.WaitForKey(() => ManageCinemaLocation(cinema));
    }

    private static void AddCinemaLocation()
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
        }while (postal.Length != 6 && postal.Substring(0,4).All(char.IsDigit) && postal.Substring(4,2).All(char.IsLetter));
        
        if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: $"Are you sure you want to add cinema {name} in {city} at {address} {postal}?"))
        {
            Console.Clear();
            _cinemaLogic.AddCinema(name, city, address, postal);
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

    private static void RemoveCinemaLocation(CinemaModel model)
    {

    }

}