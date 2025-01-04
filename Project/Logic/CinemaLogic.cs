public class CinemaLogic
{
    private List<CinemaModel> Cinemas;
    public static CinemaModel? CurrentCinema {get; private set;}

    public CinemaLogic()
    {
        Cinemas = CinemaAccess.LoadAll();
    }

    public void ChangeCinema(int newCinemaId)
    {
        CurrentCinema = Cinemas.Where(c => c.Id == newCinemaId).First();
    }

    public void AddCinema(string name, string city, string address, string postalCode)
    {
        Cinemas.Add(new CinemaModel(FindFirstAvailableID(), name, city, address, postalCode));
        CinemaAccess.WriteAll(Cinemas);
    }

    public void RemoveCinema(int idToRemove)
    {
        Cinemas.Remove(Cinemas.Where(c => c.Id == idToRemove).First());
        CinemaAccess.WriteAll(Cinemas);
    }

    private int FindFirstAvailableID()
    {
        int pointer = 0;
        List<CinemaModel> tempList = Cinemas.OrderBy(r => r.Id).ToList<CinemaModel>();
        foreach (CinemaModel cinema in tempList)
        {
            if (pointer != cinema.Id)
            {
                return pointer;
            }
            pointer++;
        }
        return pointer;
    }
}