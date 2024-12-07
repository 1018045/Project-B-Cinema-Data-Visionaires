public interface IReportable<T>
{
    public double CalculateYearlyTurnover(int whatToCalculate);

    // public double CalculateYearlyProfits(int whatToCalculate); 

    // public double CalculateYearlyCosts(int whatToCalculate);

    public List<BillModel> FindBy(T WhatToFind);
    
}