namespace XRL.World.Parts {
  public class PKFUN_OsmocystHydrator : IActivePart {
    // You are too hydrated and throw up at 50,000.
    static int THIRST_THRESHOLD = 40_000;
    // Normally a dram'll get you 10,000
    int ThirstPerDramExtracted = 8_000;

    public PKFUN_OsmocystHydrator() {
      ChargeUse = 0;
      IsEMPSensitive = false;
    }

    public override bool WantTurnTick() => true;
    public override bool WantTenTurnTick() => true;
    public override bool WantHundredTurnTick() => true;
    public override void TurnTick(long TurnNumber) => hydrateOrDiedrate();
    public override void TenTurnTick(long TurnNumber) => hydrateOrDiedrate();
    public override void HundredTurnTick(long TurnNumber) => hydrateOrDiedrate();

    public void hydrateOrDiedrate() {
      var stomach = ParentObject.Equipped.GetPart<Stomach>();
      if (stomach == null) return;
      int waterNeeded = stomach.Water - THIRST_THRESHOLD;

      while (waterNeeded > ThirstPerDramExtracted) {
        if (ConsumeLiquid("water", 1, true)) {
          stomach.Water += ThirstPerDramExtracted;
        } else {
          break;
        }
      }
    }
  }
}