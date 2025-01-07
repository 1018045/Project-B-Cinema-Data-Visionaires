using System.Globalization;

public static class Showings
{
    static private ShowingsLogic _showingsLogic = new ShowingsLogic();
    private static CinemaLogic _cinemaLogic = new CinemaLogic();

    public static void ShowAll()
    {
        Console.WriteLine(_showingsLogic.ShowAll());
    }

    public static int SelectMovie(MoviesLogic moviesLogic)
    {
        Console.Clear();

        List<MovieModel> movieList = moviesLogic.Movies;
        List<string> movies = movieList.Select(movie => movie.Title).ToList();
        movies.Add("Cancel");

        List<int> moviesIndices = movieList.Select(movie => movie.Id).ToList();
        moviesIndices.Add(-1);

        return MenuHelper.NewMenu(movies, moviesIndices, "Which movie do you want to manage?");
    }

    public static int SelectShowing(int movieId)
    {
        Console.Clear();

        List<ShowingModel> showings = _showingsLogic.Showings.Where(s => s.MovieId == movieId).ToList();
        List<string> showingsStrings = showings.Select(showing => showing.Date.ToString("dd-MM-yyyy HH:mm:ss")).ToList();
        showingsStrings.Add("Cancel");

        List<int> showingsIndices = showings.Select(showing => showing.Id).ToList();
        showingsIndices.Add(-1);

        return MenuHelper.NewMenu(showingsStrings, showingsIndices, "Which showing do you want to manage?");
    }

    public static void ManageShowings()
    {
        MoviesLogic moviesLogic = new();
        int chosenId = SelectMovie(moviesLogic); 
        if (chosenId != -1)
        {
            List<string> options = new List<string>
            {
                "View showings",
                "Add showing(s)",
                "Remove showing",
                "Return"
            };
            List<Action> actions = new List<Action>
            {
                () => ShowShowings(chosenId, moviesLogic),
                () => AddShowing(chosenId, moviesLogic),
                () => RemoveShowing(chosenId, moviesLogic),
                Menus.AdminMenu
            };
            MenuHelper.NewMenu(options, actions, $"What would you like to do with {moviesLogic.GetMovieById(chosenId).Title}?");
        }
        else
        {
            Menus.AdminMenu();
        }
    }

    public static void ManageShowings(int chosenId, MoviesLogic moviesLogic)
    {
        if (chosenId != -1)
        {
            List<string> options = new List<string>
            {
                "View showings",
                "Add showing(s)",
                "Remove showing",
                "Return"
            };
            List<Action> actions = new List<Action>
            {
                () => ShowShowings(chosenId, moviesLogic),
                () => AddShowing(chosenId, moviesLogic),
                () => RemoveShowing(chosenId, moviesLogic),
                Menus.AdminMenu
            };
            MenuHelper.NewMenu(options, actions, $"What would you like to do with {moviesLogic.GetMovieById(chosenId).Title}?");
        }
        else
        {
            Menus.AdminMenu();
        }
    }

    private static void ListShowings(int movieId, MoviesLogic moviesLogic)
    {
        List<ShowingModel> showings = _showingsLogic.FindShowingsByMovieId(movieId);
        if (showings.Count == 0)
        {
            Console.Clear();
            System.Console.WriteLine("There are no showings planned yet for this movie");
        }
        else
        {   
            Console.Clear();
            int counter = 1;
            Console.WriteLine($"Current upcoming showings for {moviesLogic.GetMovieById(movieId).Title} are:");
            foreach (ShowingModel showing in showings)
            {
                System.Console.WriteLine($"{counter++}. {showing.Date.ToString("dd-MM-yyyy HH:mm:ss")} in room {showing.Room}");
            }
        }
    }

    private static void ShowShowings(int movieId, MoviesLogic moviesLogic)
    {
        ListShowings(movieId, moviesLogic);
        MenuHelper.WaitForKey(() => ManageShowings(movieId, moviesLogic));
    }

