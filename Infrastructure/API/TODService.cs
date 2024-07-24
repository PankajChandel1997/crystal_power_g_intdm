using Domain.Entities;
using Domain.Interface.Service;
using Infrastructure.DTOs;
using Infrastructure.Helpers;
using Infrastructure.Services;

namespace Infrastructure.API
{
    public class TODService
    {
        private readonly IDataService<TOD> _dataService;
        public ErrorHelper _errorHelper;
        private readonly ApplicationContextFactory _contextFactory;
        public TODService()
        {
            _dataService = new GenericDataService<TOD>(new ApplicationContextFactory());
            _errorHelper = new ErrorHelper();
            _contextFactory = new ApplicationContextFactory();
        }
        public async Task<bool> Add(List<TOD> TODDto)
        {
            try
            {
                await Delete(TODDto.FirstOrDefault().MeterNo);
                return await _dataService.CreateRange(TODDto);
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "TOD Service : inner Exception ==> " + ex.InnerException.Message);
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> Delete(string meterNumber)
        {
            try
            {
                using (ApplicationDBContext db = _contextFactory.CreateDbContext())
                {
                    string query = "select * from TOD where MeterNo = '" + meterNumber + "'";

                    var res = await _dataService.Filter(query);

                    if (res.Any())
                    {
                        db.Set<TOD>().RemoveRange(res);
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
        public async Task<List<TODDto>> GetByMeterNo(string MeterNo)
        {
            try
            {
                string query = "select * from TOD where MeterNo = '" + MeterNo + "'";

                var response = await _dataService.Filter(query);

                List<TODDto> TODDto = await ParseDataToDTO(response);

                return TODDto;
            }
            catch (Exception ex)
            {
                _errorHelper.WriteLog(ex.Message + "TOD Service : inner Exception ==> " + ex.InnerException.Message);

                throw new Exception(ex.Message);
            }
        }

        private async Task<List<TODDto>> ParseDataToDTO(List<TOD> TOD)
        {
            int index = 1;
            List<TODDto> TODDtoList = new List<TODDto>();
            foreach (var tod in TOD)
            {
                TODDto TODDto = new TODDto();

                TODDto.Number = index;
                TODDto.MeterNo = tod.MeterNo;
                TODDto.CreatedOn = tod.CreatedOn;

                TODDto.ActiveCalenderName = tod.ActiveCalenderName;
                TODDto.ActiveDayProfileStartTime = tod.ActiveDayProfileStartTime;
                TODDto.ActiveDayProfileScript = tod.ActiveDayProfileScript;
                TODDto.ActiveDayProfileSelector = tod.ActiveDayProfileSelector;
                TODDto.PassiveCalenderName = tod.PassiveCalenderName;
                TODDto.PassiveDayProfileStartTime = tod.PassiveDayProfileStartTime;
                TODDto.PassiveDayProfileScript = tod.PassiveDayProfileScript;
                TODDto.PassiveDayProfileSelector = tod.PassiveDayProfileSelector;

                TODDtoList.Add(TODDto);
                index++;
            }

            return TODDtoList;
        }
    }
}
