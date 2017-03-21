using System.Collections.Generic;
using System.IO;
using Assets.EntityTemplates;
using Improbable;
using Improbable.Worker;
using UnityEngine;
using JetBrains.Annotations;
using UnityEditor;

namespace Assets.Editor
{
  public class SnapshotMenu : MonoBehaviour
  {

    [MenuItem("Improbable/Snapshots/Generate Default Snapshot")]
    [UsedImplicitly]
    private static void GenerateDefaultSnapshot()
    {
      var path = Application.dataPath + "/../../../snapshots/";
      var snapshot = new SnapshotBuilder("default.snapshot", path);
      SnapshotDefault.Build(snapshot); 
      snapshot.SaveSnapshot();
    }
  }
}