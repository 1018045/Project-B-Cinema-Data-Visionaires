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
    public void VerifyPostalCodeTest(string postal, bool expected)
    {
        bool actual = CinemaLogic.VerifyPostalCode(postal);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void AddCinemaLocationTest()
    {
        CinemaLogic cl = new CinemaLogic();
        int expected = cl.Cinemas.Count + 1;
        cl.AddCinema("x", "x", "x", "x", "x");
        int actual = cl.Cinemas.Count;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void RemoveCinemaLocationTest()
    {
        CinemaLogic cl = new CinemaLogic();
        CinemaModel cm = new CinemaModel(1500, "x", "x", "x", "x", "x");
        int expected = cl.Cinemas.Count;
        cl.Cinemas.Add(cm);

        cl.RemoveCinema(cm);
        int actual = cl.Cinemas.Count;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow("Roffa", "Weena 150", "1111AA")]
    [DataRow("", "", "")]
    [DataRow("", "Weena 150", "1111AA")]
    [DataRow("Roffa", "", "1111AA")]
    [DataRow("Roffa", "Weena 150", "")]
    [DataRow(null, null, null)]
    [DataRow(null, "Weena 150", "1111AA")]
    [DataRow("Roffa", null, "1111AA")]
    [DataRow("Roffa", "Weena 150", null)]
    public void EditCinemaLocationTest(string newCity, string newAddress, string newPostal)
    {
        CinemaLogic cl = new CinemaLogic();
        CinemaModel cm = new CinemaModel(1500, "x", "x", "x", "x", "x");

        cl.EditCinemaAddress(cm, newCity, newAddress, newPostal);

        string actualCity = cm.City;
        string actualAddress = cm.Address;
        string actualPostal = cm.PostalCode;

        if (newCity != null && newCity != "" && newAddress != null && newAddress != "" && newPostal != null && newPostal != "")
        {
            Assert.AreEqual(newCity, actualCity);
            Assert.AreEqual(newAddress, actualAddress);
            Assert.AreEqual(newPostal, actualPostal);
        }
        else
        {
            Assert.AreEqual("x", actualCity);
            Assert.AreEqual("x", actualAddress);
            Assert.AreEqual("x", actualPostal);
        }
    }
}