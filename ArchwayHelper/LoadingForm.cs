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
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
           
            
        }
        private static LoadingForm loadingFormInstance = null;
        private static readonly object padlock = new object();
        public static LoadingForm Instance  //Singleton
        {
            get
            {
                lock (padlock)
                {
                    if (loadingFormInstance == null)
                    {
                        loadingFormInstance = new LoadingForm();
                    }
                    return loadingFormInstance;
                }
            }
        }
        public void SetText (string text)
        {

        }

        private void LoadingForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
