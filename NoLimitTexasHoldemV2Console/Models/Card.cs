using System.ComponentModel.DataAnnotations;        //To use [Key]

namespace NoLimitTexasHoldemV2.Models
{
    public class Card
    {
        
        //Attributes/columns in Card table, keep in mind PK's and nullables
        [Key]
        public int CardId { get; set; }
        public string Rank { get; set; } = string.Empty;
        public string Suit { get; set; } = string.Empty;

        public int? PlayerHandDataId { get; set; }
        public HandData? PlayerHandData { get; set; }
        public int? MachineHandDataId { get; set; }
        public HandData? MachineHandData { get; set; }
        public int? CommunityHandDataId { get; set; }
        public HandData? CommunityHandData { get; set; }
        
        //Constructor that specifies exact card
        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
        }

        //To have a string representation for the object, overrode the method to read better for the user
        //Would otherwise just be "NoLimitTexasHoldem.Card"
        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}