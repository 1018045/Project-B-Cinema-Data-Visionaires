using System.Drawing;
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
        int returnButton = moviesLogic.GetSize();
        System.Console.WriteLine($"{returnButton + 1}: Go back");

        int chosenId = int.Parse(Console.ReadLine()) - 1;
        if (chosenId == returnButton)
            return;

        bool DoMore = true;
        while (DoMore)
        {
            System.Console.WriteLine($"What would you like to do with {moviesLogic.GetMovieById(chosenId).Title}?");
            System.Console.WriteLine("1. View showings");
            System.Console.WriteLine("2. Add showings");
            System.Console.WriteLine("3. Remove showings");
            System.Console.WriteLine("4. Go back");
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
            System.Console.WriteLine($"{counter++}. {showing.Date.ToString("dd-MM-yyyy HH:mm:ss")} in room {showing.Room}");
        }
    }

    private static void AddShowing(int movieId, MoviesLogic moviesLogic)
    {
        ShowShowings(movieId, moviesLogic);
        DateTime date = AskAndParseDateAndTime();

        bool correct;
        int room;
        do
        {
            System.Console.WriteLine("Which room will this showing be in? (1, 2, or 3)");
            correct = int.TryParse(Console.ReadLine(), out room) && room > 0 && room < 4;          
        } while (!correct);

        System.Console.WriteLine("Is this a repeat showing?");
        System.Console.WriteLine("1. Repeat daily");
        System.Console.WriteLine("2. Repeat weekly");
        System.Console.WriteLine("3. Only add a single showing");
        string userChoice;
        bool DoMore = true;
        string pattern = "";
        do
        {    
            userChoice = Console.ReadLine();
            switch (userChoice)
            {
                case "1":
                    pattern = "daily";
                    DoMore = false;
                    break;
                case "2":
                    pattern = "weekly";
                    DoMore = false;
                    break;
                case "3":
                    BookShowings(date, room, moviesLogic, movieId);
                    return;
                default:
                    break;
            }
        } while(DoMore);
        
        if (pattern == "daily")
        {
            System.Console.WriteLine("How many days do you want to repeat this showing?");
            int res;
            string input;
            do
            {
                input = Console.ReadLine();
            } while (!int.TryParse(input, out res));
            for (int i = 0; i < res; i++)
            {
                BookShowings(date.AddDays(i), room, moviesLogic, movieId);
            }
        } 
        else if (pattern == "weekly")
        {
            System.Console.WriteLine("How many weeks do you want to repeat this showing?");
            int res;
            string input;
            do
            {
                input = Console.ReadLine();
            } while (!int.TryParse(input, out res));
            for (int i = 0; i < res; i++)
            {
                BookShowings(date.AddDays(i * 7), room, moviesLogic, movieId);
            }
        }
    }

    private static void BookShowings(DateTime date, int room, MoviesLogic moviesLogic, int movieId)
    {
        if (!_showingsLogic.IsRoomFree(date, room, moviesLogic.GetMovieById(movieId).Duration, moviesLogic))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"Room {room} is not available on {date.ToString("dd-MM-yyyy HH:mm:ss")}");
            Console.ResetColor();
            return;
        }
        _showingsLogic.AddShowing(movieId, date, room);
        Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"Showing on {date.ToString("dd-MM-yyyy HH:mm:ss")} has been succesfully added to the planning");
        Console.ResetColor();
        Thread.Sleep(500);
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
        string timeInput;
        do
        {
            System.Console.WriteLine("Which date do you want the showing on? Format 'dd-MM-yyyy'");
            dateInput = Console.ReadLine();
        }
        while(!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _));
        do
        {
            System.Console.WriteLine("At what time do you want this showing? Format 'HH:mm:ss'");
            timeInput = Console.ReadLine();
        }
        while(!DateTime.TryParseExact(timeInput, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _));
        string output = $"{dateInput.Trim()} {timeInput.Trim()}";
        return DateTime.ParseExact(output, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    }
}