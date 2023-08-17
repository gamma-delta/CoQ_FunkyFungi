using XRL.Language;

namespace XRL.World.Parts {
  public class PKFUN_BurialCrownSporeProducer : IPart {
    public static string BURIAL_CROWN_NAME = "{{o-c-C-O-B sequence|burial crown}}";

    public int TriggerHealthPercentage;
    public long Cooldown = 1500;
    // -1 is a sigil meaning "never before," for asshole cases when you get the infection less than 1500 turns in
    // (such as when you are getting it turn 1 via a wish)
    public long LastTurnTriggeredOn = -1;

    public override bool WantEvent(int id, int cascade) =>
      base.WantEvent(id, cascade)
      || id == EquippedEvent.ID
      || id == UnequippedEvent.ID;

    // TODO: is there a less horrible way to do events that fire on the equipee
    public override bool HandleEvent(EquippedEvent e) {
      e.Actor.RegisterPartEvent(this, "TookDamage");
      return base.HandleEvent(e);
    }

    public override bool HandleEvent(UnequippedEvent e) {
      e.Actor.UnregisterPartEvent(this, "TookDamage");
      return base.HandleEvent(e);
    }

    public void TrySpewSpores(GameObject host) {
      if (host == null) return;
      if (100 * host.hitpoints / host.baseHitpoints > this.TriggerHealthPercentage) return;
      if (this.LastTurnTriggeredOn != -1 && The.Game.Turns <= this.LastTurnTriggeredOn + this.Cooldown) return;

      this.LastTurnTriggeredOn = The.Game.Turns;

      host.ParticleBlip("&W*");
      host.PlayWorldSound("Sounds/Abilities/sfx_ability_gasMutation_activeRelease");

      foreach (var cell in host.CurrentCell.GetLocalAdjacentCellsCircular(3)) {
        var gasObj = cell.AddObject("PKFUN_BurialCrownSpores");
        var gas = gasObj.GetPart<Gas>();
        gas.Creator = host;
        gas.Density = 300;
      }

      if (host.IsPlayer()) {
        var bp = ParentObject.EquippedOn();
        AddPlayerMessage("&yYour " + bp.GetOrdinalName() + " " + (bp.Plural ? "spew" : "spews") + " a cloud of spores.");
      } else if (IComponent<GameObject>.Visible(host)) {
        var bp = ParentObject.EquippedOn();
        AddPlayerMessage(Grammar.MakePossessive(host.The + host.ShortDisplayName) + "&y " + bp.GetOrdinalName() + " " + (bp.Plural ? "spew" : "spews") + " a cloud of spores.");
      }
    }

    public override bool FireEvent(Event e) {
      if (e.ID == "TookDamage") {
        this.TrySpewSpores(e.GetGameObjectParameter("Defender"));
      }
      return base.FireEvent(e);
    }
  }
}