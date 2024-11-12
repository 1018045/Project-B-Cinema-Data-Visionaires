using System;
using System.Collections.Generic;

static class AdminMenu
{
    public static void Start()
    {
        Console.WriteLine("Admin Menu:");
        Console.WriteLine("1. Add a movie");
        Console.WriteLine("2. Remove a user");
        Console.WriteLine("3. View all users");
        Console.WriteLine("4. Remove a movie screening");
        Console.WriteLine("5. View all movies");
        Console.WriteLine("6. Return to main menu");

        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                AddMovie();
                break;
            case "2":
                RemoveUser();
                break;
            case "3":
                ViewUsers();
                break;
            case "4":
                RemoveMovie();
                break;
            case "5":
                ViewMovies();
                break;
            case "6":
                LoginMenu.Start();
                break;
            default:
                Console.WriteLine("Invalid choice, please try again.");
                Start();
                break;
        }
    }

    private static void AddMovie()
    {
        Console.WriteLine("Enter the movie title:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter the date (dd-MM-yyyy HH:mm:ss):");
        string date = Console.ReadLine();

        Console.WriteLine("Enter the hall:");
        int room = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the minimum age:");
        int minimumAge = int.Parse(Console.ReadLine());

        // Maak een instantie van ShowingsLogic
        ShowingsLogic showingsLogic = new ShowingsLogic();
        
        // Voeg de nieuwe vertoning toe via de logica-laag
        showingsLogic.AddShowing(title, date, room, minimumAge);

        Console.WriteLine($"Movie screening ‘{title}’ has been added.");
        Start(); // Terug naar het adminmenu
    }

    private static void RemoveUser()
    {
        Console.WriteLine("Enter the email address of the user you want to remove:");
        string email = Console.ReadLine();

        AccountsLogic accountsLogic = new AccountsLogic();
        bool isRemoved = accountsLogic.RemoveUser(email);

        if (isRemoved)
        {
            Console.WriteLine($"User with email address ‘{email}’ has been removed.");
        }
        else
        {
            Console.WriteLine($"No user found with email address ‘{email}'.");
        }
        
        Start(); // Terug naar het adminmenu
    }

    private static void RemoveMovie()
    {
        Console.WriteLine("Voer het ID van de filmvertoning in die je wilt verwijderen:");
        int id = int.Parse(Console.ReadLine());

        ShowingsLogic showingsLogic = new ShowingsLogic();
        showingsLogic.RemoveShowing(id);
        Start(); // Terug naar het adminmenu
    }

    private static void ViewUsers()
    {
        var users = AccountsAccess.LoadAll(); 
        foreach (var user in users)
        {
            Console.WriteLine($"User: {user.EmailAddress}");
        }
        Start(); 
    }

    private static void ViewMovies()
    {
        ShowingsLogic showingsLogic = new ShowingsLogic();

        Console.WriteLine("\nAll movie screenings:");
        
        Console.WriteLine("----------------------------------------");
        
        
        string allShowings = showingsLogic.ShowAll();
        Console.WriteLine(allShowings);

        Console.WriteLine("\nPress any key to go back…");
        Console.ReadKey();
        Start();
        
    }
} 
