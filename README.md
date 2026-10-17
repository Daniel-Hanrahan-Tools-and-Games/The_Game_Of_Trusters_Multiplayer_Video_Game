# The_Game_Of_Trusters_Multiplayer_Video_Game
A Multi-User Dungeon (MUD) Version of The_Game_Of_Trusters_Video_Game_Adaptation that can work with local multiplayer and network multiplayer as long as the hardware has a Network Interface Card (NIC) or Serial Port.


<p>Copyright (C) 2026 Daniel Hanrahan Tools and Games SPDX-License-Identifier: AGPL-3.0-or-later A copy of the GNU Affero General Public License is included in the file LICENSE; if not, see<https://www.gnu.org/licenses/>. Information just about the stuff in this software not covered by the GNU Affero General Public License version 3: This work is licensed under Attribution-ShareAlike 4.0 International</p>


How to compile game with GForth(It acts like forth is an interpreted language):

Client:
<ol>
<li>type this in terminal: cd into where The_Game_Of_Trusters_Multiplayer_Video_Game_Client.fs file is</li>
<li>type this in terminal: gforth The_Game_Of_Trusters_Multiplayer_Video_Game_Client.fs -e "the_game-of-trusters-multiplayer-video-game-client"</li>
</ol>

Server:
<ol>
<li>type this in terminal: cd into where The_Game_Of_Trusters_Multiplayer_Video_Game_Server.fs file is</li>
<li>type this in terminal: gforth The_Game_Of_Trusters_Multiplayer_Video_Game_Server.fs -e "the_game-of-trusters-multiplayer-video-game-server"</li>
</ol>

Info to know about connections to other servers: Remember to change the server you want to go to you have to change the IP address in client source code as whatever IP address you want and for the serial port to work you have to change it to whatever your OS/Bare Metal calls your serial port, You also do that in client source code.


<h3>Both of these links take you to The_Game_Of_Trusters Video Game adaptation</h3>

<a href="https://github.com/Daniel-Hanrahan-Tools-and-Games/The_Game_Of_Trusters">The_Game_Of_Trusters Repository Page</a>

<a href="https://daniel-hanrahan-tools-and-games.github.io/The_Game_Of_Trusters/">The_Game_Of_Trusters ReadMe Page</a>


<h3>Link to page repo</h3>

<a href="https://github.com/Daniel-Hanrahan-Tools-and-Games/The_Game_Of_Trusters_Multiplayer_Video_Game">Repository Page</a>



CC BY-SA 4.0 and GNU AGPL v3.0 Conditional Exceptions to use MPL 2.0 and CC BY-SA 4.0 or CC BY 4.0

If the following condition is met, the licensing rules for both content covered by GNU AGPL v3.0 and content not covered by GNU AGPL v3.0 are modified as described below:

Condition:

The developer is distributing, porting, or integrating the software with platforms or environments that impose requirements incompatible with GPL-3.0, including but not limited to:
- proprietary or non-redistributable SDKs
- confidential hardware or platform documentation
- legally required confidentiality obligations preventing full AGPL redistribution
- safety-regulated or certified systems where full AGPL redistribution cannot be satisfied

Effect on licensing:

- Content covered by GNU AGPL v3.0: May instead be used under the Mozilla Public License 2.0.

- Content not covered by GNU AGPL v3.0 (e.g., assets): Normally may be used under CC BY-SA 4.0. If ShareAlike requirements of CC BY-SA 4.0 prevent lawful distribution under the MPL alternative, developers may instead use CC BY 4.0 **solely to the extent necessary** to enable such distribution.

These exceptions apply **only when the condition above is met**.




CC BY-SA 4.0 and GNU AGPL v3.0 Conditional Exceptions to use BSD-3-Clause and CC BY 4.0:

If **either** of the following conditions is met, the licensing
rules for both content covered by GNU AGPL v3.0 and content not
covered by GNU AGPL v3.0 are modified as described below:

Conditions:

1. The developer has made a licensing agreement with another entity
   (excluding corporate/LLC or equivalent entities) that prohibits the
   redistribution of content under copyleft licenses.

2. The developer adds assets that they do not own and uses them
   under fair use, or equivalent/similar legal arrangements, where
   the developer cannot legally contribute those assets under a
   copyleft license.

Effect on licensing:

- Content covered by GNU AGPL v3.0: May instead be used under the
  BSD 3-Clause License.

- Content not covered by GNU AGPL v3.0: Assets originally under CC BY-SA 4.0
  may instead be used under CC BY 4.0.

These exceptions apply **only when at least one of the conditions above is met**.





CC BY-SA 4.0 and GNU AGPL v3.0 Conditional Exceptions to use PolyForm Noncommercial and CC BY-NC 4.0

The PolyForm Noncommercial License (and Creative Commons
Attribution-NonCommercial 4.0 International for non-code
content) may be used as an alternative only when the combined
work is subject to binding legal, contractual, or platform-
imposed restrictions that prohibit commercial use.

Such restrictions may arise from third-party licenses,
distribution platforms, or other enforceable legal terms that
make commercial use of the combined work not legally permitted.

Content covered by the primary license (e.g., source code or
other covered material) remains governed by that license.

Content not covered by the primary license (e.g., assets,
documentation, or other non-code materials) is governed by
CC BY-NC 4.0, unless otherwise stated.

This alternative applies only to the extent necessary to
comply with such restrictions.




CC BY-SA 4.0 and GNU AGPL v3.0 Conditional Exceptions to use PolyForm Strict and CC BY-NC-ND 4.0

The PolyForm Strict License may be used as an alternative
license only when the combined work is subject to binding
legal, contractual, or platform-imposed restrictions that
require both non-commercial use and prohibit the creation of
derivative works as part of the distribution terms.

Such restrictions may arise from third-party licenses,
distribution platforms, or other enforceable legal terms that
impose both non-commercial and no-derivatives requirements on
the combined work.

Content covered by the primary license (e.g., source code or
other covered material) remains governed by that license.

Content not covered by the primary license (e.g., assets,
documentation, or other non-code materials) is governed by
Creative Commons Attribution-NonCommercial-NoDerivatives
4.0 International (CC BY-NC-ND 4.0), unless otherwise stated.

This alternative applies only to the extent necessary to
comply with such restrictions.
