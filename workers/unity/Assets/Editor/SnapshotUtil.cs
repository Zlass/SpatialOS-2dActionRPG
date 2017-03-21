using Assets.Gamelogic.Core;
using Assets.Gamelogic.EntityGenerator;
using Improbable.Math;
using Improbable.Worker;
using UnityEngine;

namespace Assets.Editor
{
  public static class SnapshotUtil
  {
    public static void AddPlayerMangerEntity(SnapshotBuilder snapshot){
      var entity = CoreEntityFactory.CreatePlayerManagerTemplate();
      snapshot.AddEntity(snapshot.GenerateEntityId(), entity);
    }
    
  }
}