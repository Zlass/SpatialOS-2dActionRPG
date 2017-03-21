using Assets.Gamelogic.Core;
using Improbable;
using Improbable.Math;
using Improbable.Collections;
using Improbable.Unity.Core;
using Improbable.Unity.Core.Acls;
using Improbable.Worker;
using World.Location;
using World.Authority;
using World.Player;
using Character.Equipment;

namespace Assets.Gamelogic.EntityGenerator
{
  public static class CoreEntityFactory
  {
    public static SnapshotEntity CreatePlayerManagerTemplate()
    {
      var template = new SnapshotEntity { Prefab = WorldSettings.PlayerManagerEntityName };
      template.Add(new Location.Data(Coordinates.ZERO));
      template.Add(new FSimAuthorityCheck.Data());
      template.Add(new PlayerManager.Data(new Map<string, EntityId>(), new Map<string, Coordinates>()));

      var permissions = Acl.Build()
      .SetReadAccess(CommonRequirementSets.PhysicsOrVisual)
      .SetWriteAccess<Location>(CommonRequirementSets.PhysicsOnly)
      .SetWriteAccess<FSimAuthorityCheck>(CommonRequirementSets.PhysicsOnly)
      .SetWriteAccess<PlayerManager>(CommonRequirementSets.PhysicsOnly);

      template.SetAcl(permissions);

      return template;
    }
  }
}