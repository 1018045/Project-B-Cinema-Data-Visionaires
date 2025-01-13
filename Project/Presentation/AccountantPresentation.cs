public class AccountantPresentation
{
    private LogicManager _logicManager;
    private MenuManager _menuManager;

    public AccountantPresentation(LogicManager logicManager, MenuManager menuManager)
    {
        _logicManager = logicManager;
        _menuManager = menuManager;
    }

    public void ViewIncomeByMonth(int month)
    {
        AccountantLogic accountantLogic = _logicManager.AccountantLogic;
        
        double Records = accountantLogic.GetIncomeByMonth(month);

        Console.WriteLine(Records);
    }

    public void ViewMonthlyExpenses()
    {
        AccountantLogic accountantLogic = _logicManager.AccountantLogic;

        Console.WriteLine(accountantLogic.CalculateCosts());
    }

    public void ViewEmployeeSalaries()
    {
        var employeeLogic = _logicManager.EmployeeLogic;
        Console.Clear();
        Console.WriteLine("Employee Salaries\n");

        var employees = employeeLogic.ListOfEmployees;

        if (employees == null || employees.Count == 0) 
        {
            Console.WriteLine("check if there are no empoyee.");
        }
        else
        {
            Console.WriteLine("Name\t\t\tID\t\tSalary");

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.EmployeeName,-20}\t{employee.EmployeeID}\t\t€{employee.EmployeeSalary:F2}");
            }

            Console.WriteLine("\n----------------------------------------");
            Console.WriteLine($"Total Monthly Cost: €{employeeLogic.GetTotalMonthlySalary():F2}");
        }

        MenuHelper.WaitForKey(_menuManager.MainMenus.AccountantMenu); 
    }
}