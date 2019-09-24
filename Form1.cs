using System;
using System.Windows.Forms;
using System.Management;


namespace BatteryLifeStatus
{
    public partial class BatteryLifeStatus : Form
    {
        public BatteryLifeStatus()
        {
            InitializeComponent();
        }

        
        private void Label1_Click(object sender, EventArgs e)
        {
            //ManagementObjectSearcher mosBat = new ManagementObjectSearcher("SELECT * FROM Win32_Battery");
            int hours = 0;
            int mins = 0;
            int minutes;
            int perc = 0;
            int status = 0;

            var batQuery = new SelectQuery("Win32_Battery");
            var mgmtScope = new ManagementScope("\\\\ADA-MI\\root\\cimv2");
            mgmtScope.Connect();
            var mgmtSrch = new ManagementObjectSearcher(mgmtScope, batQuery);
            ManagementObjectCollection collection = mgmtSrch.Get();

            foreach(ManagementObject mo in collection)
            {
                foreach(PropertyData property in mo.Properties)
                {
                    if (property.Name == "EstimatedChargeRemaining")
                        perc = Convert.ToInt32(property.Value);

                    else if (property.Name == "EstimatedRunTime")
                    {
                        minutes = Convert.ToInt32(property.Value);
                        hours = minutes / 60;
                        mins = minutes - (hours * 60);
                    }

                    else if (property.Name == "BatteryStatus")
                    {
                        status = Convert.ToInt32(property.Value);
                    }
                }
            }

            progressBar1.Visible = true;
            progressBar1.Value = perc;
            label2.Visible = true;

            if (status == 2)
                label2.Text = "Ładowanie\n" + "( " + perc + "% )";
            else
                //sleep bo zaraz po odlaczeniu pisze bzdury
                label2.Text = "Pozostało " + hours + "h " + mins + "min\n" + "( " + perc + "% )";
        }
    }
}
