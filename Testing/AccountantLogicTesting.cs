namespace Testing;

[TestClass]
public class TestAccountantLogic
{
    [TestMethod]
    public void GetUserBillsTest()
    {
        AccountantLogic ac = new AccountantLogic(new LogicManager());
        int userId = 0;
        List<BillModel> bills = ac.GetUserBills(1);
        Assert.IsTrue(bills.All(b => b.UserId == 1));
    }

    [TestMethod]
    public void AddBillTest()
    {
        AccountantLogic ac = new AccountantLogic(new LogicManager());
        int expected = ac.Bills.Count + 1;
        ac.AddBill(new BillModel(13000, -1, true, 10.00, DateTime.Now));
        int actual = ac.Bills.Count;
        Assert.AreEqual(expected, actual);
    }
}