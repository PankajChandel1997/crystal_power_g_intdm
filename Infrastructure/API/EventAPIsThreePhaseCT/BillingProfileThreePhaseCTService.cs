using Domain.Entities.ThreePhaseCTEntities;
using Domain.Entities.ThreePhaseEntities;
using Domain.Interface.Service;
using Infrastructure.DTOs.ThreePhaseEventCTDTOs;
using Infrastructure.Helpers;
using Infrastructure.Services;
using System.Globalization;

namespace Infrastructure.API.EventAPIThreePhaseCT
{
    public class BillingProfileThreePhaseCTService
    {
        private readonly IDataService<BillingProfileThreePhaseCT> _dataService;
        public ErrorHelper _errorHelper;
        private readonly ApplicationContextFactory _contextFactory;

        public BillingProfileThreePhaseCTService()
        {
            _dataService = new GenericDataService<BillingProfileThreePhaseCT>(new ApplicationContextFactory());
            _errorHelper = new ErrorHelper();
            _contextFactory = new ApplicationContextFactory();
        }
        public async Task<bool> Add(List<BillingProfileThreePhaseCT> billingProfileProfile)
        {
            try
            {
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
                    string query = "select * from BillingProfileThreePhaseCT where MeterNo = '" + meterNumber + "' ORDER by Id DESC";

                    var res = await _dataService.Filter(query);

                    if (res.Any())
                    {
                        db.Set<BillingProfileThreePhaseCT>().RemoveRange(res);
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

        public async Task<List<BillingProfileThreePhaseCTDto>> GetAll(int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from BillingProfileThreePhaseCT where MeterNo = '" + meterNumber + "' ORDER by Id DESC LIMIT " + pageSize;

                var response = await _dataService.Filter(query);

                List<BillingProfileThreePhaseCTDto> billingProfileProfileThreePhaseCT = await ParseDataToDTO(response);

                return billingProfileProfileThreePhaseCT;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<BillingProfileThreePhaseCTDto>> Filter(string startDate, string endDate, string fatchDate, int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from BillingProfileThreePhaseCT where MeterNo = '" + meterNumber + "'";

                var response = await _dataService.Filter(query);

                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    var startDateTime = DateTime.ParseExact(startDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                    var endDateTime = DateTime.ParseExact(endDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
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

                List<BillingProfileThreePhaseCTDto> billingProfileProfileThreePhaseCT = await ParseDataToDTO(response);

                if (billingProfileProfileThreePhaseCT.Count > 0)
                {
                    int currentMonth = DateTime.Now.Month;
                    int currentYear = DateTime.Now.Year;

                    billingProfileProfileThreePhaseCT.RemoveAt(billingProfileProfileThreePhaseCT.Count - 1);
                }

                return billingProfileProfileThreePhaseCT;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }

      

        private async Task<List<BillingProfileThreePhaseCTDto>> ParseDataToDTO(List<BillingProfileThreePhaseCT> BillingProfileThreePhaseCTList)
        {
            try
            {
                int index = 1;
                List<BillingProfileThreePhaseCTDto> BillingProfileThreePhaseCTDtoList = new List<BillingProfileThreePhaseCTDto>();
                foreach (var BillingProfileThreePhaseCT in BillingProfileThreePhaseCTList)
                {
                    BillingProfileThreePhaseCTDto BillingProfileThreePhaseCTDto = new BillingProfileThreePhaseCTDto();

                    BillingProfileThreePhaseCTDto.Number = index;
                    BillingProfileThreePhaseCTDto.MeterNo = BillingProfileThreePhaseCT.MeterNo;
                    BillingProfileThreePhaseCTDto.CreatedOn = BillingProfileThreePhaseCT.CreatedOn;
                    BillingProfileThreePhaseCTDto.RealTimeClock = BillingProfileThreePhaseCT.RealTimeClock;
                    BillingProfileThreePhaseCTDto.AveragePFForBillingPeriod = StringExtensions.CheckNullableOnly(BillingProfileThreePhaseCT.AveragePFForBillingPeriod); 
                    BillingProfileThreePhaseCTDto.BillingDate = BillingProfileThreePhaseCT.BillingDate;
                    BillingProfileThreePhaseCTDto.CumulativeEnergykWh = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykWh);

                    BillingProfileThreePhaseCTDto.CumulativeEnergykWh = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykWh);

                    BillingProfileThreePhaseCTDto.CumulativeEnergykWhTZ1 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykWhTZ1);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykWhTZ2 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykWhTZ2);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykWhTZ3 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykWhTZ3);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykWhTZ4 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykWhTZ4);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykWhTZ5 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykWhTZ5);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykWhTZ6 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykWhTZ6);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykWhTZ7 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykWhTZ7);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykWhTZ8 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykWhTZ8);

