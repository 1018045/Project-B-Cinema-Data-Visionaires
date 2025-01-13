using System.Runtime.CompilerServices;
using System.Net;
using System.Security;
using Project.DataModels;
using Project.Logic.Account;
using Project.Presentation;

public class MainMenus
{
    private LogicManager _logicManager;
    private MenuManager _menuManager;

    public MainMenus(LogicManager logicManager, MenuManager menuManager)
    {
        _logicManager = logicManager;
        _menuManager = menuManager;
    }


    public void GuestMenu()
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;
        List<string> options = new List<string>
        {
            "Browse movies",
            "Select a date",
            "Login",
            "Create account",
            "Select a cinema location",
            "Jobs",
            "About/Contact",
            "Exit"
        };
        List<Action> actions = new List<Action>
        {
            () => _menuManager.Movies.MoviesBrowser(),
            _menuManager.Reservation.SelectDate,
            () => _menuManager.AccountPresentation.Login(),
            () => _menuManager.AccountPresentation.CreateAccount(LoggedInMenu),
            () => _menuManager.CinemaLocations.ChooseCinema(GuestMenu, GuestMenu),
            _menuManager.JobApplications.ShowJobMenu,
            () => _menuManager.CinemaLocations.AboutContact(GuestMenu),
            () => Environment.Exit(0)
        };
        MenuHelper.NewMenu(options, actions, "Zidane", promotedMovies: moviesLogic.PromotedMovies, showCurrentLocation: true, showMenu: true);
    }

    public void LoggedInMenu()
    {
        MoviesLogic moviesLogic = _logicManager.MoviesLogic;

        List<string> options = new List<string>
        {
            "Browse movies",
            "Select a date",
            "View Reservations and Bills",
            "Select a cinema location",
            "Manage your account",
            "about/contact",
            "Log out"
        };
        List<Action> actions = new List<Action>
        {
            () => _menuManager.Movies.MoviesBrowser(),
            _menuManager.Reservation.SelectDate,
            _menuManager.BillPresentation.ViewUserBillsAndReservations,
            () => _menuManager.CinemaLocations.ChooseCinema(LoggedInMenu, LoggedInMenu),
            _menuManager.AccountPresentation.Menu,
            () => _menuManager.CinemaLocations.AboutContact(LoggedInMenu),   
            () => 
            {
                AccountsLogic.LogOut();
                GuestMenu();
            }
        };
        MenuHelper.NewMenu(options, actions, $"Logged in as: {AccountsLogic.CurrentAccount.EmailAddress}", promotedMovies: moviesLogic.PromotedMovies, showCurrentLocation: true, showMenu: true);
    }

    public void AdminMenu()
    {
        AccountantLogic accountantLogic = _logicManager.AccountantLogic;

        List<string> options = new List<string>
        {
            "Manage movies",
            "Manage showings",
            "Manage movie promotions",
            "View all users",
            "Remove a user",
            "Add an Employee",
            "Add job vacancy",
            "Remove job vacancy",
            "View all vacancies",
            "Show accountant options",
            "Manage cinema locations",
            "Add staff account",
            "Logout"
        };
        List<Action> actions = new List<Action>
        {
            _menuManager.Movies.Start,
            _menuManager.Showings.ManageShowings,
            _menuManager.Movies.SelectPromotionSlot,
            _menuManager.AccountPresentation.ViewUsers,
            _menuManager.AccountPresentation.RemoveUser,
            _menuManager.JobApplications.AddEmployee,
            _menuManager.JobApplications.AddJobVacancy,
            _menuManager.JobApplications.RemoveJobVacancy,
            _menuManager.JobApplications.ViewAllVacanciesAdmin,
            AccountantMenu,
            _menuManager.CinemaLocations.ChooseCinemaLocationToManage,
            _menuManager.AccountPresentation.AddStaffAccount,
            GuestMenu
        };
        MenuHelper.NewMenu(options, actions, "Admin menu");
    }

    public void StaffMenu()
    {
        List<string> options = new List<string>
        {
            "Make reservation for registered customer",
            "Make reservation for guest customer",
            "Logout"
        };
        List<Action> actions =
        [
            () => _menuManager.Reservation.MakeCustomerReservation(false),
            () => _menuManager.Reservation.MakeCustomerReservation(true),
            GuestMenu
        ];
        MenuHelper.NewMenu(options, actions, "Staff menu");
    }

    public void AccountantMenu()
    {
        List<string> options = new List<string>
        {
            // "View all financial records",
            "View income for specific month",
            "View current monthly expenses",
            // "View total tickets sold",
            "View employee salaries",
            // "Log out"
        };

        List<Action> actions = new List<Action>
        {
            // ViewAllRecords,
        () => 
        {
            Console.Write("Enter the month number: ");
            int month = Convert.ToInt32(Console.ReadLine());
            _menuManager.AccountantPresentation.ViewIncomeByMonth(month); // Calling with the month entered by the user
        },
        () => 
        {
            _menuManager.AccountantPresentation.ViewMonthlyExpenses();
        },
            // ViewTotalTickets,
            _menuManager.AccountantPresentation.ViewEmployeeSalaries,   
            // GuestMenu
        };

        MenuHelper.NewMenu(options, actions, "Accountant Menu"); 
    }

}