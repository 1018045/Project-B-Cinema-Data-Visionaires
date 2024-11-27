using System.Security;
using Project.Logic.Account;
using Project.Presentation;
using System.Security.Cryptography.X509Certificates;

static class Menus
{
    static public void Start()
    {
        Console.WriteLine("Enter 1 to login");
        Console.WriteLine("Enter 2 to create an account");
        Console.WriteLine("Enter 3 to show upcoming movie showings");
        Console.WriteLine("Enter 4 to show upcoming movie showings on a specific date");
        Console.WriteLine("Enter 5 View Job Menu");
        Console.WriteLine("Enter 6 to exit program");

        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                Login();
                break;
            case "2":
                ChooseAccount();
                break;
            case "3":
                Showings.ShowUpcoming();
                Start();
                break;
            case "4":
                Showings.ShowUpcomingOnDate();
                Start();
                break;
            case "5":
                ApplyForJob.ShowJobMenu();
                break;
            case "6":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid input");
                Start();
                break;
        }
    }

    public static void Login()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");

        SecureString pass = AccountsLogic.MaskInputstring();
        string Password = new System.Net.NetworkCredential(string.Empty, pass).Password; 

        AccountModel acc = AccountsLogic.Logic.CheckLogin(email, Password);
        if (acc == null)
        {
            Console.WriteLine("No account found with that email and password");
            Start();
        }

        if (acc is UserModel user)
        {
            Console.WriteLine("Welcome back " + user.FullName);
            LoggedInMenu();
        }
        else if (acc is AdminModel admin)
        {
            Console.WriteLine("Welcome back " + admin.EmailAddress);
            AdminMenu();
        }
        else if (acc is AccountantModel accountant)
        {
            System.Console.WriteLine("Welcome back " + accountant.EmailAddress);
            AccountantMenu();
        }   
    }

    public static void AdminMenu()
    {
        Console.WriteLine("Admin Menu:");
        Console.WriteLine("1. Add a movie");
        Console.WriteLine("2. Manage movie showings");
        Console.WriteLine("3. View all users");
        Console.WriteLine("4. Remove a user");
        Console.WriteLine("5. Add an Employee");
        Console.WriteLine("6. Add job vacancy");
        Console.WriteLine("7. Remove job vacancy");
        Console.WriteLine("8. View all vacancies");
        Console.WriteLine("9. Logout");

        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                AddMovie();
                AdminMenu();
                break;
            case "2":
                Showings.ManageShowings();
                AdminMenu();
                break;
            case "3":
                ViewUsers();
                AdminMenu();
                break;
            case "4":
                RemoveUser();
                AdminMenu();
                break;
            case "5":
                AddEmployee();
                AdminMenu();
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
                Start();
                break;
            default:
                Console.WriteLine("Invalid choice, please try again.");
                Start();
                break;
        }
    }

    private static void AddMovie()
    {
        MoviesLogic moviesLogic = new();
        Console.WriteLine("Enter the movie title:");
        string title = Console.ReadLine();
        if (moviesLogic.FindMovieByTitle(title) != null)
        {
            Console.WriteLine("Movie is already in the database");
            return;
        }
        Console.WriteLine("Enter the total screen time in minutes:");
        int duration = Math.Abs(int.Parse(Console.ReadLine()));

        Console.WriteLine("Enter the minimum age (11-18):");
        int minimumAge = Math.Clamp(int.Parse(Console.ReadLine()), 11, 18);

        moviesLogic.AddMovie(title, duration, minimumAge);
        Console.WriteLine($"Movie ‘{title}’ has been added to the database.");

        System.Console.WriteLine("Which extra's are mandatory for this movie?");
        // TODO
    }

    private static void RemoveUser()
    {
        Console.WriteLine("Enter the email address of the user you want to remove:");
        string email = Console.ReadLine();

        AccountsLogic accountsLogic = new AccountsLogic();
        bool isRemoved = accountsLogic.RemoveUser(email);

        if (isRemoved)
        {
            Console.WriteLine($"User with email address '{email}' has been removed.");
        }
        else
        {
            Console.WriteLine($"No user found with email address '{email}'.");
        }
    }

    private static void ViewUsers()
    {
        var users = AccountsAccess.LoadAll(); 
        foreach (var user in users)
        {
            Console.WriteLine($"User: {user.EmailAddress}");
        }
    }

    static public void LoggedInMenu()
    {
        Console.WriteLine("\n" + new string('-', Console.WindowWidth));
        Console.WriteLine($"You are logged in as {AccountsLogic.CurrentAccount.EmailAddress}");
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine("Enter 1 to make a reservation");
        Console.WriteLine("Enter 2 to show upcoming movie showings");
        Console.WriteLine("Enter 3 to show upcoming movie showings on a specific date");
        Console.WriteLine("Enter 4 to show your reservations");
        Console.WriteLine("Enter 5 to adjust your reservations");
        Console.WriteLine("Enter 6 to manage your account");
        Console.WriteLine("Enter 7 to log out");

        string input = Console.ReadLine().Trim();
        switch (input)
        {
            case "1":
                Reservation.Make();
                break;
            case "2":
                Showings.ShowUpcoming();
                Start();
                break;
            case "3":
                Showings.ShowUpcomingOnDate();
                Start();
                break;
            case "4":
                Reservation.Show(AccountsLogic.CurrentAccount.Id);
                break;
            case "5":
                Reservation.Adjust(AccountsLogic.CurrentAccount.Id);
                break;
            case "6":
                AccountPresentation.Menu();
                break;
            case "7":
                AccountsLogic.LogOut();
                Console.WriteLine("\nYou are now logged out\n");
                Start();
                break;
            default:
                Console.WriteLine("Invalid input");
                Start();
                break;
        }
    }

    public static void ChooseAccount()
    {
        Console.WriteLine("What type of account would you like to create?");
        Console.WriteLine("Choose the from the options below");
        Console.WriteLine("1)User");

        var userinput = Console.ReadLine();
        while(AccountsLogic.ParseInt(userinput) != 1)
        {
            Console.WriteLine("Input was incorrect. Try again");
            userinput = Console.ReadLine();
        }

        Console.WriteLine("Enter the information below");
        Console.WriteLine("Email: ");
        var userEmail = Console.ReadLine();
        while(AccountsLogic.VerifyEmail(userEmail) == false || AccountsLogic.CheckForExistingEmail(userEmail) == true)
        {
            Console.WriteLine("Email: ");
            userEmail = Console.ReadLine();
        }



        Console.WriteLine("Enter your password. It must contain at least 8 characters which consist of 1 capital letter, 1 number, and 1 special character e.g. $,#,% etc.");
        SecureString pass = AccountsLogic.MaskInputstring();
        string Password = new System.Net.NetworkCredential(string.Empty, pass).Password; 
   
        while (AccountsLogic.VerifyPassword(Password) == false)
        {
            Console.WriteLine("Password was not valid. Try again.");
        
        
            pass = AccountsLogic.MaskInputstring();
            Password = new System.Net.NetworkCredential(string.Empty, pass).Password; 
        }
        Console.WriteLine("Confirm your password");
        
        string confirmPassword = string.Empty;
        bool passwordsMatch = false;

        while (!passwordsMatch)
        {
            SecureString confirmPass = AccountsLogic.MaskInputstring();
            confirmPassword = new System.Net.NetworkCredential(string.Empty, confirmPass).Password;

            
            if (AccountsLogic.VerifyPassword(confirmPassword) && confirmPassword == Password)
            {
                passwordsMatch = true;
            }
            else
            {
                Console.WriteLine("Passwords do not match or are not valid. Try again.");
            }
        }

        Console.WriteLine("Password confirmed successfully.");

        Console.WriteLine("Your fullname: ");
        var fullName = Console.ReadLine();
        Console.WriteLine("Your age");
        var userAge = Console.ReadLine();
        while(AccountsLogic.IsInt(userAge) == false)
        {
            Console.WriteLine("Input is not valid. Please enter a number");
            userAge = Console.ReadLine();
        }

        AccountsLogic accountsLogic = new();
        accountsLogic.UpdateList(userEmail, Password, fullName, Convert.ToInt32(userAge));

        Console.WriteLine($"\nSuccessfully created your account, welcome {fullName}!");

        accountsLogic.CheckLogin(userEmail, Password);

        //wait so that it is more clear
        Thread.Sleep(1500);

        Console.WriteLine("\n");
        Menus.Start();
        // accountsLogic.UpdateList(new AccountModel(userEmail, userPassword, fullName));
            
        
        //    System.Console.WriteLine("2)Admin");
        //    System.Console.WriteLine("3)Finance");
        //    System.Console.WriteLine("4)Employee");
    }

    public static void AccountantMenu()
    {
        while (true)
        {
            // Maak het scherm leeg voor betere leesbaarheid
            Console.Clear();

            // Toon menu opties
            Console.WriteLine("\n=== Accountant Menu ===");
            Console.WriteLine("1. View all financial records");
            Console.WriteLine("2. View records for a date");
            Console.WriteLine("3. View total money earned");
            Console.WriteLine("4. View total tickets sold");
            Console.WriteLine("5. Return to main menu");
            Console.WriteLine("=======================");

            // Vraag gebruiker om een keuze
            string choice = Console.ReadLine();

            // Verwerk de keuze van de gebruiker
            switch (choice)
            {
                case "1":
                    ViewAllRecords();
                    break;
                case "2":
                    //ViewRecordsByDate();
                    break;
                case "3":
                    //ViewTotalMoney();
                    break;
                case "4":
                    //ViewTotalTickets();
                    break;
                case "5":
                    Start();
                    break; // Ga terug naar hoofdmenu
                default:
                    Console.WriteLine("Invalid choice. Press any key to try again.");
                    Console.ReadKey();
                    Start();
                    break;
            }
        }
    }

    private static void ViewAllRecords()
    {
        // Haal alle records op via AccountantAccess
        var records = AccountantAccess.LoadAll();
        
        Console.WriteLine("\nAll Financial Records:");
        Console.WriteLine("ID | Date | Movie | Tickets | Money | Room");
        Console.WriteLine("----------------------------------------");
        
        // Toon elk record
        // foreach(var record in records)
        // {
        //     Console.WriteLine($"{record.Id} | {record.Date} | {record.MovieTitle} | " +
        //                     $"{record.TicketsSold} | ${record.Revenue} | {record.Room}");
        // }

        WaitForKey();
    }

    private static void WaitForKey()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    private static void AddEmployee()
    {
        EmployeeLogic employeeLogic = new (); 

        Console.WriteLine("Enter the new employee's details"); 
        Console.WriteLine("Employee Name: ");
        string EmployeeName = Console.ReadLine(); 

        Console.WriteLine("Monthly Salary: "); 
        int Salary = Convert.ToInt32(Console.ReadLine());   

        EmployeeModel employeeToAdd = new (EmployeeName,employeeLogic.FindFirstAvailableID(),Salary); 
        employeeLogic.AddEmployee(employeeToAdd);
        Console.WriteLine("Employee has been successfully added!");

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
        AdminMenu();
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
        AdminMenu();
    }

    private static void ViewAllVacancies()
    {
        var vacancyLogic = new JobVacancyLogic();
        Console.WriteLine("\nAll vacancies:");
        Console.WriteLine(vacancyLogic.ShowAllVacancies());
        Console.WriteLine("\nPress any key to go back...");
        Console.ReadKey();
        AdminMenu();
    }
}