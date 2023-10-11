using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

using ServiceTimer;
using log4net;

namespace PrimeCalcTimerService
{
    /// <summary>
    /// Example service component class. Use two workers to perform prime number calculations and 
    /// write the results out to a file.
    /// 
    /// The service class inherits from TimerServiceBase, rather than inheriting from ServiceBase
    /// </summary>
    public partial class TransferConnexDealImport : TimerServiceBase
    {
        public TransferConnexDealImport()
        {
            InitializeComponent();

            this.ServiceName = "PrimeCalcService";
        }

        /// <summary>
        /// In OnStart, we need do litte more than construct our two worker classes, and 
        ///  "register" them using the base class RegisterWorker() method. We also set up 
        ///  the log4net logging
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {



            // Set up log4net logging. Note that this call by itself is enough to ensure that your
            //  worker classes also get an ILog object, provided this call is made prior to the 
            //  RegisterWorker() calls. An additional caveat is that it is assumed you are satisfied 
            //  with a call to log4net's XmlConfigurator - if you are using a different logging methodology,
            //  you'll need to hack some of the code to get exactly what you want
            DefaultLog();

            //LargePrimeWorker largeWorker = new LargePrimeWorker();
            SmallPrimeWorker smallWorker = new SmallPrimeWorker();

            //RegisterWorker(largeWorker);
            RegisterWorker(smallWorker);
        }
    }
}
