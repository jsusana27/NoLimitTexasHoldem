using NoLimitTexasHoldemV2.Models;      //To use HandData.cs

namespace NoLimitTexasHoldemV2.Data
{
    public interface IHandRepository
    {
        //Create rule
        void SaveHandData(HandData handData);
        //Read rules. One for the Repository Pattern, one for Entity Framework

        void ReadHandHistoryRepository();
        IEnumerable<HandData> ReadHandHistoryDB();

        //Delete rule
        void DeleteAllHandHistory();
    }
}