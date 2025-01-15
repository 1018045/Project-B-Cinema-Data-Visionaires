using System.Security;
using System.Net;
using Project.Logic.Account;
using Project.DataModels;
using System.Globalization;

namespace Project.Presentation;

public class AccountPresentation
{
    private LogicManager _logicManager;
    private MenuManager _menuManager;


    public AccountPresentation(LogicManager logicManager, MenuManager menuManager)
    {
        _logicManager = logicManager;
        _menuManager = menuManager;
    }
    public void Menu()
    {
        List<string> options = new List<string>
        {
            "Update account details",
            "Remove account (DANGER)",   
            "Return"
        };

        List<Action> actions = new List<Action>
        {
            () => UpdateAccountDetails(),
            () => DeleteAccount(),           
            _menuManager.MainMenus.LoggedInMenu
        };

        MenuHelper.NewMenu(options, actions, "Manage account");
    }

    public void Login(Action action = null, bool acceptOnlyCustomerLogin = false)
    {
        AccountModel acc;
        int loginAttempts = 3;
        do
        {
            Console.Clear();
            Console.WriteLine($"You have {loginAttempts} login attempts left!");
            Console.WriteLine("Please enter your email address");
            string email = Console.ReadLine();

            Console.WriteLine("Please enter your password");
            SecureString pass = AccountsLogic.MaskInputstring();
            string Password = new NetworkCredential(string.Empty, pass).Password;

            acc = _logicManager.AccountsLogic.CheckLogin(email, Password);
            loginAttempts--;
            if (acc == null)
            {
                Console.WriteLine("No account found with that email and password");
                if (loginAttempts <= 0)
                {
                    WaitAfterWrongLogin(30);
                    _menuManager.MainMenus.GuestMenu();
                    return;
                }
            }    
        }
        while (acc == null);

        if (acceptOnlyCustomerLogin && acc is not UserModel)
        {
            Console.WriteLine("Error: You can only login with a user account on this screen");
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
            _menuManager.MainMenus.LoggedInMenu();
        }
        else if (acc is AdminModel admin)
        {
            Console.WriteLine("Welcome back " + admin.EmailAddress);
            _menuManager.MainMenus.AdminMenu();
        }
        else if (acc is AccountantModel accountant)
        {
            Console.WriteLine("Welcome back " + accountant.EmailAddress);
            _menuManager.MainMenus.AccountantMenu();
        }
        else if (acc is StaffModel staff)
        {
            Console.WriteLine("Welcome back " + staff.EmailAddress);
            _menuManager.MainMenus.StaffMenu();
        }
        else 
        {
            _menuManager.MainMenus.GuestMenu();
        }
    }

    public void WaitAfterWrongLogin(int timer)
    {
        for (;timer > 0; timer--)
        {
            Console.Clear();
            Console.WriteLine($"Please wait {timer} seconds before you can try again.");
            Thread.Sleep(1000);
        }
    }

    private void UpdateAccountDetails()
    {
        List<string> options = new List<string>
        {
            "Change email",
            "Change password",
            "Return"
        };

        List<Action> actions = new List<Action>
        {
            () => ChangeEmail(),
            () => ChangePassword(),
            Menu
        };

        MenuHelper.NewMenu(options, actions, "Update account");
    }

    private void ChangeEmail()
    {
        AccountManageLogic logic = _logicManager.AccountManageLogic;
        var newEmail = "";
        for (var done = false; !done;)
        {
            Console.WriteLine("");
            Console.WriteLine("New email: ");

            newEmail = Console.ReadLine()!;
            done = logic.VerifyEmail(newEmail);
        }

        Console.WriteLine("Confirm email: ");

        if (!newEmail.Equals(Console.ReadLine()!))
        {
            Console.Clear();
            Console.WriteLine("Emails do not match");
            Thread.Sleep(2500);
            UpdateAccountDetails();
            return;
        }

        logic.ChangeEmail(newEmail);

        Console.Clear();
        Console.WriteLine($"Successfully changed your email to {newEmail}");
        UpdateAccountDetails();
        MenuHelper.WaitForKey(() => UpdateAccountDetails());
    }

    private void ChangePassword()
    {
        AccountManageLogic logic = _logicManager.AccountManageLogic;
        var newPassword = "";
        for (var done = false; !done;)
        {
            Console.WriteLine("");
            Console.WriteLine("Enter your password. It must contain at least 8 characters which consist of 1 capital letter, 1 number, and 1 special character e.g. $,#,% etc.");
            Console.WriteLine("New password: ");

            newPassword = Console.ReadLine()!;
            done = logic.VerifyPassword(newPassword);
        }

        Console.WriteLine("Confirm password: ");
        if (!newPassword.Equals(Console.ReadLine()!))
        {
            Console.Clear();
            Console.WriteLine("Passwords do not match");
            Thread.Sleep(2500);
            UpdateAccountDetails();
            return;
        }

        logic.ChangePassword(newPassword);

        Console.Clear();
        Console.WriteLine($"Successfully changed your password");
        Thread.Sleep(2500);
        MenuHelper.WaitForKey(() => UpdateAccountDetails());
    }

