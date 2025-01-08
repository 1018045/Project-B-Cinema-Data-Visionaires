using System.Runtime.CompilerServices;
using System.Security;
using Project.Logic.Account;
using Project.Presentation;

static class Menus
{
    private static CinemaLogic _cinemaLogic = new();
    static public void ChooseCinema(Action originMenu, Action cancellationMenu)
    {
        CinemaLogic cinemaLogic = new();
        List<string> options = cinemaLogic.Cinemas.Select(c => c.Name).ToList();
        List<Action> actions = new();
        foreach (CinemaModel cinema in cinemaLogic.Cinemas)
        {
            actions.Add(() => {
                cinemaLogic.ChangeCinema(cinema);
                originMenu.Invoke();
            });
        }
        options.Add("Cancel");
        actions.Add(cancellationMenu);
        MenuHelper.NewMenu(options, actions, "Please select a cinema location");
    }

    static public void GuestMenu()
    {
        MoviesLogic _moviesLogic = new();
        List<string> options = new List<string>
        {
            "Browse movies",
            "Select a date",
            "Login",
            "Create account",
            "Select a cinema location",
            "Jobs",
            "About/Contact",
            "Exit"
        };
        List<Action> actions = new List<Action>
        {
            () => Movies.MoviesBrowser(),
            Reservation.SelectDate,
            () => Login(),
            () => CreateAccount(LoggedInMenu),
            () => ChooseCinema(GuestMenu, GuestMenu),
            ApplyForJob.ShowJobMenu,
            () => AboutContact(GuestMenu),
            () => Environment.Exit(0)
        };
        MenuHelper.NewMenu(options, actions, "Zidane", promotedMovies: _moviesLogic.PromotedMovies, showCurrentLocation: true, showMenu: true);
    }

    static public void AboutContact(Action returnMenu)
    {
        Console.Clear();
        if (CinemaLogic.CurrentCinema == null)
        {
            System.Console.WriteLine("Please first select a cinema!");
            Thread.Sleep(2000);
            ChooseCinema(() => AboutContact(returnMenu), returnMenu);
            return;
        }
        
        Console.WriteLine($"=== About Cine&Dine {CinemaLogic.CurrentCinema.Name} ===\n");
        Console.WriteLine($"Welcome to Cine&Dine {CinemaLogic.CurrentCinema.Name} - where film and culinary delight come together!");
        Console.WriteLine("We offer a unique cinema experience where you can enjoy the latest movies");
        Console.WriteLine("while being pampered with delicious dishes and drinks.\n");
        
        Console.WriteLine("=== Contact Information ===");
        Console.WriteLine($"{CinemaLogic.CurrentCinema.Address}, {CinemaLogic.CurrentCinema.City} {CinemaLogic.CurrentCinema.PostalCode}");
        
        Console.WriteLine($"Phone: {CinemaLogic.CurrentCinema.PhoneNumber}");
        Console.WriteLine($"Email: info-{CinemaLogic.CurrentCinema.Name}@cineanddine.nl\n");
        
        Console.WriteLine("=== Opening Hours ===");
        Console.WriteLine("Monday through Sunday: 12:00 - 00:00\n");
        
        MenuHelper.WaitForKey(returnMenu);
    }


    static public void LoggedInMenu()
    {
        MoviesLogic _moviesLogic = new();

        List<string> options = new List<string>
        {
            "Browse movies",
            "Select a date",
            "View Reservations and Bills",
            "Select a cinema location",
            "Manage your account",
            "about/contact",
            "Log out"
        };
        List<Action> actions = new List<Action>
        {
            () => Movies.MoviesBrowser(),
            Reservation.SelectDate,
            BillPresentation.ViewUserBillsAndReservations,
            () => ChooseCinema(LoggedInMenu, LoggedInMenu),
            AccountPresentation.Menu,
            () => AboutContact(LoggedInMenu),   
            () => 
            {
                AccountsLogic.LogOut();
                GuestMenu();
            }
        };
        MenuHelper.NewMenu(options, actions, $"Logged in as: {AccountsLogic.CurrentAccount.EmailAddress}", promotedMovies: _moviesLogic.PromotedMovies, showCurrentLocation: true, showMenu: true);
    }



    public static void Login(Action action = null, bool acceptOnlyCustomerLogin = false)
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

