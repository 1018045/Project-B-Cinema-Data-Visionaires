using System.Dynamic;
using Project.Logic.Account;

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
            _menuManager.Menus.LoggedInMenu
        };

        MenuHelper.NewMenu(options, actions, "Manage account");
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
            MenuHelper.WaitForKey(_menuManager.Menus.GuestMenu);
        }
        else
        {
            Console.Clear();
            Console.WriteLine("Canceled the deletion of your account");
            Console.WriteLine("You will return to the menu");
            MenuHelper.WaitForKey(Menu);
        }
    
    }
}
