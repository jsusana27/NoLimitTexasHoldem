using NoLimitTexasHoldemV2.Data;                //To use HandRepository.cs
using NoLimitTexasHoldemV2.Enums;               //To use HandRank.cs
using NoLimitTexasHoldemV2.Models;              //To use Card.cs and HandData.cs
using NoLimitTexasHoldemV2.Services;            //To use Deck.cs and HandEvaluator.cs
using Microsoft.Extensions.Configuration;       //To use ConfigurationBuilder()
using System.Text.Json;                         //To use JsonSerializer
using System.Text.Json.Serialization;           //Tp use JsonSerializerOptions and ReferenceHandler


namespace NoLimitTexasHoldemV2
{
    class Program
    {
        //Creating object for sending HTTP requests and receiving HTTP responses, static for singleton
        static HttpClient client = new HttpClient {BaseAddress = new Uri("http://localhost:5271/")};
        
        static void Main(string[] args)
        {
            string connectionstring = "";

            //Create a ConfigurationBuilder object, add user secrets for the connection string, the build
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
                
            connectionstring = configuration.GetConnectionString("PokerDatabaseConnection");
            IHandRepository file = new HandRepository(connectionstring, 2);
            
            //Creating a HandRepository object
            IHandRepository handRepository = new HandRepository("hand_history.json", 1);

            //Instantiating a new deck
            Deck deck = new Deck();

            //Instantiating a HandEvaluator object
            HandEvaluator handEvaluator = new HandEvaluator();

            while (true)
            {
                //Clear window, then give options
                Console.Write("\u001b[2J\u001b[H");
                Console.WriteLine("No Limit Texas Hold'em\n");
                Console.WriteLine("Select an option: ");
                Console.WriteLine("1. View Hand History");
                Console.WriteLine("2. Delete Hand History");
                Console.WriteLine("3. Play");
                Console.WriteLine("4. Exit");

                string input = Console.ReadLine();
                
                if (input == "1")
                {
                    ViewHandHistory(handRepository);
                }
                else if (input == "2")
                {
                    DeleteHandHistory(handRepository);
                }
                else if (input == "3")
                {
                    PlayGame(handRepository, deck, handEvaluator);
                }
                else if (input == "4")
                {
                    return;
                }     
                else
                {
                    Console.WriteLine("Invalid option. Please try again.");
                }
            }
        }

        static void ViewHandHistory(IHandRepository handRepository)
        {
            //Clear window, show hand history
            Console.Write("\u001b[2J\u001b[H");
            handRepository.ReadHandHistoryRepository();
            handRepository.ReadHandHistoryDB();
            ViewHandHistoryAPI().Wait();
            Console.WriteLine("Press Enter to return to the main menu...");
            Console.ReadLine();
        }

        static void DeleteHandHistory(IHandRepository handRepository)
        {
            //Clear window, ask if sure, then perform actions and/or output to console accordingly
            Console.Write("\u001b[2J\u001b[H");
            Console.WriteLine("Are you sure you want to delete all hand history? (y/n)");
            string inputDeleteHistory = Console.ReadLine();
            if (inputDeleteHistory.ToLower() == "y")
            {
                handRepository.DeleteAllHandHistory();
                Console.WriteLine("Hand history deleted.");
            }
            else
            {
                Console.WriteLine("Hand history not deleted.");
            }
            Console.WriteLine("Press Enter to return to the main menu...");
            Console.ReadLine();
        }

