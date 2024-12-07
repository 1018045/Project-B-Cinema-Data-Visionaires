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


    public double CalculateYearlyTurnover(T whatToCalculate)
    {

    }

    public double CalculateYearlyProfits(T whatToCalculate)
    {

    }

    public double CalculateYearlyCosts(T whatToCalculate)
    {

    }

    List<BillModel> FindBy(T WhatToFind)
    {
        if(WhatToFind.GetType() == typeof(int))
        {
            if(WhatToFind > 13)
            {
                //find bills from this year
            }
        }

        if(WhatToFind.GetType() == typeof(DateTime))
        {

        }
    }





   

    
}
