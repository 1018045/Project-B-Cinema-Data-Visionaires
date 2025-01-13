public class JobApplications
{
    private LogicManager _logicManager;
    private MenuManager _menuManager;


    public JobApplications(LogicManager logicManager, MenuManager menuManager)
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
            _menuManager.MainMenus.GuestMenu
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

    public void AddJobVacancy()
    {
        var vacancyLogic = _logicManager.JobVacancyLogic;
        Console.Clear();
        Console.WriteLine("Enter the job title:");
        string jobTitle = Console.ReadLine();

        Console.WriteLine("Enter the job description:");
        string jobDescription = Console.ReadLine();

        Console.WriteLine("Enter the salary (leave blank if not applicable):");
        string salaryInput = Console.ReadLine();

        decimal? salary;

        if (salaryInput == null || salaryInput == "")
        {
            salary = null;
        }
        else
        {
            salary = decimal.Parse(salaryInput);
        }

        Console.WriteLine("Enter the type of employment (Full-time/Part-time):");
        string employmentType = Console.ReadLine();

        vacancyLogic.AddVacancy(jobTitle, jobDescription, salary, employmentType);

        Console.WriteLine("Job vacancy has been added.");
        MenuHelper.WaitForKey(_menuManager.MainMenus.AdminMenu);
    }

    public void RemoveJobVacancy()
    {
        var vacancyLogic = _logicManager.JobVacancyLogic;
        Console.Clear();

        Console.WriteLine("Current vacancies:");
        Console.WriteLine(vacancyLogic.ShowAllVacancies());

        Console.WriteLine("\nEnter the ID of the vacancy you want to remove (or 0 to cancel):");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            if (id == 0)
            {
                Console.WriteLine("Removal canceled.");
            }
            else
            {
                bool isRemoved = vacancyLogic.RemoveVacancy(id);
                if (isRemoved)
                {
                    Console.WriteLine($"Vacancy with ID {id} has been successfully removed.");
                }
                else
                {
                    Console.WriteLine($"No vacancy found with ID {id}.");
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid ID entered.");
        }
        MenuHelper.WaitForKey(_menuManager.MainMenus.AdminMenu);
    }

    public void ViewAllVacanciesAdmin()
    {
        var vacancyLogic = _logicManager.JobVacancyLogic;
        Console.Clear();
        Console.WriteLine("\nAll vacancies:");
        Console.WriteLine(vacancyLogic.ShowAllVacancies());
        Console.WriteLine("\nPress any key to go back...");
        Console.ReadKey();
        _menuManager.MainMenus.AdminMenu();
    }

    public void AddEmployee()
    {
        EmployeeLogic employeeLogic = _logicManager.EmployeeLogic;
        Console.Clear();
        Console.WriteLine("=== Add New Employee ===\n");
        
        Console.WriteLine("Enter employee name:");
        string employeeName = Console.ReadLine();
 
        Console.WriteLine("Enter monthly salary:");
        int salary = int.Parse(Console.ReadLine());
 
        Console.WriteLine("\nPlease confirm the following details:");
        Console.WriteLine($"Name: {employeeName}");
        Console.WriteLine($"Monthly Salary: €{salary:F2}");
        
        
        if (MenuHelper.NewMenu(new List<string> {"Yes", "No"}, new List<bool> {true, false}, subtext: "\nAre you sure you want to add this employee?"))
        {
            int newId = employeeLogic.FindFirstAvailableID();
            employeeLogic.AddEmployee(new EmployeeModel(employeeName, newId, salary));
 
            Console.WriteLine("\nEmployee successfully added!");
            Thread.Sleep(1000);
        }
        else
        {
            Console.WriteLine("\nEmployee addition cancelled.");
            Thread.Sleep(1000);
        }
 
        MenuHelper.WaitForKey(_menuManager.MainMenus.AdminMenu);
    }
} 