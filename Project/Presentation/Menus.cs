using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;

static class Menus
{
    static public void Start()
    {
        Console.Clear();
        Console.WriteLine("Enter 1 to login as User");
        Console.WriteLine("Enter 2 to create an account");
        Console.WriteLine("Enter 3 to show upcoming movie showings");
        Console.WriteLine("Enter 4 to show upcoming movie showings on a specific date");
        Console.WriteLine("Enter 5 to login as Admin");
        Console.WriteLine("Enter 6 to login as Accountant");
        Console.WriteLine("Enter 7 to exit program");

        string input = Console.ReadLine();
        switch (input)
        {
            case "1":
                UserLogin();
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
                AdminLogin();
                break;
            case "6":
                AccountantMenu();
                break;
            case "7":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid input");
                Start();
                break;
        }
    }


      public static void UserLogin()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");

        SecureString pass = AccountsLogic.MaskInputstring();  
        string Password = new System.Net.NetworkCredential(string.Empty, pass).Password; 

        AccountModel acc = AccountsLogic.Logic.CheckLogin(email, Password);
        if (acc != null)
        {
            Console.WriteLine("Welcome back " + acc.FullName);
            Console.WriteLine("Your email is " + acc.EmailAddress);

            //Write some code to go back to the menu
            Start();
        }
        else
        {
            Console.WriteLine("No account found with that email and password");
            Start();
        }
    }


    public static void AdminLogin()
    {
        Console.WriteLine("Enter your email address:");
        string email = Console.ReadLine();
        Console.WriteLine("Enter your password:");
        string password = Console.ReadLine();

        var admins = AdminAccess.LoadAll();
        var admin = admins.FirstOrDefault(a => a.EmailAddress == email && a.Password == password);

        if (admin != null)
        {
            Console.WriteLine("Login successful! Welcome," + admin.EmailAddress);
            AdminMenu();
            
        }
        else
        {
            Console.WriteLine("Invalid login credentials. Please try again.");
            Start(); // Herstart de login
        }
    }
public static void AdminMenu()
    {
        Console.WriteLine("Admin Menu:");
        Console.WriteLine("1. Add a movie");
        Console.WriteLine("2. Remove a user");
        Console.WriteLine("3. View all users");
        Console.WriteLine("4. Remove a movie screening");
        Console.WriteLine("5. View all movies");
        Console.WriteLine("6. Return to main menu");

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
        
        Start(); // Terug naar het adminmenu
    }

    private static void RemoveMovie()
    {
        Console.WriteLine("Voer het ID van de filmvertoning in die je wilt verwijderen:");
        int id = int.Parse(Console.ReadLine());

        ShowingsLogic showingsLogic = new ShowingsLogic();
        showingsLogic.RemoveShowing(id);
        Start(); // Terug naar het adminmenu
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


    static public void LoggedInMenu()
    {
        Console.WriteLine("\n" + new string('-', Console.WindowWidth));
        Console.WriteLine($"You are logged in as {AccountsLogic.CurrentAccount.FullName}");
        Console.WriteLine(new string('-', Console.WindowWidth));
        Console.WriteLine("Enter 1 to make a reservation");
        Console.WriteLine("Enter 2 to show upcoming movie showings");
        Console.WriteLine("Enter 3 to show upcoming movie showings on a specific date");
        Console.WriteLine("Enter 4 to show your reservations");
        Console.WriteLine("Enter 5 to adjust your reservations");
        Console.WriteLine("Enter 6 to log out");

        string input = Console.ReadLine().Trim();
        if (input == "1")
        {
            Reservation.Make();
        }
        else if( input == "2")
        {
            Showings.ShowUpcoming();
            Start();
        }
        else if( input == "3")
        {
            Showings.ShowUpcomingOnDate();
            Start();
        }
        else if (input == "4")
        {
            Reservation.Show(AccountsLogic.CurrentAccount.Id);
        }
        else if (input == "5")
        {
            Reservation.Adjust(AccountsLogic.CurrentAccount.Id);
        }
        else if (input == "6")
        {
            AccountsLogic.LogOut();
            Console.WriteLine("\nYou are now logged out\n");
            Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
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
        if (!AccountantLogin())
        {
            Console.WriteLine("Login failed. Returning to main menu.");
            Start();
            return;
        }

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

    private static bool AccountantLogin()
    {
        Console.WriteLine("Enter accountant username:");
        string username = Console.ReadLine();
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();

        // Simpele inlogcontrole6
        return username == "accountant" && password == "accountant123";
    }

    private static void ViewAllRecords()
    {
        // Haal alle records op via AccountantAccess
        var records = AccountantAccess.LoadAll();
        
        Console.WriteLine("\nAll Financial Records:");
        Console.WriteLine("ID | Date | Movie | Tickets | Money | Room");
        Console.WriteLine("----------------------------------------");
        
        // Toon elk record
        foreach(var record in records)
        {
            Console.WriteLine($"{record.Id} | {record.Date} | {record.MovieTitle} | " +
                            $"{record.TicketsSold} | ${record.Revenue} | {record.Room}");
        }

        WaitForKey();
    }

    private static void WaitForKey()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}