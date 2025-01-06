using System.Dynamic;
using Project.Logic.Account;

namespace Project.Presentation;

public static class AccountPresentation
{
    static AccountManageLogic _accountManageLogic = new();
    public static void Menu()
    {
        List<string> options = new List<string>
        {
            "Update account details",
            "Remove account (DANGER)",
            
            "Return"
        };

        List<Action> actions = new List<Action>
        {
            () => UpdateAccountDetails(_accountManageLogic),
            () => DeleteAccount(_accountManageLogic),
            
            Menus.LoggedInMenu
        };

        MenuHelper.NewMenu(options, actions, "Manage account");
    }

    private static void UpdateAccountDetails(AccountManageLogic logic)
    {
        List<string> options = new List<string>
        {
            "Change email",
            "Change password",
            "Return"
        };

        List<Action> actions = new List<Action>
        {
            () => ChangeEmail(_accountManageLogic),
            () => ChangePassword(_accountManageLogic),
            Menu
        };

        MenuHelper.NewMenu(options, actions, "Update account");
    }

    

    private static void ChangeEmail(AccountManageLogic logic)
    {
        var newEmail = "";
        for (var done = false; !done;)
        {
            Console.WriteLine("");
            Console.WriteLine("New email: ");

            newEmail = Console.ReadLine()!;
            done = AccountsLogic.VerifyEmail(newEmail);
        }

        Console.WriteLine("Confirm email: ");

        if (!newEmail.Equals(Console.ReadLine()!))
        {
            Console.Clear();
            Console.WriteLine("Emails do not match");
            Thread.Sleep(2500);
            UpdateAccountDetails(logic);
            return;
        }

        logic.ChangeEmail(newEmail);

        Console.Clear();
        Console.WriteLine($"Successfully changed your email to {newEmail}");
        UpdateAccountDetails(logic);
        MenuHelper.WaitForKey(() => UpdateAccountDetails(logic));
    }

    private static void ChangePassword(AccountManageLogic logic)
    {
        var newPassword = "";
        for (var done = false; !done;)
        {
            Console.WriteLine("");
            Console.WriteLine("Enter your password. It must contain at least 8 characters which consist of 1 capital letter, 1 number, and 1 special character e.g. $,#,% etc.");
            Console.WriteLine("New password: ");

            newPassword = Console.ReadLine()!;
            done = AccountsLogic.VerifyPassword(newPassword);
        }

        Console.WriteLine("Confirm password: ");
        if (!newPassword.Equals(Console.ReadLine()!))
        {
            Console.Clear();
            Console.WriteLine("Passwords do not match");
            Thread.Sleep(2500);
            UpdateAccountDetails(logic);
            return;
        }

        logic.ChangePassword(newPassword);

        Console.Clear();
        Console.WriteLine($"Successfully changed your password");
        Thread.Sleep(2500);
        MenuHelper.WaitForKey(() => UpdateAccountDetails(logic));
    }

    private static void DeleteAccount(AccountManageLogic logic)
    {

        bool confirmed = MenuHelper.NewMenu(new List<string>(){ "Yes", "No"},
                                            new List<bool>() { true, false },
                                            "Are your sure you want to delete your account?");
                                
        if (confirmed)
        {
            logic.DeleteAccount();

            Console.Clear();
            Console.WriteLine("Successfully deleted your account");
            Console.WriteLine("You will now be logged out");
            MenuHelper.WaitForKey(Menus.GuestMenu);
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
