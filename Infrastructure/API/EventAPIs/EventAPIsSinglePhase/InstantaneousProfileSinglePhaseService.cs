using Domain.Entities.SinglePhaseEntities;
using Domain.Interface.Service;
using Infrastructure.DTOs.SinglePhaseEventDTOs;
using Infrastructure.Helpers;
using Infrastructure.Services;
using System.Globalization;

namespace Infrastructure.API.EventAPIsSinglePhase
{
    public class InstantaneousProfileSinglePhaseService
    {
        private readonly IDataService<InstantaneousProfileSinglePhase> _dataService;
        public ErrorHelper _errorHelper;
        private readonly ApplicationContextFactory _contextFactory;

        public InstantaneousProfileSinglePhaseService()
        {
            _dataService = new GenericDataService<InstantaneousProfileSinglePhase>(new ApplicationContextFactory());
            _errorHelper = new ErrorHelper();
            _contextFactory = new ApplicationContextFactory();
        }

        public async Task<bool> Add(List<InstantaneousProfileSinglePhase> instantaneousProfile)
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
                    string query = "select * from InstantaneousProfileSinglePhase where MeterNo = '" + meterNumber + "' ORDER by Id DESC";

                    var res = await _dataService.Filter(query);

                    if (res.Any())
                    {
                        db.Set<InstantaneousProfileSinglePhase>().RemoveRange(res);
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

        public async Task<List<InstantaneousProfileSinglePhaseDto>> GetAll(int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from InstantaneousProfileSinglePhase where MeterNo = '" + meterNumber + "' ORDER by Id DESC LIMIT " + pageSize;

                var response = await _dataService.Filter(query);

                List<InstantaneousProfileSinglePhaseDto> instantaneousProfile = await ParseDataToDTO(response);

                return instantaneousProfile;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<InstantaneousProfileSinglePhaseDto>> Filter(string startDate, string endDate, string fetchDate, int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from InstantaneousProfileSinglePhase where MeterNo = '" + meterNumber + "'";

                var response = await _dataService.Filter(query);

                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    var startDateTime = DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    var endDateTime = DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    response = response.Where(x =>
                        DateTime.ParseExact(x.Realtimeclock, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).Date >= startDateTime.Date &&
                        DateTime.ParseExact(x.Realtimeclock, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).Date <= endDateTime.Date
                    ).Take(pageSize).ToList();
                }
                else if (!string.IsNullOrEmpty(fetchDate))
                {
                    response = response.Where(x =>
                      x.Realtimeclock == fetchDate).Take(pageSize).ToList();
                }
                else
                {
                    response = response.Take(pageSize).ToList();
                }

                List<InstantaneousProfileSinglePhaseDto> instantaneousProfile = await ParseDataToDTO(response);

                return instantaneousProfile;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);

                throw new Exception(ex.Message);
            }
        }

