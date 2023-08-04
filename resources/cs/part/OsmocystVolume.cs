using System.Collections.Generic;

namespace XRL.World.Parts {
  public class PKFUN_OsmocystVolume : LiquidVolume {
    public static HashSet<string> ALLOWED_LIQUIDS = new HashSet<string>() {
      "water", "salt",
    };

    public PKFUN_OsmocystVolume() {
    }

    public override bool WantEvent(int id, int cascade) {
      return base.WantEvent(id, cascade)
        || id == AllowLiquidCollectionEvent.ID
        || id == WantsLiquidCollectionEvent.ID
        || id == GetPreferredLiquidEvent.ID;
    }

    public bool allowLiquid(LiquidVolume v) {
      foreach (string liq in v._ComponentLiquids.Keys) {
        if (!ALLOWED_LIQUIDS.Contains(liq))
          return false;
      }
      return true;
    }

    public override bool HandleEvent(AllowLiquidCollectionEvent e) {
      return allowLiquid(e.LiquidVolume);
    }

    public override bool HandleEvent(WantsLiquidCollectionEvent e) {
      return allowLiquid(e.LiquidVolume);
    }

    public override bool HandleEvent(GetPreferredLiquidEvent e) {
      e.Liquid = "water";
      return true;
    }
  }
}