    private static void AddShowing(int movieId, MoviesLogic moviesLogic)
    {
        Console.Clear();
        List<string> cinemaOptions = _cinemaLogic.Cinemas.Select(c => c.Name).ToList();
        List<int> cinemaIndices = _cinemaLogic.Cinemas.Select(c => c.Id).ToList();

        cinemaOptions.Add("Cancel");
        cinemaIndices.Add(-1);
        int cinemaId = MenuHelper.NewMenu(cinemaOptions, cinemaIndices, "Which cinema do you want to add showings to?");
        if (cinemaId == -1)
        {
            Menus.AdminMenu();
            return;
        }

        // ListShowings(movieId, moviesLogic);
        DateTime date;
        do
        {
            date = AskAndParseDateAndTime();
        } while (date < DateTime.Now);

        bool correct;
        int room;
        do
        {
            System.Console.WriteLine("Which room will the showing be in? (1, 2, or 3)");
            correct = int.TryParse(Console.ReadLine(), out room) && room > 0 && room < 4;          
        } while (!correct);

        Console.WriteLine("Select a special for this showing:");
        Console.WriteLine("1. Premier");
        Console.WriteLine("2. Dolby");
        Console.WriteLine("3. none");

        string special = Console.ReadLine() switch
        {
            "1" => "Premier",
            "2" => "Dolby",
            _ => "none"
        };

        List<string> options = new List<string>
        {
            "Single showing",
            "Repeat daily",
            "Repeat weekly"
        };
        List<int> outputs = new List<int> {1, 2, 3};

        int userChoice = MenuHelper.NewMenu(options, outputs, "Do you want to repeat the showing?");
        string pattern = "";

        switch (userChoice)
        {
            case 1:
                BookShowings(date, room, moviesLogic, movieId, cinemaId, special);
                break;
            case 2:
                pattern = "daily";
                break;
            case 3:
                pattern = "weekly";
                break;
            default:
                ManageShowings(movieId, moviesLogic);
                break;
        }
        
        if (pattern == "daily")
        {
            Console.Clear();
            System.Console.WriteLine("How many days do you want to repeat the showing?");
            int res;
            string input;
            do
            {
                input = Console.ReadLine();
            } while (!int.TryParse(input, out res));
            for (int i = 0; i < res; i++)
            {
                BookShowings(date.AddDays(i), room, moviesLogic, movieId, cinemaId, special);
            }
        } 
        else if (pattern == "weekly")
        {
            Console.Clear();
            System.Console.WriteLine("How many weeks do you want to repeat the showing?");
            int res;
            string input;
            do
            {
                input = Console.ReadLine();
            } while (!int.TryParse(input, out res));
            for (int i = 0; i < res; i++)
            {
                BookShowings(date.AddDays(i * 7), room, moviesLogic, movieId, cinemaId, special);
            }
        }

        MenuHelper.WaitForKey(() => ManageShowings(movieId, moviesLogic));
    }

    private static void BookShowings(DateTime date, int room, MoviesLogic moviesLogic, int movieId, int chosenCinemaId, string special)
    {
        if (!_showingsLogic.IsRoomFree(date, room, moviesLogic.GetMovieById(movieId).Duration, chosenCinemaId))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"Room {room} is not available on {date.ToString("dd-MM-yyyy HH:mm:ss")}");
            Console.ResetColor();
            Thread.Sleep(250);
            return;
        }

        _showingsLogic.AddShowing(movieId, date, room, chosenCinemaId, special);
        Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"Showing has been successfully added on {date.ToString("dd-MM-yyyy HH:mm:ss")} with special: {special}");
        Console.ResetColor();
        Thread.Sleep(500);
    }

    private static void RemoveShowing(int movieId, MoviesLogic moviesLogic)
    {
        int chosenShowing = SelectShowing(movieId);
        if (chosenShowing != -1) _showingsLogic.RemoveShowing(chosenShowing);
        MenuHelper.WaitForKey(() => ManageShowings(movieId, moviesLogic));
    }

    private static DateTime AskAndParseDateAndTime()
    {  
        string dateInput;
        string timeInput;
        do
        {
            Console.Clear();
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