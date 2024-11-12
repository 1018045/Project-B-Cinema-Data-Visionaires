using System.Text.RegularExpressions;

public class ReservationsLogic
{
    public List<ReservationModel> Reservations {get; private set;}

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

    public bool ValidateBankDetails(string bankDetails)
    {
        if (bankDetails is null)
            return false;
        if (Regex.IsMatch(bankDetails.ToLower().Trim(), "^[a-z]{2}[0-9]{2}[a-z]{4}[0-9]{10}$"))
            return true;
        else
            return false;
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

    public List<ReservationModel> ShowAllUserReservations(int userId)
    {
        List<ReservationModel> reservations = new();
        foreach (ReservationModel reservation in Reservations)
        {
            if (reservation.UserId == userId)
            {
                reservations.Add(reservation);
            }
        }
        return reservations;
    }

    public string ToString(ReservationModel reservation)
    {
        string output = $"";

        return output;
    }

    public void RemoveReservation(ReservationModel reservation)
    {
        Reservations.Remove(reservation);
        ReservationsAccess.WriteAll(Reservations);
    }
}
