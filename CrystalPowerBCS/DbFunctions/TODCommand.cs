using Infrastructure.API;
using Infrastructure.DTOs;
using Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalPowerBCS.DbFunctions
{
    public class TODCommand
    {
        private readonly TODService _todService;
        public ErrorHelper _errorHelper;

        public TODCommand()
        {
            _todService = new TODService();
            _errorHelper = new ErrorHelper();
        }

        public async Task<List<TODDto>> GetByMeterNo(string meterNumber)
        {
            List<TODDto> TOD = new List<TODDto>();
            try
            {
                TOD = await _todService.GetByMeterNo(meterNumber);
            }
            catch (Exception)
            {

            }
            return TOD;
        }
    }
}
