﻿using Domain.Interface;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ThreePhaseCTEntities
{
    public class InstantaneousProfileThreePhaseCT : Entity<int>, ITrackCreated, ITrackUpdated
    {
        public string MeterNo { get; set; }
        public string RealTimeClockDateAndTime { get; set; }
        public string CurrentR { get; set; }
        public string CurrentY { get; set; }
        public string CurrentB { get; set; }
        public string VoltageR { get; set; }
        public string VoltageY { get; set; }
        public string VoltageB { get; set; }
        public string SignedPowerFactorRPhase { get; set; }
        public string SignedPowerFactorYPhase { get; set; }
        public string SignedPowerFactorBPhase { get; set; }
        public string ThreePhasePowerFactorPF { get; set; }
        public string FrequencyHz { get; set; }
        public string ApparentPowerKVA { get; set; }
        public string SignedActivePowerkW { get; set; }
        public string SignedReactivePowerkvar { get; set; }
        public string CumulativeEnergykWhImport { get; set; }
        public string CumulativeEnergykWhExport { get; set; }
        public string CumulativeEnergykVAhImport { get; set; }
        public string CumulativeEnergykVAhExport { get; set; }
        public string CumulativeEnergykVArhQ1 { get; set; }
        public string CumulativeEnergykVArhQ2 { get; set; }
        public string CumulativeEnergykVArhQ3 { get; set; }
        public string CumulativeEnergykVArhQ4 { get; set; }
        public string NumberOfPowerFailures { get; set; }
        public string CumulativePowerOFFDurationInMin { get; set; }
        public string CumulativeTamperCount { get; set; }
        public string BillingPeriodCounter { get; set; }
        public string CumulativeProgrammingCount { get; set; }
        public string BillingDateImportMode { get; set; }
        public string MaximumDemandkW { get; set; }
        public string MaximumDemandkWDateTime { get; set; }
        public string MaximumDemandkVA { get; set; }
        public string MaximumDemandkVADateTime { get; set; }
        //25-06-2024
        public string CumulativePowerOndurationMin { get; set; }
        public string Temperature { get; set; }
        public string NeutralCurrent { get; set; }
        public string MdKwExport { get; set; }
        public string MdKwExportDateTime { get; set; }
        public string MdKvaExport { get; set; }
        public string MdKvaExportDateTime { get; set; }
        public string AngleRyPhaseVoltage { get; set; }
        public string AngleRbPhaseVoltage { get; set; }
        public string PhaseSequence { get; set; }
        public string NicSignalPower { get; set; }
        public string NicSignalToNoiseRatio { get; set; }
        public string NicCellIdentifier { get; set; }
        public string LoadLimitFunctionStatus { get; set; }
       
        public string CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

