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
namespace EndSession
{
   
    public partial class EndSession : Form
    {
        //initialize variables
        //Popup Counter.  This ensures we only see the message windows that popups in the last 2 minutes once.
        int popupCounter = 0;
        
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
            //Hide the windows for the program so no one can see it running
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tagLASTINPUTINFO LastInput = new tagLASTINPUTINFO();
            Int32 IdleTime;
            LastInput.cbSize = (uint)Marshal.SizeOf(LastInput);
            LastInput.dwTime = 0;
           
            //This is where I am going to put a check for a running program



            if (GetLastInputInfo(ref LastInput))
            {
                
                IdleTime = System.Environment.TickCount - LastInput.dwTime;
              

                //DEBUGGING Text Field
               // label1.Text = IdleTime + " ms " + rowCounter + "run time:" + "last input" + LastInput.dwTime + "App start" + ((DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalMilliseconds);
                // label1.Text = idleStart +" "+ idleEnd;
                
            }
          
    IdleTime = System.Environment.TickCount - LastInput.dwTime;

          

            //Check the idle time.  If less then 100ms close the application.
            //The Reason for 100ms is if it is set to 0, the program doesn't always catch the user input.  If it is set to high the program closes right away.
            if (IdleTime <= 100)
            {
               
                Application.Exit();
            }

            //This is our time check.  If we were to use the system idle time the program would close at the time the
            //user stopped the keyboard/mouse input  We are starting the idletime based on when our app lauches
            if ((DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalMilliseconds > 780000)
            
            {
                if (popupCounter == 0)
                {
                    //increment our popup counter and show our message box.  Force it to main focus over all other windows
                    //MessageBoxOptions 0x40000
                    popupCounter++;
                    DialogResult AutoResult = MessageBox.Show("This System will reboot in 2 minutes if it is left idle", "End Session Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning,
               MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                }

            }
            //If it is 900000ms or 15 minutes and greater since the launch or the app, force close all programs and restart.
            if ((DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalMilliseconds > 900000)
            {
               // SendKeys.Send("{Enter}");
                System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 0");
            }


        }

    }
}

