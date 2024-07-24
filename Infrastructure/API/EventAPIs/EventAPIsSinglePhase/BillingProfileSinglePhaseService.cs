using Domain.Entities.SinglePhaseEntities;
using Domain.Interface.Service;
using Infrastructure.DTOs.SinglePhaseEventDTOs;
using Infrastructure.Helpers;
using Infrastructure.Services;
using System.Globalization;

namespace Infrastructure.API.EventAPIsSinglePhase
{
    public class BillingProfileSinglePhaseService
    {
        private readonly IDataService<BillingProfileSinglePhase> _dataService;
        public ErrorHelper _errorHelper;
        private readonly ApplicationContextFactory _contextFactory;
        public BillingProfileSinglePhaseService()
        {
            _dataService = new GenericDataService<BillingProfileSinglePhase>(new ApplicationContextFactory());
            _errorHelper = new ErrorHelper();
            _contextFactory = new ApplicationContextFactory();
        }
        public async Task<bool> Add(List<BillingProfileSinglePhase> billingProfileProfile)
        {
            try
            {
                await Delete(billingProfileProfile.FirstOrDefault().MeterNo);
                return await _dataService.CreateRange(billingProfileProfile);
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
                    string query = "select * from BillingProfileSinglePhase where MeterNo = '" + meterNumber + "' ORDER by Id DESC";

                    var res = await _dataService.Filter(query);

                    if (res.Any())
                    {
                        db.Set<BillingProfileSinglePhase>().RemoveRange(res);
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

        public async Task<List<BillingProfileSinglePhaseDto>> GetAll(int PageSize, string meterNumber)
        {
            try
            {
                string query = "select * from BillingProfileSinglePhase where MeterNo = '" + meterNumber + "' ORDER by Id DESC LIMIT " + PageSize;

                var response = await _dataService.Filter(query);

                List<BillingProfileSinglePhaseDto> billingProfileProfileSinglePhase = await ParseDataToDTO(response);

                return billingProfileProfileSinglePhase;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);

            }
        }

        public async Task<List<BillingProfileSinglePhaseDto>> Filter(string startDate, string endDate, string fatchDate, int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from BillingProfileSinglePhase where MeterNo = '" + meterNumber + "'";

                var response = await _dataService.Filter(query);

                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    var startDateTime = DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    var endDateTime = DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    response = response.Where(x =>
                        DateTime.ParseExact(x.RealTimeClock, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).Date >= startDateTime.Date &&
                        DateTime.ParseExact(x.RealTimeClock, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture).Date <= endDateTime.Date
                    ).Take(pageSize).ToList();
                }
                else if (!string.IsNullOrEmpty(fatchDate))
                {
                    response = response.Where(x =>
                      x.CreatedOn == fatchDate).Take(pageSize).ToList();
                }
                else
                {
                    response = response.Take(pageSize).ToList();
                }

                List<BillingProfileSinglePhaseDto> billingProfileProfileSinglePhase = await ParseDataToDTO(response);

                if (billingProfileProfileSinglePhase.Count > 0)
                {
                    int currentMonth = DateTime.Now.Month;
                    int currentYear = DateTime.Now.Year;
                }

                return billingProfileProfileSinglePhase;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<BillingProfileSinglePhase>> ParseDataToClass(List<BillingProfileSinglePhaseDto> billingProfileProfileSinglePhaseDtoList)
        {
            List<BillingProfileSinglePhase> billingProfileProfileSinglePhaseList = new List<BillingProfileSinglePhase>();

            foreach (var billingProfileProfileSinglePhaseDto in billingProfileProfileSinglePhaseDtoList)
            {
                BillingProfileSinglePhase billingProfileProfileSinglePhase = new BillingProfileSinglePhase();

                billingProfileProfileSinglePhase.MeterNo = billingProfileProfileSinglePhaseDto.MeterNo;
                billingProfileProfileSinglePhase.CreatedOn = billingProfileProfileSinglePhaseDto.CreatedOn;
                billingProfileProfileSinglePhase.RealTimeClock = billingProfileProfileSinglePhaseDto.RealTimeClock;
                billingProfileProfileSinglePhase.AveragePowerFactor = billingProfileProfileSinglePhaseDto.AveragePowerFactor;
                billingProfileProfileSinglePhase.CumulativeEnergykWhImport = billingProfileProfileSinglePhaseDto.CumulativeEnergykWhImport;
                billingProfileProfileSinglePhase.CumulativeEnergykWhTZ1 = billingProfileProfileSinglePhaseDto.CumulativeEnergykWhTZ1;
                billingProfileProfileSinglePhase.CumulativeEnergykWhTZ2 = billingProfileProfileSinglePhaseDto.CumulativeEnergykWhTZ2;
                billingProfileProfileSinglePhase.CumulativeEnergykWhTZ3 = billingProfileProfileSinglePhaseDto.CumulativeEnergykWhTZ3;
                billingProfileProfileSinglePhase.CumulativeEnergykWhTZ4 = billingProfileProfileSinglePhaseDto.CumulativeEnergykWhTZ4;
                billingProfileProfileSinglePhase.CumulativeEnergykWhTZ5 = billingProfileProfileSinglePhaseDto.CumulativeEnergykWhTZ5;
                billingProfileProfileSinglePhase.CumulativeEnergykWhTZ6 = billingProfileProfileSinglePhaseDto.CumulativeEnergykWhTZ6;
                billingProfileProfileSinglePhase.CumulativeEnergykWhTZ7 = billingProfileProfileSinglePhaseDto.CumulativeEnergykWhTZ7;
                billingProfileProfileSinglePhase.CumulativeEnergykWhTZ8 = billingProfileProfileSinglePhaseDto.CumulativeEnergykWhTZ8;
                billingProfileProfileSinglePhase.CumulativeEnergykVAhImport = billingProfileProfileSinglePhaseDto.CumulativeEnergykVAhImport;
                billingProfileProfileSinglePhase.CumulativeEnergykVAhTZ1 = billingProfileProfileSinglePhaseDto.CumulativeEnergykVAhTZ1;
                billingProfileProfileSinglePhase.CumulativeEnergykVAhTZ2 = billingProfileProfileSinglePhaseDto.CumulativeEnergykVAhTZ2;
                billingProfileProfileSinglePhase.CumulativeEnergykVAhTZ3 = billingProfileProfileSinglePhaseDto.CumulativeEnergykVAhTZ3;
                billingProfileProfileSinglePhase.CumulativeEnergykVAhTZ4 = billingProfileProfileSinglePhaseDto.CumulativeEnergykVAhTZ4;
                billingProfileProfileSinglePhase.CumulativeEnergykVAhTZ5 = billingProfileProfileSinglePhaseDto.CumulativeEnergykVAhTZ5;
                billingProfileProfileSinglePhase.CumulativeEnergykVAhTZ6 = billingProfileProfileSinglePhaseDto.CumulativeEnergykVAhTZ6;
                billingProfileProfileSinglePhase.CumulativeEnergykVAhTZ7 = billingProfileProfileSinglePhaseDto.CumulativeEnergykVAhTZ7;
                billingProfileProfileSinglePhase.CumulativeEnergykVAhTZ8 = billingProfileProfileSinglePhaseDto.CumulativeEnergykVAhTZ8;
                billingProfileProfileSinglePhase.MaximumDemandkW = billingProfileProfileSinglePhaseDto.MaximumDemandkW;
                billingProfileProfileSinglePhase.MaximumDemandkWDateTime = billingProfileProfileSinglePhaseDto.MaximumDemandkWDateTime;
                billingProfileProfileSinglePhase.MaximumDemandkVA = billingProfileProfileSinglePhaseDto.MaximumDemandkVA;
                billingProfileProfileSinglePhase.MaximumDemandkVADateTime = billingProfileProfileSinglePhaseDto.MaximumDemandkVADateTime;
                billingProfileProfileSinglePhase.BillingPowerONdurationinMinutes = billingProfileProfileSinglePhaseDto.BillingPowerONdurationinMinutes;
                billingProfileProfileSinglePhase.CumulativeEnergykWhExport = billingProfileProfileSinglePhaseDto.CumulativeEnergykWhExport;
                billingProfileProfileSinglePhase.CumulativeEnergykVAhExport = billingProfileProfileSinglePhaseDto.CumulativeEnergykVAhExport;

                billingProfileProfileSinglePhaseList.Add(billingProfileProfileSinglePhase);
            }

            return billingProfileProfileSinglePhaseList;
        }

        private async Task<List<BillingProfileSinglePhaseDto>> ParseDataToDTO(List<BillingProfileSinglePhase> BillingProfileSinglePhaseList)
        {
            string dateFormat = "dd-MM-yyyy HH:mm:ss";

            //BillingProfileSinglePhaseList = BillingProfileSinglePhaseList.OrderBy(y => DateTime.ParseExact(y.RealTimeClock, dateFormat, CultureInfo.InvariantCulture)).GroupBy(y => new { DateTime.ParseExact(y.RealTimeClock, dateFormat, CultureInfo.InvariantCulture).Month, DateTime.ParseExact(y.RealTimeClock, dateFormat, CultureInfo.InvariantCulture).Year }).Select(group => group.First()).ToList();
            var item = BillingProfileSinglePhaseList.OrderByDescending(y => DateTime.ParseExact(y.RealTimeClock, dateFormat, CultureInfo.InvariantCulture)).FirstOrDefault(x => DateTime.ParseExact(x.RealTimeClock, dateFormat, CultureInfo.InvariantCulture).Month == DateTime.UtcNow.Month);
            if(item != null)
            {
                BillingProfileSinglePhaseList.Remove(item);
            }

            int index = 1;
            List<BillingProfileSinglePhaseDto> BillingProfileSinglePhaseDtoList = new List<BillingProfileSinglePhaseDto>();
            foreach (var BillingProfileSinglePhase in BillingProfileSinglePhaseList)
            {
                BillingProfileSinglePhaseDto BillingProfileSinglePhaseDto = new BillingProfileSinglePhaseDto();

                BillingProfileSinglePhaseDto.Number = index;
                BillingProfileSinglePhaseDto.MeterNo = BillingProfileSinglePhase.MeterNo;
                BillingProfileSinglePhaseDto.CreatedOn = BillingProfileSinglePhase.CreatedOn;
                BillingProfileSinglePhaseDto.RealTimeClock = BillingProfileSinglePhase.RealTimeClock;
                BillingProfileSinglePhaseDto.AveragePowerFactor = StringExtensions.CheckNullableOnly(BillingProfileSinglePhase.AveragePowerFactor);

                BillingProfileSinglePhaseDto.CumulativeEnergykWhImport = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykWhImport, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykWhTZ1 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykWhTZ1, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykWhTZ2 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykWhTZ2, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykWhTZ3 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykWhTZ3, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykWhTZ4 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykWhTZ4, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykWhTZ5 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykWhTZ5, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykWhTZ6 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykWhTZ6, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykWhTZ7 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykWhTZ7, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykWhTZ8 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykWhTZ8, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykVAhImport = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykVAhImport, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykVAhTZ1 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykVAhTZ1, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykVAhTZ2 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykVAhTZ2, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykVAhTZ3 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykVAhTZ3, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykVAhTZ4 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykVAhTZ4, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykVAhTZ5 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykVAhTZ5, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykVAhTZ6 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykVAhTZ6, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykVAhTZ7 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykVAhTZ7, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykVAhTZ8 = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykVAhTZ8, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.MaximumDemandkW = BillingProfileSinglePhase.MaximumDemandkW != null ? Math.Round(double.Parse(BillingProfileSinglePhase.MaximumDemandkW, NumberStyles.Any) / 1000, 2).ToString() : null;
                BillingProfileSinglePhaseDto.MaximumDemandkWDateTime = BillingProfileSinglePhase.MaximumDemandkWDateTime;
                BillingProfileSinglePhaseDto.MaximumDemandkVA = BillingProfileSinglePhase.MaximumDemandkVA != null ? Math.Round(double.Parse(BillingProfileSinglePhase.MaximumDemandkVA, NumberStyles.Any) / 1000, 2).ToString() : null;
                BillingProfileSinglePhaseDto.MaximumDemandkVADateTime = BillingProfileSinglePhase.MaximumDemandkVADateTime;
                BillingProfileSinglePhaseDto.BillingPowerONdurationinMinutes = BillingProfileSinglePhase.BillingPowerONdurationinMinutes;
                BillingProfileSinglePhaseDto.CumulativeEnergykWhExport = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykWhExport, NumberStyles.Any) / 1000, 2).ToString();
                BillingProfileSinglePhaseDto.CumulativeEnergykVAhExport = Math.Round(double.Parse(BillingProfileSinglePhase.CumulativeEnergykVAhExport, NumberStyles.Any) / 1000, 2).ToString();


                BillingProfileSinglePhaseDtoList.Add(BillingProfileSinglePhaseDto);
                index++;
            }
            
            return BillingProfileSinglePhaseDtoList;
        }
    }
}
