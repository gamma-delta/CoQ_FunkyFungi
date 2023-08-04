(use cbt/xml-helpers/objects)
(use cbt/colors)

(use utf8)

(defn object-blueprints []
  [:objects
   (object "PKFUN_OsmocystInfection" "FungalInfection"
           (part :Physics :Weight 3)
           (part :Armor :WornOn "*" :DV 0 :AV 0 :Acid 10)
           (render "osmocyst" "Items/sw_waxflab.bmp" cyan dark-cyan)
           (description `Coruscant crystals of bloodshot salt pierce the surface of a cyst as dry and tight as an oak gall.`)
           (part :CurableFungalInfection)
           (part :LiquidVolume :MaxVolume 8 :StartVolume 0 :InitalLiquid "water-500,salt-500")
           # usually a dram is worth 10,000 thirst
           (part :PKFUN_OsmocystHydrator :ThirstPerDramExtracted 8_000)
           (part :PKFUN_OsmocystVolume))])