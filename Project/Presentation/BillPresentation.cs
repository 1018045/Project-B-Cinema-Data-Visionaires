using Project.Logic;
using Project.Logic.Account;

namespace Project.Presentation;

public class BillPresentation
{
    private LogicManager _logicManager;
    private MenuManager _menuManager;


    public BillPresentation(LogicManager logicManager, MenuManager menuManager)
    {
        _logicManager = logicManager;
        _menuManager = menuManager;
    }

    public void ViewUserBillsAndReservations()
    {
        if (AccountsLogic.CurrentAccount == null)
        {
            Console.WriteLine("Please log in to view your bills.");
            Thread.Sleep(1500);
            _menuManager.MainMenus.GuestMenu();
            return;
        }

        List<string> options = new List<string>
        {
            "Edit upcoming reservation",
            "View Past Reservations",
            "View Future Reservations",
            "Return to Menu"
        };

        List<Action> actions = new List<Action>
        {
            _menuManager.Reservation.Adjust,
            ShowPastReservations,
            ShowFutureReservations,
            _menuManager.MainMenus.LoggedInMenu
        };

        MenuHelper.NewMenu(options, actions, "Your Reservations and Bills");
    }

    private void ShowPastReservations()
    {
        var userReservations = _logicManager.ReservationsLogic.GetPastReservations();

        DisplayReservations(userReservations, true);
    }

    private void ShowFutureReservations()
    {    
        var userReservations = _logicManager.ReservationsLogic.GetFutureReservations();
        
        DisplayReservations(userReservations, false);
    }

    private void DisplayReservations(List<ReservationModel> reservations, bool isPastReservations = true)
    {
        Console.Clear();
        Console.WriteLine($"=== Your {(isPastReservations ? "Past" : "Future")} Reservations ===\n");

        if (reservations.Count == 0)
        {
            Console.WriteLine($"You have no {(isPastReservations ? "past" : "future")} reservations.");
        }
        else
        {
            foreach (var reservation in reservations)
            {
                var showing = _logicManager.ShowingsLogic.FindShowingByIdReturnShowing(reservation.ShowingId);
                var movie = _logicManager.MoviesLogic.GetMovieById(showing.MovieId);

                Console.WriteLine($"Movie: {movie.Title}");
                Console.WriteLine($"Date: {showing.Date:dd-MM-yyyy HH:mm}");
                Console.WriteLine($"Seats: {reservation.Seats}");
                Console.WriteLine($"Price: €{reservation.Price:F2}");
                
                if (reservation.SelectedExtras != null && reservation.SelectedExtras.Count > 0)
                {
                    Console.WriteLine("Extras:");
                    foreach (var extra in reservation.SelectedExtras)
                    {
                        Console.WriteLine($"  - {extra.Name}: €{extra.Price:F2}");
                    }
                }
                Console.WriteLine(new string('-', 40) + "\n");
            }

            Console.WriteLine($"Total Reservations: {reservations.Count}");
            Console.WriteLine($"Total Spent: €{reservations.Sum(r => r.Price):F2}");
        }

        MenuHelper.WaitForKey(ViewUserBillsAndReservations);
    }
} 