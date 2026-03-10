using TimeShared;

namespace TimeApi.Services;

public interface IRoleDataService
{
    RoleDataResponse GetRoleData(string role);
}

public class RoleDataService : IRoleDataService
{
    public RoleDataResponse GetRoleData(string role)
    {
        return role.ToLower() switch
        {
            "admin" => new RoleDataResponse { 
                Role = role, 
                Message = "Hello admin, you can do whatever you want" 
            },
            "manager" => new RoleDataResponse { 
                Role = role, 
                Message = "You can also do whatever you want but need to ask admin first!" 
            },
            "user" => new RoleDataResponse { 
                Role = role, 
                Message = "Welcome! You have 2 new notifications." 
            },
            _ => new RoleDataResponse { 
                Role = role, 
                Message = "Accessing as guest. Features limited." 
            }
        };
    }
}
