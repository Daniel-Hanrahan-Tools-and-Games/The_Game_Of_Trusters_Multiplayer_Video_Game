\ =====================================================================
\ The Game of Trusters - Forth MUD/MMO Client Engine
\ Author: Copyright (c) 2026 Daniel Hanrahan Tools and Games
\ License: SPDX-License-Identifier: AGPL-3.0-or-later
\ =====================================================================
\ DESCRIPTION:
\ The client handles local connection setups (Serial, Localhost, LAN/Modem),
\ player mode configuration (Single Player vs Local Co-op turns), parsing 
\ server nibble packets, rendering ANSI color-coded enemy attributes (7 colors),
\ mapping all 10 unique enemy types, executing d20 combat/trap rolls, 
\ and enforcing strict single-digit ASCII boundary validation.
\ =====================================================================

\ --- CLIENT PRNG UTILITIES ---
VARIABLE c-seed
77777 c-seed !

\ ( n -- u ) Client-side pseudo-random number generator
: c-random ( n -- u )
    c-seed @ 1103515245 * 12345 + 2147483647 AND
    dup c-seed !
    SWAP MOD ;

\ ( -- u ) Standard d20 dice roll function (returns 1 to 20)
: d20 ( -- u ) 20 c-random 1+ ;

\ --- CONNECTION & MULTIPLAYER STATE VARIABLES ---
VARIABLE server-handle
VARIABLE active-player 1 active-player !
VARIABLE total-players 1 total-players !   \ Default to Single Player mode

\ --- TRANSIENT GAME MATH STORAGE (Prevents Stack Underflows) ---
VARIABLE trap-intel
VARIABLE escape-roll
VARIABLE enemy-strength
VARIABLE attack-roll
VARIABLE option-var
VARIABLE raw-input
VARIABLE current-enemy-id

\ --- BITWISE NIBBLE UTILITIES ---
\ ( high low -- byte )
: pack-nibbles ( high low -- byte )
    SWAP 4 LSHIFT OR ;

\ ( byte -- high low ) Unpacks an 8-bit stream byte into high event & low payload
: unpack-nibbles ( byte -- high low )
    DUP 4 RSHIFT SWAP 0xF AND ;

\ --- CONNECTION ENDPOINT SELECTION MENU ---
: select-server 
    CR ." ==========================================" CR
    ."      THE GAME OF TRUSTERS: SERVER SELECT    " CR
    ." ==========================================" CR
    ." Choose Real Connection Endpoint:" CR
    ."   1. Direct Serial Port (COM1 / /dev/ttyS0)" CR
    ."   2. Localhost Network Link (127.0.0.1:4040)" CR
    ."   3. LAN / Modem Direct Link (Remote IP / Dial-up)" CR
    ." Enter Choice (1-3): "
    
    KEY 48 - ( Read ASCII character, subtract 48 to convert to integer )
    
    CASE
        \ --- Change this to whatever the OS/Bare Metal calls its serial ports ---
        1 OF 
            CR ." [Connected via Serial Line]" CR
            \ --- Option 1: Direct Serial Device Binding ---
            \ (Use "COM1" for Windows or "/dev/ttyS0" for Linux/macOS)
            1 server-handle !
          ENDOF
          
        2 OF 
            CR ." [Successfully Connected to Localhost!]" CR
            \ --- Option 2: Localhost Connection Endpoint ---
            1 server-handle !
          ENDOF

        \ --- Change this to whatever server you want to connect to ---  
        3 OF 
            CR ." [Connected via LAN / Modem Direct Link]" CR
            \ --- Option 3: Remote LAN IP / Modem Stream Binding ---
            1 server-handle !
          ENDOF
          
        DUP 
            CR ." Invalid selection. Exiting." CR ABORT
    ENDCASE ;

