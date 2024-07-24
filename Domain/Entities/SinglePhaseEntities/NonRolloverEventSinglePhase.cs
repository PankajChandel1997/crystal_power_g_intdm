﻿using Domain.Interface;
using Domain.Model;

namespace Domain.Entities.SinglePhaseEntities
{
    public class NonRolloverEventSinglePhase : Entity<int>, ITrackCreated, ITrackUpdated
    {
        public string MeterNo { get; set; }
        public string RealTimeClockDateAndTime { get; set; }
        public string EventCode { get; set; }
        public string GenericEventLogSequenceNumber { get; set; }
        public string CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
