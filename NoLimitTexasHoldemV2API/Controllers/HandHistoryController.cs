using Microsoft.AspNetCore.Mvc;         //To define the controller and the route
using NoLimitTexasHoldemV2.Data;        //To use HandRepository.cs
using NoLimitTexasHoldemV2.Models;      //To use HandData.cs          

namespace NoLimitTexasHoldemV2API.Controllers
{
    [ApiController]                 //Defines class as API controller     
    [Route("[controller]")]         
    public class HandController : ControllerBase
    {
        private readonly ILogger<HandController> _logger;
        private readonly IHandRepository _handRepository;       //Dependency injection for hand repository

        public HandController(ILogger<HandController> logger, IHandRepository handRepository)
        {
            _logger = logger;
            _handRepository = handRepository;
        }

        [HttpGet]
        public IEnumerable<HandData> GetEntireHandHistory()
        {
            return _handRepository.ReadHandHistoryDB();
        }
    }
}
