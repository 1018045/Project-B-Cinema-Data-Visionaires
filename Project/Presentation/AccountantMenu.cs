public static class AccountantMenu
{
    public static void Start()
    {
        if (!Login())
        {
            Console.WriteLine("Login failed. Returning to main menu.");
            Start();
            return;
        }

        while (true)
        {
            // Maak het scherm leeg voor betere leesbaarheid
            Console.Clear();

            // Toon menu opties
            Console.WriteLine("\n=== Accountant Menu ===");
            Console.WriteLine("1. View all financial records");
            Console.WriteLine("2. View records for a date");
            Console.WriteLine("3. View total money earned");
            Console.WriteLine("4. View total tickets sold");
            Console.WriteLine("5. Return to main menu");
            Console.WriteLine("=======================");

            // Vraag gebruiker om een keuze
            string choice = Console.ReadLine();

            // Verwerk de keuze van de gebruiker
            switch (choice)
            {
                case "1":
                    ViewAllRecords();
                    break;
                case "2":
                    //ViewRecordsByDate();
                    break;
                case "3":
                    //ViewTotalMoney();
                    break;
                case "4":
                    //ViewTotalTickets();
                    break;
                case "5":
                    LoginMenu.Start();
                    break; // Ga terug naar hoofdmenu
                default:
                    Console.WriteLine("Invalid choice. Press any key to try again.");
                    Console.ReadKey();
                    Menu.Start();
                    break;
            }
        }
    }

    private static bool Login()
    {
        Console.WriteLine("Enter accountant username:");
        string username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        // Simpele inlogcontrole
        return username == "accountant" && password == "accountant123";
    }

    private static void ViewAllRecords()
    {
        // Haal alle records op via AccountantAccess
        var records = AccountantAccess.LoadAll();
        
        Console.WriteLine("\nAll Financial Records:");
        Console.WriteLine("ID | Date | Movie | Tickets | Money | Room");
        Console.WriteLine("----------------------------------------");
        
        // Toon elk record
        foreach(var record in records)
        {
            // Console.WriteLine($"{record.Id} | {record.Date} | {record.MovieTitle} | " +
                           // $"{record.TicketsSold} | ${record.Revenue} | {record.Room}");
        }

        WaitForKey();
    }

    private static void WaitForKey()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}
