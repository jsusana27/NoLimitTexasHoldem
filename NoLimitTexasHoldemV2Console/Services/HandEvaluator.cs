using NoLimitTexasHoldemV2.Enums;       //For HandRank.cs
using NoLimitTexasHoldemV2.Models;      //For Card.cs

namespace NoLimitTexasHoldemV2.Services
{
    public class HandEvaluator
    {
        public HandRank EvaluateHand(List<Card> hand, out List<int> highCards)
        {
            //Groups by rank (Ace, queen, etc.), then puts in descending order by number of cards in each group, 
            //then puts groups in descending order by rank, then converts to a list
            var groupedByRank = hand.GroupBy(card => card.Rank)
                                    .OrderByDescending(group => group.Count())
                                    .ThenByDescending(group => group.Key)
                                    .ToList();

            //Groups by suit (Spades, hearts, etc.), then puts in descending order by number of cards in each group
            //the converts to a list
            var groupedBySuit = hand.GroupBy(card => card.Suit)
                                    .OrderByDescending(group => group.Count())
                                    .ToList();
            
            //For both above, end data type is List<IGrouping<int, Card>>

            //Creating a list of high cards to help deal with tiebreakers
            highCards = new List<int>();

            //Checking for straight flush
            if (IsStraightFlush(hand, out var straightFlushHighCards))
            {
                //Take 5 cards from what comes out of IsStraightFlush method
                highCards = straightFlushHighCards.Take(5).ToList();

                return HandRank.StraightFlush;
            }

            //Checking for Quads
            if (groupedByRank[0].Count() == 4)
            {
                //Taking the cards grouped by rank, adding the first group which is quads, then adding the second
                //group which is a single card
                highCards.AddRange(groupedByRank[0].Select(card => RankToInt(card.Rank)));
                highCards.AddRange(groupedByRank[1].Select(card => RankToInt(card.Rank)));
                return HandRank.Quads;
            }

            //Checking for Full House
            if (groupedByRank[0].Count() == 3 && groupedByRank[1].Count() == 2)
            {
                //Taking the cards grouped by rank, adding the first group which is trips, then adding the second
                //group which is a pair
                highCards.AddRange(groupedByRank[0].Select(card => RankToInt(card.Rank)));
                highCards.AddRange(groupedByRank[1].Select(card => RankToInt(card.Rank)));
                return HandRank.FullHouse;
            }

            //Checking for Flush
            if (groupedBySuit[0].Count() >= 5)
            {
                //Taking the cards grouped by suit, converting them to integers, put in descending order,
                //making sure we only take 5, then converting to list
                highCards = groupedBySuit[0].Select(card => RankToInt(card.Rank))
                                            .OrderByDescending(rank => rank)
                                            .Take(5)
                                            .ToList();
                return HandRank.Flush;
            }
            
            //Checking for Straight
            if (IsStraight(hand, out var straightHighCards))
            { 
                //Take 5 cards from what comes out of IsStraight method
                highCards = straightHighCards.Take(5).ToList();

                return HandRank.Straight;
            }

            //Checking for Trips
            if (groupedByRank[0].Count() == 3)
            {
                //Convert each card in 1st group to integer, then adds results to highCards list
                highCards.AddRange(groupedByRank[0].Select(card => RankToInt(card.Rank)));
                //Skip the group that we already added, convert the remaining groups/cards into integers,
                //flatten into one sequence, put into descending order, make sure we only take 2 cards,
                //then add to highCards list
                highCards.AddRange(groupedByRank.Skip(1)
                         .SelectMany(group => group.Select(card => RankToInt(card.Rank)))
                         .OrderByDescending(rank => rank)
                         .Take(2));
                         
                return HandRank.Trips;
            }

            //Checking for Two Pair
            if (groupedByRank[0].Count() == 2 && groupedByRank[1].Count() == 2)
            {
                //Convert each card in each group to integer, than add to highCards list. Already in order since
                //grouped by rank
                highCards.AddRange(groupedByRank[0].Select(card => RankToInt(card.Rank)));
                highCards.AddRange(groupedByRank[1].Select(card => RankToInt(card.Rank)));
                //Skip the 2 pairs we already added, convert to integer, flatten into one sequence, put in descending order,
                //then make sure we only take one
                highCards.AddRange(groupedByRank.Skip(2)
                                    .SelectMany(group => group.Select(card => RankToInt(card.Rank)))
                                    .OrderByDescending(rank => rank)
                                    .Take(1));
                return HandRank.TwoPair;
            }
            
            //Checking for One Pair
            if (groupedByRank[0].Count() == 2)
            {
                //Convert each card in 1st group (the pair) to integer, then adds results to highCards list
                highCards.AddRange(groupedByRank[0].Select(card => RankToInt(card.Rank)));
                //Skip the pair that we already added, convert the remaining groups/cards into integers,
                //flatten into one sequence, put into descending order, make sure we only take 3 cards total,
                //then add to highCards list
                highCards.AddRange(groupedByRank.Skip(1)
                                                .SelectMany(group => group.Select(card => RankToInt(card.Rank)))
                                                .OrderByDescending(rank => rank)
                                                .Take(3));
                return HandRank.OnePair;
            }
            
            //If we reach here, then must be high card
            //Taking the cards grouped by rank, converting them to integers, then flattening them into one sequence, then
            //putting them into descending order, making sure we only take 5 cards total (since only 5 best cards), 
            //then converting to a list
            highCards = groupedByRank.SelectMany(group => group.Select(card => RankToInt(card.Rank)))
                                     .OrderByDescending(rank => rank)
                                     .Take(5)
                                     .ToList();
            return HandRank.HighCard;

            //Keep in mind don't use else if since return keyword exits the method anyway
        }

