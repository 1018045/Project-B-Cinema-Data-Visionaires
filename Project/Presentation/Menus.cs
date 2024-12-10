using System.Security;
using Project.Logic.Account;
using Project.Presentation;
using Project.Helpers;

static class Menus
{
    static public void GuestMenu()
    {
        List<string> options = new List<string>
        {
            "Browse movies",
            "Select a date",
            "Login",
            "Create account",
            "Jobs",
            "Exit"
        };
        List<Action> actions = new List<Action>
        {
            Movies.MoviesBrowser,
            Showings.ShowUpcomingOnDate,
            () => Login(),
            ChooseAccount,
            ApplyForJob.ShowJobMenu,
            () => Environment.Exit(0)
        };
        MenuHelper.NewMenu(options, actions, "Cine&Dine Zidane");
    }
    
    static public void LoggedInMenu()
    {
        List<string> options = new List<string>
        {
            "Browse movies",
            "Show upcoming movie showings",
            "Show upcoming movie showings on a specific date",
            "Your reservations",
            "Manage your account",
            "Log out"
        };
        List<Action> actions = new List<Action>
        {
            Movies.MoviesBrowser,
            () => Showings.ShowUpcoming(),
            Showings.ShowUpcomingOnDate,
            () => Reservation.Adjust(AccountsLogic.CurrentAccount.Id),
            AccountPresentation.Menu,
            () => 
            {
                AccountsLogic.LogOut();
                GuestMenu();
            }
        };
        MenuHelper.NewMenu(options, actions, $"Logged in as: {AccountsLogic.CurrentAccount.EmailAddress}");
    }

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

    public static void Login(Action action = null)
    {
        AccountModel acc;
        int loginAttempts = 3;
        do
        {
            Console.Clear();
            System.Console.WriteLine($"You have {loginAttempts} login attempts left!");
            Console.WriteLine("Please enter your email address");
            string email = Console.ReadLine();

            Console.WriteLine("Please enter your password");
            SecureString pass = AccountsLogic.MaskInputstring();
            string Password = new System.Net.NetworkCredential(string.Empty, pass).Password;

            acc = AccountsLogic.Logic.CheckLogin(email, Password);
            loginAttempts--;
            if (acc == null)
            {
                Console.WriteLine("No account found with that email and password");
                if (loginAttempts <= 0)
                {
                    WaitAfterWrongLogin(30);
                    GuestMenu();
                }
            }    
        }
        while (acc == null);

        if (action != null)
        {
            action.Invoke();
            return;
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
        else 
        {
            GuestMenu();
        }
    }

    public static void WaitAfterWrongLogin(int timer)
    {
        for (;timer > 0; timer--)
        {
            Console.Clear();
            System.Console.WriteLine($"Please wait {timer} seconds before you can try again.");
            Thread.Sleep(1000);
        }
    }

    public static void AdminMenu()
    {
        List<string> options = new List<string>
        {
            "Manage movies",
            "Manage showings",
            "View all users",
            "Remove a user",
            "Add an Employee",
            "Add job vacancy",
            "Remove job vacancy",
            "View all vacancies",
            "Logout"
        };
        List<Action> actions = new List<Action>
        {
            Movies.Start,
            Showings.ManageShowings,
            ViewUsers,
            RemoveUser,
            AddEmployee,
            AddJobVacancy,
            RemoveJobVacancy,
            ViewAllVacancies,
            GuestMenu
        };
        MenuHelper.NewMenu(options, actions, "Admin menu");
    }

    private static void RemoveUser()
    {
        Console.Clear();
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
        Thread.Sleep(2000);
        MenuHelper.WaitForKey(AdminMenu);
    }

    private static void ViewUsers()
    {
        Console.Clear();
        var users = AccountsAccess.LoadAll(); 
        foreach (var user in users)
        {
            Console.WriteLine($"User: {user.EmailAddress}");
        }
        Thread.Sleep(1000);
        MenuHelper.WaitForKey(AdminMenu);
    }

    public static void ChooseAccount()
    {
        Console.WriteLine("Email: ");
        string userEmail;
        do
        {
            Console.WriteLine("Email: ");
            userEmail = Console.ReadLine();
        }
        while(AccountsLogic.VerifyEmail(userEmail) == false || AccountsLogic.CheckForExistingEmail(userEmail) == true);

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
    }

    public static void ChooseAccount(Action action)
    {
        Console.WriteLine("Email: ");
        string userEmail;
        do
        {
            Console.WriteLine("Email: ");
            userEmail = Console.ReadLine();
        }
        while(AccountsLogic.VerifyEmail(userEmail) == false || AccountsLogic.CheckForExistingEmail(userEmail) == true);

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
        action.Invoke();
    }

    public static void AccountantMenu()
    {
        List<string> options = new List<string>
        {
            "View all financial records",
            "View records for a date",
            "View total money earned",
            "View total tickets sold",
            "Log out"
        };

        List<Action> actions = new List<Action>
        {
            ViewAllRecords,
            ViewRecordsByDate,  //NOT IMPLEMENTED
            ViewTotalMoney,     //NOT IMPLEMENTED
            ViewTotalTickets,   //NOT IMPLEMENTED
            GuestMenu
        };

        MenuHelper.NewMenu(options, actions, "Accountant Menu"); 
    }

    private static void ViewRecordsByDate()
    {
        System.Console.WriteLine("NOT IMPLEMENTED YET");
        MenuHelper.WaitForKey(AccountantMenu);
    }

    private static void ViewTotalMoney()
    {
        System.Console.WriteLine("NOT IMPLEMENTED YET");
        MenuHelper.WaitForKey(AccountantMenu);
    }

    private static void ViewTotalTickets()
    {
        System.Console.WriteLine("NOT IMPLEMENTED YET");
        MenuHelper.WaitForKey(AccountantMenu);
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

        MenuHelper.WaitForKey();
    }

    private static void AddEmployee()
    {
        Console.Clear();
        EmployeeLogic employeeLogic = new (); 

        Console.WriteLine("Enter the new employee's details"); 
        Console.WriteLine("Employee Name: ");
        string EmployeeName = Console.ReadLine(); 

        Console.WriteLine("Monthly Salary: "); 
        int Salary = Convert.ToInt32(Console.ReadLine());   

        EmployeeModel employeeToAdd = new (EmployeeName,employeeLogic.FindFirstAvailableID(),Salary); 
        employeeLogic.AddEmployee(employeeToAdd);
        Console.WriteLine("Employee has been successfully added!");

        MenuHelper.WaitForKey(AdminMenu);

    }

    private static void AddJobVacancy()
    {
        Console.Clear();
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
        MenuHelper.WaitForKey(AdminMenu);
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
        MenuHelper.WaitForKey(AdminMenu);
    }

    private static void ViewAllVacancies()
    {
        Console.Clear();
        var vacancyLogic = new JobVacancyLogic();
        Console.WriteLine("\nAll vacancies:");
        Console.WriteLine(vacancyLogic.ShowAllVacancies());
        Console.WriteLine("\nPress any key to go back...");
        Console.ReadKey();
        AdminMenu();
    }
}