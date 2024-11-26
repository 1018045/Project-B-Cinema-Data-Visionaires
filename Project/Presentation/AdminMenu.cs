using System;
using System.Collections.Generic;

static class AdminMenu
{
    public static void Start()
    {
        Console.WriteLine("Admin Menu:");
        Console.WriteLine("1. Add a movie");
        Console.WriteLine("2. Remove a user");
        Console.WriteLine("3. View all users");
        Console.WriteLine("4. Remove a movie screening");
        Console.WriteLine("5. View all movies");
        Console.WriteLine("6. Add job vacancy");
        Console.WriteLine("7. Remove job vacancy");
        Console.WriteLine("8. View all vacancies");
        Console.WriteLine("9. Return to main menu");

        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                AddMovie();
                break;
            case "2":
                RemoveUser();
                break;
            case "3":
                ViewUsers();
                break;
            case "4":
                RemoveMovie();
                break;
            case "5":
                ViewMovies();
                break;
            case "6":
                AddJobVacancy();
                break;
            case "7":
                RemoveJobVacancy();
                break;
            case "8":
                ViewAllVacancies();
                break;
            case "9":
                LoginMenu.Start();
                break;
            default:
                Console.WriteLine("Invalid choice, please try again.");
                Start();
                break;
        }
    }

    private static void AddMovie()
    {
        Console.WriteLine("Enter the movie title:");
        string title = Console.ReadLine();

        Console.WriteLine("Enter the date (dd-MM-yyyy HH:mm:ss):");
        string date = Console.ReadLine();

        Console.WriteLine("Enter the hall:");
        int room = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter the minimum age:");
        int minimumAge = int.Parse(Console.ReadLine());

        // Maak een instantie van ShowingsLogic
        ShowingsLogic showingsLogic = new ShowingsLogic();
        
        // Voeg de nieuwe vertoning toe via de logica-laag
        showingsLogic.AddShowing(title, date, room, minimumAge);

        Console.WriteLine($"Movie screening ‘{title}’ has been added.");
        Start(); // Terug naar het adminmenu
    }

    private static void RemoveUser()
    {
        Console.WriteLine("Enter the email address of the user you want to remove:");
        string email = Console.ReadLine();

        AccountsLogic accountsLogic = new AccountsLogic();
        bool isRemoved = accountsLogic.RemoveUser(email);

        if (isRemoved)
        {
            Console.WriteLine($"User with email address ‘{email}’ has been removed.");
        }
        else
        {
            Console.WriteLine($"No user found with email address ‘{email}'.");
        }
        
        Start(); 
    }

    private static void RemoveMovie()
    {
        Console.WriteLine("Enter the ID of the movie screening you want to remove:");
        int id = int.Parse(Console.ReadLine());

        ShowingsLogic showingsLogic = new ShowingsLogic();
        showingsLogic.RemoveShowing(id);
        Start(); 
    }

    private static void ViewUsers()
    {
        var users = AccountsAccess.LoadAll(); 
        foreach (var user in users)
        {
            Console.WriteLine($"User: {user.EmailAddress}");
        }
        Start(); 
    }

    private static void ViewMovies()
    {
        ShowingsLogic showingsLogic = new ShowingsLogic();

        Console.WriteLine("\nAll movie screenings:");
        
        Console.WriteLine("----------------------------------------");
        
        
        string allShowings = showingsLogic.ShowAll();
        Console.WriteLine(allShowings);

        Console.WriteLine("\nPress any key to go back…");
        Console.ReadKey();
        Start();
        
    }

    private static void AddJobVacancy()
    {
        Console.WriteLine("Enter the job title:");
        string jobTitle = Console.ReadLine();

        Console.WriteLine("Enter the job description:");
        string jobDescription = Console.ReadLine();

        Console.WriteLine("Enter the salary (leave blank if not applicable):");
        string salaryInput = Console.ReadLine();

        decimal? salary;

        if (string.IsNullOrEmpty(salaryInput))
        {
            salary = null;
        }
        else
        {
            salary = decimal.Parse(salaryInput);
        }

        Console.WriteLine("Enter the type of employment (Full-time/Part-time):");
        string employmentType = Console.ReadLine();

        var vacancyLogic = new JobVacancyLogic();
        vacancyLogic.AddVacancy(jobTitle, jobDescription, salary, employmentType);

        Console.WriteLine("Job vacancy has been added.");
        Start();
    }

    private static void RemoveJobVacancy()
    {
        Console.Clear();
        var vacancyLogic = new JobVacancyLogic();
        
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
        
        Console.WriteLine("\nPress any key to go back...");
        Console.ReadKey();
        Start();
    }

    private static void ViewAllVacancies()
    {
        var vacancyLogic = new JobVacancyLogic();
        Console.WriteLine("\nAll vacancies:");
        Console.WriteLine(vacancyLogic.ShowAllVacancies());
        Console.WriteLine("\nPress any key to go back...");
        Console.ReadKey();
        Start();
    }
}