        //Method to convert card rankings to an int system
        public int RankToInt(string rank)
        {
            if(rank == "2") return 2;
            else if(rank == "3") return 3;
            else if(rank == "4") return 4;
            else if(rank == "5") return 5;
            else if(rank == "6") return 6;
            else if(rank == "7") return 7;
            else if(rank == "8") return 8;
            else if(rank == "9") return 9;
            else if(rank == "10") return 10;
            else if(rank == "J") return 11;
            else if(rank == "Q") return 12;
            else if(rank == "K") return 13;
            else if(rank == "A") return 14;
            else throw new ArgumentException("Invalid card rank");
        }

        //Created a method to check for straight so that above looks neater, more "methodical"
        public bool IsStraight(List<Card> hand, out List<int> highCards)
        {
            highCards = new List<int>();
            
            //Transforms each card object into just the rank attribute, removes duplicates, converts to int, 
            //then sorts in ascending order, then converts to a lsit
            var orderedByRank = hand.Select(card => card.Rank)
                                   .Distinct()
                                   .Select(RankToInt)
                                   .OrderBy(rank => rank)
                                   .ToList();

            //Since already ordered numerically and removes duplicates, if first element of list plus 4 equals the 5th
            //element of the list, then we have a straight
            for (int i = 0; i <= orderedByRank.Count - 5; i++)
            {
                if (orderedByRank[i] + 4 == orderedByRank[i + 4])
                {
                    //Using Skip(i) and Take(5) to make sure we only take cards from straight into highCards
                    highCards.AddRange(orderedByRank.Skip(i).Take(5));

                    return true;
                }
            }
            //We have a special if-statement for a low straight since A=14 in my ranked int system
            if (orderedByRank.Contains(14) && orderedByRank.Contains(2) &&
                orderedByRank.Contains(3) && orderedByRank.Contains(4) &&
                orderedByRank.Contains(5))
            {
                highCards.AddRange(new List<int> { 5, 4, 3, 2, 14 });
                return true;
            }
            return false;
        }

        //Created a method to check for straight flush so that above looks neater, more "methodical"
        public bool IsStraightFlush(List<Card> hand, out List<int> highCards)
        {
            highCards = new List<int>();
            
            //Grouping by suit, discarding groups with < 5 since a straight flush needs at least a flush first, 
            //then converting to a list
            var groupedBySuit = hand.GroupBy(card => card.Suit)
                                    .Where(group => group.Count() >= 5)
                                    .ToList();

            //For each group in the list, if it passes the IsStraight test, then the hand is a straight flush
            foreach (var group in groupedBySuit)
            {
                if (IsStraight(group.ToList(), out var straightFlushHighCards))
                {
                    //Take 5 cards from what comes out of IsStraight method
                    highCards = straightFlushHighCards.Take(5).ToList();
                    return true;
                }
            }
            return false;
        }
    }
}