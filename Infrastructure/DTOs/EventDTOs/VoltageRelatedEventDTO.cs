namespace Infrastructure.DTOs.EventDTOs
{
    public class VoltageRelatedEventDto
    {
        public int Number { get; set; }
        public string CreatedOn { get; set; }
        public string MeterNo { get; set; }
        public string RealTimeClockDateAndTime { get; set; }
        public string Event { get; set; }
        public string CurrentIr { get; set; }
        public string CurrentIy { get; set; }
        public string CurrentIb { get; set; }
        public string VoltageVrn { get; set; }
        public string VoltageVyn { get; set; }
        public string VoltageVbn { get; set; }
        public string SignedPowerFactorRPhase { get; set; }
        public string SignedPowerFactorYPhase { get; set; }
        public string SignedPowerFactorBPhase { get; set; }
        public string CumulativeEnergykWhImport { get; set; }
        public string CumulativeTamperCount { get; set; }
        public string CumulativeEnergykWhExport { get; set; }
        public string GenericEventLogSequenceNumber { get; set; }
        public string NeutralCurrent { get; set; }

        public string KVAHImportForwarded { get; set; }
        public string RPhaseActiveCurrent { get; set; }
        public string YPhaseActiveCurrent { get; set; }
        public string BPhaseActiveCurrent { get; set; }
        public string TotalPF { get; set; }
        public string KVAHExport { get; set; }
        public string Temperature { get; set; }

    }
}
