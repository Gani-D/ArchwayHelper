using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchwayHelper
{
    class Tasks
    {
        static string[] tasksTimeText; // keeps the time, so it doesn't need to sort and check the time
        
        #region TimeBoxLeave // triggered when Timebox is left
        public static string TimeBoxLeave(string[] time, int position)
        {

            time[position] = CorrectTime(time[position]);
            if (time[position].Length > 0)
            {
                if (!CheckTime(time[position]))
                {
                    System.Windows.Forms.MessageBox.Show("Please use correct time, not " + time[position]); return "";
                }
                else if (!TimeIsUnique(time, position))
                {

                    System.Windows.Forms.MessageBox.Show("The time is not unique");
                    return "";
                }

            }
            return time[position];

        }

        private static bool CheckTime(string time) //checks if the time has the right format
        {
            if (time.Length < 3) return false;
            if (time[0] - '2' > 0) return false;
            if (time[1] - '3' > 0 && time[0] - '2' == 0) return false;
            if (time[1] - '9' > 0) return false;
            if (time[2] != ':') return false;
            if (time[3] - '5' > 0) return false;
            if (time[4] - '9' > 0) return false;

            return true;
        }

        public static string CorrectTime(string time) //corrects time from 815 to 08:15 or 
        {
            if (time.Length < 3) return time;
            if (time.Length == 3)
            {
                StringBuilder temp = new StringBuilder(5);
                temp.Append("0"); temp.Append(time[0]); temp.Append(':'); temp.Append(time.Substring(1));
                return temp.ToString();
            }
            if (time.Length == 4 && time[1] != ':')
            {
                StringBuilder temp = new StringBuilder(5);
                temp.Append(time.Substring(0, 2)); temp.Append(':'); temp.Append(time.Substring(2));
                return temp.ToString();
            }
            if (time.Length == 4 && time[1] == ':')
            {
                StringBuilder temp = new StringBuilder(5);
                temp.Append('0'); temp.Append(time.Substring(0));
                return temp.ToString();
            }
            return time;
        }

        public static bool TimeIsUnique (string[] time, int position) //if the time unique
        {
            int timeCount = 0;
            if (time[position].Length > 4)
            {
                for (int i=0; i<5; i++)
                {
                    if (time[i] == time[position]) timeCount++;
                }
                if (timeCount > 1) return false;
            }
            return true;

        }
        #endregion

        #region SortTasks // sorts the tasks by time 
        public void SortTasks(string [] timeTemp, string [] text, bool [] isActive)
        {
            string[] time = { "", "", "", "", ""};
           
            int count = 0; //to count the number of active tasks
            Dictionary<string, string> tasks = new Dictionary<string, string>(8);
            tasksTimeText = new string[10];

            for (int i=0; i<5; i++)
            {
                if (isActive[i])
                {
                    time[i] = timeTemp[i];
                    tasks.Add(time[i], text[i]); count++;
                }
            }
           // System.Windows.Forms.MessageBox.Show(count.ToString() );
            
             if (count > 0)
            {
                Array.Sort(time);
                
                count = 0; //using count as new counter
                SetTimerFields(6, "", "");
                for (int i = 0; i < 5; i++)
                {
                    if (time[i].Length > 4)
                    {
                        string tempDescription = tasks[time[i]];
                        tasksTimeText[count*2] = time[i]; //adding time to a static field
                       // System.Windows.Forms.MessageBox.Show("Adding to tasksTimeText "+ time[i]+ tempDescription);
                        tasksTimeText[count * 2+1] = tempDescription; // adding task description to a static field
                        SetTimerFields(count, time[i], tempDescription);
                        count++;
                    }
                }
                
            }
            else
            {
                SetTimerFields(6, "", "");
                tasksTimeText = null;
            }
        } //sorts the tasks by time

        private void SetTimerFields (int position, string time, string description)
        {
            bool isActive = (time.Length > 0) ? true : false;
            switch (position)
            {
                case 0: FormMain.ChangeFirstTimer(time, description, isActive); break;
                case 1: FormMain.ChangeSecondTimer(time, description, isActive); break;
                case 2: FormMain.ChangeThirdTimer(time, description, isActive); break;
                case 3: FormMain.ChangeFourthTimer(time, description, isActive); break;
                case 4: FormMain.ChangeFifthTimer(time, description, isActive); break;
                case 6:
                    FormMain.ChangeFirstTimer("", "", false);
                    FormMain.ChangeSecondTimer("", "", false);
                    FormMain.ChangeThirdTimer("", "", false);
                    FormMain.ChangeFourthTimer("", "", false);
                    FormMain.ChangeFifthTimer("", "", false);
                    break;
            }
        }
        #endregion  

        public static bool CheckTasks(bool mute)
        {
            if (tasksTimeText == null) { return false; }
            string time = DateTime.Now.ToString("HH:mm");
            //System.Windows.Forms.MessageBox.Show(time);
            for (int i=0; i<10; i+=2)
            {
               // System.Windows.Forms.MessageBox.Show(tasksTimeText[i]);
                if (tasksTimeText[i] == null||tasksTimeText[i].Length < 4) continue;
                else if (time==tasksTimeText[i])
                {
                   // System.Windows.Forms.MessageBox.Show("2");
                    Popup popup = new Popup(i / 2, tasksTimeText[i], tasksTimeText[i + 1], mute);
                    popup.Show();

                    return true;
                }
            }
            return false;
        }

        public void SnoozeTask(int taskNumber, int interval = 10)
        {
            //string time = DateTime.Now.AddMinutes(interval).ToString("HH:mm");
            tasksTimeText[taskNumber * 2] = DateTime.Now.AddMinutes(interval).ToString("HH:mm");
            if (!TimeIsUnique(new string[] { tasksTimeText[0], tasksTimeText[2], tasksTimeText[4], tasksTimeText[6], tasksTimeText[8] }, taskNumber))
            {
                //MessageBox.Show("Time is not unique");
                do
                {
                    interval += 3;
                    tasksTimeText[taskNumber * 2] =  DateTime.Now.AddMinutes(interval).ToString("HH:mm"); 
                }
                while (!TimeIsUnique(new string[] { tasksTimeText[0], tasksTimeText[2], tasksTimeText[4], tasksTimeText[6], tasksTimeText[8] }, taskNumber));
                
            }

            string[] timeTemp = { tasksTimeText[0], tasksTimeText[2], tasksTimeText[4], tasksTimeText[6], tasksTimeText[8]};
            string[] description = { tasksTimeText[1], tasksTimeText[3], tasksTimeText[5], tasksTimeText[7], tasksTimeText[9] };
            bool[] isActive = new bool[5];
            for (int i = 0; i < 10; i += 2)
            {
                if (tasksTimeText[i] != null && tasksTimeText[i].Length > 4)
                    isActive[i / 2] = true;
            }
            SortTasks(timeTemp, description, isActive);

        }

      

        public void CancelTask(int taskNumber)
        {
            
            tasksTimeText[taskNumber * 2] = null;
            

            string[] timeTemp = { tasksTimeText[0], tasksTimeText[2], tasksTimeText[4], tasksTimeText[6], tasksTimeText[8] };
            string[] description = { tasksTimeText[1], tasksTimeText[3], tasksTimeText[5], tasksTimeText[7], tasksTimeText[9] };

            bool[] isActive = new bool[5];
            for (int i=0; i<10; i+=2)
            {
                if (tasksTimeText[i] != null && tasksTimeText[i].Length > 4)
                    isActive[i / 2]= true;
            }
            
            SortTasks(timeTemp, description, isActive);
        }

        public static string GetFirstTask ()
        {
            if (tasksTimeText == null) return "No upcoming events";
            if (tasksTimeText[0] == null || tasksTimeText[0].Length < 4)
            {
                return "No upcoming events";
            }
            else return tasksTimeText[0] + " " + tasksTimeText[1];
        }
        public static string GetSecondTask()
        {
            if (tasksTimeText == null) return "No upcoming events";
            if (tasksTimeText[2] == null || tasksTimeText[2].Length < 4)  return "No upcoming events"; 
            else return tasksTimeText[2] + " " + tasksTimeText[3];
        }
    }
}
