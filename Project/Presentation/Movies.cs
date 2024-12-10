using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Project.Logic.Account;

public static class Movies
{
    private static readonly MoviesLogic _moviesLogic = new ();
    private static readonly ShowingsLogic _showingsLogic = new ();
    private const string DATEFORMAT = "dd-MM-yyyy HH:mm:ss";

    public static void Start()
    {
        MenuHelper.NewMenu(
            new List<string> {"Manage movies", "Manage archived movies", "Return"},
            new List<Action> {ManageMovies, ManageArchivedMovies, Menus.AdminMenu}
        ).Invoke();
    }

    private static void ManageMovies()
    {
        int chosenId = MovieSelector(_moviesLogic.Movies, new List<(string, int)> { ("[Add movie]", -2) });
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

    private static void MovieManager(int id)
    {
        MenuHelper.NewMenu(
            new List<string> {"Edit movie details (TODO)", "Archive movie", "Return"},
            new List<Action> {() => EditMovieDetails(id), () => ArchiveMovie(id), ManageMovies},
            _moviesLogic.GetMovieById(id).Title);
    }

    private static void ManageArchivedMovies()
    {
        if (_moviesLogic.ArchivedMovies.Count == 0)
        {
            System.Console.WriteLine("There are 0 movies in the archive");
            Thread.Sleep(1000);
            MenuHelper.WaitForKey(Menus.AdminMenu);
            return;
        }

        int chosenId = MovieSelector(_moviesLogic.ArchivedMovies);

        if (chosenId == -1)
        { 
            Start();
            return;
        }

        ArchivedMovieManager(chosenId);
    }

    private static void ArchivedMovieManager(int id)
    {
        MenuHelper.NewMenu(
            new List<string> {"Edit movie details (TODO)", "Take movie out of archive", "Return"},
            new List<Action> {() => EditMovieDetails(id), () => BringArchivedMovieBack(id), ManageArchivedMovies});
    }

    private static void AddMovie()
    {
        Console.Clear();
        Console.WriteLine("Enter the movie title:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter the total screen time in minutes:");
        int duration = Math.Abs(int.Parse(Console.ReadLine()));

        Console.WriteLine("Enter the minimum age (11-18):");
        int minimumAge = Math.Clamp(int.Parse(Console.ReadLine()), 11, 18);

        System.Console.WriteLine("Enter a summary of the movie:");
        string summary = Console.ReadLine();

        System.Console.WriteLine("Enter the main cast seperated by comma's");
        List<string> actors = Console.ReadLine().Split(",").ToList();
        actors.ForEach(a => a.Trim());

        System.Console.WriteLine("Enter the movie's director:");
        string director = Console.ReadLine();

        _moviesLogic.AddMovie(title, duration, minimumAge, summary, actors, director);
        Console.WriteLine($"Movie ‘{title}’ has been added to the database.");

        // System.Console.WriteLine("Which extra's are mandatory for this movie?");
        // TODO
        Thread.Sleep(1000);
        MenuHelper.WaitForKey(ManageMovies);
    }

    private static void BringArchivedMovieBack(int id)
    { 
        MovieModel movie = _moviesLogic.GetMovieById(id);

        _moviesLogic.Movies.Add(movie);
        _moviesLogic.ArchivedMovies.Remove(movie);
        MoviesAccess.WriteAll(_moviesLogic.Movies);
        ArchivedMoviesAccess.WriteAll(_moviesLogic.ArchivedMovies);

        Console.Clear();
        System.Console.WriteLine($"Moved {movie.Title} from archive into active movies");
        Thread.Sleep(1000);
        MenuHelper.WaitForKey(ManageArchivedMovies);
    }

    private static void ArchiveMovie(int id)
    {
        MovieModel movie = _moviesLogic.GetMovieById(id);

        if (_moviesLogic.HasUpcomingShowings(_showingsLogic, movie))
        {
            System.Console.WriteLine($"Error: {movie.Title} still has upcoming showings");
            System.Console.WriteLine("Please remove these showings and try again");
            Thread.Sleep(1000);
            MenuHelper.WaitForKey(ManageMovies);
            return;
        }

        _moviesLogic.ArchivedMovies.Add(movie);
        _moviesLogic.Movies.Remove(movie);
        MoviesAccess.WriteAll(_moviesLogic.Movies);
        ArchivedMoviesAccess.WriteAll(_moviesLogic.ArchivedMovies);

        Console.Clear();
        System.Console.WriteLine($"Moved {movie.Title} into the archive");
        Thread.Sleep(1000);
        MenuHelper.WaitForKey(ManageMovies);
    }

    private static void EditMovieDetails(int id)
    {
        MovieModel movie = _moviesLogic.GetMovieById(id);
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
                MoviesAccess.WriteAll(_moviesLogic.Movies);
                EditMovieDetails(id);
            },
            () => {
                Console.WriteLine("Enter the new screen time in minutes:");
                movie.Duration = Math.Abs(int.Parse(Console.ReadLine()));
                MoviesAccess.WriteAll(_moviesLogic.Movies);
                EditMovieDetails(id);
            },
            () => {
                Console.WriteLine("Enter the new minimum age (11-18):");
                movie.MinimumAge = Math.Clamp(int.Parse(Console.ReadLine()), 11, 18);
                MoviesAccess.WriteAll(_moviesLogic.Movies);
                EditMovieDetails(id);
            },
            () => MovieManager(id)
        };

