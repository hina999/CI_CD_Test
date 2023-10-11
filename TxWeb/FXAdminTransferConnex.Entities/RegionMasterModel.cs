using System;
using System.ComponentModel.DataAnnotations;

namespace FXAdminTransferConnex.Entities
{
    public class RegionMasterModel
    {
        #region Instance Properties

        [Display(Name = "RegionID")]
        [Required]
        public Int32 RegionID { get; set; }

        [Display(Name = "RegionName")]
        [Required, StringLength(50)]
        public String RegionName { get; set; }

        public long CreatedBy { get; set; }

        public long UpdatedBy { get; set; }



        #endregion Instance Properties
    }

}
