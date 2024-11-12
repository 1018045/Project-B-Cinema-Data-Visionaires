using System.Globalization;

public static class Showings
{
    static private ShowingsLogic _showingsLogic = new ShowingsLogic();

    public static void ShowAll()
    {
        Console.WriteLine(_showingsLogic.ShowAll());
    }

    public static void ShowUpcoming(bool makingReservation = false)
    {
        string showingsOutput = _showingsLogic.ShowUpcoming(showId: makingReservation);
        if (showingsOutput == "")
            System.Console.WriteLine("No showings found");
        else
            System.Console.WriteLine(showingsOutput);    
    }

    public static void ShowUpcomingOnDate()
    {     
        DateTime date = AskAndParseDateTime();
        string showingsOutput = _showingsLogic.ShowUpcoming(date);
                if (showingsOutput == "")
            System.Console.WriteLine($"No showings found on {date:dd-MM-yyyy}\n");
        else
            System.Console.WriteLine(showingsOutput);
    }

    private static DateTime AskAndParseDateTime()
    {  
        string dateInput;
        do
        {
            System.Console.WriteLine("Please enter the date in the format 'dd-MM-yyyy'");
            dateInput = Console.ReadLine();
        }
        while(!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _));             
        return DateTime.ParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture);
    }
}