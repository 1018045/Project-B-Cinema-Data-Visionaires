using Project.Logic;
using Project.Logic.Account;

namespace Project.Presentation;

public static class BillPresentation
{
    private static readonly AccountsLogic _accountsLogic = new();
    private static readonly AccountantLogic _accountantLogic = new();
    private static readonly ReservationsLogic _reservationsLogic = new();
    private static readonly ShowingsLogic _showingsLogic = new();
    private static readonly MoviesLogic _moviesLogic = new();

    public static void ViewUserBillsAndReservations()
    {
        if (AccountsLogic.CurrentAccount == null)
        {
            Console.WriteLine("Please log in to view your bills.");
            Thread.Sleep(1500);
            Menus.GuestMenu();
            return;
        }

        List<string> options = new List<string>
        {
            "View Past Reservations",
            "View Future Reservations",
            "Return to Menu"
        };

        List<Action> actions = new List<Action>
        {
            ShowPastReservations,
            ShowFutureReservations,
            Menus.LoggedInMenu
        };

        MenuHelper.NewMenu(options, actions, "Your Reservations and Bills");
    }

    private static void ShowPastReservations()
    {
        var userReservations = _reservationsLogic.FindReservationByUserID(AccountsLogic.CurrentAccount.Id)
            .Where(r => _showingsLogic.FindShowingByIdReturnShowing(r.ShowingId).Date < DateTime.Now)
            .OrderByDescending(r => _showingsLogic.FindShowingByIdReturnShowing(r.ShowingId).Date)
            .ToList();

        DisplayReservations(userReservations, true);
    }

    private static void ShowFutureReservations()
    {
        var userReservations = _reservationsLogic.FindReservationByUserID(AccountsLogic.CurrentAccount.Id)
            .Where(r => _showingsLogic.FindShowingByIdReturnShowing(r.ShowingId).Date > DateTime.Now)
            .OrderBy(r => _showingsLogic.FindShowingByIdReturnShowing(r.ShowingId).Date)
            .ToList();

        DisplayReservations(userReservations, false);
    }

    private static void DisplayReservations(List<ReservationModel> reservations, bool isPastReservations = true)
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
                var showing = _showingsLogic.FindShowingByIdReturnShowing(reservation.ShowingId);
                var movie = _moviesLogic.GetMovieById(showing.MovieId);

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