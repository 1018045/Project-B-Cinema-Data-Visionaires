using System;
using System.Collections.Generic;

static class AdminMenu
{
    public static void Start()
    {
        Console.WriteLine("Admin Menu:");
        Console.WriteLine("1. Voeg een film toe");
        Console.WriteLine("2. Verwijder een gebruiker");
        Console.WriteLine("3. Bekijk alle gebruikers");
        Console.WriteLine("4. Verwijder een filmvertoning");
        Console.WriteLine("5. Bekijk alle films");
        Console.WriteLine("6. Terug naar hoofdmenu");

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
                Console.WriteLine("Ongeldige keuze, probeer het opnieuw.");
                Start();
                break;
        }
    }

    private static void AddMovie()
    {
        Console.WriteLine("Voer de filmtitel in:");
        string title = Console.ReadLine();

        Console.WriteLine("Voer de datum in (dd-MM-yyyy HH:mm:ss):");
        string date = Console.ReadLine();

        Console.WriteLine("Voer de zaal in:");
        int room = int.Parse(Console.ReadLine());

        Console.WriteLine("Voer de minimumleeftijd in:");
        int minimumAge = int.Parse(Console.ReadLine());

        // Maak een instantie van ShowingsLogic
        ShowingsLogic showingsLogic = new ShowingsLogic();
        
        // Voeg de nieuwe vertoning toe via de logica-laag
        showingsLogic.AddShowing(title, date, room, minimumAge);

        Console.WriteLine($"Filmvertoning '{title}' is toegevoegd.");
        Start(); // Terug naar het adminmenu
    }

    private static void RemoveUser()
    {
        Console.WriteLine("Voer het e-mailadres van de gebruiker in die je wilt verwijderen:");
        string email = Console.ReadLine();

        AccountsLogic accountsLogic = new AccountsLogic();
        bool isRemoved = accountsLogic.RemoveUser(email);

        if (isRemoved)
        {
            Console.WriteLine($"Gebruiker met e-mailadres '{email}' is verwijderd.");
        }
        else
        {
            Console.WriteLine($"Geen gebruiker gevonden met e-mailadres '{email}'.");
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
            Console.WriteLine($"Gebruiker: {user.EmailAddress}");
        }
        Start(); 
    }

    private static void ViewMovies()
    {
        ShowingsLogic showingsLogic = new ShowingsLogic();
        var movies = showingsLogic.GetAllShowings();

        Console.WriteLine("\nAlle filmvertoningen:");
        Console.WriteLine("ID | Titel | Datum | Zaal | Minimumleeftijd");
        Console.WriteLine("----------------------------------------");
        
        foreach (var movie in movies)
        {
            Console.WriteLine($"{movie.Id} | {movie.Title} | {movie.Date} | Zaal {movie.Room} | {movie.MinimumAge}+");
        }

        Console.WriteLine("\nDruk op een toets om terug te gaan...");
        Console.ReadKey();
        Start();
    }
} 
