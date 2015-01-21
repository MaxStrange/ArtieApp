namespace ArtieGUI
{
    partial class TCP_IP_ClientStartGUI
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
            this.connectButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.autoDetectRadioButton = new System.Windows.Forms.RadioButton();
            this.manualDetectRadioButton = new System.Windows.Forms.RadioButton();
            this.autoDetectSelfRadioButton = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ipTextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(68, 137);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(114, 34);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(247, 137);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(114, 34);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // autoDetectRadioButton
            // 
            this.autoDetectRadioButton.AutoSize = true;
            this.autoDetectRadioButton.Location = new System.Drawing.Point(14, 16);
            this.autoDetectRadioButton.Name = "autoDetectRadioButton";
            this.autoDetectRadioButton.Size = new System.Drawing.Size(216, 17);
            this.autoDetectRadioButton.TabIndex = 2;
            this.autoDetectRadioButton.TabStop = true;
            this.autoDetectRadioButton.Text = "Automatically detect Michelle\'s computer";
            this.autoDetectRadioButton.UseVisualStyleBackColor = true;
            // 
            // manualDetectRadioButton
            // 
            this.manualDetectRadioButton.AutoSize = true;
            this.manualDetectRadioButton.Location = new System.Drawing.Point(14, 62);
            this.manualDetectRadioButton.Name = "manualDetectRadioButton";
            this.manualDetectRadioButton.Size = new System.Drawing.Size(147, 17);
            this.manualDetectRadioButton.TabIndex = 3;
            this.manualDetectRadioButton.TabStop = true;
            this.manualDetectRadioButton.Text = "Manually enter IP address";
            this.manualDetectRadioButton.UseVisualStyleBackColor = true;
            // 
            // autoDetectSelfRadioButton
            // 
            this.autoDetectSelfRadioButton.AutoSize = true;
            this.autoDetectSelfRadioButton.Location = new System.Drawing.Point(14, 39);
            this.autoDetectSelfRadioButton.Name = "autoDetectSelfRadioButton";
            this.autoDetectSelfRadioButton.Size = new System.Drawing.Size(206, 17);
            this.autoDetectSelfRadioButton.TabIndex = 4;
            this.autoDetectSelfRadioButton.TabStop = true;
            this.autoDetectSelfRadioButton.Text = "Automatically detect my own computer";
            this.autoDetectSelfRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ipTextBox);
            this.panel1.Controls.Add(this.autoDetectRadioButton);
            this.panel1.Controls.Add(this.manualDetectRadioButton);
            this.panel1.Controls.Add(this.autoDetectSelfRadioButton);
            this.panel1.Location = new System.Drawing.Point(68, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(293, 97);
            this.panel1.TabIndex = 5;
            // 
            // ipTextBox
            // 
            this.ipTextBox.Location = new System.Drawing.Point(167, 62);
            this.ipTextBox.Name = "ipTextBox";
            this.ipTextBox.Size = new System.Drawing.Size(100, 20);
            this.ipTextBox.TabIndex = 5;
            // 
            // TCP_IP_ClientStartGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 211);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.connectButton);
            this.Name = "TCP_IP_ClientStartGUI";
            this.Text = "Connection Options";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.RadioButton autoDetectRadioButton;
        private System.Windows.Forms.RadioButton manualDetectRadioButton;
        private System.Windows.Forms.RadioButton autoDetectSelfRadioButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox ipTextBox;
    }
}