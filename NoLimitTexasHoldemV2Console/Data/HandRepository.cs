using NoLimitTexasHoldemV2.Models;      //To use HandData.cs
using Microsoft.EntityFrameworkCore;    //To use things like DbContext
using System.Text.Json;                 //To use JsonSerializer

namespace NoLimitTexasHoldemV2.Data
{
    public class HandRepository : IHandRepository
    {
        //Attributes
        public string _path = "";
        public string connectionString = "";
        //One shared PokerContext instance across all instances of HandRepository.
        static PokerContext context;
        
        //Constructor is paramaterized so that I can differentiate whether I am doing the Repository pattern version
        //or the Entity Framework version. 1 is Repository Pattern, 2 is Entity Framework
        public HandRepository(string PathOrSC, int Version)
        {
            if(Version == 1)
            {
                //Simply set file path
                _path = PathOrSC;
            }

            else if(Version == 2)
            {
                //Set connection string
                connectionString = PathOrSC;
                //Configure DbContext options to use SQL server with the connection string. The .Options part 
                //finalizes the configuration and returns a DbContextOptions<PokerContext> object.
                DbContextOptions<PokerContext> options;
                options = new DbContextOptionsBuilder<PokerContext>()
                    .UseSqlServer(connectionString)
                    .Options;
                //Set context to be a PokerContext object using options
                context = new PokerContext(options);
            }
        }

        public void SaveHandData(HandData handData)
        {
            List<HandData> handDataList = new List<HandData>();
            if (File.Exists(_path))
            {
                string existingData = File.ReadAllText(_path);
                //Deserialize json string into a list of HandData objects
                handDataList = JsonSerializer.Deserialize<List<HandData>>(existingData);
            }

            //Add HandData object that was passed in as a parameter into the list
            handDataList.Add(handData);

            //Convert handDataList into a JSON-formatted string
            string jsonData = JsonSerializer.Serialize(handDataList, new JsonSerializerOptions {WriteIndented = true});

            //Write the json string into the file
            File.WriteAllText(_path, jsonData);

            //Entity framework version, simply add to context then save to database
            context.Hands.Add(handData);
            context.SaveChanges();
        }

        public void ReadHandHistoryRepository()
        {
            if (!File.Exists(_path))
            {
                Console.WriteLine("Hand history file does not exist or could not be found.");
                return;
            }

            string jsonString = File.ReadAllText(_path);

            if (string.IsNullOrWhiteSpace(jsonString))
            {
                Console.WriteLine("Hand history file is empty.");
                return;
            }

            //Create a list for HandData from json file to go into
            List<HandData> hands;

            //Deserialize the json string into the list, otherwise thrown an exception
            try
            {
                hands = JsonSerializer.Deserialize<List<HandData>>(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to deserialize hand history file: {ex.Message}");
                return;
            }

            //If list is null or empty, output no history found
            if (hands == null || !hands.Any())
            {
                Console.WriteLine("No hand history found in the file.");
                return;
            }

            foreach (var hand in hands)
            {
                //Skip entries with default values
                if (hand.HandDateTime == default 
                    && hand.PlayerInitialStack == 0 
                    && hand.MachineInitialStack == 0 
                    && string.IsNullOrWhiteSpace(hand.Outcome)
                    && (hand.PlayerHoleCards == null || !hand.PlayerHoleCards.Any())
                    && (hand.MachineHoleCards == null || !hand.MachineHoleCards.Any())
                    && (hand.CommunityCards == null || !hand.CommunityCards.Any()))
                {
                    continue;
                }

                Console.WriteLine(hand.ToString());
                Console.WriteLine("--------------------------------------------------");
            }
        }

        public IEnumerable<HandData> ReadHandHistoryDB()
        {
            //Get all HandData records that include the below entities, then convert to a list
            List<HandData> hands = context.Hands.Include(h => h.PlayerHoleCards)
                                                .Include(h => h.MachineHoleCards)
                                                .Include(h => h.CommunityCards)
                                                .ToList();

            if (hands.Count == 0)
            {
                Console.WriteLine("No hand history found.");
                return new List<HandData>();
            }

            foreach (HandData hand in hands)
            {
                Console.WriteLine(hand.ToString());
            }
            return hands;
        }

        public void DeleteAllHandHistory()
        {
            //Get all HandData records
            List<HandData> hands = context.Hands
                .Include(h => h.PlayerHoleCards)
                .Include(h => h.MachineHoleCards)
                .Include(h => h.CommunityCards)
                .ToList();

            foreach (var hand in hands)
            {
                // Remove the related cards first
                context.Cards.RemoveRange(hand.PlayerHoleCards);
                context.Cards.RemoveRange(hand.MachineHoleCards);
                context.Cards.RemoveRange(hand.CommunityCards);
            }

            // Remove the HandData records
            context.Hands.RemoveRange(hands);
            context.SaveChanges();

            // Clear the JSON file by writing an empty array
            File.WriteAllText(_path, "[]");
        }
    }
}