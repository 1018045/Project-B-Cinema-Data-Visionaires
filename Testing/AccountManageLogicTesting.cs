using Project.Logic.Account;

namespace Testing;

[TestClass]
public class TestAccountManageLogic
{
    [TestMethod]
    public void ChangeEmailTest()
    {
        LogicManager lm = new LogicManager();
        AccountManageLogic aml = lm.AccountManageLogic;

        string expectedEmail = "test2@test.com";

        aml.UpdateList("test@test.com", "test", "Meneer test", DateTime.Now);
        // al.Accounts.Add(new UserModel(13000, "test@test.com", "test", "Meneer test", DateTime.Now));
        aml.CheckLogin("test@test.com", "test");
        
        aml.ChangeEmail(expectedEmail);
        string actualEmail = AccountsLogic.CurrentAccount.EmailAddress;

        Assert.AreEqual(expectedEmail, actualEmail);
    }

    [TestMethod]
    public void ChangePasswordTest()
    {
        LogicManager lm = new LogicManager();
        AccountManageLogic aml = lm.AccountManageLogic;

        string expectedPassword = "test2";

        aml.UpdateList("test@test.com", "test", "Meneer test", DateTime.Now);
        aml.CheckLogin("test@test.com", "test");
        
        aml.ChangePassword(expectedPassword);
        string actualPassword = AccountsLogic.CurrentAccount.Password;

        Assert.AreEqual(expectedPassword, actualPassword);
    }

    [TestMethod]
    public void RemoveAccountTest()
    {
        LogicManager lm = new LogicManager();
        AccountManageLogic aml = lm.AccountManageLogic;

        UserModel accountToRemove = (UserModel)aml.UpdateList("test@test.com", "test", "Meneer test", DateTime.Now);
        aml.CheckLogin("test@test.com", "test");
        aml.DeleteAccount();
        
        Assert.IsFalse(aml.Accounts.Contains(accountToRemove));
    }
}