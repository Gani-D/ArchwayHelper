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

        public static void ChangeTimer(int id, string time, string description, bool isActive)
        {
            string timebox = "timebox" + id.ToString();
            
            string timetext = "timetext" + id.ToString();
            string checktime = "checktime" + id.ToString();
            formMain.Controls.Find(timebox, true)[0].Text=time;
            formMain.Controls.Find(timetext, true)[0].Text = description;
            ((CheckBox)(formMain.Controls.Find(checktime, true)[0])).Checked = isActive;
            
        }
       
    }
}
