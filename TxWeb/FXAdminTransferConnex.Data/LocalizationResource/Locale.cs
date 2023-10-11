//-----------------------------------------------------------------------
// <copyright file="Locale.cs" company="Premiere Digital Services">
//     Copyright Premiere Digital Services. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace FXAdminTransferConnex.Data.LocalizationResource
{
    /// <summary>
    /// class Locale
    /// </summary>
    public class Locale
    {
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>return string</returns>
        public static string GetString(string name)
        {
            var resFormat = FXBackOfficeSystemResource.ResourceManager.GetString(name, FXBackOfficeSystemResource.Culture);
            if (string.IsNullOrEmpty(resFormat))
            {
                return name;
            }

            return resFormat;
        }
    }
}
