namespace Testing;

[TestClass]
public class TestCinemaLogic
{
    [TestMethod]
    [DataRow("0000AA", true)]
    [DataRow("0000aa", true)]
    [DataRow("0000aA", true)]
    [DataRow("0000", false)]
    [DataRow("000AAA", false)]
    [DataRow("00000A", false)]
    [DataRow("AA", false)]
    public void Check(string postal, bool expected)
    {
        CinemaLogic cl = new CinemaLogic();
        bool actual = CinemaLogic.VerifyPostalCode(postal);
        Assert.AreEqual(expected, actual);
    }
}