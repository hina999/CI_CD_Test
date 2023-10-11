using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FXAdminTransferConnex.Entities
{
    public class UserModel
    {
        [ScaffoldColumn(false)]

        public long UserId { get; set; }

        public long? TraderId { get; set; }

        public string UserTitle { get; set; }

        public byte UserTypeId { get; set; }

        [DisplayName(@"First Name")]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [DisplayName(@"Last Name")]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [DisplayName(@"Email Address")]
        [Required]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]
        //[MaxLength(250)]
        [RegularExpression(RegularExpression.RegexEmail, ErrorMessage = "Invalid Email Address")]
        [StringLength(500, ErrorMessageResourceType = typeof(LocalizationResource.FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string EmailAddress { get; set; }

        //[Required]
        [MaxLength(500)]
        public string Password { get; set; }



        public bool? IsActive { get; set; }

        [ScaffoldColumn(false)]
        public long CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public long? UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        public string UserTypeName { get; set; }

        public string ImageName { get; set; }

        public string ImageURL
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ImageName))
                {
                    var request = HttpContext.Current.Request;
                    var appUrl = HttpRuntime.AppDomainAppVirtualPath;

                    if (appUrl != "/")
                        appUrl = "/" + appUrl;

                    var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

                    return baseUrl + "/Images/Profiles/" + this.ImageName;
                }
                else
                {
                    return this.ImageName;
                }


            }
            set { }
        }

        public HttpPostedFileBase ImageFile { get; set; }

    }

    public class SettingModel
    {
        public long UserId { get; set; }

        [MaxLength(500)]
        [Required(ErrorMessage = "Old Password is required. ")]
        public string OldPassword { get; set; }

        [MaxLength(500)]
        [Required(ErrorMessage = "New Password is required. ")]
        public string NewPassword { get; set; }

        [MaxLength(500)]
        [Required(ErrorMessage = "Confirm New Password is required. ")]
        public string ConfirmPassword { get; set; }

        public long UpdatedBy { get; set; }
    }
}
