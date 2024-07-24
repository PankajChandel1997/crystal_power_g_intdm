using Domain.Entities;
using Domain.Interface.Service;
using Infrastructure.DTOs.EventDTOs;
using Infrastructure.Helpers;
using Infrastructure.Services;
using System.Globalization;

namespace Infrastructure.API.EventAPIs
{
    public class VoltageRelatedEventService
    {
        private readonly IDataService<VoltageRelatedEvent> _dataService;
        public ErrorHelper _errorHelper;
        private readonly ApplicationContextFactory _contextFactory;

        public VoltageRelatedEventService()
        {
            _dataService = new GenericDataService<VoltageRelatedEvent>(new ApplicationContextFactory());
            _errorHelper = new ErrorHelper();
            _contextFactory = new ApplicationContextFactory();
        }
        public async Task<bool> Add(List<VoltageRelatedEvent> voltageRelatedEventModel)
        {
            try
            {
                await Delete(voltageRelatedEventModel.FirstOrDefault().MeterNo);
                return await _dataService.CreateRange(voltageRelatedEventModel);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Delete(string meterNumber)
        {
            try
            {
                using (ApplicationDBContext db = _contextFactory.CreateDbContext())
                {
                    string query = "select * from VoltageRelatedEvent where MeterNo = '" + meterNumber + "' ORDER by Id DESC";

                    var res = await _dataService.Filter(query);

                    if (res.Any())
                    {
                        db.Set<VoltageRelatedEvent>().RemoveRange(res);
                        await db.SaveChangesAsync();
                    }
                };
                return true;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);

            }
        }
        public async Task<List<VoltageRelatedEventDto>> GetAll(int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from VoltageRelatedEvent where MeterNo = '" + meterNumber + "'";

                var response = await _dataService.Filter(query);

                var meter = await _dataService.GetByMeterNoAsync(meterNumber);

                var voltageRelatedEventDto = new List<VoltageRelatedEventDto>();

                if (string.IsNullOrEmpty(meter.ManSpecificFirmwareVersion) && meter.MeterType != Domain.Enums.MeterType.ThreePhaseLTCT && meter.MeterType != Domain.Enums.MeterType.ThreePhaseHTCT)
                {
                    voltageRelatedEventDto = await ParseDataToDTO(response);
                }
                else
                {
                    voltageRelatedEventDto = await ParseDataToDTOUMD(response);
                }

                return voltageRelatedEventDto;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<VoltageRelatedEventDto>> Filter(string startDate, string endDate, int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from VoltageRelatedEvent where MeterNo = '" + meterNumber + "' order by RealTimeClockDateAndTime asc";

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
                else
                {
                    response = response.Take(pageSize).ToList();
                }

                var meter = await _dataService.GetByMeterNoAsync(meterNumber);

                var voltageRelatedEventDto = new List<VoltageRelatedEventDto>();

                if (string.IsNullOrEmpty(meter.ManSpecificFirmwareVersion) && meter.MeterType != Domain.Enums.MeterType.ThreePhaseLTCT && meter.MeterType != Domain.Enums.MeterType.ThreePhaseHTCT)
                {
                    voltageRelatedEventDto = await ParseDataToDTO(response);
                }
                else
                {
                    voltageRelatedEventDto = await ParseDataToDTOUMD(response);
                }

                return voltageRelatedEventDto;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);

                throw new Exception(ex.Message);
            }
        }

        //private async Task<List<VoltageRelatedEvent>> ParseDataToClass(List<VoltageRelatedEventDto> voltageRelatedEventDtoList)
        //{
        //    List<VoltageRelatedEvent> voltageRelatedEventList = new List<VoltageRelatedEvent>();

        //    foreach (var voltageRelatedEventDto in voltageRelatedEventDtoList)
        //    {
        //        VoltageRelatedEvent voltageRelatedEvent = new VoltageRelatedEvent();

        //        voltageRelatedEvent.MeterNo = voltageRelatedEventDto.MeterNo;
        //        voltageRelatedEvent.CreatedOn = voltageRelatedEventDto.CreatedOn;
        //        voltageRelatedEvent.RealTimeClockDateAndTime = voltageRelatedEventDto.RealTimeClockDateAndTime;
        //        voltageRelatedEvent.EventCode = voltageRelatedEventDto.Event;
        //        voltageRelatedEvent.CurrentIr = voltageRelatedEventDto.CurrentIr;
        //        voltageRelatedEvent.CurrentIy = voltageRelatedEventDto.CurrentIy;
        //        voltageRelatedEvent.CurrentIb = voltageRelatedEventDto.CurrentIb;
        //        voltageRelatedEvent.VoltageVrn = voltageRelatedEventDto.VoltageVrn;
        //        voltageRelatedEvent.VoltageVyn = voltageRelatedEventDto.VoltageVyn;
        //        voltageRelatedEvent.VoltageVbn = voltageRelatedEventDto.VoltageVbn;
        //        voltageRelatedEvent.SignedPowerFactorRPhase = voltageRelatedEventDto.SignedPowerFactorRPhase;
        //        voltageRelatedEvent.SignedPowerFactorYPhase = voltageRelatedEventDto.SignedPowerFactorYPhase;
        //        voltageRelatedEvent.SignedPowerFactorBPhase = voltageRelatedEventDto.SignedPowerFactorBPhase;
        //        voltageRelatedEvent.CumulativeEnergykWhImport = voltageRelatedEventDto.CumulativeEnergykWhImport;
        //        voltageRelatedEvent.CumulativeTamperCount = voltageRelatedEventDto.CumulativeTamperCount;
        //        voltageRelatedEvent.CumulativeEnergykWhExport = voltageRelatedEventDto.CumulativeEnergykWhExport;
        //        voltageRelatedEvent.GenericEventLogSequenceNumber = voltageRelatedEventDto.GenericEventLogSequenceNumber;
        //        voltageRelatedEvent.NeutralCurrent = voltageRelatedEventDto.NeutralCurrent;

