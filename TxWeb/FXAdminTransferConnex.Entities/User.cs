using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using FXAdminTransferConnex.Common;

namespace FXAdminTransferConnex.Entities
{
   /// <summary>
   /// By Karan Trivedi
   /// 10 feb 2017
   /// User Class model 
   /// </summary>
    public class User
    {
        public class LoginUser
        {
            public long UserId { get; set; }
            public long TraderId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public bool IsActive { get; set; }
            public byte UserTypeId { get; set; }
            public long CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public List<Rights> RightsList { get; set; }
            public DateTime? LastJobExecutedDate { get; set; }

            private string RightsString
            {
                set
                {
                    RightsList = !string.IsNullOrEmpty(value) ? Utility.DeserializeFromXMLNew<RightsGroup>(value).RightsList : new RightsGroup().RightsList;
                }
            }

            public long SignalRUserId { get; set; }
            public string ImageURL { get; set; }
            public string ImageName { get; set; }
        }

        [XmlRoot("RightsGroup")]
        public class RightsGroup
        {
            public RightsGroup() { RightsList = new List<Rights>(); }

            [XmlElement("Rights", typeof(Rights))]
            public List<Rights> RightsList { get; set; }
        }

        /// <summary>
        /// Class Rights.
        /// </summary>
        public class Rights
        {
            /// <summary>
            /// Gets or sets the rights identifier.
            /// </summary>
            /// <value>The rights identifier.</value>
            public int RightsId { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string RightsName { get; set; }

            /// <summary>
            /// Gets or sets the roleid.
            /// </summary>
            /// <value>role id.</value>
            public int RoleId { get; set; }
            /// <summary>
            /// Gets or sets the name of the display.
            /// </summary>
            /// <value>The name of the display.</value>
            public string DisplayName { get; set; }

            public int Assigned { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