        if (acceptOnlyCustomerLogin && acc is not UserModel)
        {
            System.Console.WriteLine("Error: You can only login with a user account on this screen");
            Thread.Sleep(2500);
            MenuHelper.WaitForKey();
            return;
        }

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
        AccountantLogic accountantLogic = new();

        List<string> options = new List<string>
        {
            "Manage movies",
            "Manage showings",
            "Manage movie promotions",
            "View all users",
            "Remove a user",
            "Add an Employee",
            "Add job vacancy",
            "Remove job vacancy",
            "View all vacancies",
            "Show accountant options",
            "Manage cinema locations",
            "Logout"
        };
        List<Action> actions = new List<Action>
        {
            Movies.Start,
            Showings.ManageShowings,
            Movies.SelectPromotionSlot,
            ViewUsers,
            RemoveUser,
            AddEmployee,
            AddJobVacancy,
            RemoveJobVacancy,
            ViewAllVacancies,
            AccountantMenu,
            CinemaLocations.ChooseCinemaLocationToManage,
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

    public static void CreateAccount(Action action)
    {
        Console.Clear();
        Console.WriteLine("Enter your information below");
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
        DateTime userBirthDate;

        Console.WriteLine("Your BirthDay");
        userBirthDate = Reservation.AskAndParsePastDate();

        AccountsLogic accountsLogic = new();
        accountsLogic.UpdateList(userEmail, Password, fullName, userBirthDate.Date);

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
            // "View all financial records",
            "View income for specific month",
            "View current monthly expenses",
            // "View total tickets sold",
            "View employee salaries",
            // "Log out"
        };

        List<Action> actions = new List<Action>
        {
            // ViewAllRecords,
        () => 
        {
            Console.Write("Enter the month number: ");
            int month = Convert.ToInt32(Console.ReadLine());
            ViewIncomeByMonth(month); // Calling with the month entered by the user
        },
        () => 
        {
            ViewMonthlyExpenses();
        },
            // ViewTotalTickets,
            ViewEmployeeSalaries,   
            // GuestMenu
        };

        MenuHelper.NewMenu(options, actions, "Accountant Menu"); 
    }

    public static void ViewIncomeByMonth(int month)
    {
        AccountantLogic accountantLogic = new();
        
        double Records = accountantLogic.GetIncomeByMonth(month);

        Console.WriteLine(Records);
    }

   public static void ViewMonthlyExpenses()
   {
        AccountantLogic accountantLogic = new (); 

        Console.WriteLine(accountantLogic.CalculateCosts());
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

    private static void AddEmployee()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Employee ===\n");
        
        Console.WriteLine("Enter employee name:");
        string employeeName = Console.ReadLine();
 
        Console.WriteLine("Enter monthly salary:");
        int salary = int.Parse(Console.ReadLine());
 
        Console.WriteLine("\nPlease confirm the following details:");
        Console.WriteLine($"Name: {employeeName}");
        Console.WriteLine($"Monthly Salary: €{salary:F2}");
        Console.WriteLine("\nAre you sure you want to add this employee? (Y/N)");
        
        string confirmation = Console.ReadLine().ToUpper();
        if (confirmation == "Y")
        {
            EmployeeLogic employeeLogic = new();
            int newId = employeeLogic.FindFirstAvailableID();
            EmployeeModel newEmployee = new(employeeName, newId, salary);
            employeeLogic.AddEmployee(newEmployee);
 
            Console.WriteLine("\nEmployee successfully added!");
            Thread.Sleep(2000);
        }
        else
        {
            Console.WriteLine("\nEmployee addition cancelled.");
            Thread.Sleep(2000);
        }
 
        MenuHelper.WaitForKey(AdminMenu);
    }

    private static void ViewEmployeeSalaries()
    {
        Console.Clear();
        Console.WriteLine("Employee Salaries\n");

        var employeeLogic = new EmployeeLogic();
        var employees = employeeLogic.ListOfEmployees;

        if (employees == null || employees.Count == 0) 
        {
            Console.WriteLine("check if there are no empoyee.");
        }
        else
        {
            Console.WriteLine("Name\t\t\tID\t\tSalary");

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.EmployeeName,-20}\t{employee.EmployeeID}\t\t€{employee.EmployeeSalary:F2}");
            }

            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine($"Total Monthly Cost: €{employeeLogic.GetTotalMonthlySalary():F2}");
        }

        MenuHelper.WaitForKey(AccountantMenu); 
    }
}