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
}