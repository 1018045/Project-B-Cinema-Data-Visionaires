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
        