                    BillingProfileThreePhaseCTDto.CumulativeEnergykWhExport = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykWhExport);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVAhExport = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVAhExport);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVAhImport = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVAhImport);

                    BillingProfileThreePhaseCTDto.CumulativeEnergykVAhTZ1 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVAhTZ1);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVAhTZ2 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVAhTZ2);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVAhTZ3 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVAhTZ3);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVAhTZ4 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVAhTZ4);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVAhTZ5 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVAhTZ5);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVAhTZ6 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVAhTZ6);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVAhTZ7 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVAhTZ7);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVAhTZ8 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVAhTZ8);
                    BillingProfileThreePhaseCTDto.MaximumDemandkW = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkW);

                    BillingProfileThreePhaseCTDto.MaximumDemandkWDateTime = BillingProfileThreePhaseCT.MaximumDemandkWDateTime;
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ1 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkWForTZ1);
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ1DateTime = BillingProfileThreePhaseCT.MaximumDemandkWForTZ1DateTime;
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ2 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkWForTZ2);
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ2DateTime = BillingProfileThreePhaseCT.MaximumDemandkWForTZ2DateTime;
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ3 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkWForTZ3);
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ3DateTime = BillingProfileThreePhaseCT.MaximumDemandkWForTZ3DateTime;
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ4 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkWForTZ4);
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ4DateTime = BillingProfileThreePhaseCT.MaximumDemandkWForTZ4DateTime;
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ5 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkWForTZ5);
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ5DateTime = BillingProfileThreePhaseCT.MaximumDemandkWForTZ5DateTime;
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ6 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkWForTZ6);
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ6DateTime = BillingProfileThreePhaseCT.MaximumDemandkWForTZ6DateTime;
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ7 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkWForTZ7);
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ7DateTime = BillingProfileThreePhaseCT.MaximumDemandkWForTZ7DateTime;
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ8 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkWForTZ8);
                    BillingProfileThreePhaseCTDto.MaximumDemandkWForTZ8DateTime = BillingProfileThreePhaseCT.MaximumDemandkWForTZ8DateTime;
                    BillingProfileThreePhaseCTDto.MaximumDemandkVA = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkVA);
                    BillingProfileThreePhaseCTDto.MaximumDemandkVADateTime = BillingProfileThreePhaseCT.MaximumDemandkVADateTime;

                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ1 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkVAForTZ1);
                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ1DateTime = BillingProfileThreePhaseCT.MaximumDemandkVAForTZ1DateTime;

                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ2 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkVAForTZ2);
                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ2DateTime = BillingProfileThreePhaseCT.MaximumDemandkVAForTZ2DateTime;

                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ3 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkVAForTZ3);
                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ3DateTime = BillingProfileThreePhaseCT.MaximumDemandkVAForTZ3DateTime;

                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ4 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkVAForTZ4);
                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ4DateTime = BillingProfileThreePhaseCT.MaximumDemandkVAForTZ4DateTime;

                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ5 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkVAForTZ5);
                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ5DateTime = BillingProfileThreePhaseCT.MaximumDemandkVAForTZ5DateTime;

                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ6 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkVAForTZ6);
                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ6DateTime = BillingProfileThreePhaseCT.MaximumDemandkVAForTZ6DateTime;

                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ7 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkVAForTZ7);
                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ7DateTime = BillingProfileThreePhaseCT.MaximumDemandkVAForTZ7DateTime;

                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ8 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MaximumDemandkVAForTZ8);
                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ8DateTime = BillingProfileThreePhaseCT.MaximumDemandkVAForTZ8DateTime;

                    BillingProfileThreePhaseCTDto.MaximumDemandkVAForTZ8DateTime = BillingProfileThreePhaseCT.MaximumDemandkVAForTZ8DateTime;
                    BillingProfileThreePhaseCTDto.BillingPowerONdurationInMinutesDBP = BillingProfileThreePhaseCT.BillingPowerONdurationInMinutesDBP;
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVArhQ1 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVArhQ1);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVArhQ2 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVArhQ2);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVArhQ3 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVArhQ3);
                    BillingProfileThreePhaseCTDto.CumulativeEnergykVArhQ4 = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeEnergykVArhQ4);

                    BillingProfileThreePhaseCTDto.TamperCount = !string.IsNullOrEmpty(BillingProfileThreePhaseCT.TamperCount) ? BillingProfileThreePhaseCT.TamperCount : "";

                    BillingProfileThreePhaseCTDto.CumulativeMdKwImportForwarded = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeMdKwImportForwarded);
                    BillingProfileThreePhaseCTDto.CumulativeMdKvaImportForwarded = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.CumulativeMdKvaImportForwarded);
                    BillingProfileThreePhaseCTDto.BillingResetType = BillingProfileThreePhaseCT.BillingResetType;
                    BillingProfileThreePhaseCTDto.MdKwExport = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MdKwExport);
                    BillingProfileThreePhaseCTDto.MdKwExportWithDateTime = BillingProfileThreePhaseCT.MdKwExportWithDateTime;
                    BillingProfileThreePhaseCTDto.MdKvaExport = StringExtensions.CheckNullable(BillingProfileThreePhaseCT.MdKvaExport);

                    BillingProfileThreePhaseCTDto.MdKvaExportWithDateTime = BillingProfileThreePhaseCT.MdKvaExportWithDateTime;
                    BillingProfileThreePhaseCTDto.CumulativeBillingCount = BillingProfileThreePhaseCT.CumulativeBillingCount;

                    //25-06-2024

                    BillingProfileThreePhaseCTDto.FundamentalEnergy = BillingProfileThreePhaseCT.FundamentalEnergy;
                    BillingProfileThreePhaseCTDto.FundamentalEnergyExport = BillingProfileThreePhaseCT.FundamentalEnergyExport;
                    BillingProfileThreePhaseCTDto.PowerOffDuration = BillingProfileThreePhaseCT.PowerOffDuration;
                    BillingProfileThreePhaseCTDto.PowerFailCount = BillingProfileThreePhaseCT.PowerFailCount;

                    BillingProfileThreePhaseCTDtoList.Add(BillingProfileThreePhaseCTDto);

                    index++;

                }
                return BillingProfileThreePhaseCTDtoList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
 
    }
}