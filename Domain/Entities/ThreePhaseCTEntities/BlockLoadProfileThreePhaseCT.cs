using Domain.Interface;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ThreePhaseCTEntities
{
    public class BlockLoadProfileThreePhaseCT : Entity<int>, ITrackCreated, ITrackUpdated
    {
        public string MeterNo { get; set; }
        public string RealTimeClock { get; set; }
        public string CurrentR { get; set; }
        public string CurrentY { get; set; }
        public string CurrentB { get; set; }
        public string VoltageR { get; set; }
        public string VoltageY { get; set; }
        public string VoltageB { get; set; }
        public string BlockEnergykWhImport { get; set; }
        public string BlockEnergykVAhImport { get; set; }
        public string BlockEnergykWhExport { get; set; }
        public string BlockEnergykVAhExport { get; set; }
        public string MeterHealthIndicator { get; set; }
        //25-06-2024
        public string CumulativeEnergykvarhQI { get; set; }
        public string CumulativeEnergykvarhQII { get; set; }
        public string CumulativeEnergykvarhQIII { get; set; }
        public string CumulativeEnergykvarhQIV { get; set; }

        public string CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
