using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace USBSafeRT
{
    public partial class MainForm : Form
    {
        int sno = 1;
        String VolLable = "";
        //String interfaceName = "Ethernet 2";
        StringBuilder sbs = new StringBuilder();
        private const int WM_DEVICECHANGE = 0x219;
        private const int WM_DEVICEARRIVAL = 0x8000;
        private const int WM_DEVICEREMOVECOMPLETE = 0x8004;
        private const int WM_DEVICETYP_VOLUME = 0x00000002;
        String connName = System.Configuration.ConfigurationManager.AppSettings["connectionNameString"];
        // do something

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    switch ((int)m.WParam)
                    {
                        case WM_DEVICEARRIVAL:
                            DriveInfo[] allDrives = DriveInfo.GetDrives();
                            foreach (DriveInfo d in allDrives) {
                                if (d.DriveType == DriveType.Removable) {
                                    VolLable = d.VolumeLabel;
                                }
                            }

                            String date = DateTime.Now.ToString("dd/MM/yyyy");
                            String time = DateTime.Now.ToString("h:mm:ss tt");
                            String snotxt = sno++.ToString();
                            ListViewItem itema = new ListViewItem(new[] { snotxt, date, time, VolLable , "USB Connected" });

                            lblUsbStatus.ForeColor = System.Drawing.Color.Green;
                            lblUsbStatus.Text = "USB Detected..";
                            lblUsbStatus.ForeColor = System.Drawing.Color.Red;
                            listView1.Items.Add(itema);

                            logFile(snotxt + "\t"+ date + "\t" + time + "\t" + VolLable + "\tUSB Connected");


                            // Network adapter disabling code
                            Process procd = new Process();
                            procd.StartInfo.CreateNoWindow = true;
                            procd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            procd.StartInfo.FileName = "cmd.exe";
                            procd.StartInfo.Arguments = string.Format("/c netsh interface set interface " + connName + " DISABLED");
                            procd.Start();

                            //Process.Start("cmd.exe", "/c netsh interface set interface " + connName + " DISABLED");

                            break;
                        case WM_DEVICEREMOVECOMPLETE:
                            

                            String dateb = DateTime.Now.ToString("dd/MM/yyyy");
                            String timeb = DateTime.Now.ToString("h:mm:ss tt");
                            String snotxtb = sno++.ToString();
                            
                            ListViewItem itemb = new ListViewItem(new[] { snotxtb, dateb, timeb, VolLable, "USB Removed" });

                            lblUsbStatus.Text = "";
                            lblUsbStatus.ForeColor = System.Drawing.Color.Green;
                            lblUsbStatus.Text = "USB Removed..";
                            listView1.Items.Add(itemb);
                            
                            logFile(snotxtb + "\t" + dateb + "\t" + timeb + "\t" + VolLable + "\tUSB Disconnected");

                            //sw.Close();

                            // Network adapter enabling code
                            // Network adapter disabling code
                            Process proce = new Process();
                            proce.StartInfo.CreateNoWindow = true;
                            proce.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            proce.StartInfo.FileName = "cmd.exe";
                            proce.StartInfo.Arguments = string.Format("/c netsh interface set interface " + connName + " ENABLED");
                            proce.Start();


                            //Process.Start("cmd.exe", "/c netsh interface set interface " + connName + " ENABLED");
                            break;
                    }
                    break;
            }

            
        }

        private void logFile(String text)
        {
            String date = DateTime.Now.ToString("ddMMyyyy");
            StreamWriter swFile = new StreamWriter((string)("log_"+date+".txt"), append: true);
            swFile.WriteLine(text);
            swFile.Close();
        }



        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {  
            
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            lblUsbStatus.ForeColor = System.Drawing.Color.Green;
            lblUsbStatus.Text = "No USB Detected..";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
