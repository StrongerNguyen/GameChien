using GameChien.Models;
using GameChien.Models.Data;
using GameChien.Models.Ext;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GameChien.Hubs
{
    [Authorize]
    public class RealtimeHub : Hub
    {
        static IHubContext hub = null;
        public RealtimeHub()
        {
            hub = GlobalHost.ConnectionManager.GetHubContext<RealtimeHub>();
        }
        public static void pushNotifyToPlayer(string accountName, NotifyToPlayerModel notifyToPlayer)
        {
            hub.Clients.User(accountName).pushNotifyToPlayer(new NotifyToPlayerModel() { Type = notifyToPlayer.Type, Message = notifyToPlayer.Message });
        }
        public void UpdateRoom(tblPlayer_Room player_Room, bool withoutMyPlayer)
        {
            using (var db = new GameChienEntities())
            {
                //Lấy những player không thuộc 
                if (withoutMyPlayer)
                {
                    db.tblPlayer_Room.Where(t => t.RoomId == player_Room.RoomId && t.PlayerId != player_Room.PlayerId).ForEach(t =>
                    {
                        hub.Clients.User(t.tblPlayer.AccountName).updateRoom();
                    });
                }
                else
                {
                    db.tblPlayer_Room.Where(t => t.RoomId == player_Room.RoomId).ForEach(t =>
                    {
                        hub.Clients.User(t.tblPlayer.AccountName).updateRoom();
                    });
                }
            }
        }
        //private readonly static ConnectionMapping<string> _connections =
        //    new ConnectionMapping<string>();

        //public void pushNotifyToPlayer(string accountName, NotifyToPlayerModel notifyToPlayer)
        //{
        //    foreach (var connectionId in _connections.GetConnections(accountName))
        //    {
        //        hub.Clients.Client(connectionId).pushNotifyToPlayer(notifyToPlayer);
        //    }
        //}
        //public void JoinRoom(string accountName, Guid roomId)
        //{
        //    string groupName = roomId.ToString();
        //    foreach (var connectionId in _connections.GetConnections(accountName))
        //    {
        //        hub.Groups.Add(connectionId, groupName);
        //    }
        //    hub.Clients.Group(groupName).joinRoom(accountName);
        //}

        //public override Task OnConnected()
        //{
        //    string name = Context.User.Identity.Name;

        //    _connections.Add(name, Context.ConnectionId);

        //    return base.OnConnected();
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    string name = Context.User.Identity.Name;

        //    _connections.Remove(name, Context.ConnectionId);

        //    return base.OnDisconnected(stopCalled);
        //}

        //public override Task OnReconnected()
        //{
        //    string name = Context.User.Identity.Name;

        //    if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
        //    {
        //        _connections.Add(name, Context.ConnectionId);
        //    }

        //    return base.OnReconnected();
        //}
    }
}