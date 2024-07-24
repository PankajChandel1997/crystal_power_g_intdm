﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs.ThreePhaseEventCTDTOs
{
    public class InstantaneousProfileThreePhaseCTDto
    {
        public int Number { get; set; } //For Index Postion
        public string CreatedOn { get; set; }
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

        public string CumulativeEnergyWhImport { get; set; }
        public string CumulativeEnergyWhExport { get; set; }
        public string CumulativeEnergyVAhImport { get; set; }
        public string CumulativeEnergyVAhExport { get; set; }

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

        [DisplayName("MDkW")]
        public string MaximumDemandkW { get; set; }

        [DisplayName("MDkWDateTime")]
        public string MaximumDemandkWDateTime { get; set; }

        [DisplayName("MDkVA")]
        public string MaximumDemandkVA { get; set; }

        [DisplayName("MDkVADateTime")]
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
    }
}