        MenuHelper.NewMenu(options, actions, "Edit movie");
    }

    // Shows a menu with all movies in the list. Returns the int ID of the selected movie, or -1 if the user cancelled.
    private static int MovieSelector(List<MovieModel> movies, List<(string, int)> extras = null)
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

        return MenuHelper.NewMenu(options, indices, message: "Movies");
    }

    public static void MoviesBrowser()
    {
        int currentIndex = 0;
        ConsoleKey key;
        List<MovieModel> movies = _moviesLogic.Movies;
        // can still have count of 0
        List<int> showingIndices = new List<int>();
        for (int i = 0; i < movies.Count(); i++) 
            showingIndices.Add(0);

        do
        {
            Console.Clear();
            if (currentIndex != 0 && currentIndex != movies.Count - 1) Console.Write($"<-- Left{new String(' ', Console.WindowWidth - 17)}Right -->");
            else if (currentIndex == 0) Console.Write($"{new String(' ', Console.WindowWidth - 9)}Right -->");
            else if (currentIndex == movies.Count - 1) Console.Write($"<-- Left{new String(' ', Console.WindowWidth - 8)}");
            System.Console.WriteLine(new string('-', Console.WindowWidth));

            List<MovieModel> moviesOnScreen;
            if (currentIndex == 0) moviesOnScreen = movies.Slice(currentIndex, 3);
            else if (currentIndex == movies.Count - 1) moviesOnScreen = movies.Slice(currentIndex - 2, 3);
            else moviesOnScreen = movies.Slice(currentIndex - 1, 3);

            int blockWidth = Console.WindowWidth % 3 == 0 ? Console.WindowWidth : Console.WindowWidth - Console.WindowWidth % 3;
            blockWidth = blockWidth/3;
            List<List<string>> blocks = new();
            
            for (int i = 0; i < 3; i++)
            {
                if (movies[currentIndex].Id == moviesOnScreen[i].Id)
                    blocks.Add(CreateBlock(moviesOnScreen[i], blockWidth, showingIndices[currentIndex]));
                else
                    blocks.Add(CreateBlock(moviesOnScreen[i], blockWidth));
            }

            System.Console.WriteLine(CombineBlocks(blocks[0], blocks[1], blocks[2], blockWidth));

            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.LeftArrow) currentIndex--;
            if (key == ConsoleKey.RightArrow) currentIndex++;
            if (key == ConsoleKey.UpArrow) showingIndices[currentIndex]--;
            if (key == ConsoleKey.DownArrow) showingIndices[currentIndex]++;

            currentIndex = Math.Clamp(currentIndex, 0, movies.Count - 1);
            showingIndices[currentIndex] = Math.Clamp(showingIndices[currentIndex], 0, Math.Max(_showingsLogic.FindShowingsByMovieId(movies[currentIndex].Id).Count() - 1, 0));
        } while (key != ConsoleKey.Enter && key != ConsoleKey.Backspace);

        if (key == ConsoleKey.Backspace) 
        {
            if (AccountsLogic.CurrentAccount == null)
                Menus.GuestMenu();
            else
                Menus.LoggedInMenu();
        }
        else Reservation.Make(_showingsLogic.Showings[showingIndices[currentIndex]]);
    }

    private static List<string> CreateBlock(MovieModel movie, int blockWidth, int blockIndex = -1)
    {
        string openANSI = blockIndex >= 0 ? "\u001b[1m" : "\u001b[90m";
        string resetANSI = "\u001b[0m";

        int verticalSpaceForShowings = Math.Max(Console.WindowHeight - 18, 0);

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

        List<string> showings = VerticalScroller(_showingsLogic.FindShowingsByMovieId(movie.Id), verticalSpaceForShowings, blockIndex);
        foreach (string showing in showings) outputList.Add(showing);

        for (int i = 0; i < outputList.Count(); i++)
        {
            outputList[i] = $"{openANSI}{outputList[i]}{resetANSI}";
        }

        return outputList;
    }

    private static List<string> WrapString(string text, int blockWidth, int lines)
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

    private static string CombineBlocks(List<string> block1, List<string> block2, List<string> block3, int blockWidth)
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

    private static List<string> VerticalScroller(List<ShowingModel> showings, int verticalSpace, int selectedIndex)
    {
        List<string> outputList = new();
        int startingPoint = Math.Max(selectedIndex - (verticalSpace - 1), 0);

        // size = 8, verticalspace = 5
        // selectedindex 0 => startingpoint 0
        // selectedindex 4 => startingpoint 0
        // selectedindex 5 => staringpoint 1
        // selectedindex 6 => startingpoint 2
        // selectedindex 7 => staringpoint 3

        if (showings == null || showings.Count() == 0)
        {
            outputList.Add("No showings planned!");
            for (int i = 1; i <= verticalSpace - 1; i++)
            {
                outputList.Add(" ");
            }
            return outputList;
        }

        for (int i = Math.Max(startingPoint, 0); i < startingPoint + verticalSpace; i++)
        {
            if (i < showings.Count())
                outputList.Add($"{(selectedIndex == i ? $" > {showings[i].Date.ToString(DATEFORMAT)}" : showings[i].Date.ToString(DATEFORMAT))}");
            else
                outputList.Add("");
        }
        return outputList;
    }

    private static int GetLengthWithoutANSI(string text) => Regex.Replace(text, @"\u001b\[[0-9;]*m", "").Length;
}