        //        voltageRelatedEvent.KVAHImportForwarded = voltageRelatedEventDto.KVAHImportForwarded;
        //        voltageRelatedEvent.RPhaseActiveCurrent = voltageRelatedEventDto.RPhaseActiveCurrent;
        //        voltageRelatedEvent.YPhaseActiveCurrent = voltageRelatedEventDto.YPhaseActiveCurrent;
        //        voltageRelatedEvent.BPhaseActiveCurrent = voltageRelatedEventDto.BPhaseActiveCurrent;
        //        voltageRelatedEvent.TotalPF = voltageRelatedEventDto.TotalPF;
        //        voltageRelatedEvent.KVAHExport = voltageRelatedEventDto.KVAHExport;
        //        voltageRelatedEvent.Temperature = voltageRelatedEventDto.Temperature;

        //        voltageRelatedEventList.Add(voltageRelatedEvent);
        //    }

        //    return voltageRelatedEventList;
        //}

        private async Task<List<VoltageRelatedEventDto>> ParseDataToDTO(List<VoltageRelatedEvent> voltageRelatedEventList)
        {
            int index = 1;
            List<VoltageRelatedEventDto> voltageRelatedEventDtoList = new List<VoltageRelatedEventDto>();
            foreach (var voltageRelatedEvent in voltageRelatedEventList)
            {
                VoltageRelatedEventDto voltageRelatedEventDto = new VoltageRelatedEventDto();

                voltageRelatedEventDto.Number = index;
                voltageRelatedEventDto.RealTimeClockDateAndTime = voltageRelatedEvent.RealTimeClockDateAndTime;
                voltageRelatedEventDto.Event = voltageRelatedEvent.EventCode;
                voltageRelatedEventDto.CurrentIr = StringExtensions.CheckNullableOnly(voltageRelatedEvent.CurrentIr);
                voltageRelatedEventDto.CurrentIy = StringExtensions.CheckNullableOnly(voltageRelatedEvent.CurrentIy);
                voltageRelatedEventDto.CurrentIb = StringExtensions.CheckNullableOnly(voltageRelatedEvent.CurrentIb);
                voltageRelatedEventDto.VoltageVrn = StringExtensions.CheckNullableOnly(voltageRelatedEvent.VoltageVrn);
                voltageRelatedEventDto.VoltageVyn = StringExtensions.CheckNullableOnly(voltageRelatedEvent.VoltageVyn);
                voltageRelatedEventDto.VoltageVbn = StringExtensions.CheckNullableOnly(voltageRelatedEvent.VoltageVbn);
                voltageRelatedEventDto.SignedPowerFactorRPhase = StringExtensions.CheckNullableOnly(voltageRelatedEvent.SignedPowerFactorRPhase);
                voltageRelatedEventDto.SignedPowerFactorYPhase = StringExtensions.CheckNullableOnly(voltageRelatedEvent.SignedPowerFactorYPhase);
                voltageRelatedEventDto.SignedPowerFactorBPhase = StringExtensions.CheckNullableOnly(voltageRelatedEvent.SignedPowerFactorBPhase);

               voltageRelatedEventDto.CumulativeEnergykWhImport = StringExtensions.CheckNullable(voltageRelatedEvent.CumulativeEnergykWhImport);

                voltageRelatedEventDto.CumulativeTamperCount = voltageRelatedEvent.CumulativeTamperCount;
                voltageRelatedEventDto.CumulativeEnergykWhExport = StringExtensions.CheckNullable(voltageRelatedEvent.CumulativeEnergykWhExport); // (double.Parse(voltageRelatedEvent.CumulativeEnergykWhExport, System.Globalization.NumberStyles.Any) / 1000).ToString().CustomTrucate();
                voltageRelatedEventDto.CreatedOn = voltageRelatedEvent.CreatedOn;
                voltageRelatedEventDto.GenericEventLogSequenceNumber = voltageRelatedEvent.GenericEventLogSequenceNumber;
                voltageRelatedEventDto.NeutralCurrent = voltageRelatedEvent.NeutralCurrent;

                voltageRelatedEventDto.KVAHImportForwarded = StringExtensions.CheckNullable100(voltageRelatedEvent.KVAHImportForwarded); // (double.Parse(voltageRelatedEvent.KVAHImportForwarded, System.Globalization.NumberStyles.Any) / 100).ToString().CustomTrucate();
                voltageRelatedEventDto.RPhaseActiveCurrent = voltageRelatedEvent.RPhaseActiveCurrent;
                voltageRelatedEventDto.YPhaseActiveCurrent = voltageRelatedEvent.YPhaseActiveCurrent;
                voltageRelatedEventDto.BPhaseActiveCurrent = voltageRelatedEvent.BPhaseActiveCurrent;
                voltageRelatedEventDto.TotalPF = voltageRelatedEvent.TotalPF;
                voltageRelatedEventDto.KVAHExport = StringExtensions.CheckNullable100(voltageRelatedEvent.KVAHExport);// double.Parse(voltageRelatedEvent.KVAHExport, System.Globalization.NumberStyles.Any) / 100).ToString().CustomTrucate();
                voltageRelatedEventDto.Temperature = voltageRelatedEvent.Temperature;

                voltageRelatedEventDtoList.Add(voltageRelatedEventDto);
                index++;
            }

            return voltageRelatedEventDtoList;
        }

