public class EmployeeLogic
{
    
    public List<EmployeeModel> ListOfEmployees = new();
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

}
