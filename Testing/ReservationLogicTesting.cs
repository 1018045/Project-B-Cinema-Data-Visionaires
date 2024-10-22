using System.Diagnostics;

namespace Testing;

[TestClass]
public class TestReservationLogic
{
    [TestMethod]
    public void TestAddReservation()
    {
        ReservationsLogic res = new();
        int expected = res.Reservations.Count + 1;
        res.AddReservation(13, 13, "1-2", true);
        int actual = res.Reservations.Count;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestFindReservationByUserID()
    {
        int userId = 10;
        ReservationModel expected = new(10, userId, 10, "1-2", true);
        ReservationsLogic res = new();
        res.AddReservation(expected);
        List<ReservationModel> actualList = res.FindReservationByUserID(userId);
        Assert.IsTrue(actualList.Contains(expected));
    }

    [TestMethod]
    [DataRow ("0000000000000000", false)]
    [DataRow ("AAAAAAAAAAAAAAAAAA", false)]
    [DataRow ("NL00INGB000000000", false)]      // 1 too little
    [DataRow ("NL00INGB00000000000", false)]    // 1 too many
    [DataRow ("9900INGB0000000000", false)]     // no country id
    [DataRow ("NL0099990000000000", false)]     // no bank id
    [DataRow (null, false)]
    [DataRow ("NL00INGB0000000000", true)]
    [DataRow ("nl00ingb0000000000", true)]
    [DataRow ("NL00ingb0000000000", true)]
    [DataRow (" NL0099990000000000 ", false)]     
    public void TestValidateBankDetails(string bankDetails, bool expected)
    {
        ReservationsLogic reservations = new();
        bool actual = reservations.ValidateBankDetails(bankDetails);
        Assert.AreEqual(expected, actual);
    }
}