        private async Task<List<VoltageRelatedEventDto>> ParseDataToDTOUMD(List<VoltageRelatedEvent> voltageRelatedEventList)
        {
            int index = 1;
            List<VoltageRelatedEventDto> voltageRelatedEventDtoList = new List<VoltageRelatedEventDto>();
            foreach (var voltageRelatedEvent in voltageRelatedEventList)
            {
                VoltageRelatedEventDto voltageRelatedEventDto = new VoltageRelatedEventDto();

                voltageRelatedEventDto.Number = index;
                voltageRelatedEventDto.RealTimeClockDateAndTime = voltageRelatedEvent.RealTimeClockDateAndTime;
                voltageRelatedEventDto.Event = voltageRelatedEvent.EventCode;
                voltageRelatedEventDto.CurrentIr = StringExtensions.CheckNullableOnly(voltageRelatedEvent.CurrentIr);
                voltageRelatedEventDto.CurrentIr = StringExtensions.CheckNullableOnly(voltageRelatedEvent.CurrentIr);
                voltageRelatedEventDto.CurrentIy = StringExtensions.CheckNullableOnly(voltageRelatedEvent.CurrentIy);
                voltageRelatedEventDto.CurrentIb = StringExtensions.CheckNullableOnly(voltageRelatedEvent.CurrentIb);
                voltageRelatedEventDto.VoltageVrn = StringExtensions.CheckNullableOnly(voltageRelatedEvent.VoltageVrn);
                voltageRelatedEventDto.VoltageVyn = StringExtensions.CheckNullableOnly(voltageRelatedEvent.VoltageVyn);
                voltageRelatedEventDto.VoltageVbn = StringExtensions.CheckNullableOnly(voltageRelatedEvent.VoltageVbn);
                voltageRelatedEventDto.SignedPowerFactorRPhase = StringExtensions.CheckNullableOnly(voltageRelatedEvent.SignedPowerFactorRPhase);
                voltageRelatedEventDto.SignedPowerFactorYPhase = StringExtensions.CheckNullableOnly(voltageRelatedEvent.SignedPowerFactorYPhase);
                voltageRelatedEventDto.SignedPowerFactorBPhase = StringExtensions.CheckNullableOnly(voltageRelatedEvent.SignedPowerFactorBPhase);

                voltageRelatedEventDto.CumulativeEnergykWhImport = StringExtensions.CheckNullable(voltageRelatedEvent.CumulativeEnergykWhImport);
                voltageRelatedEventDto.CumulativeTamperCount = voltageRelatedEvent.CumulativeTamperCount;
                voltageRelatedEventDto.CumulativeEnergykWhExport = StringExtensions.CheckNullable(voltageRelatedEvent.CumulativeEnergykWhExport);
                voltageRelatedEventDto.CreatedOn = voltageRelatedEvent.CreatedOn;
                voltageRelatedEventDto.GenericEventLogSequenceNumber = voltageRelatedEvent.GenericEventLogSequenceNumber;
                voltageRelatedEventDto.NeutralCurrent = voltageRelatedEvent.NeutralCurrent;

                voltageRelatedEventDto.KVAHImportForwarded = StringExtensions.CheckNullable(voltageRelatedEvent.KVAHImportForwarded);
                voltageRelatedEventDto.RPhaseActiveCurrent = voltageRelatedEvent.RPhaseActiveCurrent;
                voltageRelatedEventDto.YPhaseActiveCurrent = voltageRelatedEvent.YPhaseActiveCurrent;
                voltageRelatedEventDto.BPhaseActiveCurrent = voltageRelatedEvent.BPhaseActiveCurrent;
                voltageRelatedEventDto.TotalPF = voltageRelatedEvent.TotalPF;
                voltageRelatedEventDto.KVAHExport = StringExtensions.CheckNullable(voltageRelatedEvent.KVAHExport); 
                voltageRelatedEventDto.Temperature = voltageRelatedEvent.Temperature;

                voltageRelatedEventDtoList.Add(voltageRelatedEventDto);
                index++;
            }

            return voltageRelatedEventDtoList;
        }
    }
}