using System.Runtime.InteropServices;

static class MenuHelper
{
    public static void NewMenu(List<string> options, List<Action> actions)
    {
        if (options.Count != actions.Count)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("DEV ERROR: options and actions have different length");
            Console.ResetColor();
            return;
        }

        int currentIndex = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();

            for (int i = 0; i < options.Count; i++)
            {
                if (i == currentIndex) Console.WriteLine($"-> {options[i]}");
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

        actions[currentIndex].Invoke();
    }

    public static void NewMenu(string titleMessage, List<string> options, List<Action> actions)
    {
        if (options.Count != actions.Count)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("DEV ERROR: options and actions have different length");
            Console.ResetColor();
            return;
        }

        int currentIndex = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            System.Console.WriteLine($"\u001b[1m===={titleMessage}====\u001b[0m");

            for (int i = 0; i < options.Count; i++)
            {
                if (i == currentIndex) Console.WriteLine($"-> {options[i]}");
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

        actions[currentIndex].Invoke();
    }

    public static int NewMenu(List<string> options, List<int> actions)
    {
        if (options.Count != actions.Count)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("DEV ERROR: options and actions have different length");
            Console.ResetColor();
            return -1;
        }

        int currentIndex = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();

            for (int i = 0; i < options.Count; i++)
            {
                if (i == currentIndex) Console.WriteLine($"-> {options[i]}");
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

        return actions[currentIndex];
    }

    public static int NewMenu(string titleMessage, List<string> options, List<int> actions)
    {
        if (options.Count != actions.Count)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("DEV ERROR: options and actions have different length");
            Console.ResetColor();
            return -1;
        }

        int currentIndex = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            System.Console.WriteLine($"\u001b[1m===={titleMessage}====\u001b[0m");

            for (int i = 0; i < options.Count; i++)
            {
                if (i == currentIndex) Console.WriteLine($"-> {options[i]}");
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

        return actions[currentIndex];
    }


    public static void WaitForKey()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    public static void WaitForKey(Action action)
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        action();
    }
}