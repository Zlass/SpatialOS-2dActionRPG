using Assets.Gamelogic.Core;
using Assets.Gamelogic.EntityGenerator;
using Core.Util;
using World.Player;
using World.Location;
using Improbable;
using Improbable.Collections;
using Improbable.Core;
using Improbable.Math;
using Improbable.Entity.Component;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using Improbable.Worker;
using Improbable.Worker.Query;
using Improbable.Unity.Core.EntityQueries;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

namespace Assets.Gamelogic.Core
{
  public class PlayerConnectionManager : MonoBehaviour
  {
    [Require] private PlayerManager.Writer playerManager;
    [Require] private Location.Writer playerLocation;
    private Map<string, EntityId> activePlayerEntityIds;
    private Map<string, Coordinates> inactivePlayerEntityLocations;

    private void OnEnable()
    {
      playerManager.CommandReceiver.OnSpawnPlayer.RegisterResponse(OnSpawnPlayer);

      activePlayerEntityIds = new Map<string, EntityId>(playerManager.Data.playerEntityIds);
      inactivePlayerEntityLocations = new Map<string, Coordinates>(playerManager.Data.playerSpawnLocation);

    }

    private Nothing OnSpawnPlayer(SpawnPlayerRequest request, ICommandCallerInfo callerInfo)
    {
      // @TODO Change to a UId instead of the worker Id
      // Check if the player is already in the World
      if (activePlayerEntityIds.ContainsKey(callerInfo.CallerWorkerId))
      {
        return new Nothing();
      }

      // Mark as requested
      activePlayerEntityIds.Add(callerInfo.CallerWorkerId, new EntityId());
      // Request Id and Spawns Player
      RequestPlayerEntityId(callerInfo.CallerWorkerId);
      // Respond
      SendMapUpdate();
      return new Nothing();
    }

    private Nothing OnDeletePlayer(DeletePlayerRequest request, ICommandCallerInfo callerInfo)
    {
      var despawnLocation = request.lastPosition;
      //  Check that there is a player to remove
      if (activePlayerEntityIds.ContainsKey(callerInfo.CallerWorkerId))
      {

        var playerId = activePlayerEntityIds[callerInfo.CallerWorkerId];
        if (playerId.IsValid())
        {
          // Stores Worker's last palyer location for next login
          inactivePlayerEntityLocations.Add(callerInfo.CallerWorkerId, despawnLocation);
          // Deletes the entity from the world
          SpatialOS.Commands.DeleteEntity(playerManager, playerId, result =>
          {
            if (result.StatusCode != StatusCode.Success)
            {
              Debug.LogErrorFormat("Failed to delete inactive player entity {0} with error message:{1}",
               playerId, result.ErrorMessage);
              return;
            }
          });
        }
        //  Removes player from active list
        activePlayerEntityIds.Remove(callerInfo.CallerWorkerId);
        SendMapUpdate();
      }
      return new Nothing();
    }

    private void SendMapUpdate()
    {
      var update = new PlayerManager.Update();
      update.SetPlayerEntityIds(activePlayerEntityIds);
      update.SetPlayerSpawnLocation(inactivePlayerEntityLocations);
      playerManager.Send(update);
    }

    private void RequestPlayerEntityId(string workerId)
    {
      SpatialOS.Commands.ReserveEntityId(playerManager, result =>{
        if (result.StatusCode != StatusCode.Success)
        {
         RequestPlayerEntityId(workerId);
         Debug.LogError("Failed to reserve EntityId for Player, retrying...");
         return;
        }
        if(activePlayerEntityIds != null && activePlayerEntityIds.ContainsKey(workerId) &&!activePlayerEntityIds[workerId].IsValid()){
          activePlayerEntityIds[workerId] = result.Response.Value;

          SendMapUpdate();
          SpawnPlayer(workerId, result.Response.Value);
        }
      });
    }

    private void SpawnPlayer(string workerId, EntityId playerId)
    {
      throw new NotImplementedException();
    }
  }

}