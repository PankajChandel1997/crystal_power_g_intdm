using Domain.Entities;
using Domain.Interface.Service;
using Infrastructure.DTOs.EventDTOs;
using Infrastructure.Helpers;
using Infrastructure.Services;
using System.Globalization;

namespace Infrastructure.API.EventAPIs
{
    public class OtherEventService
    {
        private readonly IDataService<OtherEvent> _dataService;
        public ErrorHelper _errorHelper;
        private readonly ApplicationContextFactory _contextFactory;

        public OtherEventService()
        {
            _dataService = new GenericDataService<OtherEvent>(new ApplicationContextFactory());
            _errorHelper = new ErrorHelper();
            _contextFactory = new ApplicationContextFactory();
        }
        public async Task<bool> Add(List<OtherEvent> otherEventModel)
        {
            try
            {
                await Delete(otherEventModel.FirstOrDefault().MeterNo);
                return await _dataService.CreateRange(otherEventModel);
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
                    string query = "select * from OtherEvent where MeterNo = '" + meterNumber + "' ORDER by Id DESC";

                    var res = await _dataService.Filter(query);

                    if (res.Any())
                    {
                        db.Set<OtherEvent>().RemoveRange(res);
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
        public async Task<List<OtherEventDto>> GetAll(int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from OtherEvent where MeterNo = '" + meterNumber + "' ORDER by Id DESC LIMIT " + pageSize;

                var response = await _dataService.Filter(query);

                var meter = await _dataService.GetByMeterNoAsync(meterNumber);

                var otherEventDto = new List<OtherEventDto>();

                if (string.IsNullOrEmpty(meter.ManSpecificFirmwareVersion) && meter.MeterType != Domain.Enums.MeterType.ThreePhaseLTCT && meter.MeterType != Domain.Enums.MeterType.ThreePhaseHTCT)
                {
                    otherEventDto = await ParseDataToDTO(response);
                }
                else
                {
                    otherEventDto = await ParseDataToDTOUMD(response);
                }

                return otherEventDto;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<OtherEventDto>> Filter(string startDate, string endDate, int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from OtherEvent where MeterNo = '" + meterNumber + "' order by RealTimeClockDateAndTime desc";

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

                var otherEventDto = new List<OtherEventDto>();

                if (string.IsNullOrEmpty(meter.ManSpecificFirmwareVersion) && meter.MeterType != Domain.Enums.MeterType.ThreePhaseLTCT && meter.MeterType != Domain.Enums.MeterType.ThreePhaseHTCT)
                {
                    otherEventDto = await ParseDataToDTO(response);
                }
                else
                {
                    otherEventDto = await ParseDataToDTOUMD(response);
                }

                return otherEventDto;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<OtherEvent>> ParseDataToClass(List<OtherEventDto> otherEventDtoList)
        {
            List<OtherEvent> otherEventList = new List<OtherEvent>();

            foreach (var otherEventDto in otherEventDtoList)
            {
                OtherEvent otherEvent = new OtherEvent();

                otherEvent.MeterNo = otherEventDto.MeterNo;
                otherEvent.CreatedOn = otherEventDto.CreatedOn;
                otherEvent.RealTimeClockDateAndTime = otherEventDto.RealTimeClockDateAndTime;
                otherEvent.EventCode = otherEventDto.Event;
                otherEvent.CurrentIr = otherEventDto.CurrentIr;
                otherEvent.CurrentIy = otherEventDto.CurrentIy;
                otherEvent.CurrentIb = otherEventDto.CurrentIb;
                otherEvent.VoltageVrn = otherEventDto.VoltageVrn;
                otherEvent.VoltageVyn = otherEventDto.VoltageVyn;
                otherEvent.VoltageVbn = otherEventDto.VoltageVbn;
                otherEvent.SignedPowerFactorRPhase = otherEventDto.SignedPowerFactorRPhase;
                otherEvent.SignedPowerFactorYPhase = otherEventDto.SignedPowerFactorYPhase;
                otherEvent.SignedPowerFactorBPhase = otherEventDto.SignedPowerFactorBPhase;
                otherEvent.CumulativeEnergykWhImport = otherEventDto.CumulativeEnergykWhImport;
                otherEvent.CumulativeTamperCount = otherEventDto.CumulativeTamperCount;
                otherEvent.CumulativeEnergykWhExport = otherEventDto.CumulativeEnergykWhExport;
                otherEvent.NeutralCurrent = otherEventDto.NuetralCurrent;

                otherEvent.KVAHImportForwarded = otherEventDto.KVAHImportForwarded;
                otherEvent.RPhaseActiveCurrent = otherEventDto.RPhaseActiveCurrent;
                otherEvent.YPhaseActiveCurrent = otherEventDto.YPhaseActiveCurrent;
                otherEvent.BPhaseActiveCurrent = otherEventDto.BPhaseActiveCurrent;
                otherEvent.TotalPF = otherEventDto.TotalPF;
                otherEvent.KVAHExport = otherEventDto.KVAHExport;
                otherEvent.Temperature = otherEventDto.Temperature;

                otherEventList.Add(otherEvent);
            }

            return otherEventList;
        }

        private async Task<List<OtherEventDto>> ParseDataToDTO(List<OtherEvent> otherEventList)
        {
            int index = 1;
            List<OtherEventDto> otherEventDtoList = new List<OtherEventDto>();
            foreach (var otherEvent in otherEventList)
            {
                OtherEventDto otherEventDto = new OtherEventDto();

                otherEventDto.Number = index;
                otherEventDto.MeterNo = otherEvent.MeterNo;
                otherEventDto.CreatedOn = otherEvent.CreatedOn;
                otherEventDto.RealTimeClockDateAndTime = otherEvent.RealTimeClockDateAndTime;
                otherEventDto.Event = otherEvent.EventCode;
                otherEventDto.CurrentIr = StringExtensions.CheckNullableOnly(otherEvent.CurrentIr);
                otherEventDto.CurrentIy = StringExtensions.CheckNullableOnly(otherEvent.CurrentIy);
                otherEventDto.CurrentIb = StringExtensions.CheckNullableOnly(otherEvent.CurrentIb);
                otherEventDto.VoltageVrn = StringExtensions.CheckNullableOnly(otherEvent.VoltageVrn);
                otherEventDto.VoltageVyn = StringExtensions.CheckNullableOnly(otherEvent.VoltageVyn);
                otherEventDto.VoltageVbn = StringExtensions.CheckNullableOnly(otherEvent.VoltageVbn);
                otherEventDto.SignedPowerFactorRPhase = StringExtensions.CheckNullableOnly(otherEvent.SignedPowerFactorRPhase);
                otherEventDto.SignedPowerFactorYPhase = StringExtensions.CheckNullableOnly(otherEvent.SignedPowerFactorYPhase);
                otherEventDto.SignedPowerFactorBPhase = StringExtensions.CheckNullableOnly(otherEvent.SignedPowerFactorBPhase);



                otherEventDto.KVAHImportForwarded = StringExtensions.CheckNullable100(otherEvent.KVAHImportForwarded);
                otherEventDto.RPhaseActiveCurrent = otherEvent.RPhaseActiveCurrent;
                otherEventDto.YPhaseActiveCurrent = otherEvent.YPhaseActiveCurrent;
                otherEventDto.BPhaseActiveCurrent = otherEvent.BPhaseActiveCurrent;
                otherEventDto.TotalPF = otherEvent.TotalPF;
                otherEventDto.KVAHExport = StringExtensions.CheckNullable100(otherEvent.KVAHExport);
                otherEventDto.Temperature = otherEvent.Temperature;

                otherEventDtoList.Add(otherEventDto);
                index++;
            }

            return otherEventDtoList;
        }

        private async Task<List<OtherEventDto>> ParseDataToDTOUMD(List<OtherEvent> otherEventList)
        {
            int index = 1;
            List<OtherEventDto> otherEventDtoList = new List<OtherEventDto>();
            foreach (var otherEvent in otherEventList)
            {
                OtherEventDto otherEventDto = new OtherEventDto();

                otherEventDto.Number = index;
                otherEventDto.MeterNo = otherEvent.MeterNo;
                otherEventDto.CreatedOn = otherEvent.CreatedOn;
                otherEventDto.RealTimeClockDateAndTime = otherEvent.RealTimeClockDateAndTime;
                otherEventDto.Event = otherEvent.EventCode;
                otherEventDto.CurrentIr = StringExtensions.CheckNullableOnly(otherEvent.CurrentIr); 
                otherEventDto.CurrentIy = StringExtensions.CheckNullableOnly(otherEvent.CurrentIy);
                otherEventDto.CurrentIb = StringExtensions.CheckNullableOnly(otherEvent.CurrentIb);
                otherEventDto.VoltageVrn = StringExtensions.CheckNullableOnly(otherEvent.VoltageVrn);
                otherEventDto.VoltageVyn = StringExtensions.CheckNullableOnly(otherEvent.VoltageVyn);
                otherEventDto.VoltageVbn = StringExtensions.CheckNullableOnly(otherEvent.VoltageVbn);
                otherEventDto.SignedPowerFactorRPhase = StringExtensions.CheckNullableOnly(otherEvent.SignedPowerFactorRPhase);
                otherEventDto.SignedPowerFactorYPhase = StringExtensions.CheckNullableOnly(otherEvent.SignedPowerFactorYPhase);
                otherEventDto.SignedPowerFactorBPhase = StringExtensions.CheckNullableOnly(otherEvent.SignedPowerFactorBPhase);

                otherEventDto.CumulativeEnergykWhImport = StringExtensions.CheckNullable(otherEvent.CumulativeEnergykWhImport); 
                otherEventDto.CumulativeTamperCount = otherEvent.CumulativeTamperCount;
                
                otherEventDto.CumulativeEnergykWhExport = StringExtensions.CheckNullable(otherEvent.CumulativeEnergykWhExport);

                otherEventDto.GenericEventLogSequenceNumber = otherEvent.GenericEventLogSequenceNumber;
                otherEventDto.NuetralCurrent = otherEvent.NeutralCurrent;

                otherEventDto.KVAHImportForwarded = StringExtensions.CheckNullable(otherEvent.KVAHImportForwarded); 
                otherEventDto.RPhaseActiveCurrent = otherEvent.RPhaseActiveCurrent;
                otherEventDto.YPhaseActiveCurrent = otherEvent.YPhaseActiveCurrent;
                otherEventDto.BPhaseActiveCurrent = otherEvent.BPhaseActiveCurrent;
                otherEventDto.TotalPF = otherEvent.TotalPF;
                otherEventDto.KVAHExport = StringExtensions.CheckNullable(otherEvent.KVAHExport); 

                otherEventDto.Temperature = otherEvent.Temperature;

                otherEventDtoList.Add(otherEventDto);
                index++;
            }

            return otherEventDtoList;
        }
    }
}