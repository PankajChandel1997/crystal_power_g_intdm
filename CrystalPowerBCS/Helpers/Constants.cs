﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalPowerBCS.Helpers
{
    public static class Constants
    {
        //Meter Connection Commands
        public const string Notification = "Notification";
        public const string AccessToPortDenied = "Access to Port is denied.";
        public const string MeterConnected = "Meter Connected SucessFully : ";
        public const string Disconnected = "Disconnected";
        public const string Anerroroccurred = "An error occurred: ";
        public const string OperationCompletedSuccessFully = "Operation Completed SuccessFully";
        public const string SomethingWentWrongPleasetryagain = "Something Went Wrong, Please try again";
        public const string consumerdetailsupdatedsuccessfully = "consumer details updated successfully";
        public const string unabletoupdateconsumerdetailspleasetryagain = "unable to update consumer details please try again";
        public const string BillingDateSetSuccessfull = "Billing Date Set Successfull";
        public const string RTCDateSetSuccessfull = "Set RTC Successful";
        public const string RealTimeClockDateAndTime = "RealTimeClockDateAndTime";
        public const string RealTimeClock = "RealTimeClock";

        //Port
        public const string Probleminopeningserialport = "Problem in opening serial port!";
        public const string UnabletoaccessComPortPleaseCheckOpticalCable = "Unable to access Com Port, Please Check Optical Cable";
        public const string UnabletoOpenPort = "Unable to Open Port";
        public const string PleaseSelectComPort = "Please Select Com Port";
        public const string PortisengagedPleasetryagain = "Port is engaged Please try again";

        //Authentication
        public const string CrystalPower = "Crystal Power";
        public const string Login = "Login SuccessFull";
        public const string IncorrectPassword = "Incorrect Password";

        //Data Fetch
        public const string TODDownloadedSuccessfully = "TOD Downloaded SuccessFully";
        public const string IndiaStandardTime = "India Standard Time";
        public const string IPDownloadedSuccessFully = "IP Downloaded SuccessFully";
        public const string IPDownloadedFailed = "IP Downloaded Failed";
        public const string IPDownloadedSuccessfullyCount = "IP Downloaded SuccessFully Count: ";
        public const string BillingProfileDownloadedSuccessFully = "Billing Profile Downloaded SuccessFully";
        public const string BillingProfileDownloadFailed = "Billing Profile Download Failed";
        public const string BillingProfileDownloadSuccessfullyCount = "Billing Profile Downloaded SuccessFully Count: ";
        public const string BlockloadProfileDownloadedSuccessfullyCount = "Block Load Profile Downloaded SuccessFully Count: ";
        public const string BlockloadProfileDownloadFailed = "Block Load Profile Download Failed";
        public const string DailyProfileDownloadedSuccessFully = "Daily Profile Downloaded SuccessFully";
        public const string DailyProfileDownloadFailed = "Daily Profile Download Failed";
        public const string DailyLoadProfileDownloadedSuccessfullyCount = "Daily Load Profile Downloaded SuccessFully Count: ";
        public const string DailyLoadProfileDownloadFailed = "Daily Load Profile  Not Downloaded";
        public const string BlockLoadProfileDownloadedSuccessFully = "Block Load Profile Downloaded SuccessFully";
        public const string BlockLoadProfileDownloadFailed = "Block Load Profile Download Failed";
        public const string ControlEventDownloadedSuccessFully = "Control Event Downloaded SuccessFully";
        public const string ControlEventDownloadedFailed = "Control Event  Downloaded Failed";
        public const string NonRollOverEventDownloadedSuccessFully = "Non Roll Over Event Downloaded SuccessFully";
        public const string NonRollOverEventDownloadFailed = "Non Roll Over Event Download Failed";
        public const string OthersEventDownloadedSuccessFully = "Others Event Downloaded SuccessFully";
        public const string OthersEventDownloadFailed = "Others Event Download Failed";
        public const string TransactionEventDownloadedSuccessFully = "Transaction Event Downloaded SuccessFully";
        public const string TransactionEventDownloadFailed = "Transaction Event Download Failed";
        public const string PowerRelatedEventDownloadedSuccessFully = "Power Related Event Downloaded SuccessFully";
        public const string PowerRelatedEventDownloadFailed = "Power Related Event Download Failed";
        public const string CurrentRelatedEventDownloadedSuccessFully = "Current Related Event Downloaded SuccessFully";
        public const string CurrentRelatedEventDownloadFailed = "Current Related Event Download Failed";
        public const string VoltageRelatedEventDownloadedSuccessFully = "Voltage Related Event Downloaded SuccessFully";
        public const string VoltageRelatedEventDownloadFailed = "Voltage Related Event Download Failed";
        public const string SelfDiagnosticDownloadedSuccessFullyCount = "Self Diagnostic Downloaded SuccessFully Count: ";
        public const string SelfDiagnosticDownloadFailed = "Self Diagnostic Not Downloaded";
        public const string SomethingWentWrongUnabletoreadMeterData = "Something Went Wrong, Unable to read Meter Data";

        //Meters
        public const string SinglePhaseMeter = "SinglePhase";
        public const string ThreePhaseMeter = "ThreePhase";
        public const string ThreePhaseLTCT = "ThreePhaseLTCT";
        public const string ThreePhaseHTCT = "ThreePhaseHTCT";

        //Export Files
        public const string AllEventsSinglePhase = "AllEventsSinglePhase";
        public const string AllEventsThreePhase = "AllEventsThreePhase";
        public const string AllEvents = "All Events ";
        public const string SinglePhaseMeterAllEvents = "All Events Single Phase Meter";
        public const string Export = "Export";
        public const string Report = "Reports";
        public const string ExportasExcel = "Export as Excel";
        public const string ExportasPdf = "Export as Pdf";
        public const string ExportasJPG = "Export as JPG";
        public const string ExportasPNG = "Export as PNG";
        public const string ControlEventSinglePhase = "ControlEventSinglePhase";
        public const string ControlEventThreePhase = "ControlEventThreePhase";
        public const string ControlEventSinglePhaseMeter = "Control Event Single Phase Meter";
        public const string ControlEvent = "Control Event ";
        public const string TransactionEventSinglePhase = "TransactionEventSinglePhase";
        public const string TransactionEventThreePhase = "TransactionEventThreePhase";
        public const string TransactionEventSinglePhaseMeter = "Transaction Event Single Phase Meter";
        public const string TransactionEvent = "Transaction Event ";
        public const string CurrentRelatedEventSinglePhase = "CurrentRelatedEventSinglePhase";
        public const string CurrentRelatedEventSinglePhaseMeter = "Current Related Event Single Phase Meter";
        public const string CurrentRelatedEventThreePhase = "CurrentRelatedEventThreePhase";
        public const string CurrentRelatedEvent = "Current Related Event ";
        public const string DailyLoadProfileSinglePhase = "DailyLoadProfileSinglePhase";
        public const string DailyLoadProfileThreePhase = "DailyLoadProfileThreePhase";
        public const string DailyLoadProfileThreePhaseCT = "DailyLoadProfileThreePhaseCT";
        public const string DailyLoadProfileSinglePhaseMeter = "Daily Load Profile Single Phase Meter";
        public const string DailyLoadProfileThreePhaseMeter = "Daily Load Profile Three Phase Meter";
        public const string DailyLoadProfile = "Daily Load Profile ";
        public const string BlockLoadProfileSinglePhase = "BlockLoadProfileSinglePhase";
        public const string BlockLoadProfileThreePhase = "BlockLoadProfileThreePhase";
        public const string BlockLoadProfileSinglePhaseMeter = "BlockLoad Profile Single Phase Meter";
        public const string BlockLoadProfileThreePhaseMeter = "BlockLoad Profile Three Phase Meter";        
        public const string Consumption = "Consumption";
        public const string ConsumptionSinglePhase = "ConsumptionSinglePhase";
        public const string ConsumptionThreePhase = "ConsumptionThreePhase";
        public const string ConsumptionThreePhaseCT = "ConsumptionThreePhaseCT";
        public const string InstantaneousProfile = "InstantaneousProfile";
        public const string InstantaneousProfileSinglePhase = "InstantaneousProfileSinglePhase";
        public const string InstantaneousProfileThreePhase = "InstantaneousProfileThreePhase";
        public const string AllReportsSinglePhase = "All_Reports_SinglePhase";
        public const string AllReportsThreePhase = "All_Reports_ThreePhase";
        public const string AllReportsThreePhaseCT = "All_Reports_ThreePhaseCT";
        public const string OtherEventSinglePhase = "OtherEventSinglePhase";
        public const string OtherEventSinglePhaseMeter = "OtherEventSinglePhaseMeter";
        public const string OtherEventThreePhase = "OtherEventThreePhase";
        public const string OtherEventThreePhaseMeter = "OtherEventThreePhaseMeter";
        public const string VoltageRelatedEventSinglePhase = "VoltageRelatedEventSinglePhase";
        public const string VoltageRelatedEventThreePhase = "VoltageRelatedEventThreePhase";
        public const string VoltageRelatedEventSinglePhaseMeter = "VoltageRelatedEventSinglePhaseMeter";
        public const string VoltageRelatedEventThreePhaseMeter = "VoltageRelatedEventThreePhaseMeter";
        public const string PowerRelatedEventSinglePhase = "PowerRelatedEventSinglePhase";
        public const string PowerRelatedEventThreePhase = "PowerRelatedEventThreePhase";
        public const string PowerRelatedEventSinglePhaseMeter = "Power Related Event Single Phase Meter";
        public const string PowerRelatedEventThreePhaseMeter = "Power Related Event Three Phase Meter";
        public const string NonRolloverEventSinglePhase = "NonRolloverEventSinglePhase";
        public const string NonRolloverEventThreePhase = "NonRolloverEventThreePhase";
        public const string NonRolloverEventSinglePhaseMeter = "Non Rollover Event Single Phase Meter";
        public const string NonRolloverEventThreePhaseMeter = "Non Rollover Event Three Phase Meter";

        //Items
        public const string CumulativeEnergykWhExport = "Cumulative Energy kWh Export";
        public const string CumulativeEnergykWhImport = "Cumulative Energy kWh Import";
        public const string CumulativeEnergykVAhExport = "Cumulative Energy kVAh Export";
        public const string CumulativeEnergykVAhImport = "Cumulative Energy kVAh Import";
        public const string CumulativeEnergykWhTZ1 = "Cumulative Energy kWh TZ1";
        public const string CumulativeEnergykWhTZ2 = "Cumulative Energy kWh TZ2";
        public const string CumulativeEnergykWhTZ3 = "Cumulative Energy kWh TZ3";
        public const string CumulativeEnergykWhTZ4 = "Cumulative Energy kWh TZ4";
        public const string CumulativeEnergykWhTZ5 = "Cumulative Energy kWh TZ5";
        public const string CumulativeEnergykWhTZ6 = "Cumulative Energy kWh TZ6";
        public const string CumulativeEnergykWhTZ7 = "Cumulative Energy kWh TZ7";
        public const string CumulativeEnergykWhTZ8 = "Cumulative Energy kWh TZ8";
        public const string CumulativeEnergykVAhTZ1 = "Cumulative Energy kVAh TZ1";
        public const string CumulativeEnergykVAhTZ2 = "Cumulative Energy kVAh TZ2";
        public const string CumulativeEnergykVAhTZ3 = "Cumulative Energy kVAh TZ3";
        public const string CumulativeEnergykVAhTZ4 = "Cumulative Energy kVAh TZ4";
        public const string CumulativeEnergykVAhTZ5 = "Cumulative Energy kVAh TZ5";
        public const string CumulativeEnergykVAhTZ6 = "Cumulative Energy kVAh TZ6";
        public const string CumulativeEnergykVAhTZ7 = "Cumulative Energy kVAh TZ7";
        public const string CumulativeEnergykVAhTZ8 = "Cumulative Energy kVAh TZ8";
        public const string MaximumDemandkW = "Maximum Demand kW";
        public const string MaximumDemandkV = "Maximum Demand kVA";
        public const string BlockEnergykWhExport = "Block Energy kWh Export";
        public const string BlockEnergykWhImport = "Block Energy kWh Import";
        public const string BlockEnergykVAhExport = "Block Energy kVAh Export";
        public const string BlockEnergykVAhImport = "Block Energy kVAh Import";
        public const string AveragePowerFactor = "Average Power Factor";
        public const string SystemPowerFactorImport = "System Power Factor Import";
        public const string CumulativeEnergykWhImportConsumption = "Cumulative Energy kWh Import Consumption";
        public const string BillingProfileSinglePhase = "BillingProfileSinglePhase";
        public const string BillingProfileThreePhase = "BillingProfileThreePhase";

        //Consumption
        public const string CumulativeEnergykWh = "Cumulative Energy kWh";
        public const string CumulativeEnergykVArhQ1 = "Cumulative Energy kVArh Q1";
        public const string CumulativeEnergykVArhQ2 = "Cumulative Energy kVArh Q2";
        public const string CumulativeEnergykVArhQ3 = "Cumulative Energy kVArh Q3";
        public const string CumulativeEnergykVArhQ4 = "Cumulative Energy kVArh Q4";
        public const string CumulativeEnergykWhExportConsumption = "Cumulative Energy kWh Export Consumption";
        public const string CumulativeEnergykVAhImportConsumption = "Cumulative Energy kVAh Import Consumption";
        public const string CumulativeEnergykVAhExportConsumption = "Cumulative Energy kVAh Export Consumption";

        //Graph
        public const string LineGraph = "Line Graph";
        public const string BarGraph = "Bar Graph";
        public const string VectorGraph = "Vector Graph";
        public const string CartesianChart = "CartesianChart";
        public const string MainGraphView = "MainGraphView";

        //Voltage,Current
        public const string Voltage = "Voltage";
        public const string AverageCurrent = "Average Current";
        public const string AverageVoltage = "Average Voltage";
        public const string VoltageR = "VoltageR";
        public const string VoltageY = "VoltageY";
        public const string VoltageB = "VoltageB";
        public const string PhaseCurrent = "Phase Current";
        public const string NeutralCurrent = "Neutral Current";
        public const string CurrentR = "CurrentR";
        public const string CurrentB = "CurrentB";
        public const string CurrentY = "CurrentY";

        //Grid
        public const string Instanteneousparameter = "Instanteneousparameter";
        public const string NamePlate = "NamePlate";
        public const string BlockLoadProfile = "BlockLoadProfile";
        public const string FetchBlockLoadProfile = "Block Load Profile";
        public const string BillingProfile = "BillingProfile";
        public const string Events = "Events";
        public const string SelfDiagnostic = "SelfDiagnostic";
        public const string DailyLoadConsumption = "DailyLoadConsumption";
        public const string ConsumerDetails = "ConsumerDetails";
        public const string TOD = "TOD";
        public const string TODConsumtion = "TODConsumtion";
        public const string All = "All";
        public const string ConnectDisconnect = "Connect Disconnect";
        public const string CurrentRelated = "Current Related";
        public const string NonRollOver = "Non Roll Over";
        public const string OthersRelated = "Others Related";
        public const string PowerRelated = "Power Related";
        public const string TransactionRelated = "Transaction Related";
        public const string VoltageRelated = "Voltage Related";
        public const string DailyLoadProfileGrid = "DailyLoadProfile";

        //DIEvent
        
        public const string DISinglePhase = "DIEventSinglePhase";
        public const string DIThreePhase = "DIEventThreePhase";
        public const string DIEvent = "DI Event";
    }
    public static class OBISConstants
    {
        public const string Realtimeclock = "0.0.1.0.0.255";
        public const string Voltage = "1.0.12.7.0.255";
        public const string AverageVoltage = "1.0.12.27.0.255";
        public const string PhaseCurrent = "1.0.11.7.0.255";
        public const string NeutralCurrent = "1.0.91.7.0.255";
        public const string BlockNeutralCurrent = "1.0.91.27.0.255";
        public const string MeterHealthIndicator = "0.0.96.10.1.255";
        public const string ImportAveragePF = "1.0.13.27.0.255";
        public const string SignedPowerFactor = "1.0.13.7.0.255";
        public const string FrequencyHz = "1.0.14.7.0.255";
        public const string ApparentPowerKVA = "1.0.9.7.0.255";
        public const string ActivePowerkW = "1.0.1.7.0.255";
        public const string CumulativeenergykWhimport = "1.0.1.8.0.255";
        public const string CumulativeenergykVAhimport = "1.0.9.8.0.255";
        public const string CumulativeenergykWhExport = "1.0.2.8.0.255";
        public const string CumulativeenergykVAhExport = "1.0.10.8.0.255";
        public const string MaximumDemandkWOrDT = "1.0.1.6.0.255";
        public const string MaximumDemandkVahOrDT = "1.0.9.6.0.255";
        public const string CumulativepowerONdurationinminute = "0.0.94.91.14.255";
        public const string Cumulativetampercount = "0.0.94.91.0.255";
        public const string Cumulativebillingcount = "0.0.0.1.0.255";
        public const string Cumulativeprogrammingcount = "0.0.96.2.0.255";       
        public const string Loadlimitfunctionstatus = "0.0.96.3.10.255";
        public const string LoadlimitvalueinkW = "0.0.17.0.0.255";
        public const string Current = "1.0.94.91.14.255";
        public const string CurrentR = "1.0.31.7.0.255";
        public const string CurrentY = "1.0.51.7.0.255";
        public const string CurrentB = "1.0.71.7.0.255";
        public const string VoltageR = "1.0.32.7.0.255";
        public const string VoltageY = "1.0.52.7.0.255";
        public const string VoltageB = "1.0.72.7.0.255";
        public const string SignedPowerFactorRPhase = "1.0.33.7.0.255";
        public const string SignedPowerFactorYPhase = "1.0.53.7.0.255";
        public const string SignedPowerFactorBPhase = "1.0.73.7.0.255";
        public const string ThreePhasePowerFactoPF = "1.0.13.7.0.255";
        public const string SignedActivePowerkW = "1.0.1.7.0.255";
        public const string SignedReactivePowerkvar = "1.0.3.7.0.255";
        public const string CumulativeEnergykVArhQ1 = "1.0.5.8.0.255";
        public const string CumulativeEnergykVArhQ2 = "1.0.6.8.0.255";
        public const string CumulativeEnergykVArhQ3 = "1.0.7.8.0.255";
        public const string CumulativeEnergykVArhQ4 = "1.0.8.8.0.255";
        public const string NumberOfPowerFailures = "0.0.96.7.0.255";
        public const string CumulativePowerOFFDurationInMin = "0.0.94.91.8.255";
        public const string BillingPeriodCounter = "0.0.0.1.0.255";
        public const string CumulativeProgrammingCount = "0.0.96.2.0.255";
        public const string BillingDateImportMode = "0.0.0.1.2.255";
        public const string LoadLimitFunctionStatus = "0.0.96.3.10.255";
        public const string LoadLimitThresholdkW = "0.0.17.0.0.255";
        public const string BlockLoadSinglePhase = "1.0.11.27.0.255";
        public const string BlockEnergykWhImport = "1.0.1.29.0.255";
        public const string BlockEnergykVAh = "1.0.9.29.0.255";
        public const string BlockEnergykWhExport = "1.0.2.29.0.255";
        public const string BlockEnergykVAhExport = "1.0.10.29.0.255";
        public const string BlockLoadThreePhaseCurrentR = "1.0.31.27.0.255";
        public const string BlockLoadThreePhaseCurrentY = "1.0.51.27.0.255";
        public const string BlockLoadThreePhaseCurrentB = "1.0.71.27.0.255";
        public const string BlockLoadThreePhaseVoltageR = "1.0.32.27.0.255";
        public const string BlockLoadThreePhaseVoltageY = "1.0.52.27.0.255";
        public const string BlockLoadThreePhaseVoltageB = "1.0.72.27.0.255";
        public const string PowerFactor = "1.0.13.7.0.255";
        public const string PowerFactorRPhase = "1.0.33.27.0.255";
        public const string PowerFactorYPhase = "1.0.53.27.0.255";
        public const string PowerFactorBPhase = "1.0.73.27.0.255";
        public const string BlockEnergykVArhQ1 = "1.0.5.29.0.255";
        public const string BlockEnergykVArhQ2 = "1.0.6.29.0.255";
        public const string BlockEnergykVArhQ3 = "1.0.7.29.0.255";
        public const string BlockEnergykVArhQ4 = "1.0.8.29.0.255";
        public const string BillingRealTime = "0.0.0.1.2.255";
        public const string AveragePowerFactor = "1.0.13.0.0.255";
        public const string CumulativeEnergykWhTZ1 = "1.0.1.8.1.255";
        public const string CumulativeEnergykWhTZ2 = "1.0.1.8.2.255";
        public const string CumulativeEnergykWhTZ3 = "1.0.1.8.3.255";
        public const string CumulativeEnergykWhTZ4 = "1.0.1.8.4.255";
        public const string CumulativeEnergykWhTZ5 = "1.0.1.8.5.255";
        public const string CumulativeEnergykWhTZ6 = "1.0.1.8.6.255";
        public const string CumulativeEnergykWhTZ7 = "1.0.1.8.7.255";
        public const string CumulativeEnergykWhTZ8 = "1.0.1.8.8.255";
        public const string CumulativeEnergykVAhTZ1 = "1.0.9.8.1.255";
        public const string CumulativeEnergykVAhTZ2 = "1.0.9.8.2.255";
        public const string CumulativeEnergykVAhTZ3 = "1.0.9.8.3.255";
        public const string CumulativeEnergykVAhTZ4 = "1.0.9.8.4.255";
        public const string CumulativeEnergykVAhTZ5 = "1.0.9.8.5.255";
        public const string CumulativeEnergykVAhTZ6 = "1.0.9.8.6.255";
        public const string CumulativeEnergykVAhTZ7 = "1.0.9.8.7.255";
        public const string CumulativeEnergykVAhTZ8 = "1.0.9.8.8.255";
        public const string BillingPowerONdurationinMinutes = "0.0.94.91.13.255";
        public const string SystemPowerFactorImport = "1.0.13.0.0.255";
        public const string MaximumDemandkWForTZ1OrDT = "1.0.1.6.1.255";
        public const string MaximumDemandkWForTZ2OrDT = "1.0.1.6.2.255";
        public const string MaximumDemandkWForTZ3OrDT = "1.0.1.6.3.255";
        public const string MaximumDemandkWForTZ4OrDT = "1.0.1.6.4.255";
        public const string MaximumDemandkWForTZ5OrDT = "1.0.1.6.5.255";
        public const string MaximumDemandkWForTZ6OrDT = "1.0.1.6.6.255";
        public const string MaximumDemandkWForTZ7OrDT = "1.0.1.6.7.255";
        public const string MaximumDemandkWForTZ8OrDT = "1.0.1.6.8.255";
        public const string MaximumDemandkVAForTZ1OrDT = "1.0.9.6.1.255";
        public const string MaximumDemandkVAForTZ2OrDT = "1.0.9.6.2.255";
        public const string MaximumDemandkVAForTZ3OrDT = "1.0.9.6.3.255";
        public const string MaximumDemandkVAForTZ4OrDT = "1.0.9.6.4.255";
        public const string MaximumDemandkVAForTZ5OrDT = "1.0.9.6.5.255";
        public const string MaximumDemandkVAForTZ6OrDT = "1.0.9.6.6.255";
        public const string MaximumDemandkVAForTZ7OrDT = "1.0.9.6.7.255";
        public const string MaximumDemandkVAForTZ8OrDT = "1.0.9.6.8.255";
        public const string BillingPowerONdurationInMinutesDBP = "0.0.94.91.13.255";
        public const string TamperCount = "0.0.94.91.0.255";
        public const string AveragePFForBillingPeriod = "1.0.13.0.0.255";
        public const string BillingDate = "0.0.0.1.2.255";
        public const string VoltageRelatedEventCode = "0.0.96.11.0.255";
        public const string CurrentRelatedEventCode = "0.0.96.11.1.255";
        public const string CurrentHighRelatedEventCode = "0.0.99.98.1.255";
        public const string PowerRelatedEventCode = "0.0.96.11.2.255";
        public const string TransactionEventCode = "0.0.96.11.3.255";
        public const string TransactionEventLeadLag = "0.0.99.98.3.255";
        public const string OtherEventCode = "0.0.96.11.4.255";
        public const string OtherEventESD = "0.0.99.98.4.255";
        public const string EventCode = "0.0.96.11.6.255";
        public const string NonRolloverEvent = "0.0.96.11.5.255";
        public const string CurrentIr = "1.0.31.7.0.255";
        public const string CurrentIy = "1.0.51.7.0.255";
        public const string CurrentIb = "1.0.71.7.0.255";
        public const string VoltageVrn = "1.0.32.7.0.255";
        public const string VoltageVyn = "1.0.52.7.0.255";
        public const string VoltageVbn = "1.0.72.7.0.255";
        public const string GenericEventLogSequenceNumberSinglePhaseVoltage = "0.0.96.15.0.255";
        public const string GenericEventLogSequenceNumberSinglePhaseCurrent = "0.0.96.15.1.255";
        public const string GenericEventLogSequenceNumberSinglePhasePower = "0.0.96.15.2.255";
        public const string GenericEventLogSequenceNumberSinglePhaseTransaction = "0.0.96.15.3.255";
        public const string GenericEventLogSequenceNumberSinglePhaseOther = "0.0.96.15.4.255";
        public const string GenericEventLogSequenceNumberSinglePhaseNonRollOver = "0.0.96.15.5.255";
        public const string GenericEventLogSequenceNumberSinglePhaseControl = "0.0.96.15.6.255";

        public const string GenericEventLogSequenceNumberThreePhaseVoltage = "0.0.96.15.0.255";
        public const string GenericEventLogSequenceNumberThreePhaseCurrent = "0.0.96.15.1.255";
        public const string GenericEventLogSequenceNumberThreePhasePower = "0.0.96.15.2.255";
        public const string GenericEventLogSequenceNumberThreePhaseTransaction = "0.0.96.15.3.255";
        public const string GenericEventLogSequenceNumberThreePhaseOther = "0.0.96.15.4.255";
        public const string GenericEventLogSequenceNumberThreePhaseNonRollOver = "0.0.96.15.5.255";
        public const string GenericEventLogSequenceNumberThreePhaseControl = "0.0.96.15.6.255";

        public const string OtherRelatedEventsExport = "1.0.2.8.0.255";

        public const string DIEventCode = "0.0.96.11.128.255";

        //25-06-2024
        public const string CumulativePowerOndurationMin = "0.0.94.91.14.255";
        public const string Temperature = "0.0.96.9.128.255";
        public const string MdKwExport = "1.0.2.6.0.255";
        public const string MdKvaExport = "1.0.10.6.0.255";
        public const string AngleRyPhaseVoltage = "1.0.81.7.10.255";
        public const string AngleRbPhaseVoltage = "1.0.81.7.20.255";
        public const string PhaseSequence = "1.0.128.7.0.255";
        public const string NicSignalPower = "0.128.96.0.4.255";
        public const string NicSignalToNoiseRatio = "0.128.96.0.11.255";
        public const string NicCellIdentifier = "0.128.96.0.13.255";
    
        //Block Load
        public const string CumulativeEnergyQI_WB = "1.0.5.29.0.255";
        public const string CumulativeEnergyQII_WB = "1.0.6.29.0.255";
        public const string CumulativeEnergyQIII_WB = "1.0.7.29.0.255";
        public const string CumulativeEnergyQIV_WB = "1.0.8.29.0.255";

        //Billing
        public const string CumulativeMdKwImportForwarded = "1.0.1.2.0.255";
        public const string CumulativeMdKvaImportForwarded = "1.0.9.2.0.255";
        public const string BillingResetType = "1.0.96.50.2.255";
        public const string CumulativeBillingCount = "0.0.0.1.0.255";

        //Billing Profile WB
        public const string FundamentalEnergy = "1.0.1.8.150.255";
        public const string FundamentalEnergyExport = "1.0.2.8.150.255";
        public const string PowerOffDuration = "0.0.94.91.150.255";
        public const string PowerFailCount = "0.0.96.7.150.255";


        //METERS
        public const string MeterNumber = "0.0.96.1.0.255";
        public const string DeviceId = "0.0.96.1.2.255";
        public const string ManufacturerName = "0.0.96.1.1.255";
        public const string FirmwareVersion = "1.0.0.2.0.255";
        public const string MeterType = "0.0.94.91.9.255";
        public const string Category = "0.0.94.91.11.255";
        public const string CurrentRating = "0.0.94.91.12.255";
        public const string MeterYearManufacturer = "0.0.96.1.4.255";
        public const string CTRatio = "1.0.0.4.2.255";
        public const string PTRatio = "1.0.0.4.3.255";

        

    }
}
