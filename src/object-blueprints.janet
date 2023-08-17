(use cbt/xml-helpers/objects)
(use cbt/colors)

(use utf8)

(def- burial-crown "{{o-c-C-O-B sequence|burial crown}}")

(defn object-blueprints []
  [:objects
   (object :PKFUN_OsmocystInfection :FungalInfection
           (part :Physics :Weight 3)
           (part :Armor :WornOn "*" :DV -1 :AV 1 :Acid 10)
           (render "osmocyst" "Items/sw_waxflab.bmp" cyan dark-cyan)
           (description `Coruscant crystals of bloodshot salt pierce the surface of a cyst as dry and tight as an oak gall.`)
           (part :CurableFungalInfection)
           (part :LiquidVolume :Volume 0 :StartVolume 0 :MaxVolume 16 :InitialLiquid "salt-500,water-500")
           # usually a dram is worth 10,000 thirst
           (part :PKFUN_OsmocystHydrator :ThirstPerDramExtracted 8_000)
           # make sure you can't store lava and shit in this
           [:intproperty {:Name "Inorganic" :Value 0}])

   # Burial crown sprays a bunch of projectiles that trail conidia.
   (object :PKFUN_BurialCrownInfection :FungalInfection
           (part :Physics :Weight 3)
           (part :Armor :WornOn "*" :DV 0 :AV 0)
           (render burial-crown "Items/sw_wreath.bmp" cyan orange)
           (description "A crust of fungal flesh penetrates the skin, breathing in composure, breathing out hate.")
           (part :CurableFungalInfection)
           (part :PKFUN_BurialCrownSporeProducer :TriggerHealthPercentage 20)
           [:intproperty {:Name "Inorganic" :Value 0}])
   (object :PKFUN_BurialCrownSpores :Gas
           (part :Render :DisplayName (string burial-crown " conidia"))
           (description "An orange-peel scent burns redolent through the sinuses; then the bloodshed begins.")
           (part :Gas :GasType "PKFUN_BurialCrownSporesGas" :ColorString (color-string cyan orange))
           (part :PKFUN_BurialCrownSporesGas)
           (tag "Gender" "plural"))])
