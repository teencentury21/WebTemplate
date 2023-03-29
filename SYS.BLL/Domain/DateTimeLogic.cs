using SYS.BLL.Base;
using SYS.DAL.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYS.BLL.Domain
{
    public interface IDateTimeLogic : IDataDrivenLogic
    {
        // Logic

        // Repository                

        // Functions
        DateTime GetCurrentTime();
        string ConvertDateTimeString(DateTime datetime);
        string GetWeekCode(DateTime currentDate, DayOfWeek firstDayOfWeek);
    }
    class DateTimeLogic : DataDrivenLogic, IDateTimeLogic
    {
        public DateTimeLogic(IBusinessLogicFactory BusinessLogicFactory, IRepositoryFactory RepositoryFactory = null) : base(BusinessLogicFactory, RepositoryFactory)
        {            
            
        }
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
        public string ConvertDateTimeString(DateTime datetime)
        {
            return datetime.ToString("yyyy/MM/dd HH:mm:ss.fff");
        }
        public string GetWeekCode(DateTime currentDate, DayOfWeek firstDayOfWeek)
        {
            var weekOfYear = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(currentDate, CalendarWeekRule.FirstDay, firstDayOfWeek);

            return weekOfYear.ToString("00");
        }
    }
}
