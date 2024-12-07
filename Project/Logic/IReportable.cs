public interface IReportable
{
    public double CalculateYearlyTurnover(T whatToCalculate);

    public double CalculateYearlyProfits(T whatToCalculate); 

    public double CalculateYearlyCosts(T whatToCalculate);

    List<BillModel> FindBy(T case);
    
}