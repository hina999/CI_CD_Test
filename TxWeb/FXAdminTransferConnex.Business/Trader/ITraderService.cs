using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.Trader
{
    public interface ITraderService
    {
        /// <summary>
        /// Prototype for get Trader list
        /// </summary>
        /// <returns></returns>
        List<TraderModel> GetTraderlist(int pageNo, int pageSize, string sortColumn, string sortDir, string search);

        /// <summary>
        /// Prototype for get trader details by traderId
        /// </summary>
        /// <returns></returns>
        TraderModel GetTraderDetailsByTraderId(long traderId);

        /// <summary>
        /// Prototype for save Trader
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        long SaveTrader(TraderModel model);

        /// <summary>
        /// Prototype for delete Trader
        /// </summary>
        /// <param name="TraderId"></param>
        /// <returns></returns>
        int DeleteTrader(long TraderId);


        /// <summary>
        /// Prototype for get Trader Commission list
        /// </summary>
        /// <returns></returns>
        List<TraderCommissionModel> GetTraderCommissionList(long TraderId);

        /// <summary>
        /// Prototype for save Trader Commission
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int SaveTraderCommission(TraderCommissionModel model);

        /// <summary>
        /// Prototype for delete Trader Commission
        /// </summary>
        /// <param name="TraderId"></param>
        /// <returns></returns>
        bool DeleteTraderCommission(long TraderCommissionId);
    }
}
