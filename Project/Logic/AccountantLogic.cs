public class AccountantLogic: IReportable
{
    public static List<BillModel> Bills = new();

    private LogicManager _logicManager;

    public AccountantLogic(LogicManager logicManager)
    {
        _logicManager = logicManager;
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
        EmployeeLogic employeeLogic = _logicManager.EmployeeLogic;
        double totalSalary = employeeLogic.GetTotalMonthlySalary();

        return totalSalary; 
    }

    public List<BillModel> FindBy<T>(T WhatToFind)
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

    public double GetIncomeByMonth(int month)
    {
    
      List<BillModel> BillsToView =  FindBy(month); 

      double TotalAmount = 0;

      foreach(BillModel bill in BillsToView)
      {
        TotalAmount += bill.TotalAmount;
      }
    
     return TotalAmount; 

    }

    public List<BillModel> GetUserBills(int userId)
    {
        return Bills.Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.Paymentdate)
                    .ToList();
    }
}
