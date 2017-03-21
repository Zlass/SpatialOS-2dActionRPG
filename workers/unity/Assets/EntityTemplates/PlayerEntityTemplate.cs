using Improbable.Worker;
using Improbable.Math;
using Improbable.Collections;
using Improbable.Unity.Core.Acls;
using UnityEngine;
using World.Location;
using World.Player;
using Character.Attributes;
using Character.Classes;
using Character.Info;
using Character.Stats;
using System;

namespace Assets.EntityTemplates
{
  public class PlayerEntityTemplate : MonoBehaviour
  {
    // Template definition for a Example entity
    public static SnapshotEntity GeneratePlayerEntity()
    {
      // Set name of Unity prefab associated with this entity
      var playerEntity = new SnapshotEntity { Prefab = "PlayerEntity" };

      // Define components attached to snapshot entity
      playerEntity.Add(new Location.Data(new LocationData(new Coordinates(0, 0, 0))));
      playerEntity.Add(new Character.Info.Name.Data(
        new Character.Info.NameData("Zach", true, new Coordinates(0, 0, 0))
        ));
      playerEntity.Add(new Charisma.Data(new CharismaData(10, 0, 0)));
      playerEntity.Add(new Constitution.Data(new ConstitutionData(10, 0, 0)));
      playerEntity.Add(new Dexterity.Data(new DexterityData(10, 0, 0)));
      playerEntity.Add(new Intelligence.Data(new IntelligenceData(10, 0, 0)));
      playerEntity.Add(new Wisdom.Data(new WisdomData(10, 0, 0)));
      playerEntity.Add(new Mana.Data(new ManaData(100)));
      playerEntity.Add(new Health.Data(new HealthData(100)));
      playerEntity.Add(new Level.Data(new LevelData(100)));
      playerEntity.Add(new Experience.Data(new ExperienceData(100)));
      var classLevels = new Map<ClassType, uint>();
      foreach (ClassType classType in Enum.GetValues(typeof(ClassType)))
      {
        classLevels.Add(classType, 0);
      }
      playerEntity.Add(new Class.Data(new ClassData(ClassType.WARRIOR, classLevels)));

      var acl = Acl.Build()
          // Both FSim (server) workers and client workers granted read access over all states
          .SetReadAccess(CommonRequirementSets.PhysicsOrVisual)

          // Only FSim workers granted write access over components
          .SetWriteAccess<Location>(CommonRequirementSets.PhysicsOnly)
          .SetWriteAccess<Charisma>(CommonRequirementSets.PhysicsOnly)
          .SetWriteAccess<Constitution>(CommonRequirementSets.PhysicsOnly)
          .SetWriteAccess<Dexterity>(CommonRequirementSets.PhysicsOnly)
          .SetWriteAccess<Intelligence>(CommonRequirementSets.PhysicsOnly)
          .SetWriteAccess<Strength>(CommonRequirementSets.PhysicsOnly)
          .SetWriteAccess<Wisdom>(CommonRequirementSets.PhysicsOnly)
          .SetWriteAccess<Health>(CommonRequirementSets.PhysicsOnly)
          .SetWriteAccess<Mana>(CommonRequirementSets.PhysicsOnly)
          .SetWriteAccess<Experience>(CommonRequirementSets.PhysicsOnly)
          .SetWriteAccess<Level>(CommonRequirementSets.PhysicsOnly)

          // Only client workers granted write access over components
          .SetWriteAccess<Character.Info.Name>(CommonRequirementSets.VisualOnly);

      playerEntity.SetAcl(acl);

      return playerEntity;
    }
  }
}