using System.Globalization;
using Project.Logic.Account;

namespace Testing;

[TestClass]
public class TestAccountLogic
{
    [TestMethod]
    [DataRow(null, "password")]
    [DataRow("email", null)]
    [DataRow(null, null)]
    public void CheckLogin_NullValues_ReturnNull(string email, string password)
    {
        AccountsLogic ac = new AccountsLogic();
        AccountModel actual = ac.CheckLogin(email, password);
        Assert.IsNull(actual);
    }

    [TestMethod]
    [DataRow("jaja", false)]
    [DataRow("bibubibuta", true)]
    public void CheckVerifyPassword(string password, bool expected)
    {
        AccountsLogic ac = new();
        bool actual = ac.VerifyPassword(password);
        Assert.AreEqual(expected,actual);
        
    }

    [TestMethod]
    [DataRow("senostei", false)]
    [DataRow("gaistabiel@gmail.com",true)]
    public void CheckVerifyEmail(string email, bool expected)
    {
        AccountsLogic ac = new();
        bool actual = ac.VerifyEmail(email);
        Assert.AreEqual(expected,actual);
    }

    [TestMethod]
    [DataRow("jaman", false)]
    [DataRow("15", true)]
    public void CheckParseAge(string input, bool expected)
    {
        bool actual = AccountsLogic.IsInt(input);
        Assert.AreEqual(expected, actual);
    }
    
    [TestMethod]
    [DataRow("123", 123)]
    [DataRow("abc", -1)]
    public void CheckParseInt(string input, int expected)
    {
        int actual = AccountsLogic.ParseInt(input);
       
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void CheckLoginTest()
    {
        AccountsLogic al = new LogicManager().AccountsLogic;
        al.UpdateList("test@test", "test", "Meneer Test", DateTime.Now);
        al.CheckLogin("test@test", "test");

        Assert.AreEqual(AccountsLogic.CurrentAccount.EmailAddress, "test@test");
    }

    [TestMethod]
    [DataRow(-17, 0, 17)]
    [DataRow(-16, -1, 15)]
    [DataRow(-16, 0, 16)]
    [DataRow(-16, 1, 16)]
    [DataRow(-15, 0, 15)]
    [DataRow(-15, -1, 14)]
    public void CalculateAgeTest(int years, int days, int expected)
    {
        int actual = AccountsLogic.CalculateAge(DateTime.Now.AddYears(years).AddDays(days));

        Assert.AreEqual(expected, actual);
    }
}