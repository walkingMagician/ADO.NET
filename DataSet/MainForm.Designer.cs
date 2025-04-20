namespace AcademyDataSet
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
            this.cbGroup = new System.Windows.Forms.ComboBox();
            this.cbDirection = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cbGroup
            // 
            this.cbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGroup.FormattingEnabled = true;
            this.cbGroup.Location = new System.Drawing.Point(13, 13);
            this.cbGroup.Name = "cbGroup";
            this.cbGroup.Size = new System.Drawing.Size(326, 21);
            this.cbGroup.TabIndex = 0;
            // 
            // cbDirection
            // 
            this.cbDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDirection.FormattingEnabled = true;
            this.cbDirection.Location = new System.Drawing.Point(449, 13);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.Size = new System.Drawing.Size(339, 21);
            this.cbDirection.TabIndex = 1;
            this.cbDirection.SelectedIndexChanged += new System.EventHandler(this.cbDirection_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.cbDirection);
            this.Controls.Add(this.cbGroup);
            this.Name = "MainForm";
            this.Text = "DataSet";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbGroup;
        private System.Windows.Forms.ComboBox cbDirection;
    }
}

