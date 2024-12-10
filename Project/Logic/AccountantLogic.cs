public class AccountantLogic<T> : IReportable<T>
{
    public static List<BillModel> Bills = new();

    public AccountantLogic()
    {
        Bills = AccountantAccess.LoadAll();
    }

    public void AddBill(BillModel bill)
    {
        Bills.Add(bill);
        AccountantAccess.WriteAll(Bills);
    }
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

    public double CalculateYearlyProfits(int Year)
    {
        return CalculateYearlyTurnover(Year) - CalculateCosts();
    }

    public double CalculateCosts()
    {
        
        EmployeeLogic employeeLogic = new();
        double totalSalary = employeeLogic.GetTotalMonthlySalary();

        return totalSalary; 

    
    }

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
