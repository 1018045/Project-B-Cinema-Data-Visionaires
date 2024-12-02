public class MoviesLogic
{
    public List<MovieModel> Movies {get; private set;}
    public List<MovieModel> ArchivedMovies {get; private set;}

    public MoviesLogic()
    {
        Movies = MoviesAccess.LoadAll();
        ArchivedMovies = ArchivedMoviesAccess.LoadAll();
    }

    public void AddMovie(string title, int duration, int minimumAge)
    {
        Movies.Add(new MovieModel(FindFirstAvailableID(), title, duration, minimumAge));
        MoviesAccess.WriteAll(Movies);
    }

    public MovieModel FindMovieByTitle(string title)
    {
        foreach (MovieModel movie in Movies)
        {
            if (movie.Title == title)
            {
                return movie;
            }
        }
        return null;
    }

    public MovieModel GetMovieById(int id)
    {
        foreach (MovieModel movie in Movies)
        {
            if (movie.Id == id)
            {
                return movie;
            }
        }
        return null;
    }

    public MovieModel GetArchivedMovieById(int id)
    {
        foreach (MovieModel movie in ArchivedMovies)
        {
            if (movie.Id == id)
            {
                return movie;
            }
        }
        return null;        
    }

    public int GetIdByTitle(string title)
    {
        foreach (MovieModel movie in Movies)
        {
            if (movie.Title == title)
            {
                return movie.Id;
            }
        }
        return -1;
    }

    public string ListMovies()
    {
        string output = "";
        foreach (MovieModel movie in Movies)
        {
            output += $"\n{movie.Id + 1}: {movie.Title}";
        }
        return output;
    }

    public int FindFirstAvailableID()
    {
        int pointer = 0;
        List<MovieModel> tempList = new();
        tempList.AddRange(Movies);
        tempList.AddRange(ArchivedMovies);
        tempList = tempList.OrderBy(r => r.Id).ToList<MovieModel>();
        foreach (MovieModel movie in tempList)
        {
            if (pointer != movie.Id)
            {
                return pointer;
            }
            pointer++;
        }
        return pointer;
    }
}