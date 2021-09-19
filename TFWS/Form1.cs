using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Diagnostics;

namespace TFWS
{
    public partial class Form1 : Form
    {
        // Service Controller Config
        WindowsServiceMonitor SysMain = new WindowsServiceMonitor("SysMain");
        WindowsServiceMonitor WinSearch = new WindowsServiceMonitor("WSearch");
        WindowsServiceMonitor WinUpdate = new WindowsServiceMonitor("wuauserv");
        RegistryKey ACRG = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows", true);

        public void SysMainCheck()
        {
            // -> Check SysMain Status
            try
            {
                groupBox1.Enabled = false;
                if (SysMain.IsDisabled && SysMain.IsStopped)
                {
                    SysMainEnableDisableShow.ForeColor = Color.DarkRed;
                    SysMainEnableDisableShow.Text = "SysMain is Disable";
                    SysMain_EnableDisableBTN.Text = "Enable";
                    groupBox1.Enabled = true;
                }
                else if (SysMain.IsRunning)
                {
                    SysMainEnableDisableShow.ForeColor = Color.DarkGreen;
                    SysMainEnableDisableShow.Text = "SysMain is Enable";
                    SysMain_EnableDisableBTN.Text = "Disable";
                    groupBox1.Enabled = true;
                }
                else
                {
                    SysMainEnableDisableShow.ForeColor = Color.DarkGreen;
                    SysMainEnableDisableShow.Text = "SysMain is Enable";
                    SysMain_EnableDisableBTN.Text = "Disable";
                    groupBox1.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show("Unforeseen error", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // --------------------------
        }
        public void WinSearchCheck()
        {
            WinSerachGroup.Enabled = false;
            try
            {
                if (WinSearch.IsDisabled)
                {
                    WinSearchShow.ForeColor = Color.DarkRed;
                    WinSearchShow.Text = "WinSearch is Disable";
                    WinSearch_EnableDisableBTN.Text = "Enable";
                    WinSerachGroup.Enabled = true;
                }
                else if (WinSearch.IsRunning)
                {
                    WinSearchShow.ForeColor = Color.DarkGreen;
                    WinSearchShow.Text = "WinSearch is Enable";
                    WinSearch_EnableDisableBTN.Text = "Disable";
                    WinSerachGroup.Enabled = true;
                }
                else
                {
                    WinSearchShow.ForeColor = Color.DarkGreen;
                    WinSearchShow.Text = "WinSearch is Enable";
                    WinSearch_EnableDisableBTN.Text = "Disable";
                    WinSerachGroup.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show("Unforeseen Error : WinSearch", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = $"TFWS {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()}  - By S19MEE";
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;
            WinSerachGroup.Enabled = false;
        }


        private void WhatIsSysMain_Button_Click(object sender, EventArgs e)
        {
            string message = "Sysmain is better known as SuperFetch. Under this name, it already got a bad reputation in Windows 7. It is a service that runs in the background. It continuously scans your computer behavior at the hard disk level and in this way determines which files and applications you use most. Based on this information, SysMain efficiently arranges your hard disk at block level. This service sometimes takes up hard disk space, which slows down your system. Disabling this service can speed up your system.";
            string title = "What is SysMain ?";
            MessageBox.Show(message,title,MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void WhatIsActionCenter_Button_Click(object sender, EventArgs e)
        {
            string message = "Notifications are displayed categorized in the Action Center, users can delete them by swiping to the right. Disabling Action Center can speed up your system.";
            string title = "What is Action Center ?";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SysMain_EnableDisableBTN_Click(object sender, EventArgs e)
        {
            try { 
            if (SysMain.IsRunning && SysMain_EnableDisableBTN.Text == "Disable")
            {
               DialogResult Sysrul = MessageBox.Show("Are you sure you want to Disable SysMain Service ?", "TFWS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Sysrul == DialogResult.Yes)
                    {
                        SysMain.Stop();
                        SysMain.Disable();
                        RegistryKey SysMainRG = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",true);
                        if (SysMainRG.GetValue("EnablePrefetcher") != null)
                        {
                            SysMainRG.DeleteValue("EnablePrefetcher");
                            SysMainRG.SetValue("EnablePrefetcher", "0", RegistryValueKind.DWord);
                        }
                        if (SysMainRG.GetValue("EnableSuperfetch") != null)
                        {
                            SysMainRG.DeleteValue("EnableSuperfetch");
                            SysMainRG.SetValue("EnableSuperfetch", "0", RegistryValueKind.DWord);
                        }
                        MessageBox.Show("SysMain Service is Disabled !", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                SysMainCheck();
            }
            else if (SysMain.IsDisabled && SysMain_EnableDisableBTN.Text == "Enable")
            {
                DialogResult Sysrul = MessageBox.Show("Are you sure you want to Enable SysMain Service ?","TFWS",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                    if (Sysrul == DialogResult.Yes)
                    {
                        SysMain.Enable();
                        SysMain.Start();
                        MessageBox.Show("SysMain Service is Enabled !", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                SysMainCheck();
            }
                else if (SysMain_EnableDisableBTN.Text == "Disable")
                {
                    DialogResult Sysrul = MessageBox.Show("Are you sure you want to Enable SysMain Service ?", "TFWS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (Sysrul == DialogResult.Yes)
                    {
                        SysMain.Stop();
                        SysMain.Disable();
                        MessageBox.Show("SysMain Service is Disabled !", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    SysMainCheck();
                }
            }
            catch
            {
                MessageBox.Show("Unforeseen error", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActionCenter_EnableDisableBTN_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult ACrul = MessageBox.Show("Are you sure you want to Enable Action Center ?", "TFWS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (ACrul == DialogResult.Yes)
                {
                    ACRG.CreateSubKey("Explorer");
                    ACRG = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer", true);
                    ACRG.SetValue("DisableNotificationCenter", "0", RegistryValueKind.DWord);
                    Process p = new Process();
                    foreach (Process exe in Process.GetProcesses())
                    {
                        if (exe.ProcessName == "explorer")
                            exe.Kill();
                    }
                    Process.Start("explorer.exe");
                    MessageBox.Show("Action Center is Enabled !", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Unforeseen error", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActionCenter_DisableBTN_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult ACrul = MessageBox.Show("Are you sure you want to Disable Action Center ?", "TFWS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (ACrul == DialogResult.Yes)
                {
                    ACRG.CreateSubKey("Explorer");
                    ACRG = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer", true);
                    ACRG.SetValue("DisableNotificationCenter", "1", RegistryValueKind.DWord);
                    Process p = new Process();
                    foreach (Process exe in Process.GetProcesses())
                    {
                        if (exe.ProcessName == "explorer")
                            exe.Kill();
                    }
                    Process.Start("explorer.exe");
                    MessageBox.Show("Action Center is Disabled !", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Unforeseen error", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void Form1_Shown(object sender, EventArgs e)
        {
            SysMainCheck();
            WinSearchCheck();
            groupBox2.Enabled = true;
        }

        private void WhyWinSearch_Click(object sender, EventArgs e)
        {
            string message = "Windows keeps indexing files and folders in your PC to make it faster for you to search for things using the Search bar. This indexing is quite heavy on the system resources, and if you don’t use the Search bar often, you are just slowing down your PC for nothing. You should disable search indexing in favor of better performance.";
            string title = "Why Windows Search Service ?";
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void WinSearch_EnableDisableBTN_Click(object sender, EventArgs e)
        {
            try
            {
                if (WinSearch.IsRunning && WinSearch_EnableDisableBTN.Text == "Disable")
                {
                    DialogResult WSrul = MessageBox.Show("Are you sure you want to Disable WinSearch Service ?", "TFWS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (WSrul == DialogResult.Yes)
                    {
                        WinSearch.Stop();
                        WinSearch.Disable();
                        MessageBox.Show("WinSearch Service is Disabled ! Please restart the system", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    WinSearchCheck();
                }
                else if (WinSearch.IsDisabled && WinSearch_EnableDisableBTN.Text == "Enable")
                {
                    DialogResult WSrul = MessageBox.Show("Are you sure you want to Enable WinSearch Service ?", "TFWS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (WSrul == DialogResult.Yes)
                    {
                        WinSearch.Enable();
                        WinSearch.Start();
                        MessageBox.Show("WinSearch Service is Enabled ! Please restart the system", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    WinSearchCheck();
                }
                else if (WinSearch_EnableDisableBTN.Text == "Disable")
                {
                    DialogResult WSrul = MessageBox.Show("Are you sure you want to Enable WinSearch Service ?", "TFWS", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (WSrul == DialogResult.Yes)
                    {
                        WinSearch.Stop();
                        WinSearch.Disable();
                        MessageBox.Show("WinSearch Service is Disabled ! Please restart the system", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    WinSearchCheck();
                }
            }
            catch
            {
                MessageBox.Show("Unforeseen error : WinSearch", "TFWS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GitHub_Click(object sender, EventArgs e)
        {
          
            Process.Start("https://github.com/s19mee/TFWS");
        
        }

    }
}