        private async Task<List<InstantaneousProfileSinglePhase>> ParseDataToClass(List<InstantaneousProfileSinglePhaseDto> instantaneousProfileSinglePhaseDtoList)
        {
            List<InstantaneousProfileSinglePhase> instantaneousProfileSinglePhaseList = new List<InstantaneousProfileSinglePhase>();

            foreach (var instantaneousProfileSinglePhaseDto in instantaneousProfileSinglePhaseDtoList)
            {
                InstantaneousProfileSinglePhase instantaneousProfileSinglePhase = new InstantaneousProfileSinglePhase();

                instantaneousProfileSinglePhase.MeterNo = instantaneousProfileSinglePhaseDto.MeterNo;
                instantaneousProfileSinglePhase.CreatedOn = instantaneousProfileSinglePhaseDto.CreatedOn;
                instantaneousProfileSinglePhase.Realtimeclock = instantaneousProfileSinglePhaseDto.Realtimeclock;
                instantaneousProfileSinglePhase.Voltage = instantaneousProfileSinglePhaseDto.Voltage;
                instantaneousProfileSinglePhase.PhaseCurrent = instantaneousProfileSinglePhaseDto.PhaseCurrent;
                instantaneousProfileSinglePhase.NeutralCurrent = instantaneousProfileSinglePhaseDto.NeutralCurrent;
                instantaneousProfileSinglePhase.SignedPowerFactor = instantaneousProfileSinglePhaseDto.SignedPowerFactor;
                instantaneousProfileSinglePhase.FrequencyHz = instantaneousProfileSinglePhaseDto.FrequencyHz;
                instantaneousProfileSinglePhase.ApparentPowerKVA = instantaneousProfileSinglePhaseDto.ApparentPowerKVA;
                instantaneousProfileSinglePhase.ActivePowerkW = instantaneousProfileSinglePhaseDto.ActivePowerkW;
                instantaneousProfileSinglePhase.CumulativeenergykWhimport = instantaneousProfileSinglePhaseDto.CumulativeenergykWhimport;
                instantaneousProfileSinglePhase.CumulativeenergykVAhimport = instantaneousProfileSinglePhaseDto.CumulativeenergykVAhimport;
                instantaneousProfileSinglePhase.MaxumumDemandkW = instantaneousProfileSinglePhaseDto.MaxumumDemandkW;
                instantaneousProfileSinglePhase.MaxumumDemandkWdateandtime = instantaneousProfileSinglePhaseDto.MaxumumDemandkWdateandtime;
                instantaneousProfileSinglePhase.MaxumumDemandkVA = instantaneousProfileSinglePhaseDto.MaxumumDemandkVA;
                instantaneousProfileSinglePhase.MaxumumDemandkVAdateandtime = instantaneousProfileSinglePhaseDto.MaxumumDemandkVAdateandtime;
                instantaneousProfileSinglePhase.CumulativepowerONdurationinminute = instantaneousProfileSinglePhaseDto.CumulativepowerONdurationinminute;
                instantaneousProfileSinglePhase.Cumulativetampercount = instantaneousProfileSinglePhaseDto.Cumulativetampercount;
                instantaneousProfileSinglePhase.Cumulativebillingcount = instantaneousProfileSinglePhaseDto.Cumulativebillingcount;
                instantaneousProfileSinglePhase.Cumulativeprogrammingcount = instantaneousProfileSinglePhaseDto.Cumulativeprogrammingcount;
                instantaneousProfileSinglePhase.CumulativeenergykWhExport = instantaneousProfileSinglePhaseDto.CumulativeenergykWhExport;
                instantaneousProfileSinglePhase.CumulativeenergykVAhExport = instantaneousProfileSinglePhaseDto.CumulativeenergykVAhExport;
                instantaneousProfileSinglePhase.Loadlimitfunctionstatus = instantaneousProfileSinglePhaseDto.Loadlimitfunctionstatus;
                instantaneousProfileSinglePhase.LoadlimitvalueinkW = instantaneousProfileSinglePhaseDto.LoadlimitvalueinkW;


                instantaneousProfileSinglePhaseList.Add(instantaneousProfileSinglePhase);
            }


            return instantaneousProfileSinglePhaseList;
        }

