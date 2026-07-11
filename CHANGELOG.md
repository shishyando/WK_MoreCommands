# Version 0.14.0 Lean and Alive (11 july 2026)

### Added:

* Added autocomplete and runtime argument validation throughout MoreCommands; invalid arguments no longer execute commands or enable cheats.
* `buffs` now shows each active container in a readable block, colors individual effects, and shows a possible source when exactly one item has the same buff definition.

### Fixed:

* Numeric commands now reject `NaN` and infinity before they can corrupt health, movement, gravity, FOV, or timer settings.
* `freezeai` now pauses AI update loops, including barnacle tongues and strider movement, while preventing new targeting without per-entity state tracking. Physics, animations, and already-running coroutines intentionally continue.
* `alwaysspawn` now invokes the same success or failure events as the vanilla spawn check.
* Consumable buff sources are inferred from complete buff definitions instead of guessing by buff ID; ambiguous matches are omitted.
* `loadrandomlevels` always chooses at least one level and reports an empty level database safely.
* `left` and `right` now preserve each item's normal inventory rotation.
* `explore` and `flash` no longer disable their shared `freerun` dependency while the other preset is still enabled.
* `nextlevel` no longer overflows for very large counts, and level teleporting now reports missing spawn points or unloaded shortcut targets instead of claiming success.
* Random, region, and subregion level loading now passes canonical level IDs to the vanilla loader and requires explicit region/subregion selections.
* Fixed timer outlines not reserving mesh padding and made `timerstyle outline <color>` choose a visible width when the current style has none.
* `timerstyle default` now captures the current base-game timer style for every new timer UI instance.

### Changed:

* Reworked the `timerstyle` presets with stronger contrast and coordinated face/outline color combinations.
* Item, entity, teleport, region, subregion, and buff-removal commands now use exact autocomplete values instead of choosing the first substring match. Read-only list filters still support substrings and cleanly report no results.
* Simplified AI freezing, prefab lookup, command execution, timer parsing, and console output paths; removed unused providers, caches, and retired command source files.
* Rewrote command descriptions to document defaults, ranges, exact IDs, filters, and argument behavior.

### Removed:

* Removed `hudstats`, which duplicated the base game's F5 debug display.
* Removed the broken `restore` command without a replacement.
* Removed `spiderman`, `justcause`, and `loadrandomlevels_spiderman`. These were showcases of funny command combinations; use the in-game `alias` command to recreate combinations you want without bloating MoreCommands.

# Version 0.13.0 Resurrect MoreCommands (29 june 2026)

### Added:

* Added `spiderman`, `justcause`, `loadrandomlevels_nodeathgoo`, and `loadrandomlevels_spiderman` shortcut commands.
* Added `Input.ToggleConsoleShortcut` config for opening the command console with an additional shortcut.
* Added position marks, level navigation, player recovery, buff inspection/cleanup, AI freeze, HUD stats, persistent FOV, and persistent timer style commands.

### Fixed:

* MoreCommands no longer overrides vanilla commands. Vanilla `spawnentity`, `listlevels` and `listperks` are preserved; the enhanced perk list is available as `lp` or `mclistperks`.
* Fixed an issue where scene unload cleanup could enable cheats and mark a clean run as cheated while disabling togglable commands.
* Fixed crashes when commands were run without an active player, world, inventory, camera, or world loader.
* Fixed `grav` cleanup crashing if the player had already been destroyed during scene unload.
* Fixed `explore` and `flash` toggling child commands unpredictably instead of setting them to the same enabled state.
* Fixed entity prefab registration mutating prefabs with generic `GameEntity` components, which could break entity listing, spawning, and wallhack matching.
* Fixed duplicate vanilla command registration returning null and crashing command builders such as FXManager's `decal_griddepth` registration.
* Fixed `wh entity color` disabling active outlines instead of changing their color.

### Removed:

* Removed the MoreCommands `spawn` / `spawnent` entity spawn command in favor of vanilla `spawnentity`, which has better autocomplete and count handling.

### Changed:

* MoreCommands now registers commands with native console descriptions, autocomplete, validation, and value previews where appropriate.

# Version 0.12.1 Anniversary update support (19 april 2026)

### Fixed:

