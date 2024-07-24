using Domain.Entities;
using Domain.Interface.Service;
using Infrastructure.DTOs.EventDTOs;
using Infrastructure.Helpers;
using Infrastructure.Services;
using System.Globalization;

namespace Infrastructure.API.EventAPIs
{
    public class CurrentRelatedEventService
    {
        private readonly IDataService<CurrentRelatedEvent> _dataService;
        public ErrorHelper _errorHelper;
        private readonly ApplicationContextFactory _contextFactory;

        public CurrentRelatedEventService()
        {
            _dataService = new GenericDataService<CurrentRelatedEvent>(new ApplicationContextFactory());
            _errorHelper = new ErrorHelper();
            _contextFactory = new ApplicationContextFactory();
        }
        public async Task<bool> Add(List<CurrentRelatedEvent> currentRelatedEventModel)
        {
            try
            {
                await Delete(currentRelatedEventModel.FirstOrDefault().MeterNo);
                return await _dataService.CreateRange(currentRelatedEventModel);
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
                    string query = "select * from CurrentRelatedEvent where MeterNo = '" + meterNumber + "' ORDER by Id DESC";

                    var res = await _dataService.Filter(query);

                    if (res.Any())
                    {
                        db.Set<CurrentRelatedEvent>().RemoveRange(res);
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

        public async Task<List<CurrentRelatedEventDto>> GetAll(int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from CurrentRelatedEvent where MeterNo = '" + meterNumber + "' ORDER by Id DESC LIMIT " + pageSize;

                var response = await _dataService.Filter(query);

                var meter = await _dataService.GetByMeterNoAsync(meterNumber);

                var currentRelatedEventDto = new List<CurrentRelatedEventDto>();

                if (string.IsNullOrEmpty(meter.ManSpecificFirmwareVersion) && meter.MeterType != Domain.Enums.MeterType.ThreePhaseLTCT && meter.MeterType != Domain.Enums.MeterType.ThreePhaseHTCT)
                {
                    currentRelatedEventDto = await ParseDataToDTO(response);
                }
                else
                {
                    currentRelatedEventDto = await ParseDataToDTOUMD(response);
                }

                return currentRelatedEventDto;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<CurrentRelatedEventDto>> Filter(string startDate, string endDate, int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from CurrentRelatedEvent where MeterNo = '" + meterNumber + "'";

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

                var currentRelatedEventDto = new List<CurrentRelatedEventDto>();

                if (string.IsNullOrEmpty(meter.ManSpecificFirmwareVersion) && meter.MeterType != Domain.Enums.MeterType.ThreePhaseLTCT && meter.MeterType != Domain.Enums.MeterType.ThreePhaseHTCT)
                {
                    currentRelatedEventDto = await ParseDataToDTO(response);
                }
                else
                {
                    currentRelatedEventDto = await ParseDataToDTOUMD(response);
                }

                return currentRelatedEventDto;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<CurrentRelatedEvent>> ParseDataToClass(List<CurrentRelatedEventDto> currentRelatedEventDtoList)
        {
            List<CurrentRelatedEvent> currentRelatedEventList = new List<CurrentRelatedEvent>();

            foreach (var currentRelatedEventDto in currentRelatedEventDtoList)
            {
                CurrentRelatedEvent currentRelatedEvent = new CurrentRelatedEvent();

                currentRelatedEvent.MeterNo = currentRelatedEventDto.MeterNo;
                currentRelatedEvent.CreatedOn = currentRelatedEventDto.CreatedOn;
                currentRelatedEvent.RealTimeClockDateAndTime = currentRelatedEventDto.RealTimeClockDateAndTime;
                currentRelatedEvent.EventCode = currentRelatedEventDto.Event;
                currentRelatedEvent.CurrentIr = currentRelatedEventDto.CurrentIr;
                currentRelatedEvent.CurrentIy = currentRelatedEventDto.CurrentIy;
                currentRelatedEvent.CurrentIb = currentRelatedEventDto.CurrentIb;
                currentRelatedEvent.VoltageVrn = currentRelatedEventDto.VoltageVrn;
                currentRelatedEvent.VoltageVyn = currentRelatedEventDto.VoltageVyn;
                currentRelatedEvent.VoltageVbn = currentRelatedEventDto.VoltageVbn;
                currentRelatedEvent.SignedPowerFactorRPhase = currentRelatedEventDto.SignedPowerFactorRPhase;
                currentRelatedEvent.SignedPowerFactorYPhase = currentRelatedEventDto.SignedPowerFactorYPhase;
                currentRelatedEvent.SignedPowerFactorBPhase = currentRelatedEventDto.SignedPowerFactorBPhase;
                currentRelatedEvent.CumulativeEnergykWhImport = currentRelatedEventDto.CumulativeEnergykWhImport;
                currentRelatedEvent.CumulativeTamperCount = currentRelatedEventDto.CumulativeTamperCount;
                currentRelatedEvent.CumulativeEnergykWhExport = currentRelatedEventDto.CumulativeEnergykWhExport;
                currentRelatedEvent.NeutralCurrent = currentRelatedEventDto.NeutralCurrent;

                currentRelatedEvent.KVAHImportForwarded = currentRelatedEventDto.KVAHImportForwarded;
                currentRelatedEvent.RPhaseActiveCurrent = currentRelatedEventDto.RPhaseActiveCurrent;
                currentRelatedEvent.YPhaseActiveCurrent = currentRelatedEventDto.YPhaseActiveCurrent;
                currentRelatedEvent.BPhaseActiveCurrent = currentRelatedEventDto.BPhaseActiveCurrent;
                currentRelatedEvent.TotalPF = currentRelatedEventDto.TotalPF;
                currentRelatedEvent.KVAHExport = currentRelatedEventDto.KVAHExport;
                currentRelatedEvent.Temperature = currentRelatedEventDto.Temperature;

                currentRelatedEventList.Add(currentRelatedEvent);
            }

            return currentRelatedEventList;
        }

        private async Task<List<CurrentRelatedEventDto>> ParseDataToDTO(List<CurrentRelatedEvent> currentRelatedEventList)
        {
            int index = 1;
            List<CurrentRelatedEventDto> currentRelatedEventDtoList = new List<CurrentRelatedEventDto>();
            foreach (var currentRelatedEvent in currentRelatedEventList)
            {
                CurrentRelatedEventDto currentRelatedEventDto = new CurrentRelatedEventDto();

                currentRelatedEventDto.Number = index;
                currentRelatedEventDto.RealTimeClockDateAndTime = currentRelatedEvent.RealTimeClockDateAndTime;
                currentRelatedEventDto.Event = currentRelatedEvent.EventCode;
                currentRelatedEventDto.CurrentIr = StringExtensions.CheckNullable100(currentRelatedEvent.CurrentIr);
                currentRelatedEventDto.CurrentIy = StringExtensions.CheckNullable100(currentRelatedEvent.CurrentIy);
                currentRelatedEventDto.CurrentIb = StringExtensions.CheckNullable100(currentRelatedEvent.CurrentIb);
                currentRelatedEventDto.VoltageVrn = StringExtensions.CheckNullable10(currentRelatedEvent.VoltageVrn);
                currentRelatedEventDto.VoltageVyn = StringExtensions.CheckNullable10(currentRelatedEvent.VoltageVyn);
                currentRelatedEventDto.VoltageVbn = StringExtensions.CheckNullable10(currentRelatedEvent.VoltageVbn);
                currentRelatedEventDto.SignedPowerFactorRPhase = StringExtensions.CheckNullable100(currentRelatedEvent.SignedPowerFactorRPhase);
                currentRelatedEventDto.SignedPowerFactorYPhase = StringExtensions.CheckNullable100(currentRelatedEvent.SignedPowerFactorYPhase);
                currentRelatedEventDto.SignedPowerFactorBPhase = StringExtensions.CheckNullable100(currentRelatedEvent.SignedPowerFactorBPhase);
                currentRelatedEventDto.CumulativeEnergykWhImport = StringExtensions.CheckNullable100(currentRelatedEvent.CumulativeEnergykWhImport);
                currentRelatedEventDto.CumulativeTamperCount =  currentRelatedEvent.CumulativeTamperCount;
                currentRelatedEventDto.CumulativeEnergykWhExport = StringExtensions.CheckNullable100(currentRelatedEvent.CumulativeEnergykWhExport);
                currentRelatedEventDto.CreatedOn = currentRelatedEvent.CreatedOn;
                currentRelatedEventDto.GenericEventLogSequenceNumber = currentRelatedEvent.GenericEventLogSequenceNumber;
                currentRelatedEventDto.NeutralCurrent = currentRelatedEvent.NeutralCurrent;

                currentRelatedEventDto.KVAHImportForwarded = StringExtensions.CheckNullable100(currentRelatedEvent.KVAHImportForwarded);
                currentRelatedEventDto.RPhaseActiveCurrent = currentRelatedEvent.RPhaseActiveCurrent;
                currentRelatedEventDto.YPhaseActiveCurrent = currentRelatedEvent.YPhaseActiveCurrent;
                currentRelatedEventDto.BPhaseActiveCurrent = currentRelatedEvent.BPhaseActiveCurrent;
                currentRelatedEventDto.TotalPF = currentRelatedEvent.TotalPF;
                currentRelatedEventDto.KVAHExport = StringExtensions.CheckNullable100(currentRelatedEvent.KVAHExport);
                currentRelatedEventDto.Temperature = currentRelatedEvent.Temperature;

                currentRelatedEventDtoList.Add(currentRelatedEventDto);
                index++;
            }

            return currentRelatedEventDtoList;
        }

        private async Task<List<CurrentRelatedEventDto>> ParseDataToDTOUMD(List<CurrentRelatedEvent> currentRelatedEventList)
        {
            int index = 1;
            List<CurrentRelatedEventDto> currentRelatedEventDtoList = new List<CurrentRelatedEventDto>();
            foreach (var currentRelatedEvent in currentRelatedEventList)
            {
                CurrentRelatedEventDto currentRelatedEventDto = new CurrentRelatedEventDto();

                currentRelatedEventDto.Number = index;
                currentRelatedEventDto.RealTimeClockDateAndTime = currentRelatedEvent.RealTimeClockDateAndTime;
                currentRelatedEventDto.Event = currentRelatedEvent.EventCode;
                currentRelatedEventDto.CurrentIr = StringExtensions.CheckNullableOnly(currentRelatedEvent.CurrentIr);
                currentRelatedEventDto.CurrentIy = StringExtensions.CheckNullableOnly(currentRelatedEvent.CurrentIy);
                currentRelatedEventDto.CurrentIb = StringExtensions.CheckNullableOnly(currentRelatedEvent.CurrentIb);
                currentRelatedEventDto.VoltageVrn = StringExtensions.CheckNullableOnly(currentRelatedEvent.VoltageVrn);
                currentRelatedEventDto.VoltageVyn = StringExtensions.CheckNullableOnly(currentRelatedEvent.VoltageVyn);
                currentRelatedEventDto.VoltageVbn = StringExtensions.CheckNullableOnly(currentRelatedEvent.VoltageVbn);
                currentRelatedEventDto.SignedPowerFactorRPhase = StringExtensions.CheckNullableOnly(currentRelatedEvent.SignedPowerFactorRPhase);
                currentRelatedEventDto.SignedPowerFactorYPhase = StringExtensions.CheckNullableOnly(currentRelatedEvent.SignedPowerFactorYPhase);
                currentRelatedEventDto.SignedPowerFactorBPhase = StringExtensions.CheckNullableOnly(currentRelatedEvent.SignedPowerFactorBPhase);


                currentRelatedEventDto.CumulativeEnergykWhImport = StringExtensions.CheckNullable(currentRelatedEvent.CumulativeEnergykWhImport);
                currentRelatedEventDto.CumulativeTamperCount = currentRelatedEvent.CumulativeTamperCount;
                currentRelatedEventDto.CumulativeEnergykWhExport = StringExtensions.CheckNullable(currentRelatedEvent.CumulativeEnergykWhExport);
                currentRelatedEventDto.CreatedOn = currentRelatedEvent.CreatedOn;
                currentRelatedEventDto.GenericEventLogSequenceNumber = currentRelatedEvent.GenericEventLogSequenceNumber;
                currentRelatedEventDto.NeutralCurrent = StringExtensions.CheckNullableOnly(currentRelatedEvent.NeutralCurrent);

                currentRelatedEventDto.KVAHImportForwarded = StringExtensions.CheckNullable(currentRelatedEvent.KVAHImportForwarded);

                currentRelatedEventDto.RPhaseActiveCurrent = currentRelatedEvent.RPhaseActiveCurrent;
                currentRelatedEventDto.YPhaseActiveCurrent = currentRelatedEvent.YPhaseActiveCurrent;
                currentRelatedEventDto.BPhaseActiveCurrent = currentRelatedEvent.BPhaseActiveCurrent;
                currentRelatedEventDto.TotalPF = currentRelatedEvent.TotalPF;
                currentRelatedEventDto.KVAHExport = StringExtensions.CheckNullable(currentRelatedEvent.KVAHExport); 
                currentRelatedEventDto.Temperature = currentRelatedEvent.Temperature;

                currentRelatedEventDtoList.Add(currentRelatedEventDto);
                index++;
            }

            return currentRelatedEventDtoList;
        }
    }
}