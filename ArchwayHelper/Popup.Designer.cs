namespace ArchwayHelper
{
    partial class Popup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.popupClose = new System.Windows.Forms.Label();
            this.labelTask = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.timerPopup = new System.Windows.Forms.Timer(this.components);
            this.buttonMute = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.buttonSnooze = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // popupClose
            // 
            this.popupClose.AutoSize = true;
            this.popupClose.Location = new System.Drawing.Point(503, 2);
            this.popupClose.Name = "popupClose";
            this.popupClose.Size = new System.Drawing.Size(25, 17);
            this.popupClose.TabIndex = 2;
            this.popupClose.Text = " X ";
            this.popupClose.Click += new System.EventHandler(this.popupClose_Click);
            // 
            // labelTask
            // 
            this.labelTask.AutoSize = true;
            this.labelTask.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTask.Location = new System.Drawing.Point(79, 18);
            this.labelTask.MaximumSize = new System.Drawing.Size(480, 50);
            this.labelTask.Name = "labelTask";
            this.labelTask.Size = new System.Drawing.Size(160, 20);
            this.labelTask.TabIndex = 3;
            this.labelTask.Text = "The task description";
            this.labelTask.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTime.Location = new System.Drawing.Point(23, 18);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(50, 20);
            this.labelTime.TabIndex = 7;
            this.labelTime.Text = "00:00";
            // 
            // timerPopup
            // 
            this.timerPopup.Enabled = true;
            this.timerPopup.Interval = 1000;
            this.timerPopup.Tick += new System.EventHandler(this.timerPopup_Tick);
            // 
            // buttonMute
            // 
            this.buttonMute.BackColor = System.Drawing.SystemColors.HighlightText;
            this.buttonMute.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonMute.Image = global::ArchwayHelper.Properties.Resources.mutesmall;
            this.buttonMute.Location = new System.Drawing.Point(212, 62);
            this.buttonMute.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonMute.Name = "buttonMute";
            this.buttonMute.Size = new System.Drawing.Size(117, 57);
            this.buttonMute.TabIndex = 6;
            this.buttonMute.Text = "Mute";
            this.buttonMute.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMute.UseVisualStyleBackColor = false;
            this.buttonMute.Click += new System.EventHandler(this.buttonMute_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Blue;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(11, 130);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // buttonSnooze
            // 
            this.buttonSnooze.BackColor = System.Drawing.SystemColors.HighlightText;
            this.buttonSnooze.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSnooze.Image = global::ArchwayHelper.Properties.Resources.cancelsmall;
            this.buttonSnooze.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonSnooze.Location = new System.Drawing.Point(359, 62);
            this.buttonSnooze.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSnooze.Name = "buttonSnooze";
            this.buttonSnooze.Size = new System.Drawing.Size(159, 57);
            this.buttonSnooze.TabIndex = 1;
            this.buttonSnooze.Text = "Snooze for 10 mins";
            this.buttonSnooze.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonSnooze.UseVisualStyleBackColor = false;
            this.buttonSnooze.Click += new System.EventHandler(this.buttonSnooze_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.BackColor = System.Drawing.SystemColors.HighlightText;
            this.buttonCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonCancel.Image = global::ArchwayHelper.Properties.Resources.oksmall;
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonCancel.Location = new System.Drawing.Point(27, 62);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(156, 57);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Finish/ Cancel Task";
            this.buttonCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // Popup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(533, 130);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.buttonMute);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.labelTask);
            this.Controls.Add(this.popupClose);
            this.Controls.Add(this.buttonSnooze);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Popup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Popup";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSnooze;
        private System.Windows.Forms.Label popupClose;
        private System.Windows.Forms.Label labelTask;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button buttonMute;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Timer timerPopup;
    }
}