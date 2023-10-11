using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace FXAdminTransferConnex.Business.Region
{
    public interface IRegionService
    {
        int AddRegion(RegionMasterModel model);
        int UpdateRegion(RegionMasterModel model);
        bool DeleteRegion(int RegionId);
        List<RegionMasterModel> GetRegion(int RegionId, string search);
        
    }
}
