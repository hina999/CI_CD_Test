using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace FXAdminTransferConnex.Business.City
{
    public interface ICityService
    {
        int AddCity(CityMasterModel model);
        int UpdateCity(CityMasterModel model);
        bool DeleteCity(int CityId);
        List<CityMasterModel> GetCity(int CityId, string search);
    }
}
