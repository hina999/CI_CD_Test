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

namespace FXAdminTransferConnex.Business.City
{
    class CityService : ICityService
    {
        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<FXAdminTransferConnex.Data.Models.CityMaster> _CityRepository;

        public CityService(IRepository<CityMaster> providerRepository)
        {
            _CityRepository = providerRepository;
        }

        public List<CityMasterModel> GetCity(int CityId, string search)
        {
            List<CityMasterModel> loginUser = null;

            SqlParameter CityidParam = new SqlParameter
            {
                ParameterName = "p_CityId",
                DbType = DbType.Int32,
                Value = CityId
            };

            SqlParameter searchParam = new SqlParameter
            {
                ParameterName = "p_search",
                DbType = DbType.String,
                Value = search
            };

            IList<CityMasterModel> CityList =
                    _CityRepository.ExecuteStoredProcedureList<CityMasterModel>(
                        "usp_get_CityMaster", CityidParam, searchParam);

            if (CityList != null && CityList.Any())
                loginUser = CityList.ToList();

            return loginUser;
        }
        public int AddCity(CityMasterModel model)
        {
            SqlParameter Cityname = new SqlParameter
            {
                ParameterName = "p_CityName",
                DbType = DbType.String,
                Value = model.CityName
            };

            SqlParameter countryid = new SqlParameter
            {
                ParameterName = "p_CountryID",
                DbType = DbType.Int32,
                Value = model.CountryID
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
                Value = (object)model.UpdateBy ?? DBNull.Value
            };

            IEnumerable<int> result = _CityRepository.ExecuteStoredProcedure<int>("usp_insert_CityMaster", Cityname, countryid, insertuserid, inserttime, updateuserid, updatetime);
            int rowCount = result.FirstOrDefault();

            return rowCount;

        }





        public int UpdateCity(CityMasterModel model)
        {
            SqlParameter Cityname = new SqlParameter
            {
                ParameterName = "p_CityName",
                DbType = DbType.String,
                Value = model.CityName
            };

            SqlParameter countryid = new SqlParameter
            {
                ParameterName = "p_CountryID",
                DbType = DbType.Int32,
                Value = model.CountryID
            };

            SqlParameter updatetime = new SqlParameter
            {
                ParameterName = "p_UpdatedDate",
                DbType = DbType.DateTime,
                Value = DateTime.UtcNow
            };


            SqlParameter updateuserid = new SqlParameter
            {
                ParameterName = "p_UpdateBy",
                DbType = DbType.Int32,
                Value = model.UpdateBy
            };

            SqlParameter Cityid = new SqlParameter
            {
                ParameterName = "w_CityID",
                DbType = DbType.Int32,
                Value = model.CityID
            };


            IEnumerable<int> result = _CityRepository.ExecuteStoredProcedure<int>("usp_update_CityMaster", Cityname, countryid, updateuserid, updatetime, Cityid);
            int rowCount = result.FirstOrDefault();

            return rowCount;

        }
        public bool DeleteCity(int CityId)
        {
            SqlParameter CityidParam = new SqlParameter
            {
                ParameterName = "p_CityID",
                DbType = DbType.Int32,
                Value = CityId
            };


            IEnumerable<int> result = _CityRepository.ExecuteStoredProcedure<int>("usp_delete_CityMaster", CityidParam);
            int rowCount = result.FirstOrDefault();

            return (rowCount == 1);
        }

    }
}
