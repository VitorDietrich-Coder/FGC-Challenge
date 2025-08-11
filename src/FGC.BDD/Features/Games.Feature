Feature: Game management

  Covers game creation and retrieval scenarios via the GamesController.

  Scenario: Create a game successfully
    Given the game has title "Cyberpunk 2077"
    And the genre "RPG"
    And the price 59.99
    When the user requests to create the game
    Then the result should contain the created game
    And the game title should be "Cyberpunk 2077"

  Scenario: Get all games successfully
    When the user requests to get all games
    Then the result should contain a list of games

  Scenario: Get game by valid ID
    Given the game ID is 1
    When the user requests the game by ID
    Then the result should contain a game with ID 1

  Scenario: Get game by invalid ID
    Given the game ID is 999
    When the user requests the game by ID
    Then an exception should be thrown
