using System;
using System.ComponentModel.DataAnnotations;

namespace FXAdminTransferConnex.Entities
{
    public class CountryMasterModel
    {


        #region Instance Properties

        [Display(Name = "CountryID")]
        [Required]
        public Int32 CountryID { get; set; }

        [Display(Name = "CountryName")]
        [Required, StringLength(50)]
        public String CountryName { get; set; }

        [Display(Name = "RegionID")]
        [Required]
        public Int32 RegionID { get; set; }

        public String RegionName { get; set; }

        [Display(Name = "CreatedDate")]
        [Required]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "CreatedBy")]
        [Required]
        public long CreatedBy { get; set; }

        [Display(Name = "UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "UpdateBy")]
        public long? UpdateBy { get; set; }


        #endregion Instance Properties
}

}
