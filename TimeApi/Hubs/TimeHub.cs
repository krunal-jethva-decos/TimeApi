using Microsoft.AspNetCore.SignalR;
using TimeShared;

namespace TimeApi.Hubs;

public class TimeHub : Hub<ITimeClient>
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }
}
