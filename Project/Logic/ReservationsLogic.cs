public class ReservationsLogic
{
    private List<ReservationModel> _reservations;

    public ReservationsLogic()
    {
        _reservations = ReservationsAccess.LoadAll();
    }

    public bool AddReservation(int userId, int showingId, List<int> seats, bool paymentComplete, string bankDetails)
    {
        if (MakePayment(bankDetails))
        {
            _reservations.Add(new ReservationModel(FindFirstAvailableID(), userId, showingId, seats, paymentComplete));
            ReservationsAccess.WriteAll(_reservations);
            return true;
        }
        else
            return false;
    }

    public bool MakePayment(string bankDetails)
    {
        // TODO: process payment details here...


        return true;
    }

    // Returns all reservations of a particular user
    public List<ReservationModel> FindReservationByUserID(int userId)
    {
        List<ReservationModel> output = new();
        foreach (ReservationModel reservation in _reservations)
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
        List<ReservationModel> tempList = _reservations.OrderBy(r => r.Id).ToList<ReservationModel>();
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