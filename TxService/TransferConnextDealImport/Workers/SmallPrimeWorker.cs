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
using System.Text;
using System.Net;

using ServiceTimer;
using log4net;
using System.IO;
using WCFService;

namespace PrimeCalcTimerService
{
    /// <summary>
    /// Sample worker class, derived from TimeWorker
    ///  Calculates the Nth prime where N is a random number from a (relatively) small range
    ///  Writes the results out to a file stream
    /// </summary>
    internal class SmallPrimeWorker : TimerWorker
    {
        private Random r = new Random();
        private System.IO.StreamWriter theFile;
        private string folderPath = @"c:\services";
        Service1 obj = new Service1();

        /// <summary>
        /// Constructor - ensure you use an available base constructor. In this case, we specify the 
        ///  constructor arguments:
        ///         -- a timerInterval value of 5000 (approximately 5 seconds)
        ///         -- a workOnElapseCount value of 3 (work will be carried out every 3rd elapse of the 
        ///                 underlying timer, which equates approximately to 3*5 = 15 seconds)
        /// Obviously it would be easy to create a non-default constructor for your concrete class, using the 
        ///  same arguments if, for example, you wanted to store these settings in your app.config (or wherever)
        /// </summary>
        internal SmallPrimeWorker()
            : base (timerInterval: 3600000, workOnElapseCount: 12)
        {
        }

        /// <summary>
        /// Create/open a resource
        /// </summary>
        private void OpenFile()
        {
            //string filePath = string.Format("{0}-output.txt", this.GetType().Name);

            string filePath = "WinService.txt";

            filePath = System.IO.Path.Combine("E:\\Projects", filePath);

            theFile = new System.IO.StreamWriter(filePath, true);
        }

        /// <summary>
        /// Close a resource
        /// </summary>
        private void CloseFile()
        {
            theFile.Close();
        }

        /// <summary>
        /// StartWork will execute once and only once, at start up of the 
        ///  Service
        /// </summary>
        /// <param name="info"></param>
        protected override void StartWork(TimerWorkerInfo info)
        {
            //OpenFile();

            // Note that you get a reference to a suitable log4net logger by way of inheritance, 
            //  and without any additional code, provided that your Service makes a call to 
            //  DefaultLog() before registering its workers

            //this.Log.InfoFormat("File '{0}' opened",(theFile.BaseStream as System.IO.FileStream).Name);
        }

        /// <summary>
        /// Use the Work override to carry out our work
        /// </summary>
        /// <param name="info"></param>
        protected override void Work(TimerWorkerInfo info)
        {
            //int N;
            //long prime;

            // In this example, we simulate some work by calculating a random Nth prime,
            //  and writing the result out to a log file. In a real service implementation, 
            //  your work would go here - polling a directory, polling a database, or 
            //  whatever else it is your service worker should do at regular intervals

            //Prime.GetNthRandomPrimeSmall(r, out N, out prime);

            //theFile.WriteLine(string.Format("Calculated prime number with ordinal {0}", N.ToString()));
            //theFile.WriteLine(string.Format("The prime is {0}", prime.ToString()));

            
            obj.ImportDeals();                        

            // The info object contains some basic information about the operation of 
            //  the underlying timer
            //theFile.WriteLine(info.ToString());

            //theFile.Flush();
        }

        protected override void OnContinue(TimerWorkerInfo info)
        {
            base.OnContinue(info); 
            
            OpenFile();
        }

        protected override void OnPause(TimerWorkerInfo info)
        {
            base.OnPause(info);

            CloseFile();
        }

        protected override void OnStop(TimerWorkerInfo info)
        {
            base.OnStop(info);

            CloseFile();
        }

        protected override void OnShutdown(TimerWorkerInfo info)
        {
            base.OnShutdown(info);

            CloseFile();
        }

        #region "Disposal"

        private bool disposed = false;

        // Protected implementation of Dispose pattern. 
        protected override void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here. 
                //
                if (theFile != null)
                    theFile.Dispose();

            }
            // Free any unmanaged objects here. 
            //
            disposed = true;

            /// Because the base TimerWorker class is IDisposable, you MUST NOT forget to 
            ///  base.Dispose(disposing), to ensure the underlying timer is properly disposed of
            base.Dispose(disposing);
        }

        #endregion
    }
}
