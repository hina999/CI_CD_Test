/* 27 Oct 2014
// This code released to community under The Code Project Open License (CPOL) 1.02
// The copyright owner and author of this version of the code is Robert Ellis
// Please retain this notice and clearly identify your own edits, amendments and/or contributions
// In line with the CPOL this code is provided "AS IS" and without warranty
// Use entirely at your own risk
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace PrimeCalcTimerService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new TransferConnexDealImport() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
