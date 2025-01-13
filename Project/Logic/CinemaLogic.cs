public class CinemaLogic
{
    public List<CinemaModel> Cinemas {get; private set;}
    public static CinemaModel? CurrentCinema {get; private set;}

    public CinemaLogic()
    {
        Cinemas = CinemaAccess.LoadAll();
    }

    public void ChangeCinema(CinemaModel newCinema)
    {
        CurrentCinema = newCinema;
    }

    public void AddCinema(string name, string city, string address, string postalCode, string phoneNumber)
    {
        Cinemas.Add(new CinemaModel(FindFirstAvailableID(), name, city, address, postalCode, phoneNumber));
        CinemaAccess.WriteAll(Cinemas);
    }

    public void RemoveCinema(CinemaModel cinema)
    {
        Cinemas.Remove(cinema);
        CinemaAccess.WriteAll(Cinemas);
    }

    public void EditCinemaName(CinemaModel cinema, string newName)
    {
        cinema.Name = newName;
        CinemaAccess.WriteAll(Cinemas);
    }
    
    public void EditCinemaAddress(CinemaModel cinema, string newCity, string newAddress, string newPostal)
    {
        cinema.City = newCity;
        cinema.Address = newAddress;
        cinema.PostalCode = newPostal;
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

    public static bool VerifyPostalCode(string postalCode)
    {
        return postalCode.Length == 6 && postalCode.Substring(0,4).All(char.IsDigit) && postalCode.Substring(4,2).All(char.IsLetter);
    }
}