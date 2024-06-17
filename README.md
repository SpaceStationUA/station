# Einstein Engines

<p align="center"><img src="https://raw.githubusercontent.com/Simple-Station/Einstein-Engines/master/Resources/Textures/Logo/splashlogo.png" width="512px" /></p>

---

Einstein Engines is a hard fork of [Space Station 14](https://github.com/space-wizards/space-station-14) built around the ideals and design inspirations of the Baystation family of servers from Space Station 13 with a focus on having modular code that anyone can use to make the RP server of their dreams.
Our founding organization is based on a democratic system whereby our mutual contributors and downstreams have a say in what code goes into their own upstream.
If you are a representative of a former downstream of Delta-V, we would like to invite you to contact us for an opportunity to represent your fork in this new upstream.

Space Station 14 is inspired heavily by Space Station 13 and runs on [Robust Toolbox](https://github.com/space-wizards/Robust-Toolbox), a homegrown engine written in C#.

As a hard fork, any code sourced from a different upstream cannot ever be merged directly here, and must instead be ported.
All code present in this repository is subject to change as desired by the council of maintainers.

## Official Server Policy

**No official servers will ever be made for Einstein-Engines**.

In order to prevent a potential conflict of interest, we will never open any server directly using the Einstein Engines codebase itself.
Any server claiming to be an official representation of this fork is not endorsed in any way by this organization.
We however would like to invite anyone wishing to create a server to make a fork of Einstein Engines.

## Links

[Website](https://simplestation.org) | [Discord](https://discord.gg/X4QEXxUrsJ) | [Steam(SSMV Launcher)](https://store.steampowered.com/app/2585480/Space_Station_Multiverse/) | [Steam(WizDen Launcher)](https://store.steampowered.com/app/1255460/Space_Station_14/) | [Standalone](https://spacestationmultiverse.com/downloads/)
![GitHub contributors from allcontributors.org](https://img.shields.io/github/contributors/SpaceCowboyServer/Pirate?style=for-the-badge)
![GitHub forks](https://img.shields.io/github/forks/SpaceCowboyServer/Pirate?style=for-the-badge)
![GitHub Repo stars](https://img.shields.io/github/stars/SpaceCowboyServer/Pirate?style=for-the-badge)
![GitHub Issues or Pull Requests](https://img.shields.io/github/issues/SpaceCowboyServer/Pirate?style=for-the-badge)
![GitHub Issues or Pull Requests](https://img.shields.io/github/issues-pr/SpaceCowboyServer/Pirate?style=for-the-badge)
![GitHub License](https://img.shields.io/github/license/SpaceCowboyServer/Pirate?style=for-the-badge)

<p align="center"> <img alt="Space Station 14 Delta-V Logo" width="128" height="128" src="https://github.com/SpaceCowboyServer/Pirate/assets/115815982/3ac6bc71-d9b9-4af5-ab01-dd621f99730e" /></p>

<h3 align="center">Троєщинські пірати</h3>
<strong>Троєщинські пірати</strong> — це форк   <a href="https://github.com/space-wizards/space-station-14"><strong>Space Station 14</strong></a>, <a href="https://github.com/DeltaV-Station/Delta-v"><strong>Delta-V</strong></a>, <a href="https://github.com/Simple-Station/Parkstation"><strong>Parkstaion</strong></a>, що поєднує в собі класичний хаос <strong>SS13</strong> і експерименти.

<br /><strong>Space Station 14</strong> — це ремейк <strong>SS13</strong>, який працює на двигуні <strong>Robust Toolbox</strong>, написаному на <strong>C#</strong>.


## Links
#### Троєщинські пірати
[![Discord][discord-shield]][discord-url]
[![Wiki][wiki-shield]][wiki-url]
#### DeltaV
[![Discord][discord-shield]][discord-url-delta]
[![Wiki][wiki-shield]][wiki-url-delta]
[![Website][website-shield]][website-url-delta]
#### Space Station 14
[![Discord][discord-shield]][discord-url-ss14]
[![Steam][steam-shield]][steam-url]
[![Website][website-shield]][website-url-ss14]
[![Forum][forum-shield]][forum-url]

## Документація
[![Docs][docs-shield]][docs-url]
<br/>Тут міститься документація щодо вмісту SS14, двигуна, дизайну тощо.

 ## Внесок

Ми раді прийняти будь-які внески до нашого репозиторію. Заходьте в нашу спільноту Discord, якщо хочете допомогти.

Будь-які зміни, внесені до файлів, що належать до нашого репозиторію, мають бути належним чином позначені відповідно до того, що вказано у них.

We are happy to accept contributions from anybody, come join our Discord if you want to help.
We've got a [list of issues](https://github.com/Simple-Station/Einstein-Engines/issues) that need to be done and anybody can pick them up. Don't be afraid to ask for help in Discord either!

We are currently accepting translations of the game on our main repository.
If you would like to translate the game into another language check the #contributor-general channel in our Discord.

## Building

Refer to [the Space Wizards' guide](https://docs.spacestation14.com/en/general-development/setup/setting-up-a-development-environment.html) on setting up a development environment for general information, but keep in mind that Einstein Engines is not the same and many things may not apply.
We provide some scripts shown below to make the job easier.

### Build dependencies

> - Git
> - .NET SDK 8.0.100


### Windows

> 1. Clone this repository
> 2. Run `git submodule update --init --recursive` in a terminal to download the engine
> 3. Run `Scripts/bat/buildAllDebug.bat` after making any changes to the source
> 4. Run `Scripts/bat/runQuickAll.bat` to launch the client and the server
> 5. Connect to localhost in the client and play

### Linux

> 1. Clone this repository
> 2. Run `git submodule update --init --recursive` in a terminal to download the engine
> 3. Run `Scripts/bat/buildAllDebug.sh` after making any changes to the source
> 4. Run `Scripts/bat/runQuickAll.sh` to launch the client and the server
> 5. Connect to localhost in the client and play

### MacOS

> I don't know anybody using MacOS to test this, but it's probably roughly the same steps as Linux
## Збірка

1. Клонуйте цей рпеозиторій
2. Запустіть `RUN_THIS.py` для ініціалізації підмодулів та завантаження двигуна.
3. Скомпілюйте.

[Більш детальна інструкція по збірці проєкту.](https://docs.spacestation14.com/en/general-development/setup.html)

## License

Content contributed to this repository after commit 87c70a89a67d0521a56388e6b1c3f2cb947943e4 (`17 February 2024 23:00:00 UTC`) is licensed under the GNU Affero General Public License version 3.0 unless otherwise stated.
See [LICENSE-AGPLv3](./LICENSE-AGPLv3.txt).

Content contributed to this repository before commit 87c70a89a67d0521a56388e6b1c3f2cb947943e4 (`17 February 2024 23:00:00 UTC`) is licensed under the MIT license unless otherwise stated.
See [LICENSE-MIT](./LICENSE-MIT.txt).

Most assets are licensed under [CC-BY-SA 3.0](https://creativecommons.org/licenses/by-sa/3.0/) unless stated otherwise. Assets have their license and the copyright in the metadata file.
[Example](./Resources/Textures/Objects/Tools/crowbar.rsi/meta.json).

Note that some assets are licensed under the non-commercial [CC-BY-NC-SA 3.0](https://creativecommons.org/licenses/by-nc-sa/3.0/) or similar non-commercial licenses and will need to be removed if you wish to use this project commercially.

[discord-shield]: https://img.shields.io/badge/Discord-%23404783?style=flat&logo=discord&logoColor=%23FFFFFF
[discord-url]: https://discord.com/invite/CrAaSgnQZR
[discord-url-delta]: https://go.delta-v.org/AtDxv
[discord-url-ss14]: https://discord.ss14.io/

[wiki-shield]: https://img.shields.io/badge/Wiki-%23e06c56?style=flat&logo=gitbook&logoColor=%23ffffff
[wiki-url]: https://spacestation14.org.ua
[wiki-url-delta]: https://wiki.delta-v.org/view/Main_Page

[website-shield]: https://img.shields.io/badge/Website-%2359d917?style=flat&logo=googleearth&logoColor=%23ffffff
[website-url-delta]: https://delta-v.org/
[website-url-ss14]: https://spacestation14.io/

[forum-shield]: https://img.shields.io/badge/Forum-%23fa42ff?style=flat
[forum-url]: https://forum.spacestation14.io/

[steam-shield]: https://img.shields.io/badge/Steam-%231b2838?style=flat&logo=steam
[steam-url]: https://store.steampowered.com/app/1255460/Space_Station_14/

[docs-shield]: https://img.shields.io/badge/Docs-%234285F4?style=flat&logo=googledocs&logoColor=%23FFFFFF
[docs-url]: https://docs.spacestation14.io/
