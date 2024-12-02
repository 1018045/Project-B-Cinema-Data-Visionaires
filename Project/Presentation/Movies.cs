using System.Runtime.InteropServices;

public static class Movies
{
    private static readonly MoviesLogic _moviesLogic = new ();

    public static void ManageArchivedMovies()
    {

    }

    public static void AddMovie()
    {
        MenuHelper.NewMenu(
            new List<string> {"Add new movie", "Add archived movie", "Cancel"},
            new List<Action> {AddNewMovie, BringArchivedMovieBack, Menus.AdminMenu});
    }

    private static void AddNewMovie()
    {
        Console.Clear();
        MoviesLogic moviesLogic = new();
        Console.WriteLine("Enter the movie title:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter the total screen time in minutes:");
        int duration = Math.Abs(int.Parse(Console.ReadLine()));

        Console.WriteLine("Enter the minimum age (11-18):");
        int minimumAge = Math.Clamp(int.Parse(Console.ReadLine()), 11, 18);

        moviesLogic.AddMovie(title, duration, minimumAge);
        Console.WriteLine($"Movie ‘{title}’ has been added to the database.");

        // System.Console.WriteLine("Which extra's are mandatory for this movie?");
        // TODO
        Thread.Sleep(1000);
        MenuHelper.WaitForKey(Menus.AdminMenu);
    }

    private static void BringArchivedMovieBack()
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
            Menus.AdminMenu();
            return;
        }
        
        MovieModel movie = _moviesLogic.GetArchivedMovieById(chosenId);

        _moviesLogic.Movies.Add(movie);
        _moviesLogic.ArchivedMovies.Remove(movie);
        MoviesAccess.WriteAll(_moviesLogic.Movies);
        ArchivedMoviesAccess.WriteAll(_moviesLogic.ArchivedMovies);

        System.Console.WriteLine($"Moved {movie.Title} from archive into active movies");
    }

    public static void ArchiveMovie()
    {
        int chosenId = MovieSelector(_moviesLogic.Movies);

        if (chosenId == -1)
        { 
            Menus.AdminMenu();
            return;
        }

        MovieModel movie = _moviesLogic.GetArchivedMovieById(chosenId);

        _moviesLogic.ArchivedMovies.Add(movie);
        _moviesLogic.Movies.Remove(movie);
        MoviesAccess.WriteAll(_moviesLogic.Movies);
        ArchivedMoviesAccess.WriteAll(_moviesLogic.ArchivedMovies);

        System.Console.WriteLine($"Moved {movie.Title} into the archive");
    }

    // Shows a menu with all movies in the list. Returns the int ID of the selected movie; or -1 if the user cancelled.
    private static int MovieSelector(List<MovieModel> movies)
    {
        List<string> options = movies.Select(m => m.Title).ToList();
        options.Add("Cancel");

        List<int> indices = movies.Select(m => m.Id).ToList();
        indices.Add(-1);

        return MenuHelper.NewMenu("Archived movies", options, indices);
    }
}