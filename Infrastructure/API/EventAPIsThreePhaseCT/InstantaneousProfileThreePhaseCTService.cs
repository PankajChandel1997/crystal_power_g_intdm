using Domain.Entities.ThreePhaseCTEntities;
using Domain.Interface.Service;
using Infrastructure.DTOs.ThreePhaseEventCTDTOs;
using Infrastructure.Helpers;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.API.EventAPIThreePhaseCT
{
    public class InstantaneousProfileThreePhaseCTService
    {
        private readonly IDataService<InstantaneousProfileThreePhaseCT> _dataService;
        public ErrorHelper _errorHelper;
        private readonly ApplicationContextFactory _contextFactory;

        public InstantaneousProfileThreePhaseCTService()
        {
            _dataService = new GenericDataService<InstantaneousProfileThreePhaseCT>(new ApplicationContextFactory());
            _errorHelper = new ErrorHelper();
            _contextFactory = new ApplicationContextFactory();
        }

        public async Task<bool> Add(List<InstantaneousProfileThreePhaseCT> instantaneousProfile)
        {
            try
            {
                //await Delete(instantaneousProfile.FirstOrDefault().MeterNo);
                return await _dataService.CreateRange(instantaneousProfile);
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Delete(string meterNumber)
        {
            try
            {
                using (ApplicationDBContext db = _contextFactory.CreateDbContext())
                {
                    string query = "select * from InstantaneousProfileThreePhaseCT where MeterNo = '" + meterNumber + "' ORDER by Id DESC";

                    var res = await _dataService.Filter(query);

                    if (res.Any())
                    {
                        db.Set<InstantaneousProfileThreePhaseCT>().RemoveRange(res);
                    }

                    await db.SaveChangesAsync();
                };
                return true;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);

            }
        }

        public async Task<List<InstantaneousProfileThreePhaseCTDto>> GetAll(int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from InstantaneousProfileThreePhaseCT where MeterNo = '" + meterNumber + "' ORDER by Id DESC LIMIT " + pageSize;

                var response = await _dataService.Filter(query);
                if (response == null)
                {
                    return null;
                }
                List<InstantaneousProfileThreePhaseCTDto> instantaneousProfile = await ParseDataToDTO(response);
                if (instantaneousProfile == null)
                {
                    return null;
                }
                return instantaneousProfile;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<InstantaneousProfileThreePhaseCTDto>> Filter(string startDate, string endDate, string fetchDate, int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from InstantaneousProfileThreePhaseCT where MeterNo = '" + meterNumber + "'";

                var response = await _dataService.Filter(query);

                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    var startDateTime = DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    var endDateTime = DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    response = response.Where(x =>
                        DateTime.ParseExact(x.RealTimeClockDateAndTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).Date >= startDateTime.Date &&
                        DateTime.ParseExact(x.RealTimeClockDateAndTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).Date <= endDateTime.Date
                    ).Take(pageSize).ToList();
                }
                else if (!string.IsNullOrEmpty(fetchDate))
                {
                    response = response.Where(x =>
                      x.RealTimeClockDateAndTime == fetchDate).Take(pageSize).ToList();
                }
                else
                {
                    response = response.Take(pageSize).ToList();
                }

                List<InstantaneousProfileThreePhaseCTDto> instantaneousProfile = await ParseDataToDTO(response);

                return instantaneousProfile;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);

                throw new Exception(ex.Message);
            }
        }

        //private async Task<List<InstantaneousProfileThreePhaseCT>> ParseDataToClass(List<InstantaneousProfileThreePhaseCTDto> instantaneousProfileThreePhaseCTDtoList)
        //{
        //    List<InstantaneousProfileThreePhaseCT> instantaneousProfileThreePhaseCTList = new List<InstantaneousProfileThreePhaseCT>();

        //    foreach (var instantaneousProfileThreePhaseCTDto in instantaneousProfileThreePhaseCTDtoList)
        //    {
        //        InstantaneousProfileThreePhaseCT instantaneousProfileThreePhaseCT = new InstantaneousProfileThreePhaseCT();

        //        instantaneousProfileThreePhaseCT.MeterNo = instantaneousProfileThreePhaseCTDto.MeterNo;
        //        instantaneousProfileThreePhaseCT.CreatedOn = instantaneousProfileThreePhaseCTDto.CreatedOn;
        //        instantaneousProfileThreePhaseCT.RealTimeClockDateAndTime = instantaneousProfileThreePhaseCTDto.RealTimeClockDateAndTime;
        //        instantaneousProfileThreePhaseCT.CurrentR = instantaneousProfileThreePhaseCTDto.CurrentR;
        //        instantaneousProfileThreePhaseCT.CurrentY = instantaneousProfileThreePhaseCTDto.CurrentY;
        //        instantaneousProfileThreePhaseCT.CurrentB = instantaneousProfileThreePhaseCTDto.CurrentB;
        //        instantaneousProfileThreePhaseCT.VoltageR = instantaneousProfileThreePhaseCTDto.VoltageR;
        //        instantaneousProfileThreePhaseCT.VoltageY = instantaneousProfileThreePhaseCTDto.VoltageY;
        //        instantaneousProfileThreePhaseCT.VoltageB = instantaneousProfileThreePhaseCTDto.VoltageB;
        //        instantaneousProfileThreePhaseCT.SignedPowerFactorRPhase = instantaneousProfileThreePhaseCTDto.SignedPowerFactorRPhase;
        //        instantaneousProfileThreePhaseCT.SignedPowerFactorYPhase = instantaneousProfileThreePhaseCTDto.SignedPowerFactorYPhase;
        //        instantaneousProfileThreePhaseCT.SignedPowerFactorBPhase = instantaneousProfileThreePhaseCTDto.SignedPowerFactorBPhase;
        //        instantaneousProfileThreePhaseCT.ThreePhasePowerFactorPF = instantaneousProfileThreePhaseCTDto.ThreePhasePowerFactoPF;
        //        instantaneousProfileThreePhaseCT.FrequencyHz = instantaneousProfileThreePhaseCTDto.FrequencyHz;
        //        instantaneousProfileThreePhaseCT.ApparentPowerKVA = instantaneousProfileThreePhaseCTDto.ApparentPowerKVA;
        //        instantaneousProfileThreePhaseCT.SignedActivePowerkW = instantaneousProfileThreePhaseCTDto.SignedActivePowerkW;
        //        instantaneousProfileThreePhaseCT.SignedReactivePowerkvar = instantaneousProfileThreePhaseCTDto.SignedReactivePowerkvar;
        //        instantaneousProfileThreePhaseCT.CumulativeEnergykWhImport = instantaneousProfileThreePhaseCTDto.CumulativeEnergykWhImport;
        //        instantaneousProfileThreePhaseCT.CumulativeEnergykWhExport = instantaneousProfileThreePhaseCTDto.CumulativeEnergykWhExport;
        //        instantaneousProfileThreePhaseCT.CumulativeEnergykVAhImport = instantaneousProfileThreePhaseCTDto.CumulativeEnergykVAhImport;
        //        instantaneousProfileThreePhaseCT.CumulativeEnergykVAhExport = instantaneousProfileThreePhaseCTDto.CumulativeEnergykVAhExport;
        //        instantaneousProfileThreePhaseCT.CumulativeEnergykVArhQ2 = instantaneousProfileThreePhaseCTDto.CumulativeEnergykVArhQ2;
        //        instantaneousProfileThreePhaseCT.CumulativeEnergykVArhQ3 = instantaneousProfileThreePhaseCTDto.CumulativeEnergykVArhQ3;
        //        instantaneousProfileThreePhaseCT.CumulativeEnergykVArhQ4 = instantaneousProfileThreePhaseCTDto.CumulativeEnergykVArhQ4;
        //        instantaneousProfileThreePhaseCT.NumberOfPowerFailures = instantaneousProfileThreePhaseCTDto.NumberOfPowerFailures;
        //        instantaneousProfileThreePhaseCT.CumulativePowerOFFDurationInMin = instantaneousProfileThreePhaseCTDto.CumulativePowerOFFDurationInMin;
        //        instantaneousProfileThreePhaseCT.CumulativeTamperCount = instantaneousProfileThreePhaseCTDto.CumulativeTamperCount;
        //        instantaneousProfileThreePhaseCT.BillingPeriodCounter = instantaneousProfileThreePhaseCTDto.BillingPeriodCounter;
        //        instantaneousProfileThreePhaseCT.CumulativeProgrammingCount = instantaneousProfileThreePhaseCTDto.CumulativeProgrammingCount;
        //        instantaneousProfileThreePhaseCT.BillingDateImportMode = instantaneousProfileThreePhaseCTDto.BillingDateImportMode;
        //        instantaneousProfileThreePhaseCT.MaximumDemandkW = instantaneousProfileThreePhaseCTDto.MaximumDemandkW;
        //        instantaneousProfileThreePhaseCT.MaximumDemandkWDateTime = instantaneousProfileThreePhaseCTDto.MaximumDemandkWDateTime;
        //        instantaneousProfileThreePhaseCT.MaximumDemandkVA = instantaneousProfileThreePhaseCTDto.MaximumDemandkVA;
        //        instantaneousProfileThreePhaseCT.MaximumDemandkVADateTime = instantaneousProfileThreePhaseCTDto.MaximumDemandkVADateTime;

        //        instantaneousProfileThreePhaseCT.CumulativePowerOndurationMin = instantaneousProfileThreePhaseCTDto.CumulativePowerOndurationMin;
        //        instantaneousProfileThreePhaseCT.Temperature = instantaneousProfileThreePhaseCTDto.Temperature;
        //        instantaneousProfileThreePhaseCT.NeutralCurrent = instantaneousProfileThreePhaseCTDto.NeutralCurrent;
        //        instantaneousProfileThreePhaseCT.MdKwExport = instantaneousProfileThreePhaseCTDto.MdKwExport;
        //        instantaneousProfileThreePhaseCT.MdKwExportDateTime = instantaneousProfileThreePhaseCTDto.MdKwExportDateTime;
        //        instantaneousProfileThreePhaseCT.MdKvaExport = instantaneousProfileThreePhaseCTDto.MdKvaExport;
        //        instantaneousProfileThreePhaseCT.MdKvaExportDateTime = instantaneousProfileThreePhaseCTDto.MdKvaExportDateTime;
        //        instantaneousProfileThreePhaseCT.AngleRyPhaseVoltage = instantaneousProfileThreePhaseCTDto.AngleRyPhaseVoltage;
        //        instantaneousProfileThreePhaseCT.AngleRbPhaseVoltage = instantaneousProfileThreePhaseCTDto.AngleRbPhaseVoltage;
        //        instantaneousProfileThreePhaseCT.PhaseSequence = instantaneousProfileThreePhaseCTDto.PhaseSequence;
        //        instantaneousProfileThreePhaseCT.NicSignalPower = instantaneousProfileThreePhaseCTDto.NicSignalPower;
        //        instantaneousProfileThreePhaseCT.NicSignalToNoiseRatio = instantaneousProfileThreePhaseCTDto.NicSignalToNoiseRatio;
        //        instantaneousProfileThreePhaseCT.NicCellIdentifier = instantaneousProfileThreePhaseCTDto.NicCellIdentifier;
        //        instantaneousProfileThreePhaseCT.LoadLimitFunctionStatus = instantaneousProfileThreePhaseCTDto.LoadLimitFunctionStatus;

        //        instantaneousProfileThreePhaseCTList.Add(instantaneousProfileThreePhaseCT);
        //    }


        //    return instantaneousProfileThreePhaseCTList;
        //}

        private async Task<List<InstantaneousProfileThreePhaseCTDto>> ParseDataToDTO(List<InstantaneousProfileThreePhaseCT> instantaneousProfileThreePhaseCTList)
        {
            int index = 1;
            List<InstantaneousProfileThreePhaseCTDto> instantaneousProfileThreePhaseCTDtoList = new List<InstantaneousProfileThreePhaseCTDto>();
            foreach (var instantaneousProfileThreePhaseCT in instantaneousProfileThreePhaseCTList)
            {
                InstantaneousProfileThreePhaseCTDto instantaneousProfileThreePhaseCTDto = new InstantaneousProfileThreePhaseCTDto();

                instantaneousProfileThreePhaseCTDto.Number = index;
                instantaneousProfileThreePhaseCTDto.MeterNo = instantaneousProfileThreePhaseCT.MeterNo;
                instantaneousProfileThreePhaseCTDto.CreatedOn = instantaneousProfileThreePhaseCT.CreatedOn;
                instantaneousProfileThreePhaseCTDto.RealTimeClockDateAndTime = instantaneousProfileThreePhaseCT.RealTimeClockDateAndTime;
                instantaneousProfileThreePhaseCTDto.CurrentR = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.CurrentR); 
                instantaneousProfileThreePhaseCTDto.CurrentR = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.CurrentR);
                instantaneousProfileThreePhaseCTDto.CurrentY = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.CurrentY);
                instantaneousProfileThreePhaseCTDto.CurrentB = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.CurrentB);
                instantaneousProfileThreePhaseCTDto.VoltageR = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.VoltageR);
                instantaneousProfileThreePhaseCTDto.VoltageY = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.VoltageY);
                instantaneousProfileThreePhaseCTDto.VoltageB = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.VoltageB);
                instantaneousProfileThreePhaseCTDto.SignedPowerFactorRPhase = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.SignedPowerFactorRPhase);
                instantaneousProfileThreePhaseCTDto.SignedPowerFactorYPhase = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.SignedPowerFactorYPhase);
                instantaneousProfileThreePhaseCTDto.SignedPowerFactorBPhase = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.SignedPowerFactorBPhase);
                instantaneousProfileThreePhaseCTDto.ThreePhasePowerFactorPF = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.ThreePhasePowerFactorPF); 
                instantaneousProfileThreePhaseCTDto.FrequencyHz = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.FrequencyHz);

                instantaneousProfileThreePhaseCTDto.ApparentPowerKVA = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.ApparentPowerKVA);
                instantaneousProfileThreePhaseCTDto.SignedReactivePowerkvar = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.SignedReactivePowerkvar);
                instantaneousProfileThreePhaseCTDto.CumulativeEnergykWhImport = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.CumulativeEnergykWhImport);
                instantaneousProfileThreePhaseCTDto.CumulativeEnergykWhExport = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.CumulativeEnergykWhExport);
                instantaneousProfileThreePhaseCTDto.CumulativeEnergykVAhImport = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.CumulativeEnergykVAhImport);
                instantaneousProfileThreePhaseCTDto.CumulativeEnergykVAhExport = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.CumulativeEnergykVAhExport);
                instantaneousProfileThreePhaseCTDto.CumulativeEnergykVArhQ1 = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.CumulativeEnergykVArhQ1);
                instantaneousProfileThreePhaseCTDto.CumulativeEnergykVArhQ2 = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.CumulativeEnergykVArhQ2);
                instantaneousProfileThreePhaseCTDto.CumulativeEnergykVArhQ3 = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.CumulativeEnergykVArhQ3);
                instantaneousProfileThreePhaseCTDto.CumulativeEnergykVArhQ4 = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.CumulativeEnergykVArhQ4);


                instantaneousProfileThreePhaseCTDto.NumberOfPowerFailures = instantaneousProfileThreePhaseCT.NumberOfPowerFailures;
                instantaneousProfileThreePhaseCTDto.CumulativePowerOFFDurationInMin = (double.Parse(instantaneousProfileThreePhaseCT.CumulativePowerOFFDurationInMin, NumberStyles.Any)).ToString().CustomTrucate();
                instantaneousProfileThreePhaseCTDto.CumulativeTamperCount = instantaneousProfileThreePhaseCT.CumulativeTamperCount;
                instantaneousProfileThreePhaseCTDto.BillingPeriodCounter = instantaneousProfileThreePhaseCT.BillingPeriodCounter;
                instantaneousProfileThreePhaseCTDto.CumulativeProgrammingCount = instantaneousProfileThreePhaseCT.CumulativeProgrammingCount;
                instantaneousProfileThreePhaseCTDto.BillingDateImportMode = instantaneousProfileThreePhaseCT.BillingDateImportMode;
                instantaneousProfileThreePhaseCTDto.MaximumDemandkW = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.MaximumDemandkW); 
                instantaneousProfileThreePhaseCTDto.MaximumDemandkWDateTime = instantaneousProfileThreePhaseCT.MaximumDemandkWDateTime;
                instantaneousProfileThreePhaseCTDto.MaximumDemandkVA = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.MaximumDemandkVA);
                instantaneousProfileThreePhaseCTDto.MaximumDemandkVADateTime = instantaneousProfileThreePhaseCT.MaximumDemandkVADateTime;

                //High Resolution values
                instantaneousProfileThreePhaseCTDto.CumulativeEnergyWhImport = !string.IsNullOrEmpty(instantaneousProfileThreePhaseCT.CumulativeEnergykWhImport) ? (double.Parse(instantaneousProfileThreePhaseCT.CumulativeEnergykWhImport, NumberStyles.Any) / 1000).ToString("0.00000") : string.Empty;          
                instantaneousProfileThreePhaseCTDto.CumulativeEnergyWhExport = (double.Parse(instantaneousProfileThreePhaseCT.CumulativeEnergykWhExport, NumberStyles.Any) / 1000).ToString("0.00000");
                instantaneousProfileThreePhaseCTDto.CumulativeEnergyVAhImport = (double.Parse(instantaneousProfileThreePhaseCT.CumulativeEnergykVAhImport, NumberStyles.Any) / 1000).ToString("0.00000");
                instantaneousProfileThreePhaseCTDto.CumulativeEnergyVAhExport = (double.Parse(instantaneousProfileThreePhaseCT.CumulativeEnergykVAhExport, NumberStyles.Any) / 1000).ToString("0.00000");


                instantaneousProfileThreePhaseCTDto.CumulativePowerOndurationMin = instantaneousProfileThreePhaseCT.CumulativePowerOndurationMin;
                instantaneousProfileThreePhaseCTDto.Temperature = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.Temperature);
                instantaneousProfileThreePhaseCTDto.NeutralCurrent = instantaneousProfileThreePhaseCT.NeutralCurrent;
                instantaneousProfileThreePhaseCTDto.MdKwExport = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.MdKwExport); 
                instantaneousProfileThreePhaseCTDto.MdKwExportDateTime = instantaneousProfileThreePhaseCT.MdKwExportDateTime;
                instantaneousProfileThreePhaseCTDto.MdKvaExport = StringExtensions.CheckNullableOnly(instantaneousProfileThreePhaseCT.MdKvaExport);
                instantaneousProfileThreePhaseCTDto.MdKvaExportDateTime = instantaneousProfileThreePhaseCT.MdKvaExportDateTime;
                instantaneousProfileThreePhaseCTDto.AngleRyPhaseVoltage = instantaneousProfileThreePhaseCT.AngleRyPhaseVoltage;
                instantaneousProfileThreePhaseCTDto.AngleRbPhaseVoltage = instantaneousProfileThreePhaseCT.AngleRbPhaseVoltage;
                instantaneousProfileThreePhaseCTDto.PhaseSequence = instantaneousProfileThreePhaseCT.PhaseSequence;
                instantaneousProfileThreePhaseCTDto.NicSignalPower = instantaneousProfileThreePhaseCT.NicSignalPower;
                instantaneousProfileThreePhaseCTDto.NicSignalToNoiseRatio = instantaneousProfileThreePhaseCT.NicSignalToNoiseRatio;
                instantaneousProfileThreePhaseCTDto.NicCellIdentifier = instantaneousProfileThreePhaseCT.NicCellIdentifier;
                instantaneousProfileThreePhaseCTDto.LoadLimitFunctionStatus = instantaneousProfileThreePhaseCT.LoadLimitFunctionStatus;

                instantaneousProfileThreePhaseCTDto.SignedActivePowerkW = StringExtensions.CheckNullable(instantaneousProfileThreePhaseCT.SignedActivePowerkW);

                instantaneousProfileThreePhaseCTDtoList.Add(instantaneousProfileThreePhaseCTDto);
                index++;
            }

            return instantaneousProfileThreePhaseCTDtoList;
        }
    }
}
