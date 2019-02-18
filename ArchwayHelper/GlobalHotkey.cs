using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ArchwayHelper
{
    public partial class GlobalHotkey : Form
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);



        // 3. Import the UnregisterHotKey Method
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public FormMain mainForm;
        public GlobalHotkey (FormMain mainForm)
        {
            this.mainForm = mainForm;
        }

        protected override void WndProc(ref Message m)
        {
            // 5. Catch when a HotKey is pressed !
            if (m.Msg == 0x0312)
            {
                int id = m.WParam.ToInt32();
                // MessageBox.Show(string.Format("Hotkey #{0} pressed", id));

                // 6. Handle what will happen once a respective hotkey is pressed
                switch (id)
                {
                    case 1:
                        TextCopy prevLine = new TextCopy();
                        prevLine.GetPreviousLine(FormMain.GetRichText());
                        break;
                    case 2:
                        TextCopy getLine = new TextCopy();
                        getLine.GetCurrentLine(FormMain.GetRichText());
                        break;
                    case 3:
                        TextCopy nextLine = new TextCopy();
                        nextLine.GetNextLine(FormMain.GetRichText());
                        break;
                    case 4:

                        TextCopy copyOwnTextOne = new TextCopy();
                        copyOwnTextOne.CopyOwnText(FormMain.GetOwnTexts()[0]);
                        break;
                    case 5:
                        TextCopy copyOwnTextTwo = new TextCopy();
                        copyOwnTextTwo.CopyOwnText(FormMain.GetOwnTexts()[1]);
                        break;
                }
            }

            base.WndProc(ref m);
        }

        

        public GlobalHotkey()
        {
            InitializeComponent();
            
        }

        private void GlobalHotkey_Load(object sender, EventArgs e)
        {
            //the form shouldn't be visible
            ShowInTaskbar = false;
            this.Opacity = 0;
            this.Visible = false;



            // Set an unique id to your Hotkey, it will be used to
            // identify which hotkey was pressed in your code to execute something
            int UniqueHotkeyId = 1;
            
            int HotKeyCode = (int)Keys.F6;
            bool F6Registered = RegisterHotKey(
                this.Handle, UniqueHotkeyId, 0x0000, HotKeyCode
            );

            int SecondHotkeyId = 2;
            int SecondHotKeyKey = (int)Keys.F7;
            bool F7Registered = RegisterHotKey(
                this.Handle, SecondHotkeyId, 0x0000, SecondHotKeyKey
            );


            int thirdHotkeyId = 3;
            int thirdHotKeyKey = (int)Keys.F8;
            Boolean F8Registered = RegisterHotKey(
                this.Handle, thirdHotkeyId, 0x0000, thirdHotKeyKey
            );
            

            int FourthHotkeyId = 4;
            int FourthHotKeyKey = (int)Keys.F9;
            Boolean F9Registered = RegisterHotKey(
                this.Handle, FourthHotkeyId, 0x0000, FourthHotKeyKey
            );

            int FifthHotkeyId = 5;
            int FifthHotKeyKey = (int)Keys.F10;
            Boolean F10Registered = RegisterHotKey(
                this.Handle, FifthHotkeyId, 0x0000, FifthHotKeyKey
            );

        }

    }
    
}
