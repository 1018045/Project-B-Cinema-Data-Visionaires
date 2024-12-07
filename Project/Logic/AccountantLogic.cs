public class AccountantLogic : IReportable
{
    List<BillModel> Bills = new();
    public int FindFirstAvailableID()
    {
        int pointer = 0;
        List<BillModel> tempList = Bills.OrderBy(b => b.ID).ToList<BillModel>();
        foreach (BillModel bill in tempList)
        {
            if (pointer != bill.ID)
            {
                return pointer;
            }
            pointer++;
        }
        return pointer;
    }







   

    
}

