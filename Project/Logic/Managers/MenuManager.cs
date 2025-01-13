using Project.Logic.Account;

public class MenuManager
{
    public AccountantPresentation AccountantPresentation {get; private set;}
    public ApplyForJob ApplyForJob {get; private set;}
    public BillPresentation BillPresentation {get; private set;}
    public CinemaLocation CinemaLocation {get; private set;}
    public Menus Menus {get; private set;}
    public Movies Movies {get; private set;}
    public Reservation Reservation {get; private set;}
    public Showings Showings {get; private set;}


    public MenuManager()
    {
        LogicManager logicManager = new();

        AccountantPresentation = new();
        ApplyForJob = new();
        BillPresentation = new();
        CinemaLocation = new();
        Movies = new();
        Reservation = new();
        Showings = new();

        Menus = new(logicManager, this);
        Menus.GuestMenu();
    }
}