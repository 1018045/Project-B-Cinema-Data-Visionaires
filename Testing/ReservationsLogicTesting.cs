using System.Diagnostics;
using Project.Logic.Account;

namespace Testing;

[TestClass]
public class TestReservationLogic
{
    [TestMethod]
    public void TestAddReservation()
    {
        ReservationsLogic res = new(new LogicManager());
        int expected = res.Reservations.Count + 1;
        res.AddReservation(13, 13, "1-2", true, 10.00, null);
        int actual = res.Reservations.Count;
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void TestFindReservationByUserID()
    {
        int userId = 10;
        ReservationModel expected = new(10, userId, 10, "1-2", true, 10.00, null);
        ReservationsLogic res = new(new LogicManager());
        res.AddReservation(userId, 10, "1-2", true, 10.00, null);
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
        ReservationsLogic reservations = new(new LogicManager());
        string actual = reservations.ValidateBankDetails(bankDetails);
        Assert.AreEqual(expected, actual.Length == 0);
    }

    [TestMethod]
    public void TestGetFutureReservations()
    {
        LogicManager lm = new();
        AccountsLogic al = lm.AccountsLogic;
        ShowingsLogic sl = lm.ShowingsLogic;
        ReservationsLogic rl = lm.ReservationsLogic;

        al.CheckLogin("t@b.c", "xyz");
        sl.AddShowing(-1, DateTime.Now.AddDays(1), 1, 0, false, "", new());
        int expectedShowingsId = sl.Showings.Last().Id;
        rl.AddReservation(AccountsLogic.CurrentAccount.Id, expectedShowingsId, "", true, 10.00, new());

        Assert.IsTrue(rl.GetFutureReservations().Any(r => r.ShowingId == expectedShowingsId));
    }

    [TestMethod]
    public void TestGetPastReservations()
    {
        LogicManager lm = new();
        AccountsLogic al = lm.AccountsLogic;
        ShowingsLogic sl = lm.ShowingsLogic;
        ReservationsLogic rl = lm.ReservationsLogic;

        al.CheckLogin("t@b.c", "xyz");
        sl.AddShowing(-1, DateTime.Now.AddDays(-1), 1, 0, false, "", new());
        int expectedShowingsId = sl.Showings.Last().Id;
        rl.AddReservation(AccountsLogic.CurrentAccount.Id, expectedShowingsId, "", true, 10.00, new());

        Assert.IsTrue(rl.GetPastReservations().Any(r => r.ShowingId == expectedShowingsId));
    }

    [TestMethod]
    public void TestAddReservation_MandatoryNotSelected()
    {
        LogicManager lm = new LogicManager();
        ShowingsLogic sl = lm.ShowingsLogic;
        ReservationsLogic res = lm.ReservationsLogic;

        List<ExtraModel> extras = new() {new ExtraModel("test", (decimal)10.00, true)};

        sl.AddShowing(13000, DateTime.Now, 1, 0, false, "", extras);
        int showingsId = sl.Showings.Last().Id;
        ReservationModel reservation = res.AddReservation(1000, showingsId, "1-2", true, 10.00, new());

        Assert.IsNull(reservation);
    }
}