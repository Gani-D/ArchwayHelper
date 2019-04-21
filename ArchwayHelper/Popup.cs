using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchwayHelper
{
    public partial class Popup : Form
    {
        private int position;
        public Popup(int position, string time, string description, bool mute = false)
        {
            InitializeComponent();
            this.position = position;
            labelTime.Text = time;
            labelTask.Text = description;
            if (mute)
            {
                timerPopup.Enabled = false;
            }
        }
        
        protected override void OnLoad(EventArgs e)
        {
            
            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);
            base.OnLoad(e);
            this.ShowInTaskbar = false;
           
        }

        private void popupClose_Click(object sender, EventArgs e)
        {
            Tasks tasks = new Tasks();
            tasks.SnoozeTask(position, 5);
            this.Close();
            
        }

        

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Tasks tasks = new Tasks();
            tasks.CancelTask(position);
            this.Close();
        }


        private void timerPopup_Tick(object sender, EventArgs e)
        {
            timerPopup.Interval = 20000;
            System.IO.Stream wavStream = Properties.Resources.woop2;
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(wavStream);
             player.Play();
        }

        private void buttonMute_Click(object sender, EventArgs e)
        {
            timerPopup.Enabled = false;
        }

        private void buttonSnooze_Click(object sender, EventArgs e)
        {
            Tasks tasks = new Tasks();
            tasks.SnoozeTask(position);
            

            this.Close();
        }
    }
}
