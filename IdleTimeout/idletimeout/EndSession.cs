using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Diagnostics;
using System.Windows.Forms;

//namespace WindowsFormsApplication1
namespace SAPLEndSession
{

public partial class EndSession : Form
    {
        [DllImport("user32.dll")]
        public static extern Boolean GetLastInputInfo(ref tagLASTINPUTINFO plii);
        public struct tagLASTINPUTINFO
        {
            public uint cbSize;
            public Int32 dwTime;

          
        }

        public EndSession()
        {
            InitializeComponent();
            //Hide the program
           // this.WindowState = FormWindowState.Minimized;
           // this.ShowInTaskbar = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tagLASTINPUTINFO LastInput = new tagLASTINPUTINFO();
            Int32 IdleTime;
            LastInput.cbSize = (uint)Marshal.SizeOf(LastInput);
            LastInput.dwTime = 0;
            //Check to see if a program is running if it is quit the session timeout program
            if (Process.GetProcessesByName("SAPL_Login").Length > 0)
            {
                Application.Exit();
            }
     



            if (GetLastInputInfo(ref LastInput))
            {

                IdleTime = System.Environment.TickCount - LastInput.dwTime;
                label1.Text = IdleTime + "ms";

            }
    IdleTime = System.Environment.TickCount - LastInput.dwTime;

            if (IdleTime > 780000 && IdleTime < 780100)
            
            {
              DialogResult AutoResult =  MessageBox.Show("This System will reboot in 2 minutes if it is left idle", "End Session Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning,
     MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);              

            }
            if (IdleTime >= 900000)
            {
               // if the IdleTime is more then 15 minutes (900000 ms)
                System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 0");
            }


        }

    }
}

