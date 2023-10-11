using FXAdminTransferConnex.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Business.BulkEmail
{
    public interface IBulkEmailService
    {
        // <summary>
        /// Prototype for get client list
        /// </summary>
        /// <returns></returns>
        List<ClientMasterModel> GetAllClientlist(int pageNo, int pageSize, string sortColumn, string sortDir, string FullName, string EmailAddress, string CompanyName, string AccountNo, string AwaitingAction = "2", string MarketOrder = "2", string SearchType = null);

        bool SendMail(string Emails, string Subject, string Body);
    }
}
