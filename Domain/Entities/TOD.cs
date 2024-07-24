using Domain.Interface;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TOD : Entity<int>, ITrackCreated, ITrackUpdated
    {
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
        public int CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
