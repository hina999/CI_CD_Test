namespace PrimeCalcTimerService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PrimeProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.PrimeServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // PrimeProcessInstaller
            // 
            this.PrimeProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.PrimeProcessInstaller.Password = null;
            this.PrimeProcessInstaller.Username = null;
            // 
            // PrimeServiceInstaller
            // 
            this.PrimeServiceInstaller.Description = "Transfer Connex Deal Import - Nitinbhai UK";
            this.PrimeServiceInstaller.DisplayName = "Transfer Connex - Deal Import";
            this.PrimeServiceInstaller.ServiceName = "TransferConnexDealImport";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.PrimeProcessInstaller,
            this.PrimeServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller PrimeProcessInstaller;
        private System.ServiceProcess.ServiceInstaller PrimeServiceInstaller;
    }
}