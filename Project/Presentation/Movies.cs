using System.Text.RegularExpressions;
using Project.Helpers;
using Project.Logic.Account;

public class Movies
{
    private const string DATEFORMAT = "dd-MM-yyyy HH:mm:ss";
    private const string EXTENDED_DATE_FORMAT = "dddd d MMMM yyyy";

    private LogicManager _logicManager;
    private MenuManager _menuManager;


    public Movies(LogicManager logicManager, MenuManager menuManager)
    {
        _logicManager = logicManager;
        _menuManager = menuManager;
    }

    public void Start()
    {
        MenuHelper.NewMenu(
            new List<string> {"Manage movies", "Manage archived movies", "Return"},
            new List<Action> {ManageMovies, ManageArchivedMovies, _menuManager.MainMenus.AdminMenu}
        );
    }

    private void ManageMovies()
    {
        int chosenId = MovieSelector(_logicManager.MoviesLogic.Movies, new List<(string, int)> { ("[Add movie]", -2) });
        if (chosenId == -1)
        { 
            Start();
            return;
        }
        else if (chosenId == -2)
        {
            AddMovie();
            return;
        }
        MovieManager(chosenId);
    }

    private void MovieManager(int id)
    {
        MenuHelper.NewMenu(
            new List<string> {"Edit movie details (TODO)", "Archive movie", "Return"},
            new List<Action> {() => EditMovieDetails(id), () => ArchiveMovie(id), ManageMovies},
            _logicManager.MoviesLogic.GetMovieById(id).Title);
    }

    private void ManageArchivedMovies()
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
        if (moviesLogic.ArchivedMovies.Count == 0)
        {
            Console.WriteLine("There are 0 movies in the archive");
            Thread.Sleep(1000);
            MenuHelper.WaitForKey(_menuManager.MainMenus.AdminMenu);
            return;
        }

        int chosenId = MovieSelector(moviesLogic.ArchivedMovies);

        if (chosenId == -1)
        { 
            Start();
            return;
        }

