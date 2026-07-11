# MoreCommands

Console commands for White Knuckle.

### Configuration

MoreCommands adds `Input.ToggleConsoleShortcut` to `BepInEx/config/shishyando.WK.MoreCommands.cfg`. The default is `F10`; change it to another BepInEx keyboard shortcut if needed. The game's vanilla `LeftShift + BackQuote` console shortcut remains available. Missing or `None` values fall back to `F10`.

`timerstyle` settings are also persisted in the MoreCommands BepInEx config.

### Console commands added:

| Command                            | Description                                                                                                                                                                      | Type      | Enables cheats |
| ---------------------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | :-------: | :------------: |
| `man`, `mhelp`, `morecommandshelp` | List MoreCommands help, optionally for one command; '+' means it enables cheats                                                                                                  | Oneshot   | `-`            |
| `freerun`                          | Toggle god mode, paused deathgoo, fullbright, infinite stamina, and no-target                                                                                                    | Togglable | `+`            |
| `cargo`                            | Max out the Back Strengtheners perk                                                                                                                                              | Oneshot   | `+`            |
| `speedyperks`                      | Max out the curated MoreCommands perk set                                                                                                                                         | Oneshot   | `+`            |
| `buff`                             | Toggle a powerful all-purpose buff without adding perks                                                                                                                          | Togglable | `+`            |
| `explore`                          | Toggle freerun and noclip together                                                                                                                                               | Togglable | `+`            |
| `flash`                            | Toggle freerun and the all-purpose buff together                                                                                                                                 | Togglable | `+`            |
| `sv_gravity`, `grav`               | Set the player gravity multiplier; no argument resets it to 1                                                                                                                    | Oneshot   | `+`            |
| `ns`, `noclipspeed`                | Set a non-negative noclip speed multiplier; no argument resets it to 1                                                                                                           | Oneshot   | `+`            |
| `give`                             | Give an item to inventory by exact ID; no argument lists items                                                                                                                    | Oneshot   | `+`            |
| `left`                             | Give an item to the left hand by exact ID; no argument lists items                                                                                                                | Oneshot   | `+`            |
| `right`                            | Give an item to the right hand by exact ID; no argument lists items                                                                                                               | Oneshot   | `+`            |
| `banhammer`                        | Give the player a banhammer in the left hand                                                                                                                                     | Oneshot   | `+`            |
| `tp`                               | Teleport to an exact named shortcut; no argument lists shortcuts                                                                                                                  | Oneshot   | `+`            |
| `mark`                             | Save a run-local position mark; the default name is 'default'                                                                                                                    | Oneshot   | `-`            |
| `warp`                             | Teleport to a run-local mark; the default name is 'default'                                                                                                                      | Oneshot   | `+`            |
| `marks`                            | List run-local warp marks                                                                                                                                                        | Oneshot   | `-`            |
| `unmark`                           | Delete a run-local warp mark by name                                                                                                                                             | Oneshot   | `-`            |
| `whereami`, `pos`                  | Print position, rotation, level, region, branch, seed, and reusable commands                                                                                                     | Oneshot   | `-`            |
| `nextlevel`                        | Teleport forward by an optional number of loaded levels                                                                                                                          | Oneshot   | `+`            |
| `prevlevel`                        | Teleport backward by an optional number of loaded levels                                                                                                                         | Oneshot   | `+`            |
| `heal`                             | Heal player by amount, or to full health with no amount                                                                                                                          | Oneshot   | `+`            |
| `buffs`                            | List active player buffs, colored effects, and unambiguous possible item sources                                                                                                 | Oneshot   | `-`            |
| `clearbuffs`                       | Clear MoreCommands buffs by default, all buffs, or one exact container                                                                                                           | Oneshot   | `+`            |
| `freezeai`                         | Pause AI update loops and targeting, including tentacles; physics, animations, and running coroutines continue                                                                   | Togglable | `+`            |
| `fov`                              | Show or set persistent player FOV from 60 to 140; reset restores 90                                                                                                              | Oneshot   | `-`            |
| `timerstyle`                       | Show or configure persistent timer presets, colors, outline, size, spacing, alpha, and font style                                                                                | Oneshot   | `-`            |
| `wallhack`, `wh`                   | Toggle default outlines, or toggle an entity outline by exact ID and optional color                                                                                               | Oneshot   | `+`            |
| `alwaysspawn`                      | Force eligible future spawn-chance checks to succeed and preserve their success/failure events                                                                                   | Togglable | `+`            |
| `lp`, `mclistperks`                | List perk titles, IDs, and descriptions, optionally filtered by substring                                                                                                        | Oneshot   | `-`            |
| `listentities`                     | List entity IDs, optionally filtered by substring                                                                                                                                | Oneshot   | `-`            |
| `listitems`                        | List item IDs, optionally filtered by substring                                                                                                                                  | Oneshot   | `-`            |
| `listregions`                      | List region names, optionally filtered by substring                                                                                                                              | Oneshot   | `-`            |
| `listsubregions`                   | List subregion names, optionally filtered by substring                                                                                                                           | Oneshot   | `-`            |
| `loadrandomlevels`                 | Start the level tester with a random non-empty set of levels                                                                                                                      | Oneshot   | `-`            |
| `loadrandomlevels_nodeathgoo`      | Start with random levels, then send deathgoo away after loading                                                                                                                   | Oneshot   | `+`            |
| `loadregions`                      | Start the level tester with levels from one or more exact region names                                                                                                           | Oneshot   | `-`            |
| `loadsubregions`                   | Start the level tester with levels from one or more exact subregion names                                                                                                        | Oneshot   | `-`            |
Togglable commands can be run with arguments `true`/`false`

### YouTube showcase videos

Version 0.10.0 (wallhack update): https://www.youtube.com/watch?v=KOxQGqisMBs

Version 0.8.1: https://www.youtube.com/watch?v=ET25Z-EP7oI

### Screenshots

<div align="left">
<img src="https://raw.githubusercontent.com/shishyando/WK_MoreCommands/main/img/man.png" style="width: 820px; height: 410px; object-fit: contain;">
</div>

<div align="left">
<img src="https://raw.githubusercontent.com/shishyando/WK_MoreCommands/main/img/wh.png" style="width: 820px; height: 410px; object-fit: contain;">
</div>

<div align="left">
<img src="https://raw.githubusercontent.com/shishyando/WK_MoreCommands/main/img/lp.png" style="width: 820px; height: 410px; object-fit: contain;">
</div>

<div align="left">
<img src="https://raw.githubusercontent.com/shishyando/WK_MoreCommands/main/img/speedy.png" style="width: 820px; height: 410px; object-fit: contain;">
</div>

<div align="left">
<img src="https://raw.githubusercontent.com/shishyando/WK_MoreCommands/main/img/enthusiast.png" style="width: 820px; height: 410px; object-fit: contain;">
</div>