* Updated the mod to work with the anniversary update! Thanks to Dotlake for reviving the mod after the anniversary update came out!

### Removed:
* `grubby` command removed in favor of in-game `climbanywhere` 
* `loadlevels` command removed in favor of in-game `loadlevels` since it has better autocomplete.
* `elevator` location for `tp` command (but you can use in-game `teleportplayertolevel` with autocomplete) since it was broken. other locations still work.


# Version 0.11.2 Load levels update (30 nov 2025)

### Fixed:

* `loadlevels`, `loadregions` and `loadsubregions` should be fixed now.
Some regions can not be loaded via `loadregions`, I did not manage to find an easy way to get all region's levels so I use in-game `GetLevels(null)` and sometimes it crashes.
You will see an error message when you try to load such region.

# Version 0.11.1 Load levels update (30 nov 2025)

### Added:
* `listlevels` - replaces in-game command with more levels which can be used for loading. List levels, filtered by `arg`.
* `listregions` - List regions, filtered by `arg`.
* `listsubregions` - List subregions, filtered by `arg`.
* `loadlevels` - Load levels by its names with substring search.
* `loadrandomlevels` - Load completely random levels. Recommended commands: `godmode`, `deathgoo-stop`, `deathgoo-height NaN`. Have fun.
* `loadregions` - Load some regions' levels by region names, filtered by `arg`. Each region loads like in a normal run, not shuffled.
* `loadsubregions` - Load all subregions' levels by subregion names, filtered by `arg`.

### Changed:
* `listperks` now replaces in-game command (`lp` alias also works as before)
* `man` has different colors for commands and sorts by color + alias
* a lot of implementation stuff again
* secret rooms are now always open while `alwaysspawn` is active
* fixed vanilla game bug which crashes your console on non-ascii input & requires run restart

# Version 0.10.3 Wallhack update (29 nov 2025)

Showcase: https://www.youtube.com/watch?v=KOxQGqisMBs

### Added:
* `wh`, `wallhack` allows you to enable colorful outlines for any entity (pickupables, denizens, planks, etc.) with substring search
* `alwaysspawn` guarantees spawns (items, handholds, supply crates, etc.). Does not spawn items which already failed random check before entering the command. Opens secret rooms.
* `listentities` replaces in-game `listentities` with more entities and substring search
* `spawn`, `spawnentity` uses an extended list of spawnable entities with substring search
* `listitems` provides an extended list of items with substring search (used with `give`, `left`, `right` commands)
* More colors!

### Changed:
* a lot of implementation stuff
* `speedy` became `speedyperks` to make it clear that it gives the player perks

### Fixed:
* Commands (`wh`, `noclipspeed`, maybe something else) are properly disabled on run restart / exiting to main menu.

### Todo:
* Replace in-game `help` and other default commands
* Better locations for `tp` command
* Chainable commands
* Some configs (persistent `alias` command? editing default `wh` preset?)
* Support ColorfulTimers mod in MoreCommands (commands to change how timer looks like)

# Version 0.9.0 noclipspeed update (26 oct 2025)

### Added:
* `ns`, `noclipspeed` command which sets noclip speed multiplier

### Todo:
* ???

# Version 0.8.0 "Give" update (13 oct 2025)

### Showcase YouTube video (click the image!)

[![Showcase](https://img.youtube.com/vi/ET25Z-EP7oI/maxresdefault.jpg)](https://www.youtube.com/watch?v=ET25Z-EP7oI)

### Added:
* `give`, `left`, `right` commands to give items (inventory/left hand/right hand respectively)
* `banhammer` command which gives you a banhammer to remove things on hit
* `tp teeth` to teleport in the saferoom before teeth elevator
* A slight separator line after each command for easier navigation

### Changed:
* Silos saferoom with experimental perks is now available via simple `tp exp` instead of `tp silos_exp_perk`
* `tp` now supports substring search (`tp aby = tp abyss`)

### Todo:
* ???

# Version 0.7.0: "Buff" update (6 oct 2025)

### Added:
* `buff` command which buffs the player without giving perks

### Changed:
* `flash` commands now use `buff` command
* `speedy` still gives you a bunch of perks
* other minor changes

### Todo:
* ???
