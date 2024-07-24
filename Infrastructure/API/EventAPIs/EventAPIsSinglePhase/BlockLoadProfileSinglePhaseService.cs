using Domain.Entities.SinglePhaseEntities;
using Domain.Interface.Service;
using Infrastructure.DTOs.SinglePhaseEventDTOs;
using Infrastructure.Helpers;
using Infrastructure.Services;
using System.Globalization;

namespace Infrastructure.API.EventAPIsSinglePhase
{
    public class BlockLoadProfileSinglePhaseService
    {
        private readonly IDataService<BlockLoadProfileSinglePhase> _dataService;
        public ErrorHelper _errorHelper;
        private readonly ApplicationContextFactory _contextFactory;

        public BlockLoadProfileSinglePhaseService()
        {
            _dataService = new GenericDataService<BlockLoadProfileSinglePhase>(new ApplicationContextFactory());
            _errorHelper = new ErrorHelper();
            _contextFactory = new ApplicationContextFactory();
        }

        public async Task<bool> Add(List<BlockLoadProfileSinglePhase> blockLoadProfile)
        {
            try
            {
                return await _dataService.CreateRange(blockLoadProfile);
                //return await Delete(blockLoadProfile.FirstOrDefault().MeterNo);
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
                    string query = "select * from BlockLoadProfileSinglePhase where MeterNo = '" + meterNumber + "' ORDER by Id DESC";

                    var res = await _dataService.Filter(query);

                    List<string> fatchedDates = res.DistinctBy(x => x.RealTimeClock).OrderByDescending(c => c.Id).Select(d => d.RealTimeClock).ToList();
                    foreach (var fatchedDate in fatchedDates)
                    {
                        var duplicateData = res.Where(x => x.RealTimeClock == fatchedDate).ToList().Skip(1);
                        if (duplicateData.Any())
                        {
                            db.Set<BlockLoadProfileSinglePhase>().RemoveRange(duplicateData);
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

        public async Task<List<BlockLoadProfileSinglePhaseDto>> GetAll(int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from BlockLoadProfileSinglePhase where MeterNo = '" + meterNumber + "' ORDER by Id DESC LIMIT " + pageSize;

                var response = await _dataService.Filter(query);

                List<BlockLoadProfileSinglePhaseDto> blockLoadProfileSinglePhase = await ParseDataToDTO(response);

                return blockLoadProfileSinglePhase;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<BlockLoadProfileSinglePhaseDto>> Filter(string startDate, string endDate, string fatchDate, int pageSize, string meterNumber)
        {
            try
            {
                string query = "select * from BlockLoadProfileSinglePhase where MeterNo = '" + meterNumber + "'";
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

                List<BlockLoadProfileSinglePhaseDto> blockLoadProfileSinglePhase = await ParseDataToDTO(response);

                return blockLoadProfileSinglePhase;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }

        //private async Task<List<BlockLoadProfileSinglePhase>> ParseDataToClass(List<BlockLoadProfileSinglePhaseDto> blockLoadProfileSinglePhaseDtoList)
        //{
        //    List<BlockLoadProfileSinglePhase> blockLoadProfileSinglePhaseList = new List<BlockLoadProfileSinglePhase>();

        //    foreach (var blockLoadProfileSinglePhaseDto in blockLoadProfileSinglePhaseDtoList)
        //    {
        //        BlockLoadProfileSinglePhase blockLoadProfileSinglePhase = new BlockLoadProfileSinglePhase();

        //        blockLoadProfileSinglePhase.MeterNo = blockLoadProfileSinglePhaseDto.MeterNo;
        //        blockLoadProfileSinglePhase.CreatedOn = blockLoadProfileSinglePhaseDto.CreatedOn;
        //        blockLoadProfileSinglePhase.RealTimeClock = blockLoadProfileSinglePhaseDto.RealTimeClock;
        //        blockLoadProfileSinglePhase.AverageVoltage = blockLoadProfileSinglePhaseDto.AverageVoltage;
        //        blockLoadProfileSinglePhase.BlockEnergykWhImport = blockLoadProfileSinglePhaseDto.BlockEnergykWhImport;
        //        blockLoadProfileSinglePhase.BlockEnergykVAh = blockLoadProfileSinglePhaseDto.BlockEnergykVAh;
        //        blockLoadProfileSinglePhase.BlockEnergykWhExport = blockLoadProfileSinglePhaseDto.BlockEnergykWhExport;
        //        blockLoadProfileSinglePhase.BlockEnergykVAhExport = blockLoadProfileSinglePhaseDto.BlockEnergykWhExport;
        //        blockLoadProfileSinglePhase.PhaseCurrent = blockLoadProfileSinglePhaseDto.PhaseCurrent;
        //        blockLoadProfileSinglePhase.MeterHealthIndicator = blockLoadProfileSinglePhaseDto.MeterHealthIndicator;
        //        blockLoadProfileSinglePhase.NeutralCurrent = blockLoadProfileSinglePhaseDto.NeutralCurrent;

        //        blockLoadProfileSinglePhaseList.Add(blockLoadProfileSinglePhase);
        //    }



        //    return blockLoadProfileSinglePhaseList;
        //}

        private async Task<List<BlockLoadProfileSinglePhaseDto>> ParseDataToDTO(List<BlockLoadProfileSinglePhase> BlockLoadProfileSinglePhaseList)
        {
            int index = 1;
            List<BlockLoadProfileSinglePhaseDto> BlockLoadProfileSinglePhaseDtoList = new List<BlockLoadProfileSinglePhaseDto>();
            foreach (var BlockLoadProfileSinglePhase in BlockLoadProfileSinglePhaseList)
            {
                BlockLoadProfileSinglePhaseDto BlockLoadProfileSinglePhaseDto = new BlockLoadProfileSinglePhaseDto();

                BlockLoadProfileSinglePhaseDto.Number = index;
                BlockLoadProfileSinglePhaseDto.MeterNo = BlockLoadProfileSinglePhase.MeterNo;
                BlockLoadProfileSinglePhaseDto.CreatedOn = BlockLoadProfileSinglePhase.CreatedOn;
                BlockLoadProfileSinglePhaseDto.RealTimeClock = BlockLoadProfileSinglePhase.RealTimeClock;
                BlockLoadProfileSinglePhaseDto.AverageVoltage = Math.Round(double.Parse(BlockLoadProfileSinglePhase.AverageVoltage, NumberStyles.Any), 2).ToString();
                BlockLoadProfileSinglePhaseDto.BlockEnergykWhImport = Math.Round(double.Parse(BlockLoadProfileSinglePhase.BlockEnergykWhImport, NumberStyles.Any) / 1000, 2).ToString();
                BlockLoadProfileSinglePhaseDto.BlockEnergykVAh = Math.Round(double.Parse(BlockLoadProfileSinglePhase.BlockEnergykVAh, NumberStyles.Any) / 1000, 2).ToString();
                BlockLoadProfileSinglePhaseDto.BlockEnergykWhExport = Math.Round(double.Parse(BlockLoadProfileSinglePhase.BlockEnergykWhExport, NumberStyles.Any) / 1000, 2).ToString();
                BlockLoadProfileSinglePhaseDto.BlockEnergykVAhExport = Math.Round(double.Parse(BlockLoadProfileSinglePhase.BlockEnergykVAhExport, NumberStyles.Any) / 1000, 2).ToString();
                BlockLoadProfileSinglePhaseDto.PhaseCurrent = Math.Round(double.Parse(BlockLoadProfileSinglePhase.PhaseCurrent, NumberStyles.Any), 2) == 0 ? "0" : Math.Round(double.Parse(BlockLoadProfileSinglePhase.PhaseCurrent, NumberStyles.Any), 2).ToString();
                BlockLoadProfileSinglePhaseDto.MeterHealthIndicator = BlockLoadProfileSinglePhase.MeterHealthIndicator;
                BlockLoadProfileSinglePhaseDto.NeutralCurrent = BlockLoadProfileSinglePhase.NeutralCurrent;

                BlockLoadProfileSinglePhaseDtoList.Add(BlockLoadProfileSinglePhaseDto);
                index++;
            }

            return BlockLoadProfileSinglePhaseDtoList;
        }
    }
}
