static class MenuHelper
{
    private static MenuManager _menuManager;

    public static void setMenuManager(MenuManager menuManager)
    {
        _menuManager = menuManager;
        System.Console.WriteLine(menuManager);
        System.Console.WriteLine(_menuManager);
        Thread.Sleep(3000);
    }

    public static T NewMenu<T>(List<string> options, List<T> actions, string header = null, string subtext = null, List<MovieModel> promotedMovies = null, bool showMenu = false, bool showCurrentLocation = false)
    {
        if (options.Count != actions.Count)
        {
            System.Console.WriteLine($"options: {options.Count} actions: {actions.Count}");
            foreach (object o in actions)
            {
                System.Console.WriteLine(o);
            }
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("DEV ERROR: options and actions have different length");
            Console.ResetColor();
            return default;
        }

        int currentIndex = 0;
        ConsoleKey key;

        if (promotedMovies != null)
        {
            int count = 0;
            foreach (MovieModel movie in promotedMovies.Where(m => m != null))
            {
                if (movie != null)
                {
                    options.Insert(count, $"{movie.Title}\n    featuring {String.Join(", ", movie.Actors)}\n");
                    actions.Insert(count++, (T)(Object)new Action(() => _menuManager.Reservation.ChooseShowing(movie)));
                    currentIndex++;
                } 
            }
        }

        do
        {
            Console.Clear();
            if (showMenu)
            {
            Console.WriteLine("   _____ _                       _ _            ");
            Console.WriteLine("  / ____(_)            ___      | (_)           ");
            Console.WriteLine(" | |     _ _ __   ___ ( _ )   __| |_ _ __   ___ ");
            Console.WriteLine(" | |    | | '_ \\ / _ \\/ _ \\/\\/ _` | | '_ \\ / _ \\");
            Console.WriteLine(" | |____| | | | |  __/ (_>  < (_| | | | | |  __/");
            Console.WriteLine("  \\_____|_|_| |_|\\___|\\___/\\/\\__,_|_|_| |_|\\___|");
            }                                           

            string headerText = "";
            if (header != null) headerText += $"\u001b[1m===={header}====\u001b[0m";
            if (showCurrentLocation)
            {
                string locationText = "";
                if (CinemaLogic.CurrentCinema != null)
                    locationText = $"Selected cinema: {CinemaLogic.CurrentCinema.Name}";
                else
                    locationText = $"No cinema selected";
                int locationLength = locationText.Length;
                int padding = Console.WindowWidth - locationLength;
                // 18 characters of ANSI code in headertext
                if (headerText.Length > 0) padding -= headerText.Length - 8;
                headerText += new string(' ', padding) + locationText;
            }
            if (headerText.Length > 0) System.Console.WriteLine(headerText);
            
            if (subtext != null) System.Console.WriteLine(subtext);
            if (promotedMovies != null) System.Console.WriteLine("Our top picks:");
            
            for (int i = 0; i < options.Count; i++)
            {
                if (i == currentIndex) 
                {
                    Console.WriteLine($"-> {options[i]}");
                }               
                    
                else Console.WriteLine(options[i]);
            }

            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.DownArrow) currentIndex++;
            if (key == ConsoleKey.UpArrow) currentIndex--;
            // wrapping
            if (currentIndex < 0) currentIndex = options.Count - 1;
            if (currentIndex > options.Count - 1) currentIndex = 0;  
        } while (key != ConsoleKey.Enter);

        var selection = actions[currentIndex];

        if (selection is Action action) { action.Invoke(); return default; }      
        if (selection is Func<T> func) return func();      
        return selection;
    }

    public static void WaitForKey(string header = "Press any key to continue...")
    {
        Console.WriteLine("\n" + header);
        Console.ReadKey();
    }

    public static void WaitForKey(Action action, string header = "\nPress any key to continue...")
    {
        Console.WriteLine(header);
        Console.ReadKey();
        action();
    }
}