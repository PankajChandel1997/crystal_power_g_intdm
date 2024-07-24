using Domain.Entities.ThreePhaseCTEntities;
using Domain.Entities.ThreePhaseEntities;
using Domain.Interface.Service;
using Infrastructure.DTOs.ThreePhaseEventCTDTOs;
using Infrastructure.Helpers;
using Infrastructure.Services;
using System.Globalization;

namespace Infrastructure.API.EventAPIThreePhaseCT
{
    public class BlockLoadProfileThreePhaseCTService
    {
        private readonly IDataService<BlockLoadProfileThreePhaseCT> _dataService;
        public ErrorHelper _errorHelper;
        private readonly ApplicationContextFactory _contextFactory;

        public BlockLoadProfileThreePhaseCTService()
        {
            _dataService = new GenericDataService<BlockLoadProfileThreePhaseCT>(new ApplicationContextFactory());
            _errorHelper = new ErrorHelper();
            _contextFactory = new ApplicationContextFactory();
        }
        public async Task<bool> Add(List<BlockLoadProfileThreePhaseCT> blockLoadProfile)
        {
            try
            {
                return await _dataService.CreateRange(blockLoadProfile);
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
                    string query = "select * from BlockLoadProfileThreePhaseCT where MeterNo = '" + meterNumber + "' ORDER by Id DESC";

                    var res = await _dataService.Filter(query);

                    List<string> fatchedDates = res.DistinctBy(x => x.RealTimeClock).OrderByDescending(c => c.Id).Select(d => d.RealTimeClock).ToList();
                    foreach (var fatchedDate in fatchedDates)
                    {
                        var duplicateData = res.Where(x => x.RealTimeClock == fatchedDate).ToList().Skip(1);
                        if (duplicateData.Any())
                        {
                            db.Set<BlockLoadProfileThreePhaseCT>().RemoveRange(duplicateData);
                        }
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

        public async Task<List<BlockLoadProfileThreePhaseCTDto>> GetAll(int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from BlockLoadProfileThreePhaseCT where MeterNo = '" + meterNumber + "' ORDER by Id DESC LIMIT " + pageSize;

                var response = await _dataService.Filter(query);

                List<BlockLoadProfileThreePhaseCTDto> blockLoadProfileThreePhase = await ParseDataToDTO(response);

                return blockLoadProfileThreePhase;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<BlockLoadProfileThreePhaseCTDto>> Filter(string startDate, string endDate, string fatchDate, int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from BlockLoadProfileThreePhaseCT where MeterNo = '" + meterNumber + "'";

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

                List<BlockLoadProfileThreePhaseCTDto> blockLoadProfileThreePhase = await ParseDataToDTO(response);

                return blockLoadProfileThreePhase;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }



        private async Task<List<BlockLoadProfileThreePhaseCTDto>> ParseDataToDTO(List<BlockLoadProfileThreePhaseCT> BlockLoadProfileThreePhaseList)
        {
            int index = 1;
            List<BlockLoadProfileThreePhaseCTDto> BlockLoadProfileThreePhaseDtoList = new List<BlockLoadProfileThreePhaseCTDto>();
            foreach (var BlockLoadProfileThreePhase in BlockLoadProfileThreePhaseList)
            {
                BlockLoadProfileThreePhaseCTDto BlockLoadProfileThreePhaseDto = new BlockLoadProfileThreePhaseCTDto();

                BlockLoadProfileThreePhaseDto.Number = index;
                BlockLoadProfileThreePhaseDto.MeterNo = BlockLoadProfileThreePhase.MeterNo;
                BlockLoadProfileThreePhaseDto.CreatedOn = BlockLoadProfileThreePhase.CreatedOn;
                BlockLoadProfileThreePhaseDto.RealTimeClock = BlockLoadProfileThreePhase.RealTimeClock;
                BlockLoadProfileThreePhaseDto.CurrentR = StringExtensions.CheckNullableOnly(BlockLoadProfileThreePhase.CurrentR);
                BlockLoadProfileThreePhaseDto.CurrentR = StringExtensions.CheckNullableOnly(BlockLoadProfileThreePhase.CurrentR);

                BlockLoadProfileThreePhaseDto.CurrentY = StringExtensions.CheckNullableOnly(BlockLoadProfileThreePhase.CurrentY);
                BlockLoadProfileThreePhaseDto.CurrentB = StringExtensions.CheckNullableOnly(BlockLoadProfileThreePhase.CurrentB);
                BlockLoadProfileThreePhaseDto.VoltageR = StringExtensions.CheckNullableOnly(BlockLoadProfileThreePhase.VoltageR);
                BlockLoadProfileThreePhaseDto.VoltageY = StringExtensions.CheckNullableOnly(BlockLoadProfileThreePhase.VoltageY);
                BlockLoadProfileThreePhaseDto.VoltageB = StringExtensions.CheckNullableOnly(BlockLoadProfileThreePhase.VoltageB);

                BlockLoadProfileThreePhaseDto.BlockEnergykWhImport = StringExtensions.CheckNullable(BlockLoadProfileThreePhase.CurrentR);
                BlockLoadProfileThreePhaseDto.BlockEnergykWhImport = StringExtensions.CheckNullable(BlockLoadProfileThreePhase.BlockEnergykWhImport);

                BlockLoadProfileThreePhaseDto.BlockEnergykVAhImport = StringExtensions.CheckNullable(BlockLoadProfileThreePhase.BlockEnergykVAhImport);
                BlockLoadProfileThreePhaseDto.BlockEnergykWhExport = StringExtensions.CheckNullable(BlockLoadProfileThreePhase.BlockEnergykWhExport);
                BlockLoadProfileThreePhaseDto.BlockEnergykVAhExport = StringExtensions.CheckNullable(BlockLoadProfileThreePhase.BlockEnergykVAhExport);
                BlockLoadProfileThreePhaseDto.CumulativeEnergykVArhQ1 = StringExtensions.CheckNullable(BlockLoadProfileThreePhase.CumulativeEnergykvarhQI);
                BlockLoadProfileThreePhaseDto.CumulativeEnergykVArhQ2 = StringExtensions.CheckNullable(BlockLoadProfileThreePhase.CumulativeEnergykvarhQII);
                BlockLoadProfileThreePhaseDto.CumulativeEnergykVArhQ3 = StringExtensions.CheckNullable(BlockLoadProfileThreePhase.CumulativeEnergykvarhQIII);
                BlockLoadProfileThreePhaseDto.CumulativeEnergykVArhQ4 = StringExtensions.CheckNullable(BlockLoadProfileThreePhase.CumulativeEnergykvarhQIV);

                BlockLoadProfileThreePhaseDto.MeterHealthIndicator = BlockLoadProfileThreePhase.MeterHealthIndicator;

                BlockLoadProfileThreePhaseDtoList.Add(BlockLoadProfileThreePhaseDto);
                index++;
            }

            return BlockLoadProfileThreePhaseDtoList;
        }

       
        //public string StringExtensions.CheckNullableOnly(string value)
        //{
        //    try
        //    {
        //        if(!string.IsNullOrEmpty(value))
        //        {
        //           return value == "0" ? value : (double.Parse(value, NumberStyles.Any)).ToString().CustomTrucate();
        //        }
        //        else
        //        {
        //            return "0";
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return string.Empty;
        //    }
        //}

    }
}
