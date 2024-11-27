using Project.Logic.Account;

namespace Project.Presentation;

public static class AccountPresentation
{
    public static void Menu()
    {
        var option = 0;
        for (var selected = false; !selected;)
        {
            Console.Clear();
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("Enter 1 to update account details");
            Console.WriteLine("Enter 2 to remove account (DANGER)");
            Console.WriteLine("Enter 3 to add extra's to your reservation (not yet implemented)");
            Console.WriteLine("Enter 4 to go back to main menu");

            selected = int.TryParse(Console.ReadLine(), out option);
        }

        var logic = new AccountManageLogic();

        switch (option)
        {
            case 1:
                UpdateAccountDetails(logic);
                break;
            case 2:
                DeleteAccount(logic);
                break;
            case 3:
                break;
            case 4:
                Console.Clear();
                Menus.LoggedInMenu();
                break;
        }
    }

    private static void UpdateAccountDetails(AccountManageLogic logic)
    {
        var option = 0;
        for (var selected = false; !selected;)
        {
            Console.Clear();
            Console.WriteLine("What would you like to change?");
            Console.WriteLine("Enter 1 to change email");
            Console.WriteLine("Enter 2 to change password");
            Console.WriteLine("Enter 3 to go back to the menu");

            selected = int.TryParse(Console.ReadLine(), out option);
        }

        switch (option)
        {
            case 1:
                ChangeEmail(logic);
                break;
            case 2:
                ChangePassword(logic);
                break;
            case 3:
                Menu();
                break;
        }
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
        Thread.Sleep(2500);
        UpdateAccountDetails(logic);
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
        UpdateAccountDetails(logic);
    }

    private static void DeleteAccount(AccountManageLogic logic)
    {
        for (var done = false; !done;)
        {
            Console.WriteLine("Are you sure you want to delete your account Y/N (DANGER): ");

            var input = Console.ReadLine()!.ToLower();
            if (input.Equals("y"))
            {
                logic.DeleteAccount();

                Console.Clear();
                Console.WriteLine("Successfully deleted your account");
                Console.WriteLine("You will now be logged out");
                Thread.Sleep(2500);
                Console.Clear();

                done = true;
                Menus.Start();
            }
            else if (input.Equals("n"))
            {
                Console.Clear();
                Console.WriteLine("Canceled the deletion of your account");
                Console.WriteLine("You will return to the menu");
                Thread.Sleep(2500);
                Console.Clear();

                done = true;
                UpdateAccountDetails(logic);
            }
        }

    }
}
