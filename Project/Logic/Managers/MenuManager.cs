using Project.Presentation;

public class MenuManager
{
    public AccountantPresentation AccountantPresentation {get; private set;}
    public AccountPresentation AccountPresentation {get; private set;}
    public JobApplications JobApplications {get; private set;}
    public BillPresentation BillPresentation {get; private set;}
    public CinemaLocations CinemaLocations {get; private set;}
    public MainMenus MainMenus {get; private set;}
    public Movies Movies {get; private set;}
    public Reservation Reservation {get; private set;}
    public Showings Showings {get; private set;}


    public MenuManager()
    {
        LogicManager logicManager = new();

        AccountantPresentation = new(logicManager, this);
        AccountPresentation = new(logicManager, this);
        JobApplications = new(logicManager, this);
        BillPresentation = new(logicManager, this);
        CinemaLocations = new(logicManager, this);
        MainMenus = new(logicManager, this);
        Movies = new(logicManager, this);
        Reservation = new(logicManager, this);
        Showings = new(logicManager, this);

        MenuHelper.setMenuManager(this);

        MainMenus.GuestMenu();
    }
}