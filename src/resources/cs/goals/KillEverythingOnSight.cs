using XRL.Core;
using XRL.World.Parts;

namespace XRL.World.AI.GoalHandlers {
  public class PKFUN_KillEverythingOnSight : GoalHandler {
    public long endTurn;

    public PKFUN_KillEverythingOnSight(int turns) {
      this.endTurn = XRLCore.Core.Game.Turns + turns;
    }

    public override bool Finished() {
      return XRLCore.Core.Game.Turns >= this.endTurn;
    }

    public override bool CanFight() {
      return true;
    }

    public override void TakeAction() {
      if (ParentObject == The.Player) {
        return;
      }

      if (XRLCore.Core.Game.Turns >= this.endTurn) {
        Pop();
        return;
      }

      var cell = ParentObject.CurrentCell;
      if (cell == null || cell.IsGraveyard())
        return;

      if (cell.ParentZone.IsActive()) {
        var target = cell.ParentZone.FindClosestObject(ParentObject, IsKillingTarget, IncludeSelf: false);
        if (target != null) {
          ParentBrain.WantToKill(target, "the fungus commands it");
          if (ParentBrain.Target != null) {
            return;
          }
        } else {
          ParentBrain.PushGoal(new WanderRandomly("3d4".RollCached()));
        }
      }

      ParentBrain.ParentObject.UseEnergy(1000);
    }

    // The fungus makes it attack things that it could infect.
    // Not using the CanApplySpores evt because only robots ignore that, so you get fantastic things like
    // creatures attacking the spores around them.
    // this is interesting emergent behavior but i want them killing each other
    public static bool IsKillingTarget(GameObject o) {
      return o.HasPart<Brain>() && o.GetIntPropertyIfSet("inorganic") != 1;
    }
  }
}