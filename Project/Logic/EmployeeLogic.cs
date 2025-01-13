public class EmployeeLogic
{
    
    private List<EmployeeModel> ListOfEmployees {get;set;}
     public EmployeeLogic()
    {
        ListOfEmployees = EmployeesAccess.LoadAll();
    }
    public void AddEmployee(EmployeeModel employeeToAdd)
    {
        ListOfEmployees.Add(employeeToAdd);
        EmployeesAccess.WriteAll(ListOfEmployees);

    }


    public void RemoveEmployee(EmployeeModel employeeToRemove)
    {
        ListOfEmployees.Remove(employeeToRemove);
        EmployeesAccess.WriteAll(ListOfEmployees);
    }


    public int FindFirstAvailableID()
    {
        int pointer = 0;
        List<EmployeeModel> tempList = ListOfEmployees.OrderBy(r => r.EmployeeID).ToList<EmployeeModel>();
        foreach (EmployeeModel employee in tempList)
        {
            if (pointer != employee.EmployeeID)
            {
                return pointer;
            }
            pointer++;
        }
        return pointer;
    }

    public double GetTotalMonthlySalary()
    {
        double totalSalary = 0;
        foreach (EmployeeModel employee in ListOfEmployees)
        {
            totalSalary += employee.EmployeeSalary;
        }
        return totalSalary;
    }

}
