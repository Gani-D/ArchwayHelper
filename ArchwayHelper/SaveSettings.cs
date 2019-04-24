using Microsoft.Win32;


namespace ArchwayHelper
{
    class SaveSettings
    {
        public void SetSettings(string smtp, string port, string from, string to, bool auth, string username)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\ArchwayHelper");
            key.SetValue("SMTP", smtp);
            key.SetValue("Port", port);
            key.SetValue("From", from);
            key.SetValue("To", to);
            key.SetValue("Auth", auth.ToString());
            key.SetValue("User", username);
            
            key.Close();
        }

        public string RemoveSettings()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\ArchwayHelper");
            if (key == null)
            {
                return "No settings were detected";
            }
            else
            {
                Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\ArchwayHelper");
             }
            return "The settings were deleted";
        }

        public string[] GetSettings ()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\ArchwayHelper");
            if (key != null)
            {
                string[] ret = {"The settings were successfully read from HKCU\\Software\\ArchwayHelper", "","","","","","" };


                try
                {
                    ret[1] = key.GetValue("SMTP").ToString();
                    ret[2] = key.GetValue("Port").ToString();
                    ret[3] = key.GetValue("From").ToString();
                    ret[4] = key.GetValue("To").ToString();
                    ret[5] = key.GetValue("Auth").ToString();
                    ret[6] = key.GetValue("User").ToString();
                    return ret;
                }
                catch
                {
                    Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\ArchwayHelper");
                    System.Windows.Forms.MessageBox.Show("The registry key was damaged, so the settings were deleted");
                    return null;
                }
              
            }
            else
            {
                return null;
            }
            
        }
    }
}
