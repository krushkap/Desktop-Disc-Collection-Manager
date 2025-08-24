namespace itrunov
{
    partial class LocationsForm
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
            this.dgvLocations = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLocationName = new System.Windows.Forms.TextBox();
            this.btnAddNewLocation = new System.Windows.Forms.Button();
            this.btnSaveLocation = new System.Windows.Forms.Button();
            this.btnDeleteLocation = new System.Windows.Forms.Button();
            this.btnCloseLocation = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocations)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvLocations
            // 
            this.dgvLocations.AllowUserToAddRows = false;
            this.dgvLocations.AllowUserToDeleteRows = false;
            this.dgvLocations.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvLocations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLocations.Location = new System.Drawing.Point(2, 133);
            this.dgvLocations.Name = "dgvLocations";
            this.dgvLocations.ReadOnly = true;
            this.dgvLocations.RowHeadersWidth = 62;
            this.dgvLocations.RowTemplate.Height = 28;
            this.dgvLocations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLocations.Size = new System.Drawing.Size(796, 305);
            this.dgvLocations.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Название местоположения:";
            // 
            // txtLocationName
            // 
            this.txtLocationName.Location = new System.Drawing.Point(240, 20);
            this.txtLocationName.Name = "txtLocationName";
            this.txtLocationName.Size = new System.Drawing.Size(147, 26);
            this.txtLocationName.TabIndex = 2;
            // 
            // btnAddNewLocation
            // 
            this.btnAddNewLocation.Location = new System.Drawing.Point(425, 12);
            this.btnAddNewLocation.Name = "btnAddNewLocation";
            this.btnAddNewLocation.Size = new System.Drawing.Size(129, 50);
            this.btnAddNewLocation.TabIndex = 3;
            this.btnAddNewLocation.Text = "Добавить";
            this.btnAddNewLocation.UseVisualStyleBackColor = true;
            // 
            // btnSaveLocation
            // 
            this.btnSaveLocation.Location = new System.Drawing.Point(425, 70);
            this.btnSaveLocation.Name = "btnSaveLocation";
            this.btnSaveLocation.Size = new System.Drawing.Size(129, 46);
            this.btnSaveLocation.TabIndex = 4;
            this.btnSaveLocation.Text = "Сохранить";
            this.btnSaveLocation.UseVisualStyleBackColor = true;
            // 
            // btnDeleteLocation
            // 
            this.btnDeleteLocation.Location = new System.Drawing.Point(572, 13);
            this.btnDeleteLocation.Name = "btnDeleteLocation";
            this.btnDeleteLocation.Size = new System.Drawing.Size(129, 49);
            this.btnDeleteLocation.TabIndex = 5;
            this.btnDeleteLocation.Text = "Удалить";
            this.btnDeleteLocation.UseVisualStyleBackColor = true;
            // 
            // btnCloseLocation
            // 
            this.btnCloseLocation.Location = new System.Drawing.Point(572, 70);
            this.btnCloseLocation.Name = "btnCloseLocation";
            this.btnCloseLocation.Size = new System.Drawing.Size(129, 45);
            this.btnCloseLocation.TabIndex = 6;
            this.btnCloseLocation.Text = "Закрыть";
            this.btnCloseLocation.UseVisualStyleBackColor = true;
            // 
            // LocationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCloseLocation);
            this.Controls.Add(this.btnDeleteLocation);
            this.Controls.Add(this.btnSaveLocation);
            this.Controls.Add(this.btnAddNewLocation);
            this.Controls.Add(this.txtLocationName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvLocations);
            this.Name = "LocationsForm";
            this.Text = "Расположение";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocations)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLocations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLocationName;
        private System.Windows.Forms.Button btnAddNewLocation;
        private System.Windows.Forms.Button btnSaveLocation;
        private System.Windows.Forms.Button btnDeleteLocation;
        private System.Windows.Forms.Button btnCloseLocation;
    }
}