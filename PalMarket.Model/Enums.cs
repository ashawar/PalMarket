using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Model
{
    public enum ErrorCode
    {
        ValidationError = 1001,
        SqlGenericError = 2001,
        GeneralError = 3001,
        RedisGenericError = 4001
    }

    public enum PeriodFilter
    {
        ThisYear = 1,
        LastYear = 2,
        LastTwoYears = 3,
        LastFiveYears = 4,
        All = 5,
        Custom = 6,
        Today = 7,
        ThisWeek = 8,
        ThisMonth = 9,
        LastMonth = 10,
    }

    public enum DeviceType
    {
        Android = 1,
        iOS = 2
    }
}
