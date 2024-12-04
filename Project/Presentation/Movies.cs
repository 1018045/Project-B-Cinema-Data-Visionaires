using System.Runtime.InteropServices;

public static class Movies
{
    private static readonly MoviesLogic _moviesLogic = new ();
    private static readonly ShowingsLogic _showingsLogic = new ();

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

        _moviesLogic.AddMovie(title, duration, minimumAge);
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

        do
        {
            Console.Clear();
            if (currentIndex != 0 && currentIndex != movies.Count - 1) Console.Write($"<-- Left{new String(' ', Console.WindowWidth - 17)}Right -->");
            else if (currentIndex == 0) Console.Write($"{new String(' ', Console.WindowWidth - 9)}Right -->");
            else if (currentIndex == movies.Count - 1) Console.Write($"<-- Left{new String(' ', Console.WindowWidth - 8)}");

            System.Console.WriteLine(ShowMovieInfo(movies[currentIndex]));

            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.LeftArrow) currentIndex--;
            if (key == ConsoleKey.RightArrow) currentIndex++;
            // wrapping
            currentIndex = Math.Clamp(currentIndex, 0, movies.Count - 1); 
        } while (key != ConsoleKey.Enter && key != ConsoleKey.Backspace);
        if (key == ConsoleKey.Backspace) Menus.LoggedInMenu();
        else Reservation.Make();
    }

    private static string ShowMovieInfo(MovieModel movie)
    {
        string info = $"\n\u001b[1m===={movie.Title}====\u001b[0m";
        info += $"\nDuration: {movie.Duration} minutes";
        info += $"\n{movie.MinimumAge}+";
        return info;
    }
}