using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchwayHelper
{
    class Tasks
    {
        /// <summary>
        /// Needed to keep the time and text of the current tasks, so it doesn't sort and checks every 10 seconds
        /// </summary>
        static string[] tasksTimeText; // keeps the time, so it doesn't need to sort and check the time
        
        #region TimeBoxLeave // triggered when Timebox is left
        /// <summary>
        /// Checks if the time is correct when you leaving the textbox. Displays a message if time is wrong.
        /// </summary>
        /// <param name="time">The entered time</param>
        /// <param name="position">Textbox position</param>
        /// <returns>The formatted time</returns>
        public static string TimeBoxLeave(string[] time, int position)
        {
            //trying to normalize the time, converting 000 to 00:00 or 0234 to 02:34, etc.
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
        /// <summary>
        /// Checks if the time has the right format
        /// </summary>
        /// <param name="time">Time</param>
        /// <returns>True if time format is right</returns>
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
        /// <summary>
        /// Tries to get a time from numbers, like 444 to 04:44, 4:12 to 04:12, etc.
        /// </summary>
        /// <param name="time">Data converting to a time</param>
        /// <returns></returns>
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
        /// <summary>
        /// Checks if the time is unique, you can't have 2 appointments at the same time
        /// </summary>
        /// <param name="time">Getting an array of time from all textboxes</param>
        /// <param name="position">Position of the checking time</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sorting tasks by time
        /// </summary>
        /// <param name="timeTemp">Array of tasks' time</param>
        /// <param name="text">Array of upcoming task's description</param>
        /// <param name="isActive">Array of upcoming task's active status</param>
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
                       
                        tasksTimeText[count * 2+1] = tempDescription; // adding task description to a static field
                        SetTimerFields(count+1, time[i], tempDescription); // +1 is for SetTimerFields method, it needs numbers from 1 to 5.
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
        /// <summary>
        /// Updating the position of sorted tasks
        /// </summary>
        /// <param name="position">The position of the time and text needed to update</param>
        /// <param name="time">The time of the task</param>
        /// <param name="description">The description of the task</param>
        private void SetTimerFields (int position, string time, string description)
        {
            if (position == 6) // purges all information from the textboxes
            {
                FormMain.ChangeTimer(1, "", "", false);
                FormMain.ChangeTimer(2, "", "", false);
                FormMain.ChangeTimer(3, "", "", false);
                FormMain.ChangeTimer(4, "", "", false);
                FormMain.ChangeTimer(5, "", "", false);
            }
            else
            {
                bool isActive = (time.Length > 0) ? true : false;
                FormMain.ChangeTimer(position, time, description, isActive);
            }
        }
        #endregion  
        /// <summary>
        /// Checks the static array if it has any entries and checks if the current time equals to the time of any appointments
        /// </summary>
        /// <param name="mute">Disable sound</param>
        /// <returns></returns>
        public static bool CheckTasks(bool mute)
        {
            if (tasksTimeText == null) { return false; }
            string time = DateTime.Now.ToString("HH:mm");
            
            for (int i=0; i<10; i+=2)
            {
               
                if (tasksTimeText[i] == null||tasksTimeText[i].Length < 4) continue;
                else if (time==tasksTimeText[i])
                {
                   
                    Popup popup = new Popup(i / 2, tasksTimeText[i], tasksTimeText[i + 1], mute);
                    popup.Show();

                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Adds 5 or 10 mins to the active task.
        /// </summary>
        /// <param name="taskNumber">The number of the task</param>
        /// <param name="interval">Adding the minutes to the task</param>
        public void SnoozeTask(int taskNumber, int interval = 10)
        {
            
            tasksTimeText[taskNumber * 2] = DateTime.Now.AddMinutes(interval).ToString("HH:mm");
            if (!TimeIsUnique(new string[] { tasksTimeText[0], tasksTimeText[2], tasksTimeText[4], tasksTimeText[6], tasksTimeText[8] }, taskNumber))
            {
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

      
        /// <summary>
        /// Remove the task from the list
        /// </summary>
        /// <param name="taskNumber">The number of the taks</param>
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
        /// <summary>
        /// Getting a first task in the list
        /// </summary>
        /// <returns>The time and text of the first task</returns>
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
