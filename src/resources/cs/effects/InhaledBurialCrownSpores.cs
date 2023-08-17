using XRL.World.Parts;
using XRL.World.AI.GoalHandlers;

namespace XRL.World.Effects {
  // The effect is mostly so the player can see what is happening.
  public class PKFUN_InhaledBurialCrownSpores : Effect {
    public int oldMinKillRadius, oldMaxKillRadius;

    public PKFUN_InhaledBurialCrownSpores(int duration) {
      base.DisplayName = GetDescription();
      base.Duration = duration;
    }

    public override int GetEffectType() =>
      TYPE_RESPIRATORY | TYPE_NEGATIVE | TYPE_NEUROLOGICAL;

    public override bool UseStandardDurationCountdown() {
      return true;
    }

    public override string GetDescription() {
      return "inhaled " + PKFUN_BurialCrownSporeProducer.BURIAL_CROWN_NAME + " conidia";
    }

    public override string GetDetails() {
      return "Forcibly attacks the nearest organic creatures.";
    }

    public override bool Apply(GameObject target) {
      if (target.HasEffect<PKFUN_InhaledBurialCrownSpores>()) return false;
      if (!(target.GetPart<Brain>() is Brain brain)) return false;

      this.oldMaxKillRadius = brain.MaxKillRadius;
      this.oldMinKillRadius = brain.MinKillRadius;

      // Make them nearsighted so you can run
      brain.MaxKillRadius = 5;
      brain.MinKillRadius = 1;
      brain.PushGoal(new PKFUN_KillEverythingOnSight(this.Duration));

      return true;
    }

    public override void Remove(GameObject target) {
      if (target.GetPart<Brain>() is Brain brain) {
        for (int i = brain.Goals.Count - 1; i >= 0; i--) {
          if (brain.Goals.Items[i] is PKFUN_KillEverythingOnSight) {
            brain.Goals.Items.RemoveAt(i);
          }
        }

        brain.MaxKillRadius = this.oldMaxKillRadius;
        brain.MinKillRadius = this.oldMinKillRadius;
      }

      base.Remove(target);
    }
  }
}