using System.Text.RegularExpressions;
using Project.Helpers;

public class MoviesLogic
{
    private const string DATEFORMAT = "dd-MM-yyyy HH:mm:ss";
    private const string EXTENDED_DATE_FORMAT = "dddd d MMMM yyyy";

    public List<MovieModel> Movies {get; private set;}
    public List<MovieModel> ArchivedMovies {get; private set;}
    public List<MovieModel> PromotedMovies {get; private set;}

    private LogicManager _logicManager;

    public MoviesLogic(LogicManager logicManager)
    {
        _logicManager = logicManager;
        Movies = MoviesAccess.LoadAll();
        ArchivedMovies = ArchivedMoviesAccess.LoadAll();
        
        PromotedMovies = Movies.Where(m => m.IsPromoted == true).ToList();
        if (PromotedMovies.Count > 3)
        {
            PromotedMovies = new List<MovieModel> {PromotedMovies[0], PromotedMovies[1], PromotedMovies[2]};
        }
        while (PromotedMovies.Count < 3)
        {
            PromotedMovies.Add(null);
        }
    }

    public void AddMovie(string title, int duration, int minimumAge, string summary, List<string> actors, string director)
    {
        Movies.Add(new MovieModel(FindFirstAvailableID(), title, duration, minimumAge, summary, actors, director));
        MoviesAccess.WriteAll(Movies);
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

        foreach (MovieModel movie in ArchivedMovies)
        {
            if (movie.Id == id)
            {
                return movie;
            }
        }
        return null;
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

    public bool HasUpcomingShowings(MovieModel movie)
    {
        IEnumerable<ShowingModel> showings = _logicManager.ShowingsLogic.Showings.Where(s => s.MovieId == movie.Id).Where(s => s.Date > DateTime.Now.Date);
        return showings.Count() > 0;
    }

    public List<MovieModel> GetMoviesWithUpcomingShowingsOnDate(DateTime date)
    {
        return Movies.Where(m => HasUpcomingShowingsOnDate(m, date)).ToList();
    }

    public bool HasUpcomingShowingsOnDate(MovieModel movie, DateTime date)
    {
        IEnumerable<ShowingModel> showings = _logicManager.ShowingsLogic.Showings.Where(s => s.MovieId == movie.Id && s.Date.Date == date.Date);
        return showings.Count() > 0;
    }

    public void PromoteMovie(MovieModel movie, int position)
    {
        if (position < 0 || position > 2) return;
        if (movie == null)
        {
            PromotedMovies[position] = null;
            return;
        }
        if (PromotedMovies[position] != null) PromotedMovies[position].IsPromoted = false;
        PromotedMovies[position] = movie;
        movie.IsPromoted = true;
        MoviesAccess.WriteAll(Movies);
    }

    public void RemovePromotion(int position)
    {
        if (PromotedMovies[position] == null) return;
        if (position < 0 || position > 2) return;
        PromotedMovies[position].IsPromoted = false;
        PromotedMovies[position] = null;
        MoviesAccess.WriteAll(Movies);
    }

    public void ArchiveMovie(MovieModel movieToMove)
    {
        ArchivedMovies.Add(movieToMove);
        Movies.Remove(movieToMove);
        MoviesAccess.WriteAll(Movies);
        ArchivedMoviesAccess.WriteAll(ArchivedMovies);
    }

    public void BringMovieBackFromArchive(MovieModel archivedMovie)
    {
        Movies.Add(archivedMovie);
        ArchivedMovies.Remove(archivedMovie);
        MoviesAccess.WriteAll(Movies);
        ArchivedMoviesAccess.WriteAll(ArchivedMovies);
    }

    public List<string> CreateBlock(MovieModel movie, int blockWidth, int blockIndex = -1)
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

    public string CombineBlocks(List<string> block1, List<string> block2, List<string> block3, int blockWidth)
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
}