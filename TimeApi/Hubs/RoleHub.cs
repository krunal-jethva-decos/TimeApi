using Microsoft.AspNetCore.SignalR;
using TimeApi.Services;
using TimeShared;

namespace TimeApi.Hubs;

public class RoleHub : Hub<IRoleClient>
{
    private readonly ActiveRoleTracker _tracker;

    public RoleHub(ActiveRoleTracker tracker)
    {
        _tracker = tracker;
    }

    public override async Task OnConnectedAsync()
    {
        var role = Context.GetHttpContext()?.Request.Query["role"].ToString() ?? "Guest";
        
        await Groups.AddToGroupAsync(Context.ConnectionId, role);
        

        _tracker.AddConnection(role);
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var role = Context.GetHttpContext()?.Request.Query["role"].ToString() ?? "Guest";
        _tracker.RemoveConnection(role);
        await base.OnDisconnectedAsync(exception);
    }
}
