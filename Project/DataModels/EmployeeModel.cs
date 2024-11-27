public class EmployeeModel 
{
    public string EmployeeName {get;set;}

    public int EmployeeID {get;set;}

    public int EmployeeSalary {get;set;}

    public EmployeeModel(string employeeName, int employeeID, int employeeSalary)
    {
        EmployeeName = employeeName; 
        EmployeeID = employeeID;
        EmployeeSalary = employeeSalary;

    }

}