        ArchivedMovieManager(chosenId);
    }

    private void ArchivedMovieManager(int id)
    {
        MenuHelper.NewMenu(
            new List<string> {"Edit movie details (TODO)", "Take movie out of archive", "Return"},
            new List<Action> {() => EditMovieDetails(id), () => BringArchivedMovieBack(id), ManageArchivedMovies});
    }

    private void AddMovie()
    {
        Console.Clear();
        Console.WriteLine("Enter the movie title:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter the total screen time in minutes:");
        int duration = Math.Abs(int.Parse(Console.ReadLine()));

        Console.WriteLine("Enter the minimum age (11-18):");
        int minimumAge = Math.Clamp(int.Parse(Console.ReadLine()), 11, 18);

        Console.WriteLine("Enter a summary of the movie:");
        string summary = Console.ReadLine();

        Console.WriteLine("Enter the main cast seperated by comma's");
        List<string> actors = Console.ReadLine().Split(",").ToList();
        actors.ForEach(a => a.Trim());

        Console.WriteLine("Enter the movie's director:");
        string director = Console.ReadLine();

        _logicManager.MoviesLogic.AddMovie(title, duration, minimumAge, summary, actors, director);
        Console.WriteLine($"Movie ‘{title}’ has been added to the database.");

        // System.Console.WriteLine("Which extra's are mandatory for this movie?");
        // TODO
        Thread.Sleep(1000);
        MenuHelper.WaitForKey(ManageMovies);
    }

    private void BringArchivedMovieBack(int id)
    { 
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
        MovieModel movie = moviesLogic.GetMovieById(id);

        moviesLogic.Movies.Add(movie);
        moviesLogic.ArchivedMovies.Remove(movie);
        MoviesAccess.WriteAll(moviesLogic.Movies);
        ArchivedMoviesAccess.WriteAll(moviesLogic.ArchivedMovies);

        Console.Clear();
        Console.WriteLine($"Moved {movie.Title} from archive into active movies");
        Thread.Sleep(1000);
        MenuHelper.WaitForKey(ManageArchivedMovies);
    }

    private void ArchiveMovie(int id)
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
        MovieModel movie = moviesLogic.GetMovieById(id);

        if (moviesLogic.HasUpcomingShowings(_logicManager.ShowingsLogic, movie))
        {
            Console.WriteLine($"Error: {movie.Title} still has upcoming showings");
            Console.WriteLine("Please remove these showings and try again");
            Thread.Sleep(1000);
            MenuHelper.WaitForKey(ManageMovies);
            return;
        }

        moviesLogic.ArchivedMovies.Add(movie);
        moviesLogic.Movies.Remove(movie);
        MoviesAccess.WriteAll(moviesLogic.Movies);
        ArchivedMoviesAccess.WriteAll(moviesLogic.ArchivedMovies);

        Console.Clear();
        Console.WriteLine($"Moved {movie.Title} into the archive");
        Thread.Sleep(1000);
        MenuHelper.WaitForKey(ManageMovies);
    }

    private void EditMovieDetails(int id)
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
        MovieModel movie = moviesLogic.GetMovieById(id);
        if (movie == null)
        {
            MovieManager(id);
            return;
        }
        List<string> options = new List<string>
        {
            $"Edit title: {movie.Title}",
            $"Edit duration: {movie.Duration} minutes",
            $"Edit minimum age: {movie.MinimumAge} years",
            "Back"
        };

        List<Action> actions = new List<Action>
        {
            () => {
                Console.Clear();
                Console.WriteLine("Enter the new movie title:");
                movie.Title = Console.ReadLine();
                MoviesAccess.WriteAll(moviesLogic.Movies);
                EditMovieDetails(id);
            },
            () => {
                Console.WriteLine("Enter the new screen time in minutes:");
                movie.Duration = Math.Abs(int.Parse(Console.ReadLine()));
                MoviesAccess.WriteAll(moviesLogic.Movies);
                EditMovieDetails(id);
            },
            () => {
                Console.WriteLine("Enter the new minimum age (11-18):");
                movie.MinimumAge = Math.Clamp(int.Parse(Console.ReadLine()), 11, 18);
                MoviesAccess.WriteAll(moviesLogic.Movies);
                EditMovieDetails(id);
            },
            () => MovieManager(id)
        };

        MenuHelper.NewMenu(options, actions, "Edit movie");
    }

    // Shows a menu with all movies in the list. Returns the int ID of the selected movie, or -1 if the user cancelled.
    private int MovieSelector(List<MovieModel> movies, List<(string, int)> extras = null)
    {
        List<string> options = movies.Select(m => m.Title).ToList();
        List<int> indices = movies.Select(m => m.Id).ToList();

        if (extras != null)
        {
            foreach ((string, int) item in extras)
            {
                options.Add(item.Item1);
                indices.Add(item.Item2);
            }
        }

        options.Add("Return");
        indices.Add(-1);

        return MenuHelper.NewMenu(options, indices, header: "Movies");
    }

    public void MoviesBrowser(bool makeForGuest = false, int startingIndex = 0, int customerId = -1)
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
        ShowingsLogic showingsLogic = _logicManager.ShowingsLogic;
        if (CinemaLogic.CurrentCinema == null)
        {
            Console.Clear();
            if (customerId != -1)
            {
                _menuManager.CinemaLocations.ChooseCinema(() => MoviesBrowser(makeForGuest, 0, customerId), _menuManager.MainMenus.StaffMenu);
            }
            else
            {
                _menuManager.CinemaLocations.ChooseCinema(() => MoviesBrowser(makeForGuest, 0, customerId), () => 
                {
                    if (AccountsLogic.CurrentAccount == null) _menuManager.MainMenus.GuestMenu();
                    else _menuManager.MainMenus.LoggedInMenu();
                });
            }
            return;
        }
        // Window needs to have a height of at least 17 to show all movie info
        if (Console.WindowHeight < 19)
        {
            Console.Clear();
            Console.WriteLine("Your console is not tall enough!\nPlease expand your console window to browse movies.");
            Thread.Sleep(2000);
            MenuHelper.WaitForKey(AccountsLogic.CurrentAccount == null ? _menuManager.MainMenus.GuestMenu : _menuManager.MainMenus.LoggedInMenu);
            return;
        }
        List<MovieModel> movies = moviesLogic.Movies;
        // no current movies and showings
        if (movies.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("It seems there aren't any movies...\nPlease be patient while we resolve this issue.");
            Thread.Sleep(2000);
            MenuHelper.WaitForKey(AccountsLogic.CurrentAccount == null ? _menuManager.MainMenus.GuestMenu : _menuManager.MainMenus.LoggedInMenu);
            return;
        }

        int currentIndex = startingIndex;
        ConsoleKey key;

        List<int> showingIndices = new List<int>();
        for (int i = 0; i < movies.Count(); i++) showingIndices.Add(0);

        do
        {
            Console.Clear();
            while (Console.WindowWidth < 149)
            {
                Console.Clear();
                Console.WriteLine("Please increase the width of your window to continue...");
                Thread.Sleep(1000);
            }

            if (currentIndex != 0 && currentIndex != movies.Count - 1) Console.Write($"<-- Left{new String(' ', Console.WindowWidth - 17)}Right -->");
            else if (currentIndex == 0) Console.Write($"{new String(' ', Console.WindowWidth - 9)}Right -->");
            else if (currentIndex == movies.Count - 1) Console.Write($"<-- Left{new String(' ', Console.WindowWidth - 8)}");
            Console.WriteLine(new string('-', Console.WindowWidth));

            List<MovieModel> moviesOnScreen;
            if (currentIndex == 0) moviesOnScreen = movies.Slice(currentIndex, 3);
            else if (currentIndex == movies.Count - 1) moviesOnScreen = movies.Slice(currentIndex - 2, 3);
            else moviesOnScreen = movies.Slice(currentIndex - 1, 3);

            // width % 3 moet 2 zijn
            // % 3 = 1 dan - 2 ; % 3 = 0 dan - 1 ; 
            int blockWidth = Console.WindowWidth % 3 == 2 ? Console.WindowWidth : 
                                Console.WindowWidth % 3 == 1 ? Console.WindowWidth - 2 :
                                Console.WindowWidth - 1;
            blockWidth = blockWidth/3;
            List<List<string>> blocks = new();
            
            for (int i = 0; i < 3; i++)
            {
                if (movies[currentIndex].Id == moviesOnScreen[i].Id)
                    blocks.Add(CreateBlock(moviesOnScreen[i], blockWidth, showingIndices[currentIndex]));
                else
                    blocks.Add(CreateBlock(moviesOnScreen[i], blockWidth));
            }

            Console.WriteLine(CombineBlocks(blocks[0], blocks[1], blocks[2], blockWidth));

            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.LeftArrow) currentIndex--;
            if (key == ConsoleKey.RightArrow) currentIndex++;
            if (key == ConsoleKey.UpArrow) showingIndices[currentIndex]--;
            if (key == ConsoleKey.DownArrow) showingIndices[currentIndex]++;

            currentIndex = Math.Clamp(currentIndex, 0, movies.Count - 1);
            showingIndices[currentIndex] = Math.Clamp(showingIndices[currentIndex], 0, Math.Max(showingsLogic.FindShowingsByMovieId(movies[currentIndex].Id, CinemaLogic.CurrentCinema.Id).Count - 1, 0));
        } while (key != ConsoleKey.Enter && key != ConsoleKey.Backspace);

        if (key == ConsoleKey.Enter) 
        {
            if (showingsLogic.FindShowingsByMovieId(movies[currentIndex].Id, CinemaLogic.CurrentCinema.Id).Count != 0)
            {
                // make a reservation of the showinngidices'th item in the list of showings of the movie in currentIndex
                ShowingModel showing = showingsLogic.FindShowingsByMovieId(movies[currentIndex].Id, CinemaLogic.CurrentCinema.Id)[showingIndices[currentIndex]];
                _menuManager.Reservation.Make(showing, makeForGuest, customerId);
                // Reservation.Make(_showingsLogic.Showings[showingIndices[currentIndex]]);
            }
            else
            {
                MoviesBrowser(makeForGuest, startingIndex: currentIndex, customerId);
            }
        }
        else 
        {
            if (AccountsLogic.CurrentAccount == null)
                _menuManager.MainMenus.GuestMenu();
            else
                _menuManager.MainMenus.LoggedInMenu();
        }
    }

    private List<string> CreateBlock(MovieModel movie, int blockWidth, int blockIndex = -1)
    {
        string openANSI = blockIndex >= 0 ? "\u001b[1m" : "\u001b[90m";
        string resetANSI = "\u001b[0m";

        int verticalSpaceForShowings = Math.Max(Console.WindowHeight - 18, 1);

        List<string> wrappedTitle = WrapString(movie.Title, blockWidth, 2);
        List<string> wrappedSummary = WrapString(movie.Summary, blockWidth, 4);
        List<string> wrappedActors = WrapString(String.Join(", ", movie.Actors), blockWidth, 3);

        List<string> outputList = new();

        foreach (string line in wrappedTitle) outputList.Add(line);
        foreach (string line in wrappedSummary) outputList.Add(line);
        outputList.Add("");
        foreach (string line in wrappedActors) outputList.Add(line);
        outputList.Add($"Directed by {movie.Director}");
        outputList.Add("");
        outputList.Add($"Duration: {movie.Duration} minutes; {movie.MinimumAge}+");
        outputList.Add($"");

        List<string> showings = VerticalScroller(_logicManager.ShowingsLogic.FindShowingsByMovieId(movie.Id, CinemaLogic.CurrentCinema.Id), verticalSpaceForShowings, blockIndex);
        foreach (string showing in showings) outputList.Add(showing);

        for (int i = 0; i < outputList.Count(); i++)
        {
            outputList[i] = $"{openANSI}{outputList[i]}{resetANSI}";
        }

        return outputList;
    }

    private List<string> WrapString(string text, int blockWidth, int lines)
    {
        List<string> wrappedString = new();
        List<string> singleWords = text.Split(' ').ToList();

        string line = "";
        foreach (string word in singleWords)
        {
            if (blockWidth > line.Length + word.Length) 
                line += word + " ";
            else 
            {
                wrappedString.Add(line);
                line = word + " ";
            }
        }
        wrappedString.Add(line);

        if (text.Length > blockWidth * lines)
        {
            // replace last 3 characters with dots
        }

        while (wrappedString.Count() < lines)
        {
            wrappedString.Add("");
        }

        return wrappedString;
    }

    private string CombineBlocks(List<string> block1, List<string> block2, List<string> block3, int blockWidth)
    {
        string output = "";

        for (int i = 0; i < block1.Count; i++)
        {
            string line1 = block1[i] + new String(' ', blockWidth - GetLengthWithoutANSI(block1[i]));
            string line2 = block2[i] + new String(' ', blockWidth - GetLengthWithoutANSI(block2[i]));
            string line3 = block3[i] + new String(' ', blockWidth - GetLengthWithoutANSI(block3[i]));
            output += $"{line1}|{line2}|{line3}\n";
        }
        return output;
    }

    private List<string> VerticalScroller(List<ShowingModel> showings, int verticalSpace, int selectedIndex)
    {
        List<string> outputList = new();
        int startingPoint = Math.Max(selectedIndex - (verticalSpace - 1), 0);

        if (showings == null || showings.Count() == 0)
        {
            outputList.Add("No showings planned!");
            for (int i = 1; i <= verticalSpace - 1; i++)
            {
                outputList.Add(" ");
            }
            return outputList;
        }

        // size = 8, verticalspace = 5
        // selectedindex 0 => startingpoint 0
        // selectedindex 4 => startingpoint 0
        // selectedindex 5 => staringpoint 1
        // selectedindex 6 => startingpoint 2
        // selectedindex 7 => staringpoint 3
        for (int i = Math.Max(startingPoint, 0); i < startingPoint + verticalSpace; i++)
        {
            if (i < showings.Count())
                if (showings[i].Is3D) outputList.Add($"{(selectedIndex == i ? $" > {showings[i].Date.ToString(EXTENDED_DATE_FORMAT)}" : showings[i].Date.ToString(EXTENDED_DATE_FORMAT))} in 3D        {SeatSelectionHelpers.GetTakenSeats(showings[i].Id).Count}/168");
                else outputList.Add($"{(selectedIndex == i ? $" > {showings[i].Date.ToString(EXTENDED_DATE_FORMAT)}" : showings[i].Date.ToString(EXTENDED_DATE_FORMAT))}              {SeatSelectionHelpers.GetTakenSeats(showings[i].Id).Count}/168");
            else
                outputList.Add("");
        }
        return outputList;
    }

    private static int GetLengthWithoutANSI(string text) => Regex.Replace(text, @"\u001b\[[0-9;]*m", "").Length;

    public void PromoteMovies(int slot)
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
        List<string> options = moviesLogic.Movies.Select(m => m.Title).ToList();
        options.Add("Empty slot");
        options.Add("Back");

        List<object> actions = new List<object> {};
        moviesLogic.Movies.ForEach(actions.Add);
        actions.Add(() => moviesLogic.RemovePromotion(slot));
        actions.Add(SelectPromotionSlot);

        Console.WriteLine($"options: {options.Count}, actions: {actions.Count}");

        var movieToPromote = MenuHelper.NewMenu(options, actions, $"Which movie do you want to promote in slot {slot + 1}");

        Console.Clear();
        if (movieToPromote is MovieModel movie) 
        {
            moviesLogic.PromoteMovie(movie, slot);
            Console.WriteLine($"Succesfully changed the movie in promotionslot {slot + 1} to {movie.Title}");
        }
        if (movieToPromote is null) 
        {
            moviesLogic.PromoteMovie(null, slot);
            Console.WriteLine($"Succesfully emptied promotionslot {slot + 1}");
        }

        Thread.Sleep(1000);
        MenuHelper.WaitForKey(_menuManager.MainMenus.AdminMenu);
    }

    public void SelectPromotionSlot()
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
        Console.WriteLine();
        MenuHelper.NewMenu(new List<string> {   $"1: currently promoted: {(moviesLogic.PromotedMovies[0] != null ? moviesLogic.PromotedMovies[0].Title : "Empty")}",
                                                $"2: currently promoted: {(moviesLogic.PromotedMovies[1] != null ? moviesLogic.PromotedMovies[1].Title : "Empty")}",

                                                "return"}, 
            new List<object> {() => PromoteMovies(0), () => PromoteMovies(1), _menuManager.MainMenus.AdminMenu},
            "Which slot do you want to change?");
    }
}