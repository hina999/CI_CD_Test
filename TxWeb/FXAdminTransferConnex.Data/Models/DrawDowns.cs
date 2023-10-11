using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Data.Models
{
    public class DrawDowns
    {
        [ScaffoldColumn(false)]

        public int dDrawDownId { get; set; }

      
        [ScaffoldColumn(false)]
        public int dStagingDealId { get; set; }

        [ScaffoldColumn(false)]
        [Required]
        [DisplayName("DealId")]
        public int dDealId { get; set; }

        [UIHint("DatePicker")]
        [DisplayName("Date")]
        public DateTime dDrawDownDate { get; set; }

         [UIHint("NumericTextBox")]
        [Required]
        [DisplayName("Sell Amount")]
        public double dSellAmount { get; set; }

        [UIHint("NumericTextBox")]
         [Required]
         [DisplayName("Sell Amount C4U")]
         public double dSellAmountCcy { get; set; }

        [UIHint("NumericTextBox")]
        [Required]
        [DisplayName("Buy Amount")]
        public double dBuyAmount { get; set; }

        [UIHint("NumericTextBox")]
        [Required]
        [DisplayName("Buy Amount C4U")]
        public double dBuyAmountLp { get; set; }


        [UIHint("NumericTextBox")]
        [Required]
        [DisplayName("Client Rate")]
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        public double dClientRate { get; set; }

        [UIHint("NumericTextBox")]
        [Required]
        [DisplayName("Market Rate")]
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        public double dMarketRate { get; set; }

        [UIHint("NumericTextBox")]
        [Required]
        [DisplayName("Transfer Cost")]
        public double dTransferCost { get; set; }

        [UIHint("NumericTextBox")]
        [Required]
        [DisplayName("GBP Net Profit")]
        public double dGBPNetProfit { get; set; }


    }
}
