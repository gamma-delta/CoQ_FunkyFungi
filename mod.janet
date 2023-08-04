#!/usr/bin/env janet
(use /src/object-blueprints)

(use cbt)

(build-metadata
  :qud-dlls "/home/petrak/.local/share/Steam/steamapps/common/Caves of Qud/CoQ_Data/Managed/"
  :qud-mods-folder "/home/petrak/.config/unity3d/Freehold Games/CavesOfQud/Mods/")

(declare-mod
  "funky-fungi"
  "Funky Fungi"
  "petrak@"
  "0.1.0")

(generate-xml "ObjectBlueprints.xml" object-blueprints)

# (set-debug-output true)

