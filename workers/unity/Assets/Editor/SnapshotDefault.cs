using Assets.Gamelogic.Core;
using Assets.Gamelogic.EntityGenerator;
using Improbable.Math;
using Improbable.Worker;
using UnityEngine;

namespace Assets.Editor
{
  public static class SnapshotDefault
  {
    public static void Build(SnapshotBuilder snapshot){

      SnapshotUtil.AddPlayerMangerEntity(snapshot);
      
    }
  }

}