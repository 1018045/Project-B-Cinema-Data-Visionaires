using System.ComponentModel.Design;
using System.Runtime.InteropServices;

static class MenuHelper
{
    public static T NewMenu<T>(List<string> options, List<T> actions, string message = null)
    {
        if (options.Count != actions.Count)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("DEV ERROR: options and actions have different length");
            Console.ResetColor();
            return default;
        }

        int currentIndex = 0;
        ConsoleKey key;

        do
        {
            Console.Clear();
            if (message != null) System.Console.WriteLine($"\u001b[1m===={message}====\u001b[0m");

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

        var selection = actions[currentIndex];

        if (selection is Action action)
        {
            action.Invoke();
            return default;
        }
        else if (selection is Func<T> func)
        {
            return func();
        }
        else 
        {
            return selection;
        }
    }

    public static void WaitForKey(string message = "Press any key to continue...")
    {
        Console.WriteLine("\n" + message);
        Console.ReadKey();
    }

    public static void WaitForKey(Action action, string message = "\nPress any key to continue...")
    {
        Console.WriteLine(message);
        Console.ReadKey();
        action();
    }
}