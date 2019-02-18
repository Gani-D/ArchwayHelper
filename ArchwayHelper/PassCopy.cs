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
    public partial class PassCopy : Form
    {
        
        public PassCopy()
        {
            InitializeComponent();
        }

        public PassCopy(string text)
        {
            InitializeComponent();
            MyText = text;
        }

        public string MyText { get; set; }

        private void PassCopy_Load(object sender, EventArgs e)
        {
            if (MyText != null)
                labelCopiedData.Text = MyText;
            
        }
        protected override void OnLoad(EventArgs e)
        {
            var screen = Screen.FromPoint(this.Location);
            this.Location = new Point(screen.WorkingArea.Right - this.Width, screen.WorkingArea.Bottom - this.Height);
            base.OnLoad(e);
            this.ShowInTaskbar = false;
            
                       
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Interval = 100;
            this.Opacity -= 0.1;
            if (this.Opacity == 0) { this.Close(); }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
