public class ShowingsLogic
{
    public List<ShowingModel> Showings {get; private set;}
    
    private const int CLEANUP_TIME_BETWEEN_SHOWINGS = 30;

    private LogicManager _logicManager;


    public ShowingsLogic(LogicManager logicManager)
    {
        _logicManager = logicManager;
        Showings = ShowingsAccess.LoadAll();
    }

    public List<ShowingModel> FindShowingsByMovieId(int id)
    {
        List<ShowingModel> showings = new();
        foreach (ShowingModel showing in Showings)
        {
            if (showing.MovieId == id && showing.Date > DateTime.Now)
            {
                showings.Add(showing);
            }
        }
        return showings.OrderBy(s => s.Date).ToList();
    }

    public List<ShowingModel> FindShowingsByMovieId(int id, int cinemaId)
    {
        List<ShowingModel> showings = new();
        foreach (ShowingModel showing in Showings)
        {
            if (showing.MovieId == id && showing.Date > DateTime.Now && showing.CinemaId == cinemaId)
            {
                showings.Add(showing);
            }
        }
        return showings.OrderBy(s => s.Date).ToList();
    }

    public ShowingModel FindShowingByIdReturnShowing(int id)
    {
        foreach (ShowingModel showing in Showings)
        {
            if (showing.Id == id)
            {
                return showing;
            }
        }
        return null;
    }

    public string ToString(ShowingModel showing, bool showId = false) 
    {
        MoviesLogic log = _logicManager.MoviesLogic;
        string output = $"{log.GetMovieById(showing.MovieId).Title}\n";
        output += $"    {showing.Date}\n";
        output += $"    Room: {showing.Room}; Aged {log.GetMovieById(showing.MovieId).MinimumAge} and above.\n";
        return output;
    }

    public string ShowAll() 
    {
        string output = "";
        foreach (ShowingModel showing in Showings)
        {
            output += ToString(showing);
        }
        return output;
    }

    public List<ShowingModel> GetUpcomingShowingsOfMovie(string movieName, int cinemaId)
    {
        MoviesLogic log = _logicManager.MoviesLogic;
        List<ShowingModel> showings = new();
        foreach (ShowingModel showing in Showings)
        {
            if (showing.Date > DateTime.Now && log.GetMovieById(showing.MovieId).Title == movieName)
                showings.Add(showing);
        }
        return showings;
    }

    public void AddShowing(int movieId, DateTime date, int room, int cinemaId, bool is3d, string special, List<ExtraModel> extras)
    {
        int newId = FindNextAvailableId();
        var showing = new ShowingModel(newId, movieId, date, room, cinemaId, extras, is3d, special);
        Showings.Add(showing);
        ShowingsAccess.WriteAll(Showings);
    }

    public void RemoveShowing(int id)
    {
        ShowingModel showingToRemove = Showings.FirstOrDefault(s => s.Id == id);      
        if (showingToRemove != null)
        {
            Showings.Remove(showingToRemove);
            ShowingsAccess.WriteAll(Showings);
        }
    }

    private int FindNextAvailableId()
    {
        int pointer = 0;
        List<ShowingModel> tempList = Showings.OrderBy(a => a.Id).ToList<ShowingModel>();
        foreach (ShowingModel showing in tempList)
        {
            if (pointer != showing.Id)
            {
                return pointer;
            }
            pointer++;
        }
        return pointer;
    }

    public bool IsRoomFree(DateTime newDate, int room, int showingDuration, int chosenCinemaId)
    {
        foreach (ShowingModel showing in Showings)
        {
            if (showing.Room == room && showing.CinemaId == chosenCinemaId)
            {   // de showings overlappen:
                // alleen als het BEGIN van showing 2 EERDER is dan het begin van showing 1, en het EINDE van showing 2 LATER is dan het begin van showing 1 (of andersom)
                if ((newDate <= showing.Date && newDate.AddMinutes(CLEANUP_TIME_BETWEEN_SHOWINGS + showingDuration) >= showing.Date) ||
                    (showing.Date <= newDate && showing.Date.AddMinutes(CLEANUP_TIME_BETWEEN_SHOWINGS + showingDuration) >= newDate))
                {
                    return false;
                }             
            }
        }
        return true;
    }
}