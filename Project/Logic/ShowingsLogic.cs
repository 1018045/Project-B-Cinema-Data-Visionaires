public class ShowingsLogic
{
    public List<ShowingModel> Showings {get; private set;}
    
    private const int CLEANUP_TIME_BETWEEN_SHOWINGS = 30;


    public ShowingsLogic()
    {
        Showings = ShowingsAccess.LoadAll();
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
        MoviesLogic log = new();
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
        MoviesLogic log = new();
        List<ShowingModel> showings = new();
        foreach (ShowingModel showing in Showings)
        {
            if (showing.Date > DateTime.Now && log.GetMovieById(showing.MovieId).Title == movieName)
                showings.Add(showing);
        }
        return showings;
    }

    public void AddShowing(int movieId, DateTime date, int room, int cinemaId, string special)
    {
        int newId = FindNextAvailableId();
        var showing = new ShowingModel(newId, movieId, date, room, cinemaId, special);
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
            Console.WriteLine($"Showing with ID {id} has been removed.");
        }
        else
        {
            Console.WriteLine($"No showing found with ID {id}.");
        }
    }

    public void RemoveShowing(ShowingModel showing)
    {
        Showings.Remove(showing);
        ShowingsAccess.WriteAll(Showings);
        Console.WriteLine($"Showing at {showing.Date.ToString("dd-MM-yyyy HH:mm:ss")} has been removed.");
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