using Project.Logic.Account;
using Project.Presentation;

public class MenuManager
{
    public AccountPresentation AccountPresentation {get; private set;}
    public ApplyForJob ApplyForJob {get; private set;}
    public BillPresentation BillPresentation {get; private set;}
    public CinemaLocations CinemaLocations {get; private set;}
    public Menus Menus {get; private set;}
    public Movies Movies {get; private set;}
    public Reservation Reservation {get; private set;}
    public Showings Showings {get; private set;}


    public MenuManager()
    {
        LogicManager logicManager = new();

        AccountPresentation = new(logicManager, this);
        ApplyForJob = new(logicManager, this);
        BillPresentation = new(logicManager, this);
        CinemaLocations = new(logicManager, this);
        Menus = new(logicManager, this);
        Movies = new(logicManager, this);
        Reservation = new(logicManager, this);
        Showings = new(logicManager, this);

        Menus.GuestMenu();
    }
}