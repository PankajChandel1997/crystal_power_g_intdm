﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs.EventDTOs
{
    public class OtherEventSinglePhaseDTO
    {
        public int Number { get; set; }
        public string CreatedOn { get; set; }
        public string MeterNo { get; set; }
        public string DateandTimeofEvent { get; set; }
        //public string EventCode { get; set; }
        public string Event { get; set; }
        public string Current { get; set; }
        public string Voltage { get; set; }
        public string PowerFactor { get; set; }
        public string CumulativeEnergykWhImport { get; set; }
        public string CumulativeEnergykWhExport { get; set; }
        public string CumulativeTamperCount { get; set; }
        public string GenericEventLogSequenceNumber { get; set; }

    }
}