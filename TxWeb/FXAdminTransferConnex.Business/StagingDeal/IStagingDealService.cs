using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.StagingDeal
{
   public interface IStagingDealService
    {
        /// <summary>
        /// Prototype for get staging list
        /// </summary>
        /// <returns></returns>
        List<StagingDealModel> GetStagingDeallist(int pageNo, int pageSize, string sortColumn, string sortDir, string DealNo, string CompanyName, DateTime? FromDate = null, DateTime? ToDate = null);

        /// <summary>
        /// Prototype for get staging deal details by StagingdealId
        /// </summary>
        /// <returns></returns>
        StagingDealModel GetStagingDealDetailsByStagingDealId(long stagingDealId);

        /// <summary>
        /// Prototype for Import staging deal
        /// </summary>
        /// <returns></returns>
        int ImportStagingDeal(DataTable model);


        /// <summary>
        /// Prototype for add staging deal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int SaveStagingDeal(StagingDealModel model);


        /// <summary>
        /// Prototype for reload staging deal
        /// </summary>
        /// <param name="StagingDealId"></param>
        /// <returns></returns>
        bool ReloadStagingDeal(long StagingDealId);


        /// <summary>
        /// Prototype for proceed staging deal
        /// </summary>
        /// <param name="StagingDealId"></param>
        /// <returns></returns>
        bool ProceedStagingDeal(long StagingDealId);

        /// <summary>
        /// Prototype for discard staging deal
        /// </summary>
        /// <param name="StagingDealId"></param>
        /// <returns></returns>
        bool DiscardStagingDeal(long StagingDealId);
    }
}

