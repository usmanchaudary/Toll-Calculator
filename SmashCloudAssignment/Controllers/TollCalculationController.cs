using Microsoft.AspNetCore.Mvc;
using SmashCloudAssignment.DTO;
using SmashCloudAssignment.Services;

namespace SmashCloudAssignment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TollCalculationController : ControllerBase
    {
        private static List<EntryData> entryDataList = new List<EntryData>();
        private readonly TollRoad _tollRoad;
        public TollCalculationController(TollRoad tollRoad)
        {
            _tollRoad = tollRoad;
        }


        [HttpPost("EntryPointData")]
        public async Task<IActionResult> EntryData([FromBody] EntryData entryData) //EntryData --> InterChange :string, NumberPlate:string, DateTime:string
        {
            entryDataList.Add(entryData);
            return Ok(entryData);
        }



        [HttpPost("ExitPointData")]
        public async Task<IActionResult> ExitData([FromBody] ExitData exitData) //as entry data and exit data are same, we can use same DTO
        {
            var entryData = entryDataList.FirstOrDefault(x => x.NumberPlate == exitData.NumberPlate);
            if (entryData == null)
            {
                return BadRequest("Invalid Number Plate");
            }
            var (discount, baseRate, distanceBreakDown, totalToBeCharged) = CalculateDistanceAmongPoints(entryData.InterChange, exitData.InterChange, entryData.DateTime, exitData.NumberPlate);
            return Ok(new { baseRate, discount, distanceBreakDown, totalToBeCharged });
        }


        #region Helper Methods

        private (double discount, double baseRate, double distanceBreakDown, double totalToBeCharged) CalculateDistanceAmongPoints(string entryPoint1, string exitPoint, DateTime entryDate, string plateNumber)
        {
            
            return _tollRoad.CalculateToll(entryPoint1, exitPoint, entryDate, plateNumber);
        }

        #endregion
    }
}