        static void PlayGame(IHandRepository handRepository, Deck deck, HandEvaluator handEvaluator)
        {
            //Declaring and initializing all player stacks
            int playerStack = 10;
            int machineStack = 10;
            int playerBet;

            //Flag for tiebreakers
            bool handOver = false;

            while (true)
            {
                //Save initial stacks before the hand starts
                int initialPlayerStack = playerStack;
                int initialMachineStack = machineStack;

                //At start of every hand, clear window and output player stacks
                Console.Write("\u001b[2J\u001b[H");
                Console.WriteLine($"Player stack: {playerStack}");
                Console.WriteLine($"Machine stack: {machineStack}");

                //Initialize and shuffle deck at start of each hand
                deck.InitializeDeck();
                deck.Shuffle();

                //Dealing hole cards to player and machine
                Card playerCard1 = deck.Deal();
                Card machineCard1 = deck.Deal();
                Card playerCard2 = deck.Deal();
                Card machineCard2 = deck.Deal();

                //Outputting what the player and machine's hole cards are
                Console.WriteLine($"You have {playerCard1} and {playerCard2}");

                //Dealing flop, turn, and river in way it would be done in real life
                Card burnCard1 = deck.Deal();
                Card communityCard1 = deck.Deal();
                Card communityCard2 = deck.Deal();
                Card communityCard3 = deck.Deal();
                Card burnCard2 = deck.Deal();
                Card communityCard4 = deck.Deal();
                Card burnCard3 = deck.Deal();
                Card communityCard5 = deck.Deal();

                Console.WriteLine($"The community cards are \n {communityCard1} \n {communityCard2} \n {communityCard3} \n {communityCard4} \n {communityCard5}");

                Console.WriteLine("Enter your bet (at least 1, whole numbers only):");
                //While loop is input validation
                while (!int.TryParse(Console.ReadLine(), out playerBet) || playerBet < 1 || playerBet > playerStack)
                {
                    Console.WriteLine("Invalid bet. Enter a value between 1 and your current stack.");
                }

                playerStack -= playerBet;
                machineStack -= playerBet;

                //Combining hole cards with community cards for both players
                List<Card> playerHand = new List<Card> { playerCard1, playerCard2, communityCard1, communityCard2, communityCard3, communityCard4, communityCard5 };
                List<Card> machineHand = new List<Card> { machineCard1, machineCard2, communityCard1, communityCard2, communityCard3, communityCard4, communityCard5 };

                //Evaluating each player's hand
                HandRank playerHandRank = handEvaluator.EvaluateHand(playerHand, out List<int> playerHighCards);
                HandRank machineHandRank = handEvaluator.EvaluateHand(machineHand, out List<int> machineHighCards);

                //Outputting machine's hole cards
                Console.WriteLine($"Machine has {machineCard1} and {machineCard2}");

                //Outputting hand ranks
                Console.WriteLine($"Player has {playerHandRank}");
                Console.WriteLine($"Machine has {machineHandRank}");

                //Since enums are assigned integers, and I implemented HandRank as an enum, these conditional statements work
                string outcome = "";
                if (playerHandRank > machineHandRank)
                {
                    outcome = "Player wins the hand!";
                    Console.WriteLine(outcome);
                    playerStack += playerBet * 2;
                }
                else if (machineHandRank > playerHandRank)
                {
                    outcome = "Machine wins the hand.";
                    Console.WriteLine(outcome);
                    machineStack += playerBet * 2;
                }
                //If both players have the same hand rank...
                else
                {
                    //Iterate through player's list of high cards
                    for (int i = 0; i < playerHighCards.Count; i++)
                    {
                        //If high cards determines tiebreaker, output to user the result, adjust stacks accordingly, 
                        //set handOver flag, and break from loop
                        if (playerHighCards[i] > machineHighCards[i])
                        {
                            outcome = "Player wins the hand!";
                            Console.WriteLine(outcome);
                            playerStack += playerBet * 2;
                            handOver = true;
                            break;
                        }
                        if (playerHighCards[i] < machineHighCards[i])
                        {
                            outcome = "Machine wins the hand.";
                            Console.WriteLine(outcome);
                            machineStack += playerBet * 2;
                            handOver = true;
                            break;
                        }
                    }

                    //If the hand really is a chop after looking at the high cards...
                    if (!handOver)
                    {
                        outcome = "Chop it up.";
                        Console.WriteLine(outcome);
                        playerStack += playerBet;
                        machineStack += playerBet;
                    }
                }

                //Creating a handData object for each hand
                HandData handData = new HandData
                {
                    HandDateTime = DateTime.Now,
                    PlayerInitialStack = initialPlayerStack,
                    MachineInitialStack = initialMachineStack,
                    Bet = playerBet,
                    PlayerHoleCards = new List<Card> { playerCard1, playerCard2 },
                    MachineHoleCards = new List<Card> { machineCard1, machineCard2 },
                    CommunityCards = new List<Card> { communityCard1, communityCard2, communityCard3, communityCard4, communityCard5 },
                    PlayerHandRank = playerHandRank,
                    MachineHandRank = machineHandRank,
                    Outcome = outcome
                };

                //Saving that hand into the text file
                handRepository.SaveHandData(handData);

                //If either player busts, the game is over
                if (playerStack <= 0)
                {
                    Console.WriteLine("You busted! Game over.");
                    Console.WriteLine("Press Enter to return to the main menu...");
                    Console.ReadLine();
                    break;
                }
                if (machineStack <= 0)
                {
                    Console.WriteLine("Machine busted! You win!!!");
                    Console.WriteLine("Press Enter to return to the main menu...");
                    Console.ReadLine();
                    break;
                }

                // Wait for player to hit enter to proceed to the next hand
                Console.WriteLine("Press Enter to continue to the next hand...");
                Console.ReadLine();
            }
        }

        static async Task ViewHandHistoryAPI()
        {
            //Creating a client handler object
            HttpClientHandler handler = new HttpClientHandler
            {
                //Bypassing SSL certificate validation just to get started
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            //Using client handler to bypass SSL (passing handler to constructor)
            using HttpClient client = new HttpClient(handler)
            {
                BaseAddress = new Uri("http://localhost:5271")
            };

            try
            {
                //Store response from GET request
                HttpResponseMessage response = await client.GetAsync("Hand");

                //This method will throw an exception if response status code indicates an error
                response.EnsureSuccessStatusCode();

                //Store then output response
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response JSON:");
                Console.WriteLine(responseContent);

                //Configuring how json will be deserialized
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                //Deserialize into collection of HandData objects
                IEnumerable<HandData> handHistory = JsonSerializer.Deserialize<IEnumerable<HandData>>(responseContent, options);

                //If collection is null/empty, output accordingly
                if (handHistory == null || !handHistory.Any())
                {
                    Console.WriteLine("No hand history found.");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }
    }
}   