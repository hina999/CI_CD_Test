using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WinService_C4UBackOffice
{
    public partial class Service1 : ServiceBase
    {
        

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //timer1.Tick += new ElapsedEventHandler(OnElapsedTime);
            //timer1.Interval = TimeSpan.FromMinutes(5).TotalMilliseconds; // Every 60 Minutes / 1 Hour            
            timer1.Start();
        }

        protected override void OnStop()
        {
            timer1.Stop();            
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Hey I am here @ " + DateTime.Now.ToString());
            //WCFService.Service1 service1 = new WCFService.Service1();
            //service1.SendRemindersMails();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Hey I am here @ " + DateTime.Now.ToString());
            WCFService.Service1 service1 = new WCFService.Service1();
            service1.ImportDeals();
        }
    }
}
