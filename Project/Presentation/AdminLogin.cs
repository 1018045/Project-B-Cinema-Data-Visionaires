using System;

static class AdminLogin
{
    public static void Start()
    {
        Console.WriteLine("Enter your email address:");
        string email = Console.ReadLine();
        Console.WriteLine("Enter your password:");
        string password = Console.ReadLine();

        var admins = AdminAccess.LoadAll();
        var admin = admins.FirstOrDefault(a => a.EmailAddress == email && a.Password == password);

        if (admin != null)
        {
            Console.WriteLine("Login successful! Welcome," + admin.EmailAddress);
            AdminMenu.Start();
            
        }
        else
        {
            Console.WriteLine("Invalid login credentials. Please try again.");
            Start(); // Herstart de login
        }
    }
}
