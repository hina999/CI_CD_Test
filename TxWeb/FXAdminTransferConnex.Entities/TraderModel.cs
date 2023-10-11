using FXAdminTransferConnex.Entities.LocalizationResource;
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
    public class TraderModel
    {
        public int TotalCount { get; set; }

        public long TraderId { get; set; }

        [DisplayName(@"First Name")]
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string FirstName { get; set; }

        [DisplayName(@"Last Name")]
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string LastName { get; set; }


        [DisplayName(@"Email Address")]
        [Required]
        [RegularExpression(RegularExpression.RegexEmail, ErrorMessage = "Invalid Email Address")]
        [StringLength(500, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string EmailAddress { get; set; }

        [DisplayName(@"Company Name")]
        [Required]
        [StringLength(500, ErrorMessageResourceType = typeof(FXAdminTransferConnexResource), ErrorMessageResourceName = "Validation_StringLengthMessage")]
        public string CompanyName { get; set; }

        [DisplayName(@"Mobile No.")]
        [Required]
        public string MobileNo { get; set; }

        public DateTime? JoiningDate { get; set; }

        [DisplayName(@"Joining Date")]
        public string JoiningDateString { get; set; }

        public bool IsClose { get; set; }

        [Required]
        [MaxLength(500)]
        public string Password { get; set; }

        public long UserId { get; set; }

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
}
