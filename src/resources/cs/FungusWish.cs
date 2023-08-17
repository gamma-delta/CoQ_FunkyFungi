using XRL.Wish;
using XRL;
using XRL.UI;
using XRL.World;
using XRL.World.Effects;
using XRL.World.Anatomy;

namespace PKFUN {
  [HasWishCommand]
  public class WishForFungus {
    [WishCommand(Command = "pkfungus")]
    public static bool GetFungus(string rest) {
      if (!GameObjectFactory.Factory.HasBlueprint(rest)) {
        Popup.Show("No blueprint named " + rest + ".");
        return true;
      }

      BodyPart targetPart = null;
      string _idc;
      if (FungalSporeInfection.ChooseLimbForInfection(rest, out targetPart, out _idc)) {
        bool success = FungalSporeInfection.ApplyFungalInfection(The.Player, rest, targetPart);
        if (!success)
          Popup.Show("Didn't take for some reason, I have no idea why");
      }

      return true;
    }
  }
}