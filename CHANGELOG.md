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

