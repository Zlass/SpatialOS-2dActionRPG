using Improbable.Unity;
using Improbable.Unity.Configuration;
using Improbable.Unity.Core;
using UnityEngine;

namespace Assets.Gamelogic.Core
{
  // Placed on a gameobject in client scene to execute connection logic on client startup
  public class Bootstrap : MonoBehaviour
  {
    public WorkerConfigurationData Configuration = new WorkerConfigurationData();

    private bool readyToCollectClient;

    public void Start()
    {
      SpatialOS.ApplyConfiguration(Configuration);

      Time.fixedDeltaTime = 1.0f / WorldSettings.FixedFramerate;

      switch (SpatialOS.Configuration.WorkerPlatform)
      {
        case WorkerPlatform.UnityWorker:
          Application.targetFrameRate = WorldSettings.TargetFramerateFSim;
          SpatialOS.OnDisconnected += reason => Application.Quit();
          SpatialOS.Connect(gameObject);
          break;
        case WorkerPlatform.UnityClient:
          Application.targetFrameRate = WorldSettings.TargetFramerate;
          SpatialOS.OnConnected += ClientPlayerSpawner.SpawnPlayer;
          readyToCollectClient = true;
          break;
      }

    }
    public bool IsReadyToConnectClient()
    {
      return readyToCollectClient;
    }

    public void AttemptClientConnect()
    {
      SpatialOS.Connect(gameObject);
    }

    private void OnApplicationQuit()
    {
      if (SpatialOS.IsConnected)
      {
        SpatialOS.Disconnect();
      }
    }
  }
}