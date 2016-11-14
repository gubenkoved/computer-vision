namespace RegionTracking
{
    partial class MainForm
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
            this.ImageBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AccuracyLabel = new System.Windows.Forms.Label();
            this.RegionPictureBox = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ConservativityTrackBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.AccuracyTrackBar = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.MaxVelocityTrackBar = new System.Windows.Forms.TrackBar();
            this.FPSLabel = new System.Windows.Forms.Label();
            this.ControlCursorCheckBox = new System.Windows.Forms.CheckBox();
            this.OpenDeviceButton = new System.Windows.Forms.Button();
            this.AbsolutePositioningCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegionPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConservativityTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccuracyTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxVelocityTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // ImageBox
            // 
            this.ImageBox.BackColor = System.Drawing.Color.White;
            this.ImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImageBox.Location = new System.Drawing.Point(12, 33);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(320, 240);
            this.ImageBox.TabIndex = 0;
            this.ImageBox.TabStop = false;
            this.ImageBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseDown);
            this.ImageBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseMove);
            this.ImageBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ImageBox_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "* select region to tracking on image bellow";
            // 
            // AccuracyLabel
            // 
            this.AccuracyLabel.AutoSize = true;
            this.AccuracyLabel.Location = new System.Drawing.Point(343, 86);
            this.AccuracyLabel.Name = "AccuracyLabel";
            this.AccuracyLabel.Size = new System.Drawing.Size(11, 14);
            this.AccuracyLabel.TabIndex = 3;
            this.AccuracyLabel.Text = "-";
            // 
            // RegionPictureBox
            // 
            this.RegionPictureBox.BackColor = System.Drawing.Color.White;
            this.RegionPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RegionPictureBox.Location = new System.Drawing.Point(338, 33);
            this.RegionPictureBox.Name = "RegionPictureBox";
            this.RegionPictureBox.Size = new System.Drawing.Size(50, 50);
            this.RegionPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.RegionPictureBox.TabIndex = 4;
            this.RegionPictureBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(338, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "Feature";
            // 
            // ConservativityTrackBar
            // 
            this.ConservativityTrackBar.Location = new System.Drawing.Point(352, 121);
            this.ConservativityTrackBar.Maximum = 100;
            this.ConservativityTrackBar.Name = "ConservativityTrackBar";
            this.ConservativityTrackBar.Size = new System.Drawing.Size(136, 45);
            this.ConservativityTrackBar.TabIndex = 1;
            this.ConservativityTrackBar.TickFrequency = 10;
            this.ConservativityTrackBar.Value = 95;
            this.ConservativityTrackBar.Scroll += new System.EventHandler(this.Parameters_Changed);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(386, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 14);
            this.label3.TabIndex = 7;
            this.label3.Text = "Conservativity";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(338, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "0.0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(479, 124);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 14);
            this.label5.TabIndex = 9;
            this.label5.Text = "1.0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(479, 184);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 14);
            this.label6.TabIndex = 13;
            this.label6.Text = "1.0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(338, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 14);
            this.label7.TabIndex = 12;
            this.label7.Text = "0.0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(377, 164);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 14);
            this.label8.TabIndex = 11;
            this.label8.Text = "Accuracy threshold";
            // 
            // AccuracyTrackBar
            // 
            this.AccuracyTrackBar.Location = new System.Drawing.Point(352, 181);
            this.AccuracyTrackBar.Maximum = 100;
            this.AccuracyTrackBar.Name = "AccuracyTrackBar";
            this.AccuracyTrackBar.Size = new System.Drawing.Size(136, 45);
            this.AccuracyTrackBar.TabIndex = 2;
            this.AccuracyTrackBar.TickFrequency = 10;
            this.AccuracyTrackBar.Value = 90;
            this.AccuracyTrackBar.Scroll += new System.EventHandler(this.Parameters_Changed);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(479, 245);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 14);
            this.label9.TabIndex = 17;
            this.label9.Text = "40";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(338, 245);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 14);
            this.label10.TabIndex = 16;
            this.label10.Text = "1";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(352, 225);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(140, 14);
            this.label11.TabIndex = 15;
            this.label11.Text = "Max velocity (performance!)";
            // 
            // MaxVelocityTrackBar
            // 
            this.MaxVelocityTrackBar.Location = new System.Drawing.Point(352, 242);
            this.MaxVelocityTrackBar.Maximum = 40;
            this.MaxVelocityTrackBar.Minimum = 1;
            this.MaxVelocityTrackBar.Name = "MaxVelocityTrackBar";
            this.MaxVelocityTrackBar.Size = new System.Drawing.Size(136, 45);
            this.MaxVelocityTrackBar.TabIndex = 4;
            this.MaxVelocityTrackBar.TickFrequency = 4;
            this.MaxVelocityTrackBar.Value = 13;
            this.MaxVelocityTrackBar.Scroll += new System.EventHandler(this.Parameters_Changed);
            // 
            // FPSLabel
            // 
            this.FPSLabel.AutoSize = true;
            this.FPSLabel.BackColor = System.Drawing.Color.LightGray;
            this.FPSLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FPSLabel.Location = new System.Drawing.Point(12, 257);
            this.FPSLabel.Name = "FPSLabel";
            this.FPSLabel.Size = new System.Drawing.Size(33, 16);
            this.FPSLabel.TabIndex = 18;
            this.FPSLabel.Text = "0 fps";
            // 
            // ControlCursorCheckBox
            // 
            this.ControlCursorCheckBox.AutoSize = true;
            this.ControlCursorCheckBox.Location = new System.Drawing.Point(395, 49);
            this.ControlCursorCheckBox.Name = "ControlCursorCheckBox";
            this.ControlCursorCheckBox.Size = new System.Drawing.Size(93, 18);
            this.ControlCursorCheckBox.TabIndex = 19;
            this.ControlCursorCheckBox.Text = "Control cursor";
            this.ControlCursorCheckBox.UseVisualStyleBackColor = true;
            this.ControlCursorCheckBox.CheckedChanged += new System.EventHandler(this.ControlCursorCheckBox_CheckedChanged);
            // 
            // OpenDeviceButton
            // 
            this.OpenDeviceButton.Location = new System.Drawing.Point(394, 14);
            this.OpenDeviceButton.Name = "OpenDeviceButton";
            this.OpenDeviceButton.Size = new System.Drawing.Size(106, 32);
            this.OpenDeviceButton.TabIndex = 0;
            this.OpenDeviceButton.Text = "Open device";
            this.OpenDeviceButton.UseVisualStyleBackColor = true;
            this.OpenDeviceButton.Click += new System.EventHandler(this.OpenDeviceButton_Click);
            // 
            // AbsolutePositioningCheckBox
            // 
            this.AbsolutePositioningCheckBox.AutoSize = true;
            this.AbsolutePositioningCheckBox.Location = new System.Drawing.Point(395, 65);
            this.AbsolutePositioningCheckBox.Name = "AbsolutePositioningCheckBox";
            this.AbsolutePositioningCheckBox.Size = new System.Drawing.Size(114, 18);
            this.AbsolutePositioningCheckBox.TabIndex = 20;
            this.AbsolutePositioningCheckBox.Text = "Absolute positiong";
            this.AbsolutePositioningCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 292);
            this.Controls.Add(this.AbsolutePositioningCheckBox);
            this.Controls.Add(this.OpenDeviceButton);
            this.Controls.Add(this.ControlCursorCheckBox);
            this.Controls.Add(this.FPSLabel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.MaxVelocityTrackBar);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.AccuracyTrackBar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ConservativityTrackBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.RegionPictureBox);
            this.Controls.Add(this.AccuracyLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ImageBox);
            this.Font = new System.Drawing.Font("PT Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Region tracking";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RegionPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConservativityTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AccuracyTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxVelocityTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox ImageBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label AccuracyLabel;
        private System.Windows.Forms.PictureBox RegionPictureBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar ConservativityTrackBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar AccuracyTrackBar;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TrackBar MaxVelocityTrackBar;
        private System.Windows.Forms.Label FPSLabel;
        private System.Windows.Forms.CheckBox ControlCursorCheckBox;
        private System.Windows.Forms.Button OpenDeviceButton;
        private System.Windows.Forms.CheckBox AbsolutePositioningCheckBox;
    }
}

