static class MenuHelper
{
    public static T NewMenu<T>(List<string> options, List<T> actions, string header = null, string subtext = null, MovieModel[] promotedMovies = null)
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
                    actions.Insert(count++, (T)(Object)new Action(() => Reservation.ChooseShowing(movie)));
                    currentIndex++;
                } 
            }
        }

        do
        {
            Console.Clear();
            if (header != null) System.Console.WriteLine($"\u001b[1m===={header}====\u001b[0m");
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