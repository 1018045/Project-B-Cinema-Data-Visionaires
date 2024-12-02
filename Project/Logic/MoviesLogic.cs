public class MoviesLogic
{
    private List<MovieModel> _movies;

    public MoviesLogic()
    {
        _movies = MoviesAccess.LoadAll();
    }

    public void AddMovie(string title, int duration, int minimumAge)
    {
        _movies.Add(new MovieModel(FindFirstAvailableID(), title, duration, minimumAge));
        MoviesAccess.WriteAll(_movies);
    }

    public MovieModel FindMovieByTitle(string title)
    {
        foreach (MovieModel movie in _movies)
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
        foreach (MovieModel movie in _movies)
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
        foreach (MovieModel movie in _movies)
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
        foreach (MovieModel movie in _movies)
        {
            output += $"\n{movie.Id + 1}: {movie.Title}";
        }
        return output;
    }

    public int FindFirstAvailableID()
    {
        int pointer = 0;
        List<MovieModel> tempList = _movies.OrderBy(r => r.Id).ToList<MovieModel>();
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

    public int GetSize() => _movies.Count;

    public List<MovieModel> GetMovies()
    {
        return _movies;
    }
}