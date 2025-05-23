﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class GCPartnerModel
    {
    }
    public class GCPartnerClientModel
    {
        public List<GCPartnerClient> ReturnObject { get; set; }

    }

    public class GCPartnerClient
    {
        public long AgentId { get; set; }
        public DateTime? DateAdded { get; set; }
        public string ClientType { get; set; }
        public string EmploymentStatus { get; set; }
        public string IndustryOfWork { get; set; }
        public long ClientId { get; set; }
        public string TitleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string TownCity { get; set; }
        public string CountyRegion { get; set; }
        public string Postcode { get; set; }
        public string CountryId { get; set; }
        public string Telephone { get; set; }
        public string EmailAddress { get; set; }
        public string Mobile { get; set; }
        public string CompanyName { get; set; }
        public string MMaidenName { get; set; }
        public string BirthPlace { get; set; }
    }

    public class GCPartnerDealModel
    {
        public List<GCPartnerDeal> ReturnObject { get; set; }
    }
    public class GCPartnerDeal
    {
        public long AgentId { get; set; }
        public long ClientId { get; set; }
        public string DlCode { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public string SourceCurrency { get; set; }
        public string PurchaseCurrency { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TransactionCharge { get; set; }
        public decimal CustodianCharge { get; set; }
        public decimal SellAmount { get; set; }
        public decimal BuyAmount { get; set; }
        public string Status { get; set; }
        public string DealReference { get; set; }
        public decimal DealRate { get; set; }
        public DateTime? BookedDate { get; set; }
        public string Type { get; set; }

    }

    public class DealJSONData
    {
        public long DealId { get; set; }
        public string DealJsonStr { get; set; }
        public long? UserId { get; set; }

    }

    public class GCPartnerMarketOrderRateModel
    {
        public MerketOrderModel ReturnObject { get; set; }
    }

    public class MerketOrderModel
    {
        public decimal AED { get; set; }
        public decimal AFN { get; set; }
        public decimal ALL { get; set; }
        public decimal AMD { get; set; }
        public decimal ANG { get; set; }
        public decimal AOA { get; set; }
        public decimal ARS { get; set; }
        public decimal AUD { get; set; }
        public decimal AWG { get; set; }
        public decimal AZN { get; set; }
        public decimal BAM { get; set; }
        public decimal BBD { get; set; }
        public decimal BDT { get; set; }
        public decimal BGN { get; set; }
        public decimal BHD { get; set; }
        public decimal BIF { get; set; }
        public decimal BMD { get; set; }
        public decimal BND { get; set; }
        public decimal BOB { get; set; }
        public decimal BRL { get; set; }
        public decimal BSD { get; set; }
        public decimal BTC { get; set; }
        public decimal BTN { get; set; }
        public decimal BWP { get; set; }
        public decimal BYN { get; set; }
        public decimal BZD { get; set; }
        public decimal CAD { get; set; }
        public decimal CDF { get; set; }
        public decimal CHF { get; set; }
        public decimal CLF { get; set; }
        public decimal CLP { get; set; }
        public decimal CNH { get; set; }
        public decimal CNY { get; set; }
        public decimal COP { get; set; }
        public decimal CRC { get; set; }
        public decimal CUC { get; set; }
        public decimal CUP { get; set; }
        public decimal CVE { get; set; }
        public decimal CZK { get; set; }
        public decimal DJF { get; set; }
        public decimal DKK { get; set; }
        public decimal DOP { get; set; }
        public decimal DZD { get; set; }
        public decimal EGP { get; set; }
        public decimal ERN { get; set; }
        public decimal ETB { get; set; }
        public decimal EUR { get; set; }
        public decimal FJD { get; set; }
        public decimal FKP { get; set; }
        public decimal GBP { get; set; }
        public decimal GEL { get; set; }
        public decimal GGP { get; set; }
        public decimal GHS { get; set; }
        public decimal GIP { get; set; }
        public decimal GMD { get; set; }
        public decimal GNF { get; set; }
        public decimal GTQ { get; set; }
        public decimal GYD { get; set; }
        public decimal HKD { get; set; }
        public decimal HNL { get; set; }
        public decimal HRK { get; set; }
        public decimal HTG { get; set; }
        public decimal HUF { get; set; }
        public decimal IDR { get; set; }
        public decimal ILS { get; set; }
        public decimal IMP { get; set; }
        public decimal INR { get; set; }
        public decimal IQD { get; set; }
        public decimal IRR { get; set; }
        public decimal ISK { get; set; }
        public decimal JEP { get; set; }
        public decimal JMD { get; set; }
        public decimal JOD { get; set; }
        public decimal JPY { get; set; }
        public decimal KES { get; set; }
        public decimal KGS { get; set; }
        public decimal KHR { get; set; }
        public decimal KMF { get; set; }
        public decimal KPW { get; set; }
        public decimal KRW { get; set; }
        public decimal KWD { get; set; }
        public decimal KYD { get; set; }
        public decimal KZT { get; set; }
        public decimal LAK { get; set; }
        public decimal LBP { get; set; }
        public decimal LKR { get; set; }
        public decimal LRD { get; set; }
        public decimal LSL { get; set; }
        public decimal LYD { get; set; }
        public decimal MAD { get; set; }
        public decimal MDL { get; set; }
        public decimal MGA { get; set; }
        public decimal MKD { get; set; }
        public decimal MMK { get; set; }
        public decimal MNT { get; set; }
        public decimal MOP { get; set; }
        public decimal MRO { get; set; }
        public decimal MRU { get; set; }
        public decimal MUR { get; set; }
        public decimal MVR { get; set; }
        public decimal MWK { get; set; }
        public decimal MXN { get; set; }
        public decimal MYR { get; set; }
        public decimal MZN { get; set; }
        public decimal NAD { get; set; }
        public decimal NGN { get; set; }
        public decimal NIO { get; set; }
        public decimal NOK { get; set; }
        public decimal NPR { get; set; }
        public decimal NZD { get; set; }
        public decimal OMR { get; set; }
        public decimal PAB { get; set; }
        public decimal PEN { get; set; }
        public decimal PGK { get; set; }
        public decimal PHP { get; set; }
        public decimal PKR { get; set; }
        public decimal PLN { get; set; }
        public decimal PYG { get; set; }
        public decimal QAR { get; set; }
        public decimal RON { get; set; }
        public decimal RSD { get; set; }
        public decimal RUB { get; set; }
        public decimal RWF { get; set; }
        public decimal SAR { get; set; }
        public decimal SBD { get; set; }
        public decimal SCR { get; set; }
        public decimal SDG { get; set; }
        public decimal SEK { get; set; }
        public decimal SGD { get; set; }
        public decimal SHP { get; set; }
        public decimal SLL { get; set; }
        public decimal SOS { get; set; }
        public decimal SRD { get; set; }
        public decimal SSP { get; set; }
        public decimal STD { get; set; }
        public decimal STN { get; set; }
        public decimal SVC { get; set; }
        public decimal SYP { get; set; }
        public decimal SZL { get; set; }
        public decimal THB { get; set; }
        public decimal TJS { get; set; }
        public decimal TMT { get; set; }
        public decimal TND { get; set; }
        public decimal TOP { get; set; }
        public decimal TRY { get; set; }
        public decimal TTD { get; set; }
        public decimal TWD { get; set; }
        public decimal TZS { get; set; }
        public decimal UAH { get; set; }
        public decimal UGX { get; set; }
        public decimal USD { get; set; }
        public decimal UYU { get; set; }
        public decimal UZS { get; set; }
        public decimal VEF { get; set; }
        public decimal VES { get; set; }
        public decimal VND { get; set; }
        public decimal VUV { get; set; }
        public decimal WST { get; set; }
        public decimal XAF { get; set; }
        public decimal XAG { get; set; }
        public decimal XAU { get; set; }
        public decimal XCD { get; set; }
        public decimal XDR { get; set; }
        public decimal XOF { get; set; }
        public decimal XPD { get; set; }
        public decimal XPF { get; set; }
        public decimal XPT { get; set; }
        public decimal YER { get; set; }
        public decimal ZAR { get; set; }
        public decimal ZMW { get; set; }
        public decimal ZWL { get; set; }
        public decimal BYR { get; set; }
    }
}
