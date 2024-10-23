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
        bool actual = AccountsLogic.VerifyPassword(password);
        Assert.AreEqual(expected,actual);
        
    }

    [TestMethod]
    [DataRow("senostei", false)]
    [DataRow("gaistabiel@gmail.com",true)]
    public void CheckVerifyEmail(string email, bool expected)
    {
        bool actual = AccountsLogic.VerifyEmail(email);
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




}