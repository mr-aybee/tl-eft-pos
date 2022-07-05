namespace FTPPOSService
{
	partial class frmSRV
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
			this.txtReceipt = new System.Windows.Forms.RichTextBox();
			this.ipTxt = new System.Windows.Forms.TextBox();
			this.portTxt = new System.Windows.Forms.TextBox();
			this.button_Test = new System.Windows.Forms.Button();
			this.button_Start = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtReceipt
			// 
			this.txtReceipt.Location = new System.Drawing.Point(12, 42);
			this.txtReceipt.Name = "txtReceipt";
			this.txtReceipt.Size = new System.Drawing.Size(651, 389);
			this.txtReceipt.TabIndex = 0;
			this.txtReceipt.Text = "";
			// 
			// ipTxt
			// 
			this.ipTxt.Location = new System.Drawing.Point(11, 13);
			this.ipTxt.Name = "ipTxt";
			this.ipTxt.Size = new System.Drawing.Size(132, 20);
			this.ipTxt.TabIndex = 1;
			this.ipTxt.Text = "127.0.0.1";
			// 
			// portTxt
			// 
			this.portTxt.Location = new System.Drawing.Point(149, 13);
			this.portTxt.Name = "portTxt";
			this.portTxt.Size = new System.Drawing.Size(46, 20);
			this.portTxt.TabIndex = 2;
			this.portTxt.Text = "2011";
			// 
			// button_Test
			// 
			this.button_Test.Location = new System.Drawing.Point(588, 13);
			this.button_Test.Name = "button_Test";
			this.button_Test.Size = new System.Drawing.Size(75, 23);
			this.button_Test.TabIndex = 4;
			this.button_Test.Text = "Test";
			this.button_Test.UseVisualStyleBackColor = true;
			this.button_Test.Click += new System.EventHandler(this.button_Test_Click);
			// 
			// button_Start
			// 
			this.button_Start.Location = new System.Drawing.Point(201, 12);
			this.button_Start.Name = "button_Start";
			this.button_Start.Size = new System.Drawing.Size(75, 23);
			this.button_Start.TabIndex = 5;
			this.button_Start.Text = "Start";
			this.button_Start.UseVisualStyleBackColor = true;
			this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
			// 
			// frmSRV
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(682, 442);
			this.Controls.Add(this.button_Start);
			this.Controls.Add(this.button_Test);
			this.Controls.Add(this.portTxt);
			this.Controls.Add(this.ipTxt);
			this.Controls.Add(this.txtReceipt);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmSRV";
			this.Text = "EFT POS Bridge";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox txtReceipt;
		private System.Windows.Forms.TextBox ipTxt;
		private System.Windows.Forms.TextBox portTxt;
		private System.Windows.Forms.Button button_Test;
		private System.Windows.Forms.Button button_Start;
	}
}