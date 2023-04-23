﻿namespace SlickControls
{
	partial class SlickLibraryViewer
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlickLibraryViewer));
			this.PB_Bar = new SlickControls.SlickPictureBox();
			this.P_Spacer = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.PB_Loader = new SlickControls.SlickPictureBox();
			this.P_Bar = new System.Windows.Forms.Panel();
			this.TB_Path = new SlickControls.SlickPathTextBox();
			this.ioList = new SlickControls.Controls.Other.IoListControl();
			((System.ComponentModel.ISupportInitialize)(this.PB_Bar)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PB_Loader)).BeginInit();
			this.P_Bar.SuspendLayout();
			this.SuspendLayout();
			// 
			// PB_Bar
			// 
			this.PB_Bar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PB_Bar.Location = new System.Drawing.Point(0, 0);
			this.PB_Bar.Margin = new System.Windows.Forms.Padding(0);
			this.PB_Bar.Name = "PB_Bar";
			this.PB_Bar.Size = new System.Drawing.Size(591, 100);
			this.PB_Bar.TabIndex = 0;
			this.PB_Bar.TabStop = false;
			this.PB_Bar.Paint += new System.Windows.Forms.PaintEventHandler(this.P_Bar_Paint);
			this.PB_Bar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.P_Bar_MouseClick);
			// 
			// P_Spacer
			// 
			this.P_Spacer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.P_Spacer, 2);
			this.P_Spacer.Location = new System.Drawing.Point(70, 105);
			this.P_Spacer.Margin = new System.Windows.Forms.Padding(70, 0, 100, 0);
			this.P_Spacer.Name = "P_Spacer";
			this.P_Spacer.Size = new System.Drawing.Size(733, 1);
			this.P_Spacer.TabIndex = 3;
			this.P_Spacer.Click += new System.EventHandler(this.Generic_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.PB_Loader, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.P_Bar, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.P_Spacer, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(915, 106);
			this.tableLayoutPanel1.TabIndex = 4;
			this.tableLayoutPanel1.Click += new System.EventHandler(this.Generic_Click);
			// 
			// PB_Loader
			// 
			this.PB_Loader.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.PB_Loader.Loading = true;
			this.PB_Loader.Location = new System.Drawing.Point(771, 36);
			this.PB_Loader.Margin = new System.Windows.Forms.Padding(0, 0, 100, 0);
			this.PB_Loader.Name = "PB_Loader";
			this.PB_Loader.Size = new System.Drawing.Size(32, 32);
			this.PB_Loader.TabIndex = 6;
			this.PB_Loader.TabStop = false;
			this.PB_Loader.Visible = false;
			this.PB_Loader.Click += new System.EventHandler(this.Generic_Click);
			// 
			// P_Bar
			// 
			this.P_Bar.Controls.Add(this.TB_Path);
			this.P_Bar.Controls.Add(this.PB_Bar);
			this.P_Bar.Dock = System.Windows.Forms.DockStyle.Top;
			this.P_Bar.Location = new System.Drawing.Point(75, 5);
			this.P_Bar.Margin = new System.Windows.Forms.Padding(75, 5, 105, 0);
			this.P_Bar.Name = "P_Bar";
			this.P_Bar.Size = new System.Drawing.Size(591, 100);
			this.P_Bar.TabIndex = 9;
			this.P_Bar.Click += new System.EventHandler(this.Generic_Click);
			// 
			// TB_Path
			// 
			this.TB_Path.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TB_Path.FileExtensions = new string[0];
			this.TB_Path.Folder = true;
			this.TB_Path.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TB_Path.Image = ((System.Drawing.Image)(resources.GetObject("TB_Path.Image")));
			this.TB_Path.LabelText = "Folder";
			this.TB_Path.Location = new System.Drawing.Point(0, 0);
			this.TB_Path.MaximumSize = new System.Drawing.Size(9999, 0);
			this.TB_Path.MinimumSize = new System.Drawing.Size(50, 35);
			this.TB_Path.Name = "TB_Path";
			this.TB_Path.Placeholder = "Folder Path";
			this.TB_Path.SelectedText = "";
			this.TB_Path.SelectionLength = 0;
			this.TB_Path.SelectionStart = 0;
			this.TB_Path.ShowLabel = false;
			this.TB_Path.Size = new System.Drawing.Size(790, 35);
			this.TB_Path.TabIndex = 1;
			this.TB_Path.Visible = false;
			this.TB_Path.TextChanged += new System.EventHandler(this.TB_Path_TextChanged);
			this.TB_Path.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_Path_KeyPress);
			this.TB_Path.Leave += new System.EventHandler(this.TB_Path_Leave);
			// 
			// ioList
			// 
			this.ioList.AutoInvalidate = false;
			this.ioList.AutoScroll = true;
			this.ioList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ioList.GridItemSize = new System.Drawing.Size(85, 85);
			this.ioList.GridView = true;
			this.ioList.ItemHeight = 28;
			this.ioList.Location = new System.Drawing.Point(0, 106);
			this.ioList.Name = "ioList";
			this.ioList.Size = new System.Drawing.Size(915, 700);
			this.ioList.TabIndex = 10;
			// 
			// SlickLibraryViewer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.ioList);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MinimumSize = new System.Drawing.Size(200, 50);
			this.Name = "SlickLibraryViewer";
			this.Size = new System.Drawing.Size(915, 806);
			((System.ComponentModel.ISupportInitialize)(this.PB_Bar)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.PB_Loader)).EndInit();
			this.P_Bar.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SlickControls.SlickPictureBox PB_Bar;
		private System.Windows.Forms.Panel P_Spacer;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private SlickControls.SlickPictureBox PB_Loader;
		private System.Windows.Forms.Panel P_Bar;
		private SlickControls.SlickPathTextBox TB_Path;
		internal Controls.Other.IoListControl ioList;
	}
}
