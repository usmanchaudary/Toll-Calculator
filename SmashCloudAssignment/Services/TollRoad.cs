using Microsoft.AspNetCore.Routing.Constraints;
using SmashCloudAssignment.ApplicationConstants;

namespace SmashCloudAssignment.Services
{
    public class TollRoad
    {
        private Dictionary<string, int> entryPointsDistance;

        public TollRoad()
        {
            InitializeEntryPointsDistance();
        }

        private void InitializeEntryPointsDistance()
        {
            entryPointsDistance = new Dictionary<string, int>();

            // Add distances between entry points
            entryPointsDistance["Zero Point"] = 0;
            entryPointsDistance["NS Interchange"] = 5;
            entryPointsDistance["Ph4 Interchange"] = 10;
            entryPointsDistance["Ferozpur Interchange"] = 17;
            entryPointsDistance["Lake City Interchange"] = 24;
            entryPointsDistance["Raiwand Interchange"] = 29;
            entryPointsDistance["Bahria Interchange"] = 34;

        }
        #region Distance Calculation
        private int CalculateDistanceBetweenEntryPoints(string entryPoint1, string entryPoint2)
        {
            int distance = 0;

            foreach (var point in entryPointsDistance)
            {
                if (point.Key == entryPoint1 || point.Key == entryPoint2)
                {
                    distance += point.Value;
                }
            }

            return Math.Abs(entryPointsDistance[entryPoint1] - entryPointsDistance[entryPoint2]);
        }

        private int GetDistanceBetweenEntryPoints(string entryPoint1, string entryPoint2)
        {
            // Check if both entry points exist
            if (!entryPointsDistance.ContainsKey(entryPoint1) || !entryPointsDistance.ContainsKey(entryPoint2))
            {
                throw new ArgumentException("Invalid entry points.");
            }

            return CalculateDistanceBetweenEntryPoints(entryPoint1, entryPoint2);
        }
        #endregion

        #region Toll Calculation

        // Method to calculate toll
        public (double discount, double baseRate,double distanceBreakDown, double totalToBeCharged) CalculateToll(string entryPoint1, string entryPoint2, DateTime entryDate, string numberPlate)
        {
            // Get distance between entry points
            int distance = GetDistanceBetweenEntryPoints(entryPoint1, entryPoint2);
            Console.WriteLine($"Distance between {entryPoint1} and {entryPoint2} is {distance} km.");
            // Calculate base rate
            double baseRate = TollRate.BaseRate;

            // Apply distance rate
            double distanceRate = TollRate.RatePerKm;
            double totalDistanceRate = distance * distanceRate;

            // Check if it's a weekend
            bool isWeekend = entryDate.DayOfWeek == DayOfWeek.Saturday || entryDate.DayOfWeek == DayOfWeek.Sunday;
            if (isWeekend)
            {
                distanceRate *= TollRate.WeekendRateMultiplier; // Increase distance rate by 1.5x on weekends
                totalDistanceRate = distance * distanceRate;
            }

            // Check for discount based on number plate and day of week
            double discount = 0;
            bool isDiscountDay = IsDiscountDay(entryDate);
            bool isEvenPlate = IsEvenPlate(numberPlate);

            if (IsDiscountDay(entryDate) && isEvenPlate)
            {
                discount = TollRate.EvenNumberDiscount;
            }
            else if ((entryDate.DayOfWeek == DayOfWeek.Tuesday || entryDate.DayOfWeek == DayOfWeek.Thursday) && !isEvenPlate)
            {
                discount = TollRate.OddNumberDiscount;
            }

            // Apply national holiday discount
            if (IsNationalHoliday(entryDate))
            {
                discount = TollRate.HolidayDiscount;
            }

            // Calculate total to be charged after discount
            double totalToBeCharged = (baseRate + totalDistanceRate) * (1 - discount);

            return (discount, baseRate,totalDistanceRate, totalToBeCharged);
        }

        // Helper methods for discount rules
        private bool IsDiscountDay(DateTime entryDate)
        {
            return entryDate.DayOfWeek == DayOfWeek.Monday || entryDate.DayOfWeek == DayOfWeek.Tuesday ||
                   entryDate.DayOfWeek == DayOfWeek.Wednesday || entryDate.DayOfWeek == DayOfWeek.Thursday;
        }

        private bool IsEvenPlate(string numberPlate)
        {
            int lastDigit = int.Parse(numberPlate.Split('-').LastOrDefault());
            return lastDigit % 2 == 0;
        }

        private bool IsNationalHoliday(DateTime entryDate)
        {
            return entryDate.Month == 3 || entryDate.Month == 8 || entryDate.Month == 12;
        }
        #endregion
    }
}
