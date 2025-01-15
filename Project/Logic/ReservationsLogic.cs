using Project.Logic.Account;

public class ReservationsLogic
{
    public List<ReservationModel> Reservations {get; private set;}

    private List<string> ApprovedBankCodes = new List<string>
                {
                    "ABNA","INGB","RABO","SNSB","ASNB","TRIO","KNAB","BUNQ","MOYO","FVLN","FRBK","REVO"
                };

    private LogicManager _logicManager;
        
    public ReservationsLogic(LogicManager logicManager)
    {
        _logicManager = logicManager;
        Reservations = ReservationsAccess.LoadAll();
    }

    public ReservationModel AddReservation(int userId, int showingId, string seats, bool paymentComplete, double price, List<ExtraModel> selectedExtras)
    {
        IEnumerable<ExtraModel> mandatoryExtras = _logicManager.ShowingsLogic.FindShowingByIdReturnShowing(showingId).Extras.Where(e => e.IsMandatory);
        foreach (ExtraModel extra in mandatoryExtras)
        {
            if (!selectedExtras.Contains(extra))
            {
                // DEV-Error, not all mandatory extra's selected
                return null;
            }
        }
        
        ReservationModel reservation = new ReservationModel(FindFirstAvailableID(), userId, showingId, seats, paymentComplete, price, selectedExtras);
        Reservations.Add(reservation);
        ReservationsAccess.WriteAll(Reservations);
        return reservation;
    }

    public string ValidateBankDetails(string bankDetails)
    {
        bankDetails = bankDetails.Trim();
        if (bankDetails is null || bankDetails == "")
            return "Error: no details given. Please try again!";
        if (bankDetails.Length != 18)
            return "Error: incorrect IBAN length. Please try again!";
        
        if (bankDetails.Substring(0,2).ToUpper() != "NL")
        {
            return "The first 2 characters of the IBAN should be 'NL' (Country signature). Please try again!";
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
        return Reservations.Where(r => r.UserId == userId).ToList();
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

    public void UpdateReservation(ReservationModel reservation)
    {
        var index = Reservations.FindIndex(r => r.Id == reservation.Id);
        if (index != -1)
        {
            Reservations[index] = reservation;
            ReservationsAccess.WriteAll(Reservations);
        }
    }

    public List<ReservationModel> GetFutureReservations()
    {
        return _logicManager.ReservationsLogic.FindReservationByUserID(AccountsLogic.CurrentAccount.Id)
                .Where(r => _logicManager.ShowingsLogic.FindShowingByIdReturnShowing(r.ShowingId).Date > DateTime.Now)
                .OrderBy(r => _logicManager.ShowingsLogic.FindShowingByIdReturnShowing(r.ShowingId).Date)
                .ToList();
    }

    public List<ReservationModel> GetPastReservations()
    {
        return _logicManager.ReservationsLogic.FindReservationByUserID(AccountsLogic.CurrentAccount.Id)
                .Where(r =>_logicManager.ShowingsLogic.FindShowingByIdReturnShowing(r.ShowingId).Date < DateTime.Now)
                .OrderByDescending(r => _logicManager.ShowingsLogic.FindShowingByIdReturnShowing(r.ShowingId).Date)
                .ToList();
    }
}
