public interface IReportable
{
    public double CalculateYearlyTurnover(int whatToCalculate);

    public double CalculateYearlyProfits(int whatToCalculate); 

    public double CalculateCosts();

    public List<BillModel> FindBy<T>(T WhatToFind);
    
}