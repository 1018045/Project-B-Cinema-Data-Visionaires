using System.Globalization;

public class ShowingsLogic
{
    private const int CLEANUP_TIME_BETWEEN_SHOWINGS = 30;
    
    public List<ShowingModel> Showings {get; private set;}


    public ShowingsLogic()
    {
        Showings = ShowingsAccess.LoadAll();
    }

    // Returns all showings a particular user has reserved
    public List<ShowingModel> FindReservationByUserID(int id)
    {
        List<ShowingModel> output = new();
        foreach (ShowingModel showing in Showings)
        {
            if (showing.Id == id)
                output.Add(showing);
        }
        return output;
    }

    public string FindShowingById(int id)
    {
        foreach (ShowingModel showing in Showings)
        {
            if (showing.Id == id)
            {
                return ToString(showing);
            }
        }
        return "Showing not found!";
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
        return showings;
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

    public string ShowUpcoming(bool showId = false) 
    {
        string output = "";
        foreach (ShowingModel showing in Showings)
        {
            if (showing.Date > DateTime.Now)
            {
                output += showId ? $"{showing.Id}. " : "";
                output += ToString(showing) + "\n";
            }
        }
        return output;
    }

    public string ShowUpcoming(DateTime date) 
    {
        string output = "";
        foreach (ShowingModel showing in Showings)
        {
            if (showing.Date.Date == date.Date)
                output += ToString(showing);
        }
        return output;
    }

    public List<ShowingModel> GetUpcomingShowingsOfMovie(string movieName)
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

    public void AddShowing(int movieId, DateTime date, int room)
    {
        ShowingModel newShowing = new ShowingModel(FindNextAvailableId(), movieId, date, room);
        Showings.Add(newShowing);
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

    public bool IsRoomFree(DateTime newDate, int room, int showingDuration, MoviesLogic log)
    {
        foreach (ShowingModel showing in Showings)
        {
            if (showing.Room == room)
            {   // de showings overlappen:
                // alleen als het BEGIN van showing 2 EERDER is dan het begin van showing 1, en het EINDE van showing 2 LATER is dan het begin van showing 1
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