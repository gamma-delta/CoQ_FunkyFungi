using XRL.UI;

namespace XRL.World.Parts {
  public class PKFUN_OsmocystHydrator : IActivePart {
    public int ThirstPerDramExtracted = 8_000;
    public bool HadWaterLastTime = true;

    public PKFUN_OsmocystHydrator() {
      ChargeUse = 0;
      IsEMPSensitive = false;
      WorksOnEquipper = true;
    }

    public static int getThirstThreshold() {
      if (Options.AutoSip) {
        // auihgdsjklfhlgkjhfdljkghfdjgjksg fd
        int threshold;
        string horribleBad = Options.AutoSipLevel;
        if (horribleBad == "Dehydrated")
          threshold = 0;
        else if (horribleBad == "Parched")
          threshold = Stomach.PARCHED;
        else if (horribleBad == "Thirsty")
          threshold = Stomach.THIRSTY;
        else if (horribleBad == "Quenched")
          threshold = Stomach.QUENCHED;
        else
          threshold = Stomach.TUMESCENT;

        return threshold + 5000;
      } else {
        return Stomach.THIRSTY;
      }
    }

    public string societyIfOptionalTypeGAPLDF() {
      var lv = ParentObject.LiquidVolume;
      if (lv == null) return "ProcessInputMissing";
      foreach (var liquid in lv.ComponentLiquids.Keys) {
        if (liquid != "salt" && liquid != "water") {
          return "ProcessInputInvalid";
        }
      }
      return null;
    }

    public override bool GetActivePartLocallyDefinedFailure() {
      return societyIfOptionalTypeGAPLDF() != null;
    }

    public override string GetActivePartLocallyDefinedFailureDescription() {
      return societyIfOptionalTypeGAPLDF();
    }

    public override bool WantTurnTick() => true;
    public override bool WantTenTurnTick() => true;
    public override bool WantHundredTurnTick() => true;
    public override void TurnTick(long TurnNumber) => hydrateOrDiedrate();
    public override void TenTurnTick(long TurnNumber) => hydrateOrDiedrate();
    public override void HundredTurnTick(long TurnNumber) => hydrateOrDiedrate();

    public void hydrateOrDiedrate() {
      var stomach = ParentObject.Equipped?.GetPart<Stomach>();

      // It's OK to not do this in a while loop assuming you don't get thirsty more than once every 100 turns
      if (stomach.Water < getThirstThreshold()) {
        if (ConsumeLiquid("water", 1, true)) {
          stomach.Water += ThirstPerDramExtracted;
          this.HadWaterLastTime = true;
          XRL.Messages.MessageQueue.AddPlayerMessage("The " + ParentObject.ShortDisplayName + " twinges. You feel a pressure in your veins.", "G");
        } else {
          if (this.HadWaterLastTime) {
            Popup.Show("You feel the crackle of salt in your veins.\n\nYour " + ParentObject.ShortDisplayName + " is out of water!");
            this.HadWaterLastTime = false;
          }
        }
      }
    }
  }
}