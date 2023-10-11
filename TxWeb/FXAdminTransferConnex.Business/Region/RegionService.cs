using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using FXAdminTransferConnex.Business.Common;
using FXAdminTransferConnex.Common;
using FXAdminTransferConnex.Data.Databases;
using FXAdminTransferConnex.Data.Repository;
using FXAdminTransferConnex.Entities;
using log4net;
using System.Collections.Generic;
using FXAdminTransferConnex.Data.Models;

namespace FXAdminTransferConnex.Business.Region
{
    class RegionService : IRegionService
    {
        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<FXAdminTransferConnex.Data.Models.RegionMaster> _RegionRepository;

        public RegionService(IRepository<RegionMaster> providerRepository)
        {
            _RegionRepository = providerRepository;
        }

        //#endregion

        //#region Methods

        ///// <summary>
        ///// By Karan Trivedi
        ///// 10 FEB 2017
        ///// Service that authenticates admin
        ///// </summary>
        ///// <param name="emailAddress"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        public List<RegionMasterModel> GetRegion(int RegionId, string search)
        {
            List<RegionMasterModel> loginUser = null;

            SqlParameter regionidParam = new SqlParameter
            {
                ParameterName = "p_RegionId",
                DbType = DbType.Int32,
                Value = RegionId
            };

            SqlParameter searchParam = new SqlParameter
            {
                ParameterName = "p_search",
                DbType = DbType.String,
                Value = search
            };

            IList<RegionMasterModel> regionList =
                    _RegionRepository.ExecuteStoredProcedureList<RegionMasterModel>(
                        "usp_get_RegionMaster", regionidParam, searchParam);

            if (regionList != null && regionList.Any())
                loginUser = regionList.ToList();


            return loginUser;
        }

        public int AddRegion(RegionMasterModel model)
        {
            SqlParameter regionname = new SqlParameter
            {
                ParameterName = "p_RegionName",
                DbType = DbType.String,
                Value = model.RegionName
            };

            SqlParameter inserttime = new SqlParameter
            {
                ParameterName = "p_CreatedDate",
                DbType = DbType.DateTime,
                Value = DateTime.UtcNow
            };

            SqlParameter updatetime = new SqlParameter
            {
                ParameterName = "p_UpdatedDate",
                DbType = DbType.DateTime,
                Value = DateTime.UtcNow
            };

            SqlParameter insertuserid = new SqlParameter
            {
                ParameterName = "p_CreatedBy",
                DbType = DbType.Int32,
                Value = model.CreatedBy
            };

            SqlParameter updateuserid = new SqlParameter
            {
                ParameterName = "p_UpdatedBy",
                DbType = DbType.Int32,
                Value = model.UpdatedBy
            };

            IEnumerable<int> result = _RegionRepository.ExecuteStoredProcedure<int>("usp_insert_RegionMaster", regionname, insertuserid, inserttime, updateuserid, updatetime);
            int rowCount = result.FirstOrDefault();

            return rowCount;
        }





        public int UpdateRegion(RegionMasterModel model)
        {
            SqlParameter regionname = new SqlParameter
            {
                ParameterName = "p_RegionName",
                DbType = DbType.String,
                Value = model.RegionName
            };

            SqlParameter updatetime = new SqlParameter
            {
                ParameterName = "p_UpdatedDate",
                DbType = DbType.DateTime,
                Value = DateTime.UtcNow
            };


            SqlParameter updateuserid = new SqlParameter
            {
                ParameterName = "p_UpdatedBy",
                DbType = DbType.String,
                Value = model.UpdatedBy
            };

            SqlParameter Regionid = new SqlParameter
            {
                ParameterName = "w_RegionID",
                DbType = DbType.Int32,
                Value = model.RegionID
            };

            IEnumerable<int> result = _RegionRepository.ExecuteStoredProcedure<int>("usp_update_RegionMaster", regionname, updateuserid, updatetime, Regionid);
            int rowCount = result.FirstOrDefault();

            return rowCount;
        }

        public bool DeleteRegion(int RegionId)
        {
            SqlParameter regionidParam = new SqlParameter
            {
                ParameterName = "p_RegionID",
                DbType = DbType.Int32,
                Value = RegionId
            };

            IEnumerable<int> result = _RegionRepository.ExecuteStoredProcedure<int>("usp_delete_RegionMaster", regionidParam);
            int rowCount = result.FirstOrDefault();

            return (rowCount == 1);
        }

    }
}

