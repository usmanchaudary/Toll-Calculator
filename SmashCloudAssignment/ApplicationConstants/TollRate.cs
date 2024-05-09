using System;

namespace SmashCloudAssignment.ApplicationConstants
{
    public class TollRate
    {
        public const int BaseRate = 20;
        public const double RatePerKm = 0.2;
        public const double WeekendRateMultiplier = 1.5;
        public const double EvenNumberDiscount = 0.1;
        public const double OddNumberDiscount = 0.1;
        public const double HolidayDiscount = 0.5;
        //public static List<DateTime> Holidays = new List<DateTime>
        //{
        //    new DateTime(DateTime.Now.Year, 3, 23),
        //    //14 august of every year
        //    new DateTime(DateTime.Now.Year, 8, 14),
        //    //25 december of every year
        //    new DateTime(DateTime.Now.Year, 12, 25)
        //};
    }
}
