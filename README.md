# No Limit Texas Hold'em Poker Game - Version 2

## Overview

This project is the second version of a No Limit Texas Hold'em poker game, enhanced with a RESTful API and a console-based user interface. The game now supports persistent data storage and offers a more modular and testable codebase through the implementation of dependency injection. This version is designed to provide a robust and flexible platform for further expansion and integration with other applications.

## Features

- **Card Class**: Represents the data structure of a card, including suit and rank.
- **Deck Class**: Handles business logic related to deck initialization, card shuffling, and dealing.
- **HandRank Enum**: Provides an easy way to compare poker hand rankings.
- **HandEvaluator Class**: Implements game logic to evaluate poker hands, such as royal flushes and four of a kind.
- **Game Logic**: Includes betting mechanics, dealing hole cards, and managing community cards within the console application.
- **API Layer**: RESTful API endpoints for interacting with the game logic, allowing external applications to integrate with the game.
- **Database Integration**: Persistent storage for game states, player profiles, and statistics using Entity Framework Core.
- **Dependency Injection**: Utilized throughout the project to enhance modularity, testability, and maintainability.
- **Console Application**: A text-based interface for playing the game and interacting with the game logic.

## Technologies Used

- **C#**: Core programming language used for the project.
- **.NET Core**: Framework used for building the API and console application.
- **Entity Framework Core**: ORM used for database interactions.
- **ASP.NET**: Framework used for building scalable and secure web applications and APIs.
- **Visual Studio**: Integrated development environment (IDE) for coding, building, and debugging.

## Getting Started

### Prerequisites

- Install **Visual Studio 2019** or later.
- Ensure **.NET Core SDK** is installed on your machine.
- Set up a **SQL Server** database or modify the connection string in `appsettings.json` to match your preferred database.

### Setup

1. Clone the repository:
    ```bash
    git clone https://github.com/240708-NET/JoshuaSusana
    ```
2. Open the solution file (`NoLimitTexasHoldemV2.sln`) in Visual Studio.
3. Build the solution to restore NuGet packages and compile the project.
4. Apply database migrations to set up the database schema:
    ```bash
    dotnet ef database update --project NoLimitTexasHoldemV2Console
    ```
5. Run the API project from Visual Studio or via the command line:
    ```bash
    dotnet run --project NoLimitTexasHoldemV2API
    ```
6. Optionally, run the console application:
    ```bash
    dotnet run --project NoLimitTexasHoldemV2Console
    ```

## Usage

### API Usage

- Access the API at `http://localhost:5000` (or your configured port).
- Use tools like Postman or `curl` to interact with the API endpoints.

### Console Application

- Run the console application to play No Limit Texas Hold'em.
- Follow the on-screen prompts for betting, folding, or checking.
- The game state and player actions are persisted to the database.

## Contributors

- **Joshua Susana** - Developer
