using Microsoft.AspNetCore.SignalR;

namespace GameStoreMini.Hubs
{
    public class CartHub : Hub
    {
        // client can call JoinGroup("user:123") or JoinGroup("anon:abcd")
        public Task JoinGroup(string group) => Groups.AddToGroupAsync(Context.ConnectionId, group);
        public Task LeaveGroup(string group) => Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
    }
}
