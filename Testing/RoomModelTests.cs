
namespace Testing;

[TestClass]
public class TestRoomModel
{
    [TestMethod]
    public void RoomModel_Constructor_ShouldInitializeProperties()
    {
        
        int expectedId = 1;
        List<List<int>> expectedRows = new List<List<int>>
        {
            new List<int> { 1, 2, 3 },
            new List<int> { 4, 5, 6 }
        };
        int expectedPrice = 100;

        
        var roomModel = new RoomModel(expectedId, expectedRows, expectedPrice);

        
        Assert.AreEqual(expectedId, roomModel.Id);
        Assert.AreEqual(expectedPrice, roomModel.Price);
        Assert.IsTrue(AreNestedListsEqual(expectedRows, roomModel.Row));
    }

    [TestMethod]
    public void RoomModel_SetId_ShouldUpdateId()
    {
        
        var roomModel = new RoomModel(1, new List<List<int>>(), 100);
        int newId = 2;

        
        roomModel.Id = newId;

        
        Assert.AreEqual(newId, roomModel.Id);
    }

    [TestMethod]
    public void RoomModel_SetPrice_ShouldUpdatePrice()
    {
        
        var roomModel = new RoomModel(1, new List<List<int>>(), 100);
        int newPrice = 200;

        
        roomModel.Price = newPrice;

        
        Assert.AreEqual(newPrice, roomModel.Price);
    }

    [TestMethod]
    public void RoomModel_SetRow_ShouldUpdateRow()
    {
        
        var roomModel = new RoomModel(1, new List<List<int>>(), 100);
        List<List<int>> newRows = new List<List<int>>
        {
            new List<int> { 7, 8, 9 },
            new List<int> { 10, 11, 12 }
        };

        
        roomModel.Row = newRows;

        
        Assert.IsTrue(AreNestedListsEqual(newRows, roomModel.Row));
    }

    
    private bool AreNestedListsEqual(List<List<int>> list1, List<List<int>> list2)
    {
        if (list1 == null && list2 == null)
            return true;
        if (list1 == null || list2 == null)
            return false;
        if (list1.Count != list2.Count)
            return false;
        for (int i = 0; i < list1.Count; i++)
        {
            var innerList1 = list1[i];
            var innerList2 = list2[i];

            if (innerList1 == null && innerList2 == null)
                continue;
            if (innerList1 == null || innerList2 == null)
                return false;
            if (innerList1.Count != innerList2.Count)
                return false;

            for (int j = 0; j < innerList1.Count; j++)
            {
                if (innerList1[j] != innerList2[j])
                    return false;
            }
        }
        return true;
    }
}












