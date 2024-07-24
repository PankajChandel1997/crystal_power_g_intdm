﻿using Domain.Interface;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ThreePhaseCTEntities
{
    public class ESWThreePhaseCT : Entity<int>, ITrackCreated, ITrackUpdated
    {
        public string MeterNo { get; set; }
        public bool RPhaseVoltageMissing { get; set; }
        public bool YPhaseVoltageMissing { get; set; }
        public bool BPhaseVoltageMissing { get; set; }
        public bool OverVoltage { get; set; }
        public bool LowVoltage { get; set; }
        public bool VoltagUnbalance { get; set; }
        public bool RPhaseCurrentReverse { get; set; }
        public bool YPhaseCurrentReverse { get; set; }
        public bool BPhaseCurrentReverse { get; set; }
        public bool CurrentUnbalance { get; set; }
        public bool CurrentBypass { get; set; }
        public bool OverCurrent { get; set; }
        public bool VerylowPF { get; set; }
        public bool InfluenceOfPermanetMagnetOorAcDc { get; set; }
        public bool NeutralDisturbance { get; set; }
        public bool MeterCoverOpen { get; set; }
        public bool MeterLoadDisconnectConnected { get; set; }
        public bool LastGasp { get; set; }
        public bool FirstBreath { get; set; }
        public bool IncrementInBillingCounterMRI { get; set; }

        public string CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}