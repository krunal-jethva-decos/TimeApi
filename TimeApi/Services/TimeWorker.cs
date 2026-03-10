using Microsoft.AspNetCore.SignalR;
using TimeApi.Hubs;
using TimeShared;

namespace TimeApi.Services;

public class TimeWorker : BackgroundService
{
    private readonly IHubContext<TimeHub, ITimeClient> _timeHubContext;
    private readonly IHubContext<RoleHub, IRoleClient> _roleHubContext;
    private readonly ActiveRoleTracker _presenceTracker;
    private readonly IRoleDataService _roleDataService;
    private readonly ILogger<TimeWorker> _logger;

    public TimeWorker(
        IHubContext<TimeHub, ITimeClient> timeHubContext,
        IHubContext<RoleHub, IRoleClient> roleHubContext,
        ActiveRoleTracker presenceTracker,
        IRoleDataService roleDataService,
        ILogger<TimeWorker> logger)
    {
        _timeHubContext = timeHubContext;
        _roleHubContext = roleHubContext;
        _presenceTracker = presenceTracker;
        _roleDataService = roleDataService;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {

            var time = DateTime.Now.ToString("O");
            await _timeHubContext.Clients.All.ReceiveTime(time);


            var activeRoles = _presenceTracker.GetActiveRoles();
            foreach (var role in activeRoles)
            {
                var roleSpecificData = _roleDataService.GetRoleData(role);
                if (roleSpecificData != null)
                {
                    await _roleHubContext.Clients.Group(role).ReceiveRoleData(roleSpecificData);
                }
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}

