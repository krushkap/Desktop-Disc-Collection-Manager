namespace itrunov
{
    partial class GenresForm
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
            this.dgvGenres = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGenreName = new System.Windows.Forms.TextBox();
            this.btnAddNewGenre = new System.Windows.Forms.Button();
            this.btnSaveGenre = new System.Windows.Forms.Button();
            this.btnDeleteGenre = new System.Windows.Forms.Button();
            this.btnCloseGenre = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGenres)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvGenres
            // 
            this.dgvGenres.AllowUserToAddRows = false;
            this.dgvGenres.AllowUserToDeleteRows = false;
            this.dgvGenres.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvGenres.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGenres.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvGenres.Location = new System.Drawing.Point(0, 145);
            this.dgvGenres.Name = "dgvGenres";
            this.dgvGenres.ReadOnly = true;
            this.dgvGenres.RowHeadersWidth = 62;
            this.dgvGenres.RowTemplate.Height = 28;
            this.dgvGenres.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGenres.Size = new System.Drawing.Size(815, 322);
            this.dgvGenres.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Название жанра:";
            // 
            // txtGenreName
            // 
            this.txtGenreName.Location = new System.Drawing.Point(167, 37);
            this.txtGenreName.Name = "txtGenreName";
            this.txtGenreName.Size = new System.Drawing.Size(168, 26);
            this.txtGenreName.TabIndex = 2;
            // 
            // btnAddNewGenre
            // 
            this.btnAddNewGenre.Location = new System.Drawing.Point(409, 16);
            this.btnAddNewGenre.Name = "btnAddNewGenre";
            this.btnAddNewGenre.Size = new System.Drawing.Size(109, 47);
            this.btnAddNewGenre.TabIndex = 3;
            this.btnAddNewGenre.Text = "Добавить";
            this.btnAddNewGenre.UseVisualStyleBackColor = true;
            // 
            // btnSaveGenre
            // 
            this.btnSaveGenre.Location = new System.Drawing.Point(409, 69);
            this.btnSaveGenre.Name = "btnSaveGenre";
            this.btnSaveGenre.Size = new System.Drawing.Size(109, 46);
            this.btnSaveGenre.TabIndex = 4;
            this.btnSaveGenre.Text = "Сохранить";
            this.btnSaveGenre.UseVisualStyleBackColor = true;
            // 
            // btnDeleteGenre
            // 
            this.btnDeleteGenre.Location = new System.Drawing.Point(549, 16);
            this.btnDeleteGenre.Name = "btnDeleteGenre";
            this.btnDeleteGenre.Size = new System.Drawing.Size(109, 47);
            this.btnDeleteGenre.TabIndex = 5;
            this.btnDeleteGenre.Text = "Удалить";
            this.btnDeleteGenre.UseVisualStyleBackColor = true;
            // 
            // btnCloseGenre
            // 
            this.btnCloseGenre.Location = new System.Drawing.Point(549, 68);
            this.btnCloseGenre.Name = "btnCloseGenre";
            this.btnCloseGenre.Size = new System.Drawing.Size(109, 47);
            this.btnCloseGenre.TabIndex = 6;
            this.btnCloseGenre.Text = "Закрыть";
            this.btnCloseGenre.UseVisualStyleBackColor = true;
            // 
            // GenresForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 467);
            this.Controls.Add(this.btnCloseGenre);
            this.Controls.Add(this.btnDeleteGenre);
            this.Controls.Add(this.btnSaveGenre);
            this.Controls.Add(this.btnAddNewGenre);
            this.Controls.Add(this.txtGenreName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvGenres);
            this.Name = "GenresForm";
            this.Text = "Жанры";
            ((System.ComponentModel.ISupportInitialize)(this.dgvGenres)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvGenres;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGenreName;
        private System.Windows.Forms.Button btnAddNewGenre;
        private System.Windows.Forms.Button btnSaveGenre;
        private System.Windows.Forms.Button btnDeleteGenre;
        private System.Windows.Forms.Button btnCloseGenre;
    }
}