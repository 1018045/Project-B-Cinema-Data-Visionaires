public static class AccountantMenu
{
    public static void Start()
    {
        if (!Login())
        {
            Console.WriteLine("Login failed. Returning to main menu.");
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
                    ViewRecordsByDate();
                    break;
                case "3":
                    ViewTotalMoney();
                    break;
                case "4":
                    ViewTotalTickets();
                    break;
                case "5":
                    return; // Ga terug naar hoofdmenu
                default:
                    Console.WriteLine("Invalid choice. Press any key to try again.");
                    Console.ReadKey();
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

        // Simpele inlogcontrole (vervang dit met een echte controle)
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
            Console.WriteLine($"{record.Id} | {record.Date} | {record.MovieTitle} | " +
                            $"{record.TicketsSold} | ${record.Revenue} | {record.Room}");
        }

        WaitForKey();
    }

    private static void ViewRecordsByDate()
    {
        Console.WriteLine("\nEnter date (yyyy-MM-dd):");
        string date = Console.ReadLine();

        // Haal alle records op en filter op datum
        var records = AccountantAccess.LoadAll().Where(r => r.Date == date).ToList();

        if(records.Any())
        {
            Console.WriteLine($"\nRecords for {date}:");
            foreach(var record in records)
            {
                Console.WriteLine($"Movie: {record.MovieTitle}");
                Console.WriteLine($"Tickets sold: {record.TicketsSold}");
                Console.WriteLine($"Money earned: ${record.Revenue}");
                Console.WriteLine($"Room: {record.Room}");
                Console.WriteLine("---------------");
            }
        }
        else
        {
            Console.WriteLine($"No records found for {date}");
        }

        WaitForKey();
    }

    private static void ViewTotalMoney()
    {
        // Haal alle records op en tel de inkomsten bij elkaar op
        var records = AccountantAccess.LoadAll();
        decimal totalMoney = records.Sum(r => r.Revenue);

        Console.WriteLine($"\nTotal money earned: ${totalMoney}");
        WaitForKey();
    }

    private static void ViewTotalTickets()
    {
        // Haal alle records op en tel de tickets bij elkaar op
        var records = AccountantAccess.LoadAll();
        int totalTickets = records.Sum(r => r.TicketsSold);

        Console.WriteLine($"\nTotal tickets sold: {totalTickets}");
        WaitForKey();
    }

    // Hulpmethode om te wachten op gebruikersinput
    private static void WaitForKey()
    {
        Console.WriteLine("\nPress any key to go back...");
        Console.ReadKey();
    }
}
