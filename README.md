# MoreCommands

Console commands factory for White Knuckle. Also adds a few commands.

### Console commands added:

| Command                            | Description                                                                                                                       | Type      | Enables cheats  |
|------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------|:---------:|:---------------:|
| `man`, `mhelp`, `morecommandshelp` | List MoreCommands with descriptions, filtered by `arg` (substring search)                                                         | Oneshot   | `-`             |
| `lp`                               | List perks info, filtered by `arg` (substring search)                                                                             | Oneshot   | `-`             |
| `freerun`                          | `godmode` + `deathgoo-stop` + `fullbright` + `infinitestamina` + `notarget`                                                       | Togglable | `+`             |
| `explore`                          | `freerun` + `noclip`                                                                                                              | Togglable | `+`             |
| `cargo`                            | Max Backstrength                                                                                                                  | Oneshot   | `+`             |
| `speedy`                           | Get some movement perks                                                                                                           | Oneshot   | `+`             |
| `grubby`                           | Grab anything (like after a sluggrub)                                                                                             | Togglable | `+`             |
| `buff`                             | Buff everything, but without perks                                                                                                | Togglable | `+`             |
| `flash`                            | `freerun` + `buff`                                                                                                                | Togglable | `+`             |
| `sv_gravity`, `grav`               | Change player gravity multiplier                                                                                                  | Oneshot   | `+`             |
| `tp`                               | Teleport player to `arg` (e.g. `tp abyss = tp aby (substring search) = cheats + teleportplayertolevel m3_habitation_lab_ending`)  | Oneshot   | `+`             |
| `banhammer`                        | Give player a banhammer which removes objects on hit                                                                              | Oneshot   | `+`             |
| `give`, `left`, `right`            | Give player an item `arg`, substring search (inventory/left hand/right hand respectively), no `arg` = list items                  | Oneshot   | `+`             |

Togglable commands can be run with arguments `true`/`false`

### Showcase YouTube video (click the image!)

[![Showcase](https://img.youtube.com/vi/ET25Z-EP7oI/maxresdefault.jpg)](https://www.youtube.com/watch?v=ET25Z-EP7oI)

### Screenshots

`man` example:
<div align="left">
<img src="https://raw.githubusercontent.com/shishyando/WK_MoreCommands/main/img/man.png" style="width: 820px; height: 410px; object-fit: contain;">
</div>

List perks (`lp`) example:
<div align="left">
<img src="https://raw.githubusercontent.com/shishyando/WK_MoreCommands/main/img/lp.png" style="width: 820px; height: 410px; object-fit: contain;">
</div>

`flash` example:
<div align="left">
<img src="https://raw.githubusercontent.com/shishyando/WK_MoreCommands/main/img/flash.png" style="width: 820px; height: 410px; object-fit: contain;">
</div>
