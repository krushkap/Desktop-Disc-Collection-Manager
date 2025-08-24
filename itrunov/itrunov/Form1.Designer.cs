namespace itrunov
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDisks = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDisksAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDisksEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDisksDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReferences = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReferencesCategories = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReferencesLocations = new System.Windows.Forms.ToolStripMenuItem();
            this.menuReferencesGenres = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnResetSearch = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbSearchLocation = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbSearchGenre = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSearchCategory = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSearchTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvDiscs = new System.Windows.Forms.DataGridView();
            this.discsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.itrunovDataSet = new itrunov.itrunovDataSet();
            this.discsTableAdapter = new itrunov.itrunovDataSetTableAdapters.DiscsTableAdapter();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblDiscCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.discsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itrunovDataSet)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuDisks,
            this.menuReferences});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1168, 36);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileExit});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(69, 29);
            this.menuFile.Text = "Файл";
            // 
            // menuFileExit
            // 
            this.menuFileExit.Name = "menuFileExit";
            this.menuFileExit.Size = new System.Drawing.Size(270, 34);
            this.menuFileExit.Text = "Выход";
            this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click_2);
            // 
            // menuDisks
            // 
            this.menuDisks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDisksAdd,
            this.menuDisksEdit,
            this.menuDisksDelete});
            this.menuDisks.Name = "menuDisks";
            this.menuDisks.Size = new System.Drawing.Size(77, 29);
            this.menuDisks.Text = "Диски";
            // 
            // menuDisksAdd
            // 
            this.menuDisksAdd.Name = "menuDisksAdd";
            this.menuDisksAdd.Size = new System.Drawing.Size(336, 34);
            this.menuDisksAdd.Text = "Добавить новый диск";
            this.menuDisksAdd.Click += new System.EventHandler(this.menuDisksAdd_Click_1);
            // 
            // menuDisksEdit
            // 
            this.menuDisksEdit.Name = "menuDisksEdit";
            this.menuDisksEdit.Size = new System.Drawing.Size(336, 34);
            this.menuDisksEdit.Text = "Редактировать выбранный";
            this.menuDisksEdit.Click += new System.EventHandler(this.menuDisksEdit_Click_1);
            // 
            // menuDisksDelete
            // 
            this.menuDisksDelete.Name = "menuDisksDelete";
            this.menuDisksDelete.Size = new System.Drawing.Size(336, 34);
            this.menuDisksDelete.Text = "Удалить выбранный";
            this.menuDisksDelete.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // menuReferences
            // 
            this.menuReferences.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuReferencesCategories,
            this.menuReferencesLocations,
            this.menuReferencesGenres});
            this.menuReferences.Name = "menuReferences";
            this.menuReferences.Size = new System.Drawing.Size(139, 29);
            this.menuReferences.Text = "Справочники";
            // 
            // menuReferencesCategories
            // 
            this.menuReferencesCategories.Name = "menuReferencesCategories";
            this.menuReferencesCategories.Size = new System.Drawing.Size(257, 34);
            this.menuReferencesCategories.Text = "Категории";
            this.menuReferencesCategories.Click += new System.EventHandler(this.menuReferencesCategories_Click_1);
            // 
            // menuReferencesLocations
            // 
            this.menuReferencesLocations.Name = "menuReferencesLocations";
            this.menuReferencesLocations.Size = new System.Drawing.Size(270, 34);
            this.menuReferencesLocations.Text = "Местоположения";
            this.menuReferencesLocations.Click += new System.EventHandler(this.menuReferencesLocations_Click_1);
            // 
            // menuReferencesGenres
            // 
            this.menuReferencesGenres.Name = "menuReferencesGenres";
            this.menuReferencesGenres.Size = new System.Drawing.Size(270, 34);
            this.menuReferencesGenres.Text = "Жанры";
            this.menuReferencesGenres.Click += new System.EventHandler(this.menuReferencesGenres_Click_1);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnResetSearch);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.cmbSearchLocation);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmbSearchGenre);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmbSearchCategory);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSearchTitle);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1168, 100);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // btnResetSearch
            // 
            this.btnResetSearch.Location = new System.Drawing.Point(637, 51);
            this.btnResetSearch.Name = "btnResetSearch";
            this.btnResetSearch.Size = new System.Drawing.Size(115, 35);
            this.btnResetSearch.TabIndex = 9;
            this.btnResetSearch.Text = "Сбросить";
            this.btnResetSearch.UseVisualStyleBackColor = true;
            this.btnResetSearch.Click += new System.EventHandler(this.btnResetSearch_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(637, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(115, 36);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "Применить";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click_1);
            // 
            // cmbSearchLocation
            // 
            this.cmbSearchLocation.FormattingEnabled = true;
            this.cmbSearchLocation.Location = new System.Drawing.Point(453, 51);
            this.cmbSearchLocation.Name = "cmbSearchLocation";
            this.cmbSearchLocation.Size = new System.Drawing.Size(154, 28);
            this.cmbSearchLocation.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(366, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Место:";
            // 
            // cmbSearchGenre
            // 
            this.cmbSearchGenre.FormattingEnabled = true;
            this.cmbSearchGenre.Location = new System.Drawing.Point(453, 12);
            this.cmbSearchGenre.Name = "cmbSearchGenre";
            this.cmbSearchGenre.Size = new System.Drawing.Size(154, 28);
            this.cmbSearchGenre.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(366, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Жанр:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // cmbSearchCategory
            // 
            this.cmbSearchCategory.FormattingEnabled = true;
            this.cmbSearchCategory.Location = new System.Drawing.Point(176, 51);
            this.cmbSearchCategory.Name = "cmbSearchCategory";
            this.cmbSearchCategory.Size = new System.Drawing.Size(154, 28);
            this.cmbSearchCategory.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Категория:";
            // 
            // txtSearchTitle
            // 
            this.txtSearchTitle.Location = new System.Drawing.Point(176, 12);
            this.txtSearchTitle.Name = "txtSearchTitle";
            this.txtSearchTitle.Size = new System.Drawing.Size(154, 26);
            this.txtSearchTitle.TabIndex = 1;
            this.txtSearchTitle.TextChanged += new System.EventHandler(this.txtSearchTitle_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Название:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // dgvDiscs
            // 
            this.dgvDiscs.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvDiscs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiscs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDiscs.Location = new System.Drawing.Point(0, 136);
            this.dgvDiscs.Name = "dgvDiscs";
            this.dgvDiscs.RowHeadersWidth = 62;
            this.dgvDiscs.RowTemplate.Height = 28;
            this.dgvDiscs.Size = new System.Drawing.Size(1168, 459);
            this.dgvDiscs.TabIndex = 2;
            this.dgvDiscs.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDiscs_CellContentClick);
            // 
            // discsBindingSource
            // 
            this.discsBindingSource.DataMember = "Discs";
            this.discsBindingSource.DataSource = this.itrunovDataSet;
            // 
            // itrunovDataSet
            // 
            this.itrunovDataSet.DataSetName = "itrunovDataSet";
            this.itrunovDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // discsTableAdapter
            // 
            this.discsTableAdapter.ClearBeforeFill = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblDiscCount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 563);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1168, 32);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblDiscCount
            // 
            this.lblDiscCount.Name = "lblDiscCount";
            this.lblDiscCount.Size = new System.Drawing.Size(179, 25);
            this.lblDiscCount.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1168, 595);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dgvDiscs);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Домашняя дискотека";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiscs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.discsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itrunovDataSet)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSearchCategory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSearchTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbSearchGenre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnResetSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbSearchLocation;
        private System.Windows.Forms.DataGridView dgvDiscs;
        private itrunovDataSet itrunovDataSet;
        private System.Windows.Forms.BindingSource discsBindingSource;
        private itrunovDataSetTableAdapters.DiscsTableAdapter discsTableAdapter;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblDiscCount;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuDisks;
        private System.Windows.Forms.ToolStripMenuItem menuDisksAdd;
        private System.Windows.Forms.ToolStripMenuItem menuDisksEdit;
        private System.Windows.Forms.ToolStripMenuItem menuDisksDelete;
        private System.Windows.Forms.ToolStripMenuItem menuReferences;
        private System.Windows.Forms.ToolStripMenuItem menuReferencesCategories;
        private System.Windows.Forms.ToolStripMenuItem menuReferencesLocations;
        private System.Windows.Forms.ToolStripMenuItem menuReferencesGenres;
    }
}

