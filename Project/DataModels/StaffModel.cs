
namespace Project.DataModels;

public class StaffModel : AccountModel
{
    public StaffModel(int id, string emailAddress, string password) : base(id, "staff", emailAddress, password) { }
}