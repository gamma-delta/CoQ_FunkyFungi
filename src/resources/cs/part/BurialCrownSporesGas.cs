using System;
using XRL.World.Effects;

namespace XRL.World.Parts {
  public class PKFUN_BurialCrownSporesGas : IPart {
    public string GasType = "PKFUN_BurialCrownSporesGas";

    public bool ApplyHateEverything(GameObject GO) {
      if (GO == ParentObject) {
        return false;
      }
      if (GO.pBrain == null) {
        return false;
      }
      if (!GO.Respires) {
        return false;
      }
      if (GO.HasEffect("Asleep")) {
        return false;
      }
      Gas gas = ParentObject.GetPart("Gas") as Gas;
      if (!IsAffectable(GO, gas)) {
        return false;
      }
      if (GO == gas.Creator) {
        return false;
      }
      if (GO.HasEffect<PKFUN_InhaledBurialCrownSpores>()) {
        return false;
      }

      int difficulty = GetRespiratoryAgentPerformanceEvent.GetFor(GO, ParentObject, gas, null, 0, 0, WillAllowSave: true);
      if (difficulty > 0 && !GO.MakeSave("Toughness", 5 + gas.Level + difficulty, Vs: "Inhaled Burial Crown Conidia", Source: ParentObject)) {
        GO.ApplyEffect(new PKFUN_InhaledBurialCrownSpores("7d10".RollCached()));
        return true;
      }

      return false;
    }

    public void ApplyHateEverything() {
      ApplyHateEverything(ParentObject.CurrentCell);
    }

    public void ApplyHateEverything(Cell C) {
      if (C != null) {
        for (int i = 0; i < C.Objects.Count; i++) {
          ApplyHateEverything(C.Objects[i]);
        }
      }
    }

    public override bool WantEvent(int ID, int cascade) {
      return base.WantEvent(ID, cascade)
        || ID == GetAdjacentNavigationWeightEvent.ID
        || ID == GetNavigationWeightEvent.ID
        || ID == ObjectEnteredCellEvent.ID;
    }

    public override bool HandleEvent(GetNavigationWeightEvent E) {
      if (E.Actor.HasEffect<PKFUN_InhaledBurialCrownSpores>()) {
        return base.HandleEvent(E);
      }
      if (E.PhaseMatches(ParentObject)) {
        if (E.Smart) {
          E.Uncacheable = true;
          Gas gas = ParentObject.GetPart("Gas") as Gas;
          if (IsAffectable(E.Actor, gas)) {
            int num = gas.Level;
            E.MinWeight(StepValue(gas.Density) / 2 + num, Math.Min(10 + num, 50));
          }
        } else {
          E.MinWeight(2);
        }
      }
      return base.HandleEvent(E);
    }

    public override bool HandleEvent(GetAdjacentNavigationWeightEvent E) {
      if (E.Actor.HasEffect<PKFUN_InhaledBurialCrownSpores>()) {
        return base.HandleEvent(E);
      }
      if (E.PhaseMatches(ParentObject)) {
        if (E.Smart) {
          E.Uncacheable = true;
          Gas gas = ParentObject.GetPart("Gas") as Gas;
          if (IsAffectable(E.Actor, gas)) {
            int num = gas.Level;
            E.MinWeight(StepValue(gas.Density) / 10 + num / 5, Math.Min(10 + num / 5, 14));
          }
        } else {
          E.MinWeight(1);
        }
      }
      return base.HandleEvent(E);
    }

    public override bool HandleEvent(ObjectEnteredCellEvent E) {
      ApplyHateEverything(E.Object);
      return base.HandleEvent(E);
    }

    public override bool WantTurnTick() {
      return true;
    }

    public override bool WantTenTurnTick() {
      return true;
    }

    public override bool WantHundredTurnTick() {
      return true;
    }

    public override void TurnTick(long TurnNumber) {
      ApplyHateEverything();
    }

    public override void TenTurnTick(long TurnNumber) {
      ApplyHateEverything();
    }

    public override void HundredTurnTick(long TurnNumber) {
      ApplyHateEverything();
    }

    public override void Register(GameObject Object) {
      Object.RegisterPartEvent(this, "DensityChange");
      base.Register(Object);
    }

    public override bool FireEvent(Event E) {
      if (E.ID == "DensityChange" && StepValue(E.GetIntParameter("OldValue")) != StepValue(E.GetIntParameter("NewValue"))) {
        FlushNavigationCaches();
      }
      return base.FireEvent(E);
    }

    public bool IsAffectable(GameObject Object, Gas Gas = null) {
      if (!CheckGasCanAffectEvent.Check(Object, ParentObject, Gas)) {
        return false;
      }
      if (Object == null) {
        return true;
      }
      return Object.PhaseMatches(this.ParentObject);
    }

    public override bool SameAs(IPart p) {
      if ((p as PKFUN_BurialCrownSporesGas).GasType != GasType) {
        return false;
      }
      return base.SameAs(p);
    }

  }
}
