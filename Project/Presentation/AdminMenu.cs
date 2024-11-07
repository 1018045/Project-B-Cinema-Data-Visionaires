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
        Console.WriteLine("4. Terug naar hoofdmenu");

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
                LoginMenu.Start(); // Terug naar het hoofdmenu
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
        // Voeg hier logica toe om de film op te slaan
        
        Console.WriteLine($"Film '{title}' is toegevoegd.");
        Start(); // Terug naar het adminmenu
    }

    private static void RemoveUser()
    {
        Console.WriteLine("Voer het e-mailadres van de gebruiker in die je wilt verwijderen:");
        string email = Console.ReadLine();
        // Voeg hier logica toe om de gebruiker te verwijderen
        Console.WriteLine($"Gebruiker met e-mailadres '{email}' is verwijderd.");
        Start(); // Terug naar het adminmenu
    }

    private static void ViewUsers()
    {
        var users = AdminAccess.LoadAll(); // Zorg ervoor dat je een methode hebt om gebruikers te laden
        foreach (var user in users)
        {
            Console.WriteLine($"Gebruiker: {user.EmailAddress}");
        }
        Start(); // Terug naar het adminmenu
    }
} 
