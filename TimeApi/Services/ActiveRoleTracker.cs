using System.Collections.Concurrent;

namespace TimeApi.Services;

public class ActiveRoleTracker
{
    private readonly ConcurrentDictionary<string, int> _activeRoles = new();

    public void AddConnection(string role)
    {
        _activeRoles.AddOrUpdate(role, 1, (_, count) => count + 1);
    }

    public void RemoveConnection(string role)
    {
        _activeRoles.AddOrUpdate(role, 0, (_, count) => Math.Max(0, count - 1));
    }

    public IEnumerable<string> GetActiveRoles() => 
        _activeRoles.Where(x => x.Value > 0).Select(x => x.Key);
}
