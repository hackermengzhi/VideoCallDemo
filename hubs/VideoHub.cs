using Microsoft.AspNetCore.SignalR;

namespace VideoCallDemo.Hubs
{
    public class VideoHub : Hub
    {
        private static readonly Dictionary<string, List<string>> Rooms = new();
        private static readonly object Lock = new();

        public async Task JoinRoom(string roomId)
        {
            string connId = Context.ConnectionId;
            List<string> existingUsers;

            lock (Lock)
            {
                if (!Rooms.ContainsKey(roomId))
                    Rooms[roomId] = new List<string>();

                if (Rooms[roomId].Count >= 3)
                {
                    Clients.Caller.SendAsync("RoomFull");
                    return;
                }

                existingUsers = new List<string>(Rooms[roomId]);
                Rooms[roomId].Add(connId);
            }

            await Groups.AddToGroupAsync(connId, roomId);

            // 告诉新加入者当前房间里都有谁（他需要向每人发 offer）
            await Clients.Caller.SendAsync("Joined", roomId, existingUsers);

            // 告诉已有成员有新人加入（他们等着收 offer 即可）
            if (existingUsers.Count > 0)
                await Clients.OthersInGroup(roomId).SendAsync("UserJoined", connId);
        }

        // 以下消息都携带 senderId，让接收方知道是谁发的
        public async Task SendOffer(string targetId, string sdp)
            => await Clients.Client(targetId).SendAsync("ReceiveOffer", Context.ConnectionId, sdp);

        public async Task SendAnswer(string targetId, string sdp)
            => await Clients.Client(targetId).SendAsync("ReceiveAnswer", Context.ConnectionId, sdp);

        public async Task SendIceCandidate(string targetId, string candidate)
            => await Clients.Client(targetId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            string connId = Context.ConnectionId;
            string? affectedRoom = null;

            lock (Lock)
            {
                foreach (var room in Rooms)
                {
                    if (room.Value.Remove(connId))
                    {
                        affectedRoom = room.Key;
                        if (room.Value.Count == 0)
                            Rooms.Remove(room.Key);
                        break;
                    }
                }
            }

            if (affectedRoom != null)
                await Clients.Group(affectedRoom).SendAsync("UserLeft", connId);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
