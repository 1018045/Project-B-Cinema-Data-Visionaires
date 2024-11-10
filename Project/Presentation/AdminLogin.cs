using System;

static class AdminLogin
{
    public static void Start()
    {
        Console.WriteLine("Voer uw e-mailadres in:");
        string email = Console.ReadLine();
        Console.WriteLine("Voer uw wachtwoord in:");
        string password = Console.ReadLine();

        var admins = AdminAccess.LoadAll();
        var admin = admins.FirstOrDefault(a => a.EmailAddress == email && a.Password == password);

        if (admin != null)
        {
            Console.WriteLine("Login succesvol! Welkom, " + admin.EmailAddress);
            AdminMenu.Start();
            
        }
        else
        {
            Console.WriteLine("Ongeldige inloggegevens. Probeer het opnieuw.");
            Start(); // Herstart de login
        }
    }
}
