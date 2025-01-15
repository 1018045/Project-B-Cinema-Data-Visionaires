namespace Testing;

[TestClass]
public class TestShowingsLogic
{
    [TestMethod]
    public void AddExtrasTest()
    {
        ShowingsLogic sl = new(new LogicManager());
        List<ExtraModel> extrasExpected = new();
        extrasExpected.Add(new ExtraModel("test", (decimal)10.00, true));
        sl.AddShowing(-1, DateTime.Now, 1, 0, false, "", extrasExpected);
        List<ExtraModel> extrasActual = sl.Showings.Last().Extras;
        Assert.AreEqual(extrasExpected, extrasActual);
    }

    [TestMethod]
    public void AddExtras_Multiple_Test()
    {
        ShowingsLogic sl = new(new LogicManager());
        List<ExtraModel> extras = new();
        extras.Add(new ExtraModel("test1", (decimal)10.00, true));
        extras.Add(new ExtraModel("test2", (decimal)10.00, false));
        extras.Add(new ExtraModel("test3", (decimal)10.00, true));
        sl.AddShowing(-1, DateTime.Now, 1, 0, false, "", extras);
        
        int expected = 3;
        int actual = sl.Showings.Last().Extras.Count;

        Assert.AreEqual(expected, actual);
    }
}