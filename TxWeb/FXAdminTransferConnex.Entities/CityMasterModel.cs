using System;
using System.ComponentModel.DataAnnotations;

namespace FXAdminTransferConnex.Entities
{
    public class CityMasterModel
    {


        #region Instance Properties

        [Display(Name = "CityID")]
        [Required]
        public Int32 CityID { get; set; }

        [Display(Name = "CityName")]
        [Required, StringLength(50)]
        public String CityName { get; set; }

        [Display(Name = "CountryID")]
        [Required]
        public Int32 CountryID { get; set; }


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
