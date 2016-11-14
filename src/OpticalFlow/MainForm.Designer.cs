namespace OpticalFlow
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
            this.components = new System.ComponentModel.Container();
            this.OpenDeviceButton = new System.Windows.Forms.Button();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.TrackerSelector = new System.Windows.Forms.ComboBox();
            this.mainFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.OutputPictureBox = new System.Windows.Forms.PictureBox();
            this.PreprocessedPictureBox = new System.Windows.Forms.PictureBox();
            this.SourcePictureBox = new System.Windows.Forms.PictureBox();
            this.VelocitiesDistanceTrackBar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreprocessedPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SourcePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VelocitiesDistanceTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenDeviceButton
            // 
            this.OpenDeviceButton.Location = new System.Drawing.Point(341, 447);
            this.OpenDeviceButton.Margin = new System.Windows.Forms.Padding(2);
            this.OpenDeviceButton.Name = "OpenDeviceButton";
            this.OpenDeviceButton.Size = new System.Drawing.Size(309, 43);
            this.OpenDeviceButton.TabIndex = 3;
            this.OpenDeviceButton.Text = "Open device";
            this.OpenDeviceButton.UseVisualStyleBackColor = true;
            this.OpenDeviceButton.Click += new System.EventHandler(this.OpenDeviceButton_Click);
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.Font = new System.Drawing.Font("PT Sans", 8.25F);
            this.InfoLabel.Location = new System.Drawing.Point(342, 311);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(90, 14);
            this.InfoLabel.TabIndex = 4;
            this.InfoLabel.Text = "Summary motion:";
            // 
            // TrackerSelector
            // 
            this.TrackerSelector.DataSource = this.mainFormBindingSource;
            this.TrackerSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TrackerSelector.FormattingEnabled = true;
            this.TrackerSelector.Location = new System.Drawing.Point(391, 269);
            this.TrackerSelector.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TrackerSelector.Name = "TrackerSelector";
            this.TrackerSelector.Size = new System.Drawing.Size(259, 22);
            this.TrackerSelector.TabIndex = 7;
            this.TrackerSelector.SelectedIndexChanged += new System.EventHandler(this.TrackerSelector_SelectedIndexChanged);
            // 
            // mainFormBindingSource
            // 
            this.mainFormBindingSource.AllowNew = false;
            this.mainFormBindingSource.DataMember = "TrackerAlgorithms";
            this.mainFormBindingSource.DataSource = typeof(OpticalFlow.MainForm);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(342, 272);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "Method";
            // 
            // OutputPictureBox
            // 
            this.OutputPictureBox.BackColor = System.Drawing.Color.White;
            this.OutputPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutputPictureBox.Location = new System.Drawing.Point(330, 11);
            this.OutputPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.OutputPictureBox.Name = "OutputPictureBox";
            this.OutputPictureBox.Size = new System.Drawing.Size(320, 240);
            this.OutputPictureBox.TabIndex = 0;
            this.OutputPictureBox.TabStop = false;
            // 
            // PreprocessedPictureBox
            // 
            this.PreprocessedPictureBox.BackColor = System.Drawing.Color.White;
            this.PreprocessedPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PreprocessedPictureBox.Location = new System.Drawing.Point(11, 250);
            this.PreprocessedPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.PreprocessedPictureBox.Name = "PreprocessedPictureBox";
            this.PreprocessedPictureBox.Size = new System.Drawing.Size(320, 240);
            this.PreprocessedPictureBox.TabIndex = 0;
            this.PreprocessedPictureBox.TabStop = false;
            // 
            // SourcePictureBox
            // 
            this.SourcePictureBox.BackColor = System.Drawing.Color.White;
            this.SourcePictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SourcePictureBox.Location = new System.Drawing.Point(11, 11);
            this.SourcePictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.SourcePictureBox.Name = "SourcePictureBox";
            this.SourcePictureBox.Size = new System.Drawing.Size(320, 240);
            this.SourcePictureBox.TabIndex = 0;
            this.SourcePictureBox.TabStop = false;
            this.SourcePictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SourcePictureBox_MouseDown);
            // 
            // VelocitiesDistanceTrackBar
            // 
            this.VelocitiesDistanceTrackBar.Location = new System.Drawing.Point(341, 367);
            this.VelocitiesDistanceTrackBar.Maximum = 100;
            this.VelocitiesDistanceTrackBar.Minimum = 1;
            this.VelocitiesDistanceTrackBar.Name = "VelocitiesDistanceTrackBar";
            this.VelocitiesDistanceTrackBar.Size = new System.Drawing.Size(309, 45);
            this.VelocitiesDistanceTrackBar.TabIndex = 11;
            this.VelocitiesDistanceTrackBar.Value = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(11, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Source (click to add track point)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(330, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Velocities field";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(11, 250);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 16);
            this.label4.TabIndex = 14;
            this.label4.Text = "Preprocessed";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(416, 350);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 14);
            this.label5.TabIndex = 15;
            this.label5.Text = "Velocities probe distance";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 507);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OutputPictureBox);
            this.Controls.Add(this.PreprocessedPictureBox);
            this.Controls.Add(this.SourcePictureBox);
            this.Controls.Add(this.VelocitiesDistanceTrackBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OpenDeviceButton);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.TrackerSelector);
            this.Font = new System.Drawing.Font("PT Sans", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Optical flow estimantion";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainFormBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OutputPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PreprocessedPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SourcePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VelocitiesDistanceTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenDeviceButton;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.ComboBox TrackerSelector;
        private System.Windows.Forms.BindingSource mainFormBindingSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox OutputPictureBox;
        private System.Windows.Forms.PictureBox PreprocessedPictureBox;
        private System.Windows.Forms.PictureBox SourcePictureBox;
        private System.Windows.Forms.TrackBar VelocitiesDistanceTrackBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}