    private void DeleteAccount()
    {
        AccountManageLogic logic = _logicManager.AccountManageLogic;
        bool confirmed = MenuHelper.NewMenu(new List<string>(){ "Yes", "No"},
                                            new List<bool>() { true, false },
                                            "Are your sure you want to delete your account?");
                                
        if (confirmed)
        {
            logic.DeleteAccount();

            Console.Clear();
            Console.WriteLine("Successfully deleted your account");
            Console.WriteLine("You will now be logged out");
            MenuHelper.WaitForKey(_menuManager.MainMenus.GuestMenu);
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Canceled the deletion of your account");
            Console.WriteLine("You will return to the menu");
            MenuHelper.WaitForKey(Menu);
        }
    
    }

    public void CreateAccount(Action action)
    {
        AccountsLogic accountsLogic = _logicManager.AccountsLogic;
        Console.Clear();
        Console.WriteLine("Enter your information below");
        string userEmail;
        do
        {
            Console.WriteLine("Email: ");
            userEmail = Console.ReadLine();
        }
        while(accountsLogic.VerifyEmail(userEmail) == false || accountsLogic.CheckForExistingEmail(userEmail) == true);

        Console.WriteLine("Enter your password. It must contain at least 8 characters which consist of 1 capital letter, 1 number, and 1 special character e.g. $,#,% etc.");
        SecureString pass = AccountsLogic.MaskInputstring();
        string Password = new NetworkCredential(string.Empty, pass).Password;
   
        while (accountsLogic.VerifyPassword(Password) == false)
        {
            Console.WriteLine("Password was not valid. Try again.");
            pass = AccountsLogic.MaskInputstring();
            Password = new NetworkCredential(string.Empty, pass).Password;
        }
        Console.WriteLine("Confirm your password");
        
        string confirmPassword = string.Empty;
        bool passwordsMatch = false;

        while (!passwordsMatch)
        {
            SecureString confirmPass = AccountsLogic.MaskInputstring();
            confirmPassword = new NetworkCredential(string.Empty, confirmPass).Password;

            if (accountsLogic.VerifyPassword(confirmPassword) && confirmPassword == Password)
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
        userBirthDate = AskAndParsePastDate();

        accountsLogic.UpdateList(userEmail, Password, fullName, userBirthDate.Date);

        Console.WriteLine($"\nSuccessfully created your account, welcome {fullName}!");

        accountsLogic.CheckLogin(userEmail, Password);

        //wait so that it is more clear
        Thread.Sleep(1500);
        action.Invoke();
    }

    public void ViewUsers()
    {
        Console.Clear();
    
        foreach (var user in _logicManager.AccountsLogic.Accounts)
        {
            Console.WriteLine($"User: {user.EmailAddress}");
        }
        Thread.Sleep(1000);
        MenuHelper.WaitForKey(_menuManager.MainMenus.AdminMenu);
    }

    public void RemoveUser()
    {
        Console.Clear();
        Console.WriteLine("Enter the email address of the user you want to remove:");
        string email = Console.ReadLine();

        AccountsLogic accountsLogic = _logicManager.AccountsLogic;
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
        MenuHelper.WaitForKey(_menuManager.MainMenus.AdminMenu);
    }

    public void AddStaffAccount()
    {
        Console.Clear();
        Console.WriteLine("Enter staff email:");
        var email = Console.ReadLine()!;

        string password = "";
        for (bool match = false; !match;)
        {
            Console.Clear();
            Console.WriteLine("Enter staff password");
            password = Console.ReadLine();
            Console.WriteLine("Re-enter password");
            match = Console.ReadLine()!.Equals(password);
        }

        _logicManager.AccountManageLogic.CreateStaffAccount(password, email);

        Console.Clear();
        Console.WriteLine("Staff account created successfully");
        Console.WriteLine("Press any key to continue");

        Console.ReadKey();

        _menuManager.MainMenus.AdminMenu();
    }

    private DateTime AskAndParsePastDate()
    {  
        string dateInput;
        do
        {
            Console.Clear();
            System.Console.WriteLine("Please enter a future date in this format 'dd-MM-yyyy'");
            dateInput = Console.ReadLine();
        }
        while(!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date) || DateTime.Now.Date < date.Date);             
        return DateTime.ParseExact(dateInput, "dd-MM-yyyy", CultureInfo.InvariantCulture);
    }
}