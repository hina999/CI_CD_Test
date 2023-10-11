using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class AuthTokenModel
    {
        public string auth_token { get; set; }
    }

    public class CurrencyCloudContactModel
    {
        public List<ContactModel> contacts { get; set; }
        public Pagination pagination { get; set; }
    }

    public class ContactModel
    {
        public string login_id { get; set; }
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string account_id { get; set; }
        public string account_name { get; set; }
        public string status { get; set; }
        public string locale { get; set; }
        public object timezone { get; set; }
        public string email_address { get; set; }
        public string mobile_phone_number { get; set; }
        public string phone_number { get; set; }
        public string your_reference { get; set; }
        public string date_of_birth { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class ProfitLossModel
    {
        public string account_id { get; set; }
        public string contact_id { get; set; }
        public string event_account_id { get; set; }
        public string event_contact_id { get; set; }
        public string conversion_id { get; set; }
        public string event_type { get; set; }
        public string amount { get; set; }
        public string currency { get; set; }
        public string notes { get; set; }
        public DateTime? event_date_time { get; set; }
    }
    public class AccountModel
    {
        public string id { get; set; }
        public string account_name { get; set; }
        public string brand { get; set; }
        public object your_reference { get; set; }
        public string status { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public object state_or_province { get; set; }
        public string country { get; set; }
        public string postal_code { get; set; }
        public string spread_table { get; set; }
        public string legal_entity_type { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string identification_type { get; set; }
        public string identification_value { get; set; }
        public string short_reference { get; set; }
        public string settlement_type { get; set; }
    }


    public class CurrencyCloudConversionModel
    {
        public List<ConversionModel> conversions { get; set; }
        public Pagination pagination { get; set; }
    }
    public class CurrencyCloudProfitLossModel
    {
        public List<ProfitLossModel> conversion_profit_and_losses { get; set; }
        public Pagination pagination { get; set; }
    }

    public class ConversionModel
    {
        public string id { get; set; }
        public DateTime settlement_date { get; set; }
        public DateTime conversion_date { get; set; }
        public string short_reference { get; set; }
        public string creator_contact_id { get; set; }
        public string account_id { get; set; }
        public string currency_pair { get; set; }
        public string status { get; set; }
        public string buy_currency { get; set; }
        public string sell_currency { get; set; }
        public string client_buy_amount { get; set; }
        public string client_sell_amount { get; set; }
        public string client_sell_amount_In_GBP { get; set; }
        public string fixed_side { get; set; }
        public string core_rate { get; set; }
        public string partner_rate { get; set; }
        public string partner_status { get; set; }
        public string partner_buy_amount { get; set; }
        public string partner_sell_amount { get; set; }
        public string client_rate { get; set; }
        public bool deposit_required { get; set; }
        public string deposit_amount { get; set; }
        public string deposit_currency { get; set; }
        public string deposit_status { get; set; }
        public string deposit_required_at { get; set; }
        public object[] payment_ids { get; set; }
        public string unallocated_funds { get; set; }
        public object unique_request_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string mid_market_rate { get; set; }
        public string GBPConversationRate { get; set; }
        public decimal GrossCommGBP { get; set; }
        public decimal GrossComm { get; set; }
        public string GrossCommCurrency { get; set; }

        public decimal amount_profit_loss { get; set; }
        public DateTime? event_date { get; set; }
        

    }

    public class Pagination
    {
        public int total_entries { get; set; }
        public int total_pages { get; set; }
        public int current_page { get; set; }
        public int per_page { get; set; }
        public int previous_page { get; set; }
        public int next_page { get; set; }
        public string order { get; set; }
        public string order_asc_desc { get; set; }
    }


    public class CurrencyCloudSettlementModel
    {
        public List<SettlementModel> settlements { get; set; }
        public Pagination pagination { get; set; }
    }

    public class SettlementModel
    {
        public string id { get; set; }
        public string status { get; set; }
        public string short_reference { get; set; }
        public string type { get; set; }
        public string[] conversion_ids { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string released_at { get; set; }
    }

}
