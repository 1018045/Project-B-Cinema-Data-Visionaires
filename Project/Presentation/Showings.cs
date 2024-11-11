using System.Globalization;

public static class Showings
{
    static private ShowingsLogic _showingsLogic = new ShowingsLogic();

    public static void ShowAll()
    {
        Console.WriteLine(_showingsLogic.ShowAll());
    }

    public static void ShowUpcoming()
    {
        Console.WriteLine(_showingsLogic.ShowUpcoming());
    }

    public static void ShowUpcomingOnDate()
    {     
        DateTime date = ParseDateTime();
        Console.WriteLine(_showingsLogic.ShowUpcoming(date));       
    }

    private static DateTime ParseDateTime()
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