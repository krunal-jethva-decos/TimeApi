namespace TimeShared;

public interface ITimeClient
{
    Task ReceiveTime(string time);
}

public interface IRoleClient
{
    Task ReceiveRoleData(RoleDataResponse data);
}

public class RoleDataResponse
{
    public string Role { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}
