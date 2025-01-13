using System.Globalization;

public class Showings
{
    private LogicManager _logicManager;
    private MenuManager _menuManager;

    public Showings(LogicManager logicManager, MenuManager menuManager)
    {
        _logicManager = logicManager;
        _menuManager = menuManager;
    }

    public void ShowAll()
    {
        Console.WriteLine(_logicManager.ShowingsLogic.ShowAll());
    }

    public int SelectMovie(MoviesLogic moviesLogic)
    {
        Console.Clear();

        List<MovieModel> movieList = moviesLogic.Movies;
        List<string> movies = movieList.Select(movie => movie.Title).ToList();
        movies.Add("Cancel");

        List<int> moviesIndices = movieList.Select(movie => movie.Id).ToList();
        moviesIndices.Add(-1);

        return MenuHelper.NewMenu(movies, moviesIndices, "Which movie do you want to manage?");
    }

    public int SelectShowing(int movieId)
    {
        Console.Clear();

        List<ShowingModel> showings = _logicManager.ShowingsLogic.FindShowingsByMovieId(movieId);
        List<string> showingsStrings = showings.Select(showing => showing.Date.ToString("dd-MM-yyyy HH:mm:ss")).ToList();
        showingsStrings.Add("Cancel");

        List<int> showingsIndices = showings.Select(showing => showing.Id).ToList();
        showingsIndices.Add(-1);

        return MenuHelper.NewMenu(showingsStrings, showingsIndices, "Which showing do you want to manage?");
    }

    public void ManageShowings()
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
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
                () => ShowShowings(chosenId),
                () => AddShowing(chosenId),
                () => RemoveShowing(chosenId),
                _menuManager.MainMenus.AdminMenu
            };
            MenuHelper.NewMenu(options, actions, $"What would you like to do with {moviesLogic.GetMovieById(chosenId).Title}?");
        }
        else
        {
            _menuManager.MainMenus.AdminMenu();
        }
    }

    public void ManageShowings(int chosenId)
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
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
                () => ShowShowings(chosenId),
                () => AddShowing(chosenId),
                () => RemoveShowing(chosenId),
                _menuManager.MainMenus.AdminMenu
            };
            MenuHelper.NewMenu(options, actions, $"What would you like to do with {moviesLogic.GetMovieById(chosenId).Title}?");
        }
        else
        {
            _menuManager.MainMenus.AdminMenu();
        }
    }

    private void ListShowings(int movieId)
    {
        List<ShowingModel> showings = _logicManager.ShowingsLogic.FindShowingsByMovieId(movieId);
        if (showings.Count == 0)
        {
            Console.Clear();
            System.Console.WriteLine("There are no showings planned yet for this movie");
        }
        else
        {   
            Console.Clear();
            int counter = 1;
            Console.WriteLine($"Current upcoming showings for {_logicManager.MoviesLogic.GetMovieById(movieId).Title} are:");
            foreach (ShowingModel showing in showings)
            {
                System.Console.WriteLine($"{counter++}. {showing.Date.ToString("dd-MM-yyyy HH:mm:ss")} in room {showing.Room}");
            }
        }
    }

    private void ShowShowings(int movieId)
    {
        ListShowings(movieId);
        MenuHelper.WaitForKey(() => ManageShowings(movieId));
    }

    private void AddShowing(int movieId)
    {
        CinemaLogic cinemaLogic = _logicManager.CinemaLogic;
        Console.Clear();
        List<string> cinemaOptions = cinemaLogic.Cinemas.Select(c => c.Name).ToList();
        List<int> cinemaIndices = cinemaLogic.Cinemas.Select(c => c.Id).ToList();

        cinemaOptions.Add("Cancel");
        cinemaIndices.Add(-1);
        int cinemaId = MenuHelper.NewMenu(cinemaOptions, cinemaIndices, "Which cinema do you want to add showings to?");
        if (cinemaId == -1)
        {
            _menuManager.MainMenus.AdminMenu();
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

        bool is3d = MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, "Will this showing be in 3D?");

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

        bool answer = MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: "Would you like to add extra's?");
        List<ExtraModel> extras = new();
        if (answer)
        {  
            int extraCount;
            string extraCountTemp;
            do  
            {   
                Console.Clear();
                Console.WriteLine("How many extras would you like to add?");
                extraCountTemp = Console.ReadLine();
            } while(!int.TryParse(extraCountTemp, out extraCount));
   
            for (int i = 0; i < extraCount; i++)
            {
                Console.WriteLine($"Adding extra {i + 1} of {extraCount}:");

                Console.WriteLine("What will be the name for this extra?");
                string extraName = Console.ReadLine();
       
                Console.WriteLine("What will be the price for this extra?");
                decimal extraPrice = decimal.Parse(Console.ReadLine());
            
                Console.WriteLine("Is this extra mandatory?");
                bool mandatory = MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: "Is this extra mandatory?");
              
                ExtraModel extra = new(extraName, extraPrice, mandatory);
             
                extras.Add(extra);
            }
            Console.WriteLine($"{extraCount} extra(s) have been successfully added!");
        }

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
                BookShowings(date, room, movieId, cinemaId, is3d,special,extras);
                break;
            case 2:
                pattern = "daily";
                break;
            case 3:
                pattern = "weekly";
                break;
            default:
                ManageShowings(movieId);
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
                BookShowings(date.AddDays(i), room, movieId, cinemaId, is3d, special, extras);
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
                BookShowings(date.AddDays(i * 7), room, movieId, cinemaId, is3d, special, extras);
            }
        }
        MenuHelper.WaitForKey(() => ManageShowings(movieId));
    }

    private void BookShowings(DateTime date, int room, int movieId, int chosenCinemaId, bool is3d, string special, List<ExtraModel> extras)
    {
        ShowingsLogic showingsLogic = _logicManager.ShowingsLogic;
        if (!showingsLogic.IsRoomFree(date, room, _logicManager.MoviesLogic.GetMovieById(movieId).Duration, chosenCinemaId))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"Room {room} is not available on {date.ToString("dd-MM-yyyy HH:mm:ss")}");
            Console.ResetColor();
            Thread.Sleep(250);
            return;
        }

        showingsLogic.AddShowing(movieId, date, room, chosenCinemaId, is3d,special,extras);
        Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"Showing has been successfully added on {date.ToString("dd-MM-yyyy HH:mm:ss")} with special: {special}");
        Console.ResetColor();
        Thread.Sleep(500);
    }

    private void RemoveShowing(int movieId)
    {
        int chosenShowing = SelectShowing(movieId);
        if (chosenShowing != -1) _logicManager.ShowingsLogic.RemoveShowing(chosenShowing);
        MenuHelper.WaitForKey(() => ManageShowings(movieId));
    }

    private DateTime AskAndParseDateAndTime()
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