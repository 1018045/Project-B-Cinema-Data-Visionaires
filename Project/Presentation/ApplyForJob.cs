public class ApplyForJob
{
    private LogicManager _logicManager;
    private MenuManager _menuManager;


    public ApplyForJob(LogicManager logicManager, MenuManager menuManager)
    {
        _logicManager = logicManager;
        _menuManager = menuManager;
    }

    public void ShowJobMenu()
    {

        List<string> options = new List<string>
        {
            "View All Vacancies",
            "Apply for Job",
            "Return to Main Menu"
        };

        List<Action> actions = new List<Action>
        {
            ShowAllVacancies,
            ApplyToVacancy, // TODO: implement menu system
            _menuManager.Menus.GuestMenu
        };
        MenuHelper.NewMenu(options, actions, "Job Vacancies");
        
    }

    private void ShowAllVacancies()
    {
        Console.Clear();
        Console.WriteLine(_logicManager.JobVacancyLogic.ShowAllVacancies());
        MenuHelper.WaitForKey(ShowJobMenu);
    }

    private void ApplyToVacancy()
    {
        JobVacancyLogic vacancyLogic = _logicManager.JobVacancyLogic;
        Console.Clear();
        List<string> options = vacancyLogic.Vacancies.Select(v => v.JobTitle).ToList();

        List<int> indices = vacancyLogic.Vacancies.Select(v => v.VacancyId).ToList();
        
        int vacancyId = MenuHelper.NewMenu(options, indices, "Vacancies");

        DisplayVacancy(vacancyLogic.GetJobVacancyById(vacancyId));
        MenuHelper.WaitForKey("Press enter to continue the process");

        bool apply = MenuHelper.NewMenu(new List<string>() {"Yes", "No"}, new List<bool>() {true, false}, "Do you want to apply?");

        if (apply)
        {
            Console.Clear();
            Console.WriteLine("Enter your email address:\n");
            string email = Console.ReadLine();

            Console.WriteLine("Enter your motivation:\n");
            string motivation = Console.ReadLine();

            vacancyLogic.AddApplication(vacancyId, email, motivation, DateTime.Now);

            Console.Clear();
            Console.WriteLine("\nYour application has been submitted successfully!");
            Console.WriteLine("We will contact you via the provided email address.");
            }
        
        Thread.Sleep(1000);        
        MenuHelper.WaitForKey(ShowJobMenu);
    }

    private void DisplayVacancy(JobVacancy vacancy)
    {
        Console.WriteLine($"Position: {vacancy.JobTitle}");
        Console.WriteLine($"Description: {vacancy.JobDescription}");
        Console.WriteLine($"Type: {vacancy.EmploymentType}");
        Console.WriteLine($"Salary: {vacancy.Salary:C}");
        Console.WriteLine($"Date Posted: {vacancy.DatePosted:d}");
    }
}    