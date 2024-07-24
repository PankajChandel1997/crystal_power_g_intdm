using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class TODDto
    {
        public int Number { get; set; }
        public string MeterNo { get; set; }

        public string ActiveCalenderName { get; set; }
        public string ActiveDayProfileStartTime { get; set; }
        public string ActiveDayProfileScript { get; set; }
        public string ActiveDayProfileSelector { get; set; }

        public string PassiveCalenderName { get; set; }
        public string PassiveDayProfileStartTime { get; set; }
        public string PassiveDayProfileScript { get; set; }
        public string PassiveDayProfileSelector { get; set; }

        public string CreatedOn { get; set; }
    }
}