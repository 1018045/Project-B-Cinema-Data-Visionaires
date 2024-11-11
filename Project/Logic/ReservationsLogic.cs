using System.Linq;


public class ReservationsLogic
{
    public List<ReservationModel> Reservations {get; private set;}

    private List<string> ApprovedBankCodes = new List<string>
                {
                    "ABNA","INGB","RABO","SNSB","ASNB","TRIO","KNAB","BUNQ","MOYO","FVLN","FRBK","REVO"
                };
        
    public ReservationsLogic()
    {
        Reservations = ReservationsAccess.LoadAll();
    }

    public void AddReservation(int userId, int showingId, string seats, bool paymentComplete)
    {
        Reservations.Add(new ReservationModel(FindFirstAvailableID(), userId, showingId, seats, paymentComplete));
        ReservationsAccess.WriteAll(Reservations);
    }
    public void AddReservation(ReservationModel reservation)
    {
        Reservations.Add(reservation);
        ReservationsAccess.WriteAll(Reservations);
    }

    public string ValidateBankDetails(string bankDetails)
    {
        bankDetails = bankDetails.Trim();
        if (bankDetails is null || bankDetails == "")
            return "Error: no details given. Please try again!";
        if (bankDetails.Length != 18)
            return "Error: incorrect IBAN length. Please try again!";
        
        if (!bankDetails.Substring(0,2).All(char.IsLetter))
        {
            return "The first 2 characters of the IBAN should only be letters (Country signature). Please try again!";
        }
        if (!bankDetails.Substring(2,2).All(char.IsNumber))
        {
            return "The first 2 characters after the Country signature should only be numbers. Please try again!";
        }

        if (!bankDetails.Substring(4,4).All(char.IsLetter) || !ApprovedBankCodes.Contains(bankDetails.Substring(4,4)))
        {
            return "Please check the bank identifiers in your IBAN. Please try again!";
        }
        if (!bankDetails.Substring(8,10).All(char.IsNumber))
        {
            return "The last 10 characters in your IBAN should only be numbers. Please try again!";
        }
        return "";
    }

    // Returns all reservations of a particular user
    public List<ReservationModel> FindReservationByUserID(int userId)
    {
        List<ReservationModel> output = new();
        foreach (ReservationModel reservation in Reservations)
        {
            if (reservation.UserId == userId)
                output.Add(reservation);
        }
        return output;
    }

    // returns the first int ID that is not already used in another reservation
    private int FindFirstAvailableID()
    {
        int pointer = 0;
        List<ReservationModel> tempList = Reservations.OrderBy(r => r.Id).ToList<ReservationModel>();
        foreach (ReservationModel reservation in tempList)
        {
            if (pointer != reservation.Id)
            {
                return pointer;
            }
            pointer++;
        }
        return pointer;
    }
}
