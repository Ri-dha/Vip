using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;

namespace VipTest.Hubs
    
{
    public class TrackingHub : Hub
    {
        // Concurrent dictionary to keep track of group connections
        private static readonly ConcurrentDictionary<string, HashSet<string>> GroupConnections = new();

        // Starts tracking a specific ride by adding the client to a group
        public async Task StartTrackingRide(Guid rideId)
        {
            var groupId = rideId.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
            
            // Add connection to the group
            GroupConnections.AddOrUpdate(groupId,
                _ => new HashSet<string> { Context.ConnectionId },
                (_, connections) => { connections.Add(Context.ConnectionId); return connections; });

            await Clients.Caller.SendAsync("RideTrackingStarted", rideId);
        }

        // Stops tracking a specific ride by removing all clients from the group
        public async Task StopTrackingRide(Guid rideId)
        {
            var groupId = rideId.ToString();
            // Send a message to only the clients in the specific group
            await Clients.Group(groupId).SendAsync("RideTrackingStopped", rideId);
            if (GroupConnections.TryRemove(groupId, out var connections))
            {
                // Remove each client connection from the group
                foreach (var connectionId in connections)
                {
                    await Groups.RemoveFromGroupAsync(connectionId, groupId);
                }
            }
        }

        // Sends updated location to all clients tracking the specified ride
        public async Task UpdateLocation(Guid rideId, decimal latitude, decimal longitude)
        {
            await Clients.Group(rideId.ToString()).SendAsync("ReceiveUpdatedLocation", rideId, latitude, longitude);
        }
    }
}