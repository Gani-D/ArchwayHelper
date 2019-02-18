using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchwayHelper
{
    public partial class FormMain
    {

        public GlobalHotkey globalHotkey; // to close GlobalHotkey form
        private static FormMain formMain; // allowing access to FormMain to static methods

  
        public static void ChangeFirstTimer(string time, string description, bool isActive)
        {
            formMain.timebox1.Text = time;
            formMain.timetext1.Text = description;
            formMain.checktime1.Checked = isActive;
        }
        public static void ChangeSecondTimer(string time, string description, bool isActive)
        {
            formMain.timebox2.Text = time;
            formMain.timetext2.Text = description;
            formMain.checktime2.Checked = isActive;
        }
        public static void ChangeThirdTimer(string time, string description, bool isActive)
        {
            formMain.timebox3.Text = time;
            formMain.timetext3.Text = description;
            formMain.checktime3.Checked = isActive;
        }
        public static void ChangeFourthTimer(string time, string description, bool isActive)
        {
            formMain.timebox4.Text = time;
            formMain.timetext4.Text = description;
            formMain.checktime4.Checked = isActive;
        }
        public static void ChangeFifthTimer(string time, string description, bool isActive)
        {
            formMain.timebox5.Text = time;
            formMain.timetext5.Text = description;
            formMain.checktime5.Checked = isActive;
        }


        public bool checkalarm = false;


    }
}
