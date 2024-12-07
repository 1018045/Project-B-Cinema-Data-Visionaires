public class AccountantLogic<T> : IReportable<T>
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



    public double CalculateYearlyTurnover(int Year)
    {
        List<BillModel> BillsInThisYear = new(); 
        double YearTotal = 0; 

        foreach(BillModel bill in Bills)
        {
            if(bill.Paymentdate.Year == Year)
            {
                YearTotal += bill.TotalAmount;
            }
            
        }
        return YearTotal;
    }

    // public double CalculateYearlyProfits(int whatToCalculate)
    // {

    // }

    // public double CalculateYearlyCosts(int whatToCalculate)
    // {

    // }

    public List<BillModel> FindBy(T WhatToFind)
    {
        List<BillModel> BillsToReturn = new();

        if(WhatToFind is int month && month > 0 && month <= 12)
        {
            foreach(BillModel bill in Bills)
            {
                BillsToReturn.Add(bill);
                
            }
            return BillsToReturn;
        }

        if(WhatToFind is int year && year >= 2024)
        {
                
                foreach(BillModel bill in Bills)
                {
                    if(bill.Paymentdate.Year == year)
                    {
                        BillsToReturn.Add(bill);
                    }

                }
            
            return BillsToReturn;
        }

        if(WhatToFind is DateTime specificDate)
        {
            foreach(BillModel bill in Bills)
            {
                if(bill.Paymentdate.Date == specificDate.Date)
                {
                    BillsToReturn.Add(bill);
                }
            }
            return BillsToReturn;
        }
        
        return BillsToReturn;
    }





   

    
}
