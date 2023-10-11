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

namespace FXAdminTransferConnex.Business.Country
{
    public class CountryServices : ICountryServices
    {
        /// <summary>
        /// Declare The provider repository object for perform operation on Provider repository
        /// </summary>
        private readonly IRepository<FXAdminTransferConnex.Data.Models.CountryMaster> _CountryRepository;

        public CountryServices(IRepository<CountryMaster> providerRepository)
        {
            _CountryRepository = providerRepository;
        }

        //#endCountry

        //#Country Methods

        ///// <summary>
        ///// By Karan Trivedi
        ///// 10 FEB 2017
        ///// Service that authenticates admin
        ///// </summary>
        ///// <param name="emailAddress"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        public List<CountryMasterModel> GetCountry(int CountryId, string search)
        {
            List<CountryMasterModel> loginUser = null;

            SqlParameter CountryidParam = new SqlParameter
            {
                ParameterName = "p_CountryId",
                DbType = DbType.Int32,
                Value = CountryId
            };

            SqlParameter searchParam = new SqlParameter
            {
                ParameterName = "p_search",
                DbType = DbType.String,
                Value = search
            };

            IList<CountryMasterModel> CountryList =
                    _CountryRepository.ExecuteStoredProcedureList<CountryMasterModel>(
                        "usp_get_CountryMaster", CountryidParam, searchParam);

            if (CountryList != null && CountryList.Any())
                loginUser = CountryList.ToList();

            return loginUser;
        }

    
        public int AddCountry(CountryMasterModel model)
        {
            SqlParameter Countryname = new SqlParameter
            {
                ParameterName = "p_CountryName",
                DbType = DbType.String,
                Value = model.CountryName
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
                Value = model.UpdateBy
            };

            IEnumerable<int> result = _CountryRepository.ExecuteStoredProcedure<int>("usp_insert_CountryMaster", Countryname, insertuserid, inserttime, updateuserid, updatetime);
            int rowCount = result.FirstOrDefault();

            return rowCount;

        }





        public int UpdateCountry(CountryMasterModel model)
        {
            SqlParameter Countryname = new SqlParameter
            {
                ParameterName = "p_CountryName",
                DbType = DbType.String,
                Value = model.CountryName
            };

            SqlParameter regionid = new SqlParameter
            {
                ParameterName = "p_RegionID",
                DbType = DbType.Int32,
                Value = model.RegionID
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

            SqlParameter Countryid = new SqlParameter
            {
                ParameterName = "w_CountryID",
                DbType = DbType.Int32,
                Value = model.CountryID
            };

            IEnumerable<int> result = _CountryRepository.ExecuteStoredProcedure<int>("usp_update_CountryMaster", Countryname, regionid, updateuserid, updatetime, Countryid);
            int rowCount = result.FirstOrDefault();

            return rowCount;

        }

    
        public bool DeleteCountry(int CountryId)
        {
            SqlParameter CountryidParam = new SqlParameter
            {
                ParameterName = "p_CountryID",
                DbType = DbType.Int32,
                Value = CountryId
            };


            IEnumerable<int> result = _CountryRepository.ExecuteStoredProcedure<int>("usp_delete_CountryMaster", CountryidParam);
            int rowCount = result.FirstOrDefault();

            return (rowCount == 1);

        }

    }
}