\ --- PLAYER MODE CONFIGURATION (Single Player vs Local Co-op) ---
: configure-players
    CR ." ==========================================" CR
    ."         PLAYER MODE CONFIGURATION           " CR
    ." ==========================================" CR
    ." Enter Number of Players (1 for Single Player, or 2-4 for Local Co-op): "
    
    KEY 48 -
    
    \ Validate input range (1 to 4 players allowed)
    DUP 1 < OVER 4 > OR IF
        DROP 
        1 total-players !
        CR ." Invalid entry. Defaulting to Single Player Mode (1 Player)." CR
    ELSE
        total-players !
        CR ." Mode set: " total-players @ . ." Player(s)." CR
    THEN ;

\ --- 1. ENEMY COLOR LOOKUP SWITCH (7 Colors) ---
: print-color ( n -- )
    CASE
        1 OF ." red" ENDOF
        2 OF ." orange" ENDOF
        3 OF ." yellow" ENDOF
        4 OF ." green" ENDOF
        5 OF ." blue" ENDOF
        6 OF ." purple" ENDOF
        7 OF ." black" ENDOF
        DUP ." unknown"
    ENDCASE ;

\ --- 2. ENEMY TYPE LOOKUP SWITCH (10 Unique Monsters) ---
: print-enemy-type ( n -- )
    CASE
        1 OF ." a fingerblade killer" ENDOF
        2 OF ." a vampire" ENDOF
        3 OF ." a giant ant" ENDOF
        4 OF ." a leprechaun" ENDOF
        5 OF ." a chair monster" ENDOF
        6 OF ." a barrel monster" ENDOF
        7 OF ." a robot with chainsaws for teeth" ENDOF
        8 OF ." a dragon" ENDOF
        9 OF ." a wyvern" ENDOF
        10 OF ." a tree monster" ENDOF
        DUP ." an unknown entity"
    ENDCASE ;

\ --- 3. TRAP ENCOUNTER RESOLUTION HANDLER ---
: handle-trap
    CR ." *** TRAP TRIGGERED BY SERVER! ***" CR
    10 c-random 1+ trap-intel ! ( Generate trap intelligence rating 1-10 )
    d20 escape-roll !           ( Roll d20 escape check )
    
    \ Display rolls safely using variables to prevent stack underflows
    escape-roll @ . ." vs Trap Intellect: " trap-intel @ . CR
    
    escape-roll @ trap-intel @ > IF 
        ." Your party successfully escaped the trap." CR
    ELSE 
        ." Party caught in trap! Game Over." CR ABORT 
    THEN ;

\ --- 4. COMBAT ENCOUNTER RESOLUTION HANDLER ---
: handle-enemy ( enemy-id -- )
    CR ." ==========================================" CR
    7 c-random 1+ CR ." Enemy outline color: " print-color CR
    7 c-random 1+ ." Enemy base color: " print-color CR
    
    \ Print specific monster type and prompt user for action choice
    DUP ." You have encountered " print-enemy-type ." , what will you do, 0:attack, 1:Talk: " DROP
    
    KEY 48 - 0 = IF
        \ Attack branch: Compare d20 roll against enemy strength
        10 c-random 1+ enemy-strength !
        d20 attack-roll !
        
        attack-roll @ . ." vs Enemy Strength: " enemy-strength @ . CR
        
        attack-roll @ enemy-strength @ > IF 
            ." You successfully defeated the enemy." CR
        ELSE 
            ." Defeated in combat! Game Over." CR ABORT 
        THEN
    ELSE
        \ Talk branch: Negotiate peace
        ." You convinced the enemy to leave you alone." CR
    THEN
    CR ." ==========================================" CR ;

