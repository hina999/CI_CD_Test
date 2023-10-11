//-----------------------------------------------------------------------
// <copyright file="ErrorHelper.cs" company="Premiere Digital Services">
//     Copyright Premiere Digital Services. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace FXAdminTransferConnex.Common
{
    /// <summary>
    /// Class ErrorHelper.
    /// </summary>
    public static class ErrorHelper
    {
        /// <summary>
        /// Gets the custom error message.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>Return System.String.</returns>
        public static string GetCustomErrorMessage(Exception ex)
        {
            const string message = "System is not able to process your request. Please try after sometime or contact Administrator.";

            return message;
        }
    }
}
