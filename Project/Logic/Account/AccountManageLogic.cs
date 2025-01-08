using Project.DataModels;

namespace Project.Logic.Account;

public class AccountManageLogic : AccountsLogic
{
    public void CreateStaffAccount(string password, string email)
    {
        var accounts = AccountsAccess.LoadAll();
        accounts.Add(new StaffModel(accounts[^1].Id + 1, email, password!));
        AccountsAccess.WriteAll(accounts);
    }

    public void ChangeEmail(string newEmail)
    {
        if (CurrentAccount == null)
            throw new ApplicationException("DEV-ERROR: Should not be able to get here without being logged in!");

        Accounts.Find(a => a.Id == CurrentAccount.Id)!.EmailAddress = newEmail;
        AccountsAccess.WriteAll(Accounts); //todo verify that the currentAccount is the same instance as in the list so it is properly written
    }
    public void ChangePassword(string newPassword)
    {
        if (CurrentAccount == null)
            throw new ApplicationException("DEV-ERROR: Should not be able to get here without being logged in!");

        Accounts.Find(a => a.Id == CurrentAccount.Id)!.Password = newPassword;
        AccountsAccess.WriteAll(Accounts); //todo verify that the currentAccount is the same instance as in the list so it is properly written
    }

    public void DeleteAccount()
    {
        if (CurrentAccount == null)
            throw new ApplicationException("DEV-ERROR: Should not be able to get here without being logged in!");

        Accounts.RemoveAll(a => a.Id == CurrentAccount.Id);
        LogOut();
        AccountsAccess.WriteAll(Accounts);
    }
}