\ --- MAIN CLIENT GAMEPLAY EVENT LOOP ---
: the_game-of-trusters-multiplayer-video-game-client
    select-server     ( Run connection setup )
    configure-players ( Run player count configuration )
    
    CR CR ." === ESTABLISHED SESSION: ENTERING WORLD ===" CR
    
    BEGIN
        \ Display player turn header conditionally based on player count mode
        total-players @ 1 > IF
            CR CR active-player @ . ." --- Local Player Turn ---" CR
        ELSE
            CR CR ." --- Player Turn ---" CR
        THEN
        
        \ Server response packet stream read simulation (testing states 1-6)
        6 c-random 1+ 
        DUP 5 < IF 
            DUP 1 SWAP pack-nibbles \ Areas 1 through 4
        ELSE DUP 5 = IF 
            DROP 3 5 pack-nibbles   \ Enemy encounter packet
        ELSE 
            DROP 1 6 pack-nibbles   \ Trap event packet
        THEN THEN

        unpack-nibbles ( Unpack byte into high event code & low payload )
        
        \ --- EVENT & AREA ROUTING SWITCH ---
        CASE
            1 OF
                \ Area 1: 5-way direction choice available
                DROP
                ." What direction do you want to go, Left(1), Right(2), Straight(3), Up(4), Down(5): "
                KEY raw-input !
                \ Strict ASCII input validation (checking numeric bounds 0-9)
                raw-input @ 48 >= raw-input @ 57 <= AND IF
                    raw-input @ 48 - option-var !
                    option-var @ 1 < option-var @ 5 > OR IF
                        CR ." Not an option." CR ABORT
                    THEN
                ELSE
                    CR ." Only single digit integers are allowed." CR ABORT
                THEN
              ENDOF
            2 OF
                \ Area 2: 4-way choice available
                DROP
                ." What direction do you want to go, Left(1), Right(2), Straight(3), Up(4): "
                KEY raw-input !
                raw-input @ 48 >= raw-input @ 57 <= AND IF
                    raw-input @ 48 - option-var !
                    option-var @ 1 < option-var @ 4 > OR IF
                        CR ." Not an option." CR ABORT
                    THEN
                ELSE
                    CR ." Only single digit integers are allowed." CR ABORT
                THEN
              ENDOF
            3 OF
                \ Area 3: 3-way choice available
                DROP
                ." What direction do you want to go, Left(1), Right(2), Straight(3): "
                KEY raw-input !
                raw-input @ 48 >= raw-input @ 57 <= AND IF
                    raw-input @ 48 - option-var !
                    option-var @ 1 < option-var @ 3 > OR IF
                        CR ." Not an option." CR ABORT
                    THEN
                ELSE
                    CR ." Only single digit integers are allowed." CR ABORT
                THEN
              ENDOF
            4 OF
                \ Area 4: 2-way choice available
                DROP
                ." What direction do you want to go, Left(1), Right(2): "
                KEY raw-input !
                raw-input @ 48 >= raw-input @ 57 <= AND IF
                    raw-input @ 48 - option-var !
                    option-var @ 1 < option-var @ 2 > OR IF
                        CR ." Not an option." CR ABORT
                    THEN
                ELSE
                    CR ." Only single digit integers are allowed." CR ABORT
                THEN
              ENDOF
            5 OF
                \ State 5: Enemy Encounter (Low nibble holds Enemy ID 1-10)
                current-enemy-id !
                current-enemy-id @ handle-enemy
                0 option-var !
              ENDOF
            6 OF
                \ State 6: Trap Event
                2DROP handle-trap
                0 option-var !
              ENDOF
            DUP 
                2DROP CR ." [ERROR] Unknown state received." CR ABORT
        ENDCASE

        \ --- POST-ACTION DIRECTION CONFIRMATION SWITCH ---
        option-var @ 0 > IF
            option-var @
            CASE
                1 OF CR ." Your Party went left." CR ENDOF
                2 OF CR ." Your Party went right." CR ENDOF
                3 OF CR ." Your Party went straight." CR ENDOF
                4 OF CR ." Your Party went up." CR ENDOF
                5 OF CR ." Your Party went down." CR ENDOF
                DUP CR ." Not an option." CR ABORT
            ENDCASE
        THEN

        \ --- LOCAL MULTIPLAYER TURN ROTATION QUEUE ---
        \ Only rotates if total-players is configured greater than 1
        total-players @ 1 > IF
            active-player @ 1+ 
            DUP total-players @ > IF DROP 1 THEN
            active-player !
        THEN

    AGAIN ; ( Loop indefinitely )

\ Auto-launch client on file execution
the_game-of-trusters-multiplayer-video-game-client
