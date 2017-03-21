using Improbable;
using Improbable.Unity.Core;
using Improbable.Unity.Core.EntityQueries;
using Improbable.Worker;
using Core.Util;
using World.Player;
using System;
using UnityEngine;

namespace Assets.Gamelogic.Core
{
  public class ClientPlayerSpawner
  {
    public static EntityId WorldManagerEntityId;

    public static void SpawnPlayer()
    {
      FindWorldManagerEntityId(RequestPlayerSpawn);
    }

    public static void DeletePlayer()
    {
      if (WorldManagerEntityId.IsValid())
      {
        SpatialOS.Connection.SendCommandRequest(WorldManagerEntityId,
          new PlayerManager.Commands.DeletePlayer.Request(new DeletePlayerRequest()), null);
      }
    }

    private static void FindWorldManagerEntityId(Action<EntityId> callback)
    {
      if (WorldManagerEntityId.IsValid())
      {
        callback.Invoke(WorldManagerEntityId);
        return;
      }
      var entityQuery = Query.HasComponent<PlayerManager>().ReturnOnlyEntityIds();
      SpatialOS.WorkerCommands.SendQuery(entityQuery, response => OnSearchResult(callback, response));
    }

    private static void OnSearchResult(Action<EntityId> callback,
       ICommandCallbackResponse<EntityQueryResult> response)
    {
      if (!response.Response.HasValue || response.StatusCode != StatusCode.Success)
      {
        Debug.LogError("Find Player Spawn Manager query failed with error: " + response.ErrorMessage);
        return;
      }
      var result = response.Response.Value;
      if( result.EntityCount < 1){
        Debug.LogError("Failed to find any Player Spawn Managers: No entities found with PlayerManager component.");
        return;
      }
      
      WorldManagerEntityId = result.Entities.First.Value.Key;
      callback(WorldManagerEntityId);
    }

    private static void RequestPlayerSpawn(EntityId worldManagerEntityId){
      SpatialOS.WorkerCommands.SendCommand(PlayerManager.Commands.SpawnPlayer.Descriptor,new SpawnPlayerRequest(), worldManagerEntityId, response => OnSpawnPlayerResponse(worldManagerEntityId, response));
    }

    private static void OnSpawnPlayerResponse(EntityId worldManagerEntityId, ICommandCallbackResponse<Nothing> response){
      if(!response.Response.HasValue || response.StatusCode != StatusCode.Success){
        Debug.LogError("SpawnPlayer Command: " + response.ErrorMessage + ", trying again...");
        RequestPlayerSpawn(worldManagerEntityId);
      }
    }

  }
}