using Project.Logic.Account;

public class LogicManager
{
    public AccountantLogic AccountantLogic {get; private set;}
    public CinemaLogic CinemaLogic {get; private set;}
    public EmployeeLogic EmployeeLogic {get; private set;}
    public JobVacancyLogic JobVacancyLogic {get; private set;}
    public MoviesLogic MoviesLogic {get; private set;}
    public ReservationsLogic ReservationsLogic {get; private set;}
    public ShowingsLogic ShowingsLogic {get; private set;}
    public AccountsLogic AccountsLogic {get; private set;}
    public AccountManageLogic AccountManageLogic {get; private set;}

    public LogicManager()
    {
        AccountantLogic = new(this);
        CinemaLogic = new();
        EmployeeLogic = new();
        JobVacancyLogic = new();
        MoviesLogic = new(this);
        ReservationsLogic = new();
        ShowingsLogic = new(this);
        AccountsLogic = new();
        AccountManageLogic = new();
    }
}