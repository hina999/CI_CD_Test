using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace FXAdminTransferConnex.Business.Country
{
    public interface ICountryServices
    {
        int AddCountry(CountryMasterModel model);
        int UpdateCountry(CountryMasterModel model);
        bool DeleteCountry(int CountryId);
        List<CountryMasterModel> GetCountry(int CountryId, string search);
    }
}

