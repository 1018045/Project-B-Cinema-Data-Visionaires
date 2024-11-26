
public class AdminModel : AccountModel
{
    public AdminModel(int id, string emailAddress, string password) : base(id, "admin", emailAddress, password) { }
}