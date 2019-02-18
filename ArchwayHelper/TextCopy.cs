
using System;
using System.Media;
using System.Text;
using System.Windows.Forms;

namespace ArchwayHelper
{
    class TextCopy
    {
        public static int _lineNum { get; set; }
        private static int txtToCopyHash;
        private static string[] txtLines;
        private void SetText (string textToCopy)
        {
            if (textToCopy.GetHashCode() !=txtToCopyHash)
            {
                txtToCopyHash = textToCopy.GetHashCode();
                txtLines = textToCopy.Split('\n');
               // if (txtLines.Length < _lineNum) _lineNum = 0;
            }
            
        }
        public string GetCurrentLine (string text)
        {
            SetText(text);
            if (_lineNum == 0)
            {
                PassCopy popup = new PassCopy();
                popup.Show();
                System.Windows.Forms.Clipboard.Clear();
                SystemSounds.Beep.Play();
                return "Cannot copy the text";
            }
            
            if (txtLines.Length>_lineNum&&txtLines[_lineNum-1].Length>0)
            {
                string tempLine = txtLines[_lineNum-1];
                PassCopy popup = new PassCopy(tempLine);
                popup.Show();
                System.Windows.Forms.Clipboard.SetText(tempLine);
                return "The current line has been copied";
            }
            else if (txtLines[_lineNum - 1].Length == 0)
            {
                PassCopy popup = new PassCopy("Empty line");
                popup.Show();
                System.Windows.Forms.Clipboard.Clear();
                SystemSounds.Beep.Play();
                return "Empty line";
            }

            else
            {
                PassCopy popup = new PassCopy();
                popup.Show();
                System.Windows.Forms.Clipboard.Clear();
                SystemSounds.Beep.Play();

            }
            return "Cannot copy the text";
        }

        private void ShowPopup(string text, string clipboardText=null)
        {
            if (clipboardText==null||clipboardText.Length<1)
            {
                System.Windows.Forms.Clipboard.Clear();
                PassCopy popup = new PassCopy(text);
                popup.Show();
            }
            else
            {
                System.Windows.Forms.Clipboard.SetText(clipboardText);
                PassCopy popup = new PassCopy(text);
                popup.Show();
            }
        }

        public string GetNextLine (string text)
        {
            SetText(text);
            if (_lineNum<txtLines.Length)
            {
                string tempLine = txtLines[_lineNum++];
                if (tempLine.Length > 0)
                {
                    ShowPopup(tempLine, tempLine);
                    FormMain.SetLabelText(_lineNum.ToString());
                    return "The next line has been copied";
                }
                else
                {
                    ShowPopup("Empty line");
                    FormMain.SetLabelText(_lineNum.ToString());
                    return "Empty line";
                }
            }
            ShowPopup("No text to copy");
            FormMain.SetLabelText(_lineNum.ToString());
            return "Cannot copy the text";
        }

        public string GetPreviousLine (string text)
        {
            SetText(text);
            if (_lineNum>1&&_lineNum<=txtLines.Length)
            {
                string tempLine = txtLines[--_lineNum-1];
                FormMain.SetLabelText(_lineNum.ToString());
                if (tempLine.Length > 0)
                {
                    ShowPopup(tempLine, tempLine);
                    return "The previous line has been copied";
                }
                else
                {
                    ShowPopup("Empty line");
                    return "Empty line";
                }
            }
            else if (_lineNum>txtLines.Length&&txtLines.Length>1)
            {
                _lineNum = txtLines.Length - 1;
                if (txtLines[_lineNum].Length > 0)
                {
                    ShowPopup(txtLines[_lineNum], txtLines[_lineNum]);
                    FormMain.SetLabelText(_lineNum.ToString());
                    return "The previous line has been copied";
                }
                else
                {
                    ShowPopup("Empty line");
                    FormMain.SetLabelText(_lineNum.ToString());
                    return "Empty line";
                }

            }
            else
            {
                ShowPopup("No text to copy");
                return "No text to copy";
            }
        }

        public string [] LinNumChange(string theText, string text)
        {
            SetText(theText);
            if (text == "0"||text=="1")
            {
                _lineNum = 0;
                
                return  new string[] {"0", "The line number has been successfully changed" };
            }
            else
                try
                {
                    int tmp = Convert.ToInt32(text) - 1;
                    if (txtLines.Length >= tmp && tmp >= 0)
                    {
                        _lineNum = tmp;
                        return new string[] {_lineNum.ToString() ,"The line number has been successfully changed" };
                        
                    }
                    else
                    {
                        return new string[] { _lineNum.ToString(), "The text doesn't contain that many lines" };
                    }
                }
                catch { return new string[] { _lineNum.ToString(), "Please use the right number" }; }
        }

        public string RemoveCharacter (string text, string strToRem)
        {
            if (strToRem.Length == 0) return text;
            char charToRem = strToRem[0];
            if (text.Length > 0)
            {
                StringBuilder newtext = new StringBuilder(text.Length);
                foreach (char richchar in text)
                {
                    if (richchar != charToRem) newtext.Append(richchar);
                }
                return newtext.ToString();
                
            }
            else System.Windows.Forms.MessageBox.Show("Empty text");
            return "";
        }

        public void CopyOwnText (string text)
        {
            if (text.Length == 0)
            {
                Clipboard.Clear();
                SystemSounds.Beep.Play();
                PassCopy passcopy = new PassCopy();
                passcopy.Show();
            }
            else
            {
                Clipboard.SetText(text);
                PassCopy passCopy = new PassCopy(text);
                passCopy.Show();
            }
        }

        public string ChangeTabsToEnter (string text)
        {
            if (text.Length > 0)
            {
                
                    StringBuilder textLines = new StringBuilder(text.Length);
                    //checkBoxRemovLines.Enabled = false;
                    for (int i=0; i<text.Length; i++)
                    {
                        if (text[i] == '\t') textLines.Append('\n');
                        else textLines.Append(text[i]);
                    }

                    return textLines.ToString();
                
            }
            return text;
        }

        public string RemoveTabs(string text)
        {
            if (text.Length > 0)
            {
                StringBuilder textLines = new StringBuilder(text.Length);
                for (int i = 0; i < text.Length; i++)
                    {
                    if (text[i] == '\t') { }
                    else textLines.Append(text[i]);
                    }

                    return textLines.ToString();
             }
             return text;
        }
        public string RemoveEmptyLines (string text)
        {
            if (text.Length>0)
            {
                SetText(text);
                StringBuilder tempText = new StringBuilder(text.Length);
                for (int i=0; i<txtLines.Length; i++)
                {
                    for (int j=0; j<txtLines[i].Length; j++)
                    {
                        if (txtLines[i][j]!=' ')
                        {
                            tempText.AppendLine(txtLines[i]);
                            break ;
                        }
                        
                    }
                }
                if (tempText.Length>2)
                tempText.Remove(tempText.Length - 2, 2);
                return tempText.ToString();
            }
            return text;
        }


    }
}
