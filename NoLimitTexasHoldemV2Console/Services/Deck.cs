using NoLimitTexasHoldemV2.Models;      //To use Card.cs

namespace NoLimitTexasHoldemV2.Services
{
    public class Deck
    {
        //Attributes of a deck include the cards and a random object linked to the deck to be able to shuffle
        private List<Card> cards;
        private Random random;

        public Deck()
        {
            //Instantiating a List object that holds Card objects
            cards = new List<Card>();

            //Instantiating a Random object
            random = new Random();

            //Call this upon instantiation
            InitializeDeck();
        }

        //Created this method to be able to "pick up all the cards" after every hand
        public void InitializeDeck()
        {
            //First making list/deck empty
            cards.Clear();
            
            //Rest is repopulating list/deck
            string[] suits = { "Spades", "Hearts", "Diamonds", "Clubs" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

            foreach (string suit in suits)
            {
                foreach (string rank in ranks)
                {
                    cards.Add(new Card(suit, rank));
                }
            }
        }

        public void Shuffle()
        {
            for(int i=0; i<52; i++)
            {
                //Generate a random number between 0 and i+1 (Including 0 and exclusing i+1)
                int j = random.Next(i + 1); 
                //Using temp method to swap cards an indices i and j
                Card temp = cards[i];       
                cards[i] = cards[j];
                cards[j] = temp;

                //This uses the Fisher-Yates Shuffle, one of the first things that came up for me in my online research
            }
        }

        public Card Deal()
        {
            //Cannot deal an empty deck of cards
            if (cards.Count == 0)
            {
                 return null;
            }
            //Actually get the card before removing so we can return it
            Card card = cards[0];

            //We can always remove at indice 0 since a List always shifts everything to the left and dynamically resizes
            cards.RemoveAt(0);
            return card;
        }
    }
}