using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FXAdminTransferConnex.Entities
{
    public class RegularExpression
    {
        public const string RegexNumber = @"^[0-9]*$";

        /// <summary>
        /// The regular expression for telephone.
        /// </summary>
        public const string RegExpTelephone = @"^[0-9 (,),-]{0,15}$";

        /// <summary>
        /// Telephone Validation
        /// </summary>
        public const string TelephoneExpression = @"^[0-9 \+\(\)]{11,20}$";

        /// <summary>
        /// The regular expression email.
        /// </summary>
        public const string RegExpEmail = @"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}";


        /// <summary>
        /// The regular expression without http and www.
        /// </summary>
        public const string RegExpWebsite = @"[a-z0-9._%+-]+[a-z0-9.-]+\.[a-z]{2,4}";

        /// <summary>
        /// The regular expression url.
        /// </summary>
        public const string RegExpURL = @"^(http|http(s)?://)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?";

        /// <summary>
        /// The regular expression up to 2 decimal place.
        /// </summary>
        public const string RegExpUpTo2Decimal = @"^\d{1,14}(\.\d{1,2})?$";


        /// <summary>
        /// The regular expression up to 10 decimal place.
        /// </summary>
        public const string RegExpUpTo10Decimal = @"^\d{1,14}(\.\d{1,10})?$";

        /// <summary>
        /// Regular expression for email
        /// </summary>
        //public const string RegexEmail = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}";
        //public const string RegexEmail = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}";
        public const string RegexEmail = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,7}";   //changes

        /// <summary>
        /// Regular expression for I.P Address
        /// </summary>
        public const string RegexIp = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

        /// <summary>
        /// Regular expression for Alpha numeric
        /// </summary>
        public const string RegexAlphaNumeric = @"^[a-zA-Z][a-zA-Z0-9]*$";

    }
}
