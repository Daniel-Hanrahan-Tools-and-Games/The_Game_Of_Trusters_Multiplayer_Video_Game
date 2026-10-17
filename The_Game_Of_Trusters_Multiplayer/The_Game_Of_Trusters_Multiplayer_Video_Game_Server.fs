\ =====================================================================
\ The Game of Trusters - Forth MUD/MMO Server Engine
\ Author: Copyright (c) 2026 Daniel Hanrahan Tools and Games
\ License: SPDX-License-Identifier: AGPL-3.0-or-later
\ =====================================================================
\ DESCRIPTION:
\ This server engine runs an event loop that listens for client connection
\ signals, rolls procedural world/encounter states (1 through 6), packs
\ the high-level event code and low-level payloads into an 8-bit byte 
\ using nibble-shifting, and transmits it back to the client.
\ =====================================================================

\ --- PRNG SEED & LINEAR CONGRUENTIAL GENERATOR ---
VARIABLE seed
31337 seed ! ( Initialize random seed with a default prime value )

\ ( n -- u ) Generates a pseudo-random number from 0 to n-1
: random ( n -- u )
    seed @ 1103515245 * 12345 + 2147483647 AND ( LCG algorithm formula )
    dup seed !                               ( Save updated seed back to variable )
    SWAP MOD ;                               ( Modulo to fit within range n )

\ --- WORLD STATE TRACKER ---
VARIABLE current-area

\ --- NIBBLE PACKING PROTOCOL ---
\ ( high low -- byte ) Combines two 4-bit nibbles into a single 8-bit byte
\ High nibble = Event/State Type (1-6)
\ Low Nibble  = Sub-state / Payload Data (e.g., Enemy ID 1-10)
: pack-nibbles ( high low -- byte )
    SWAP 4 LSHIFT OR ;

\ --- SERVER TURN LOGIC ---
\ ( input-nibble -- response-byte )
: process-server-turn ( input-nibble -- response-byte )
    DROP ( Disregard incoming client request byte for stateless roll )
    
    \ Roll a random world state from 1 to 6
    6 random 1+ current-area !

    \ Evaluate state and pack corresponding response packet
    current-area @
    CASE
        1 OF 1 0x1 pack-nibbles ENDOF \ State 1: Area 1 (5 directional choices) -> Hex 0x11
        2 OF 2 0x1 pack-nibbles ENDOF \ State 2: Area 2 (4 directional choices) -> Hex 0x21
        3 OF 3 0x1 pack-nibbles ENDOF \ State 3: Area 3 (3 directional choices) -> Hex 0x31
        4 OF 4 0x1 pack-nibbles ENDOF \ State 4: Area 4 (2 directional choices) -> Hex 0x41
        5 OF 
             \ State 5: Enemy Encounter 
             \ High nibble = 5 (Combat event), Low nibble = Random Enemy ID (1 to 10)
             10 random 1+ 5 swap pack-nibbles 
          ENDOF
        6 OF 6 0x1 pack-nibbles ENDOF \ State 6: Trap Event -> Hex 0x61
        0x00 0x00 pack-nibbles        \ Default fallback NOP packet
    ENDCASE ;

\ --- MAIN SERVER EVENT LISTENER LOOP ---
: the_game-of-trusters-multiplayer-video-game-server
    CR ." [The Game Of Trusters Multiplayer Video Game Server] Active & Listening..." CR
    BEGIN
        KEY                 \ Wait for incoming stream byte from client
        process-server-turn \ Process game logic and generate response packet
        EMIT                \ Transmit response byte back over the stream
    AGAIN ;                 \ Infinite server loop
