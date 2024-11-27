public static class ApplyForJob
{
    private static JobVacancyLogic _vacancyLogic = new JobVacancyLogic();
    private static List<JobApplication> _applications = JobApplicationAccess.LoadAll();

    public static void ShowJobMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== Job Vacancies ===");
            Console.WriteLine("1. View All Vacancies");
            Console.WriteLine("2. Apply for Job");
            Console.WriteLine("0. Return to Main Menu");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowAllVacancies();
                    break;
                case "2":
                    ApplyToVacancy();
                    break;
                case "0":
                    LoginMenu.Start();
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Thread.Sleep(2000);
                    break;
            }
        }
    }

    private static void ShowAllVacancies()
    {
        Console.Clear();
        Console.WriteLine(_vacancyLogic.ShowAllVacancies());
        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    private static void ApplyToVacancy()
    {
        Console.Clear();
        Console.WriteLine(_vacancyLogic.ShowAllVacancies());
        
        Console.WriteLine("\nEnter the ID of the vacancy you want to apply for (0 to cancel):");
        string input = Console.ReadLine();
        int vacancyId;

        if (!int.TryParse(input, out vacancyId) || vacancyId == 0)
        {
            Console.WriteLine("Application cancelled.");
            Thread.Sleep(2000);
            return;
        }

      
        if (!_vacancyLogic.VacancyExists(vacancyId))
        {
            Console.WriteLine("Vacancy not found.");
            Thread.Sleep(2000);
            return;
        }

        Console.WriteLine("\nEnter your email address:");
        string email = Console.ReadLine();

        Console.WriteLine("\nEnter your motivation:");
        string motivation = Console.ReadLine();

        
        int newId;

        if (_applications.Count == 0)
        {
            newId = 1;
        }
        else
        {
            newId = _applications.Max(a => a.ApplicationId) + 1;
        }
        var application = new JobApplication(newId, vacancyId, email, motivation, DateTime.Now);
        
        _applications.Add(application);
        JobApplicationAccess.WriteAll(_applications);

        Console.WriteLine("\nYour application has been submitted successfully!");
        Console.WriteLine("We will contact you via the provided email address.");
        Thread.Sleep(3000);
    }

    private static void DisplayVacancy(JobVacancy vacancy)
    {
        Console.WriteLine($"ID: {vacancy.VacancyId}");
        Console.WriteLine($"Position: {vacancy.JobTitle}");
        Console.WriteLine($"Description: {vacancy.JobDescription}");
        Console.WriteLine($"Type: {vacancy.EmploymentType}");
        Console.WriteLine($"Salary: {vacancy.Salary:C}");
        Console.WriteLine($"Date Posted: {vacancy.DatePosted:d}");
    }
}    