        private async Task<List<InstantaneousProfileSinglePhaseDto>> ParseDataToDTO(List<InstantaneousProfileSinglePhase> instantaneousProfileSinglePhaseList)
        {
            int index = 1;
            List<InstantaneousProfileSinglePhaseDto> instantaneousProfileSinglePhaseDtoList = new List<InstantaneousProfileSinglePhaseDto>();
            foreach (var instantaneousProfileSinglePhase in instantaneousProfileSinglePhaseList)
            {
                InstantaneousProfileSinglePhaseDto instantaneousProfileSinglePhaseDto = new InstantaneousProfileSinglePhaseDto();

                instantaneousProfileSinglePhaseDto.Number = index;
                instantaneousProfileSinglePhaseDto.MeterNo = instantaneousProfileSinglePhase.MeterNo;
                instantaneousProfileSinglePhaseDto.CreatedOn = instantaneousProfileSinglePhase.CreatedOn;
                instantaneousProfileSinglePhaseDto.Realtimeclock = instantaneousProfileSinglePhase.Realtimeclock;
                instantaneousProfileSinglePhaseDto.Voltage = Math.Round(double.Parse(instantaneousProfileSinglePhase.Voltage, NumberStyles.Any), 2).ToString();
                instantaneousProfileSinglePhaseDto.PhaseCurrent = Math.Round(double.Parse(instantaneousProfileSinglePhase.PhaseCurrent, NumberStyles.Any), 2) == 0 ? "0" : Math.Round(double.Parse(instantaneousProfileSinglePhase.PhaseCurrent, NumberStyles.Any), 2).ToString();
                instantaneousProfileSinglePhaseDto.NeutralCurrent = Math.Round(double.Parse(instantaneousProfileSinglePhase.NeutralCurrent, NumberStyles.Any), 2) == 0 ? "0" : Math.Round(double.Parse(instantaneousProfileSinglePhase.NeutralCurrent, NumberStyles.Any), 2).ToString();
                instantaneousProfileSinglePhaseDto.SignedPowerFactor = instantaneousProfileSinglePhase.SignedPowerFactor == null ? "0" : (double.Parse(instantaneousProfileSinglePhase.SignedPowerFactor, NumberStyles.Any)).ToString("0.00");
                instantaneousProfileSinglePhaseDto.FrequencyHz = Math.Round(double.Parse(instantaneousProfileSinglePhase.FrequencyHz, NumberStyles.Any), 2).ToString();
                instantaneousProfileSinglePhaseDto.ApparentPowerKVA = Math.Round(double.Parse(instantaneousProfileSinglePhase.ApparentPowerKVA, NumberStyles.Any) / 1000, 2).ToString();
                instantaneousProfileSinglePhaseDto.ActivePowerkW = Math.Round(double.Parse(instantaneousProfileSinglePhase.ActivePowerkW, NumberStyles.Any) / 1000, 2).ToString();
                instantaneousProfileSinglePhaseDto.CumulativeenergykWhimport = Math.Round(double.Parse(instantaneousProfileSinglePhase.CumulativeenergykWhimport, NumberStyles.Any) / 1000, 2).ToString();
                instantaneousProfileSinglePhaseDto.CumulativeenergykVAhimport = Math.Round(double.Parse(instantaneousProfileSinglePhase.CumulativeenergykVAhimport, NumberStyles.Any) / 1000, 2).ToString();
                instantaneousProfileSinglePhaseDto.MaxumumDemandkW = Math.Round(double.Parse(instantaneousProfileSinglePhase.MaxumumDemandkW, NumberStyles.Any) / 1000, 2).ToString();
                instantaneousProfileSinglePhaseDto.MaxumumDemandkWdateandtime = instantaneousProfileSinglePhase.MaxumumDemandkWdateandtime;
                instantaneousProfileSinglePhaseDto.MaxumumDemandkVA = Math.Round(double.Parse(instantaneousProfileSinglePhase.MaxumumDemandkVA, NumberStyles.Any) / 1000, 2).ToString();
                instantaneousProfileSinglePhaseDto.MaxumumDemandkVAdateandtime = instantaneousProfileSinglePhase.MaxumumDemandkVAdateandtime;
                instantaneousProfileSinglePhaseDto.CumulativepowerONdurationinminute = instantaneousProfileSinglePhase.CumulativepowerONdurationinminute;
                instantaneousProfileSinglePhaseDto.Cumulativetampercount = instantaneousProfileSinglePhase.Cumulativetampercount;
                instantaneousProfileSinglePhaseDto.Cumulativebillingcount = instantaneousProfileSinglePhase.Cumulativebillingcount;
                instantaneousProfileSinglePhaseDto.Cumulativeprogrammingcount = instantaneousProfileSinglePhase.Cumulativeprogrammingcount;
                instantaneousProfileSinglePhaseDto.CumulativeenergykWhExport = Math.Round(double.Parse(instantaneousProfileSinglePhase.CumulativeenergykWhExport, NumberStyles.Any) / 1000, 2).ToString();
                instantaneousProfileSinglePhaseDto.CumulativeenergykVAhExport = Math.Round(double.Parse(instantaneousProfileSinglePhase.CumulativeenergykVAhExport, NumberStyles.Any) / 1000, 2).ToString();
                instantaneousProfileSinglePhaseDto.Loadlimitfunctionstatus = instantaneousProfileSinglePhase.Loadlimitfunctionstatus;
                instantaneousProfileSinglePhaseDto.LoadlimitvalueinkW = Math.Round(double.Parse(instantaneousProfileSinglePhase.LoadlimitvalueinkW, NumberStyles.Any) / 1000, 2).ToString();

                instantaneousProfileSinglePhaseDtoList.Add(instantaneousProfileSinglePhaseDto);
                index++;
            }

            return instantaneousProfileSinglePhaseDtoList;
        }
    }
}
