# Unity-Blackjack-Poker
This repository contains a PC-based software combining two classic card games: Blackjack and Video Poker (Jacks or Better). Developed for Arrow International, it serves the purpose of showcasing game development skills and techniques using Unity for review.


README for Unity-Blackjack-Poker
How to Play
Main Menu Controls
Intro Skip: A 9-second intro can be skipped by clicking anywhere or pressing any key.
Navigation: Utilize click controls for navigation. Buttons are provided at the bottom to add money to your total. Use the exit button to quit the game.
Game Selection: Click on the poker button to play Jacks or Better, or the blackjack button to engage in a game of blackjack.
Blackjack Rules
In Blackjack, the goal is to beat the dealer's hand without going over 21. Each player starts with two cards, one of the dealer's cards is hidden until the end.

Blackjack Controls
Gameplay: Click the hit button to draw another card, or stand to keep your current hand value. The menu button allows you to pause the game, resume, or return to the main menu.
Poker (Jacks or Better) Rules
Jacks or Better is a variant of five-card draw poker. Players win by having at least a pair of Jacks or better. After the first draw, players can choose to hold or draw new cards to replace the ones they discard.

Poker Controls
Betting: Click the bet 1 button to cycle through betting 1-5 dollars, or the bet 5 button for a maximum bet of 5 dollars. The current bet is displayed on-screen.
Drawing Cards: After betting, press the deal button to draw the first five cards. Cards can be clicked to hold. Press the draw button to replace unheld cards and see the round's result.

Known Bugs

Main Menu

Intro Delay: The intro video delays, briefly revealing the main menu.

Blackjack
Stand Bug: After busting, hitting the stand button deals two cards at a time, causing a loop of drawing cards if the player busts with the first four cards.
1) It is possioble, though unlikely, that the same card(s) can be drawn after the deck is empty, then re-initialialized

Poker
Visual Effects: Missing visual effects, including visible cards at the start or during play. Instant actions without delay for the draw and deal functions. Cards do not disappear until the deal button is pressed again.

Missing Features
1) Sound Effects: No sound effects across all games.
2) Money Mechanics: Games can be played without money, allowing for negative totals in poker.
3) Blackjack dealer will not autowin/draw if they have blackjack
