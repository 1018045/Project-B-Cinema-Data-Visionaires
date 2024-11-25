using System.Globalization;

public static class Showings
{
    static private ShowingsLogic _showingsLogic = new ShowingsLogic();

    public static void ShowAll()
    {
        Console.WriteLine(_showingsLogic.ShowAll());
    }

    public static void ManageShowings()
    {
        MoviesLogic moviesLogic = new();
        Console.WriteLine("Which movie do you want to manage?");
        Thread.Sleep(100);
        System.Console.WriteLine(moviesLogic.ListMovies());
        int chosenId = int.Parse(Console.ReadLine()) - 1;
        
        bool DoMore = true;
        while (DoMore)
        {
            System.Console.WriteLine($"What would you like to do with {moviesLogic.GetMovieById(chosenId).Title}?");
            System.Console.WriteLine("1. View showings");
            System.Console.WriteLine("2. Add showings");
            System.Console.WriteLine("3. Remove showings");
            System.Console.WriteLine("4. Go back to the menu");
            string userChoice = Console.ReadLine();
            switch (userChoice.Trim())
            {
                case "1":
                    ShowShowings(chosenId, moviesLogic);
                    break;
                case "2":
                    AddShowing(chosenId, moviesLogic);
                    break;
                case "3":
                    RemoveShowing(chosenId, moviesLogic);
                    break;
                case "4":
                    DoMore = false;
                    break;
                default:
                    System.Console.WriteLine("Invalid input");
                    break;
            }
        }
    }

    private static void ShowShowings(int movieId, MoviesLogic moviesLogic)
    {
        List<ShowingModel> showings = _showingsLogic.FindShowingsByMovieId(movieId);
        if (showings.Count == 0)
        {
            System.Console.WriteLine("There are no showings planned yet for this movie");
            return;
        }
        int counter = 1;
        Console.WriteLine($"Current upcoming showings for {moviesLogic.GetMovieById(movieId).Title} are:");
        foreach (ShowingModel showing in showings)
        {
            System.Console.WriteLine($"{counter++}. {showing.Date} in room {showing.Room}");
        }
    }

    private static void AddShowing(int movieId, MoviesLogic moviesLogic)
    {
        ShowShowings(movieId, moviesLogic);
        System.Console.WriteLine("Which date and time do you want this show on?");
        DateTime date = AskAndParseDateAndTime();

        bool correct;
        int room;
        do
        {
            System.Console.WriteLine("Which room will this showing be in? (1, 2, or 3)");
            correct = int.TryParse(Console.ReadLine(), out room) && room > 0 && room < 4;          
        } while (!correct);
        if (!_showingsLogic.IsRoomFree(date, room, moviesLogic.GetMovieById(movieId).Duration, moviesLogic))
        {
            System.Console.WriteLine($"The room you selected is not available on {date}");
            return;
        }
        _showingsLogic.AddShowing(movieId, date, room);
        System.Console.WriteLine("Showing has been succesfully added to the planning");
    }

    private static void RemoveShowing(int movieId, MoviesLogic moviesLogic)
    {
        ShowShowings(movieId, moviesLogic);
        List<ShowingModel> tempList = _showingsLogic.FindShowingsByMovieId(movieId);
        int chosenId;
        bool correct;
        do
        {
            System.Console.WriteLine("Which showing do you want to remove? Type the number in front of the showing");
            correct = int.TryParse(Console.ReadLine(), out chosenId) && chosenId > 0 && chosenId <= _showingsLogic.FindShowingsByMovieId(movieId).Count;          
        } while (!correct);
        _showingsLogic.RemoveShowing(tempList[chosenId - 1]);
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
        DateTime date = AskAndParseDate();
        string showingsOutput = _showingsLogic.ShowUpcoming(date);
        if (showingsOutput == "")
            System.Console.WriteLine($"No showings found on {date:dd-MM-yyyy}\n");
        else
            System.Console.WriteLine(showingsOutput);
    }

    private static DateTime AskAndParseDate()
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

    private static DateTime AskAndParseDateAndTime()
    {  
        string dateInput;
        do
        {
            System.Console.WriteLine("Please enter the date in the format 'dd-MM-yyyy HH:mm:ss'");
            dateInput = Console.ReadLine();
        }
        while(!DateTime.TryParseExact(dateInput, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _));             
        return DateTime.ParseExact(dateInput, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    }
}