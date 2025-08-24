using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace itrunov
{
    public partial class Form1 : Form
    {
        private bool isLoadingData = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            isLoadingData = true;
            try
            {
                this.discsTableAdapter.Fill(this.itrunovDataSet.Discs);
                
                if (dgvDiscs.DataSource == null || !(dgvDiscs.DataSource is BindingSource) || ((BindingSource)dgvDiscs.DataSource).DataSource != this.itrunovDataSet.Discs)
                {
                     dgvDiscs.DataSource = this.discsBindingSource; // Убедитесь, что discsBindingSource привязан к itrunovDataSet.Discs в дизайнере
                }

                ConfigureDataGridViewColumns();
                LoadCategoriesComboBox();
                LoadGenresComboBox();
                LoadLocationsComboBox();
                UpdateStatusLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке формы: " + ex.Message, "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoadingData = false;
            }
        }

        private void ConfigureDataGridViewColumns()
        {
            dgvDiscs.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvDiscs.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            int displayIdx = 0;

            if (dgvDiscs.Columns.Contains("DiscID"))
                dgvDiscs.Columns["DiscID"].Visible = false;
            if (dgvDiscs.Columns.Contains("CategoryID"))
                dgvDiscs.Columns["CategoryID"].Visible = false;
            if (dgvDiscs.Columns.Contains("LocationID"))
                dgvDiscs.Columns["LocationID"].Visible = false;

            if (dgvDiscs.Columns.Contains("Title"))
            {
                dgvDiscs.Columns["Title"].HeaderText = "Название";
                dgvDiscs.Columns["Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgvDiscs.Columns["Title"].MinimumWidth = 150;
                dgvDiscs.Columns["Title"].DisplayIndex = displayIdx++;
            }

            if (dgvDiscs.Columns.Contains("CategoryName"))
            {
                dgvDiscs.Columns["CategoryName"].HeaderText = "Категория";
                dgvDiscs.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                dgvDiscs.Columns["CategoryName"].Width = 120;
                dgvDiscs.Columns["CategoryName"].DisplayIndex = displayIdx++;
            }

            if (dgvDiscs.Columns.Contains("LocationName"))
            {
                dgvDiscs.Columns["LocationName"].HeaderText = "Местоположение";
                dgvDiscs.Columns["LocationName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                dgvDiscs.Columns["LocationName"].Width = 150;
                dgvDiscs.Columns["LocationName"].DisplayIndex = displayIdx++;
            }
            
            if (dgvDiscs.Columns.Contains("DateAdded"))
            {
                dgvDiscs.Columns["DateAdded"].HeaderText = "Дата добавления";
                dgvDiscs.Columns["DateAdded"].DefaultCellStyle.Format = "dd.MM.yyyy";
                dgvDiscs.Columns["DateAdded"].Width = 100;
                dgvDiscs.Columns["DateAdded"].DisplayIndex = displayIdx++;
                dgvDiscs.Columns["DateAdded"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvDiscs.Columns.Contains("Notes"))
            {
                dgvDiscs.Columns["Notes"].HeaderText = "Примечания";
                dgvDiscs.Columns["Notes"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvDiscs.Columns["Notes"].MinimumWidth = 100;
                dgvDiscs.Columns["Notes"].DisplayIndex = displayIdx++;
                dgvDiscs.Columns["Notes"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgvDiscs.Columns["Notes"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            }
            
            dgvDiscs.ReadOnly = true;
            dgvDiscs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDiscs.AllowUserToAddRows = false;
            dgvDiscs.AllowUserToDeleteRows = false;
            dgvDiscs.MultiSelect = false;
        }

        private string GetConnectionString()
        {
            string connectionStringName = "itrunov.Properties.Settings.itrunovConnectionString";
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[connectionStringName];

            if (settings != null)
            {
                return settings.ConnectionString;
            }
            else
            {
                MessageBox.Show($"Строка подключения с именем '{connectionStringName}' не найдена в app.config. Используется строка по умолчанию.",
                                "Ошибка конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return @"Data Source=KATA\SQLEXPRESS;Initial Catalog=itrunov;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
            }
        }

        private void LoadComboBoxData(ComboBox comboBox, string query, string displayMember, string valueMember, string defaultText)
        {
            string connectionString = GetConnectionString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    DataRow row = dataTable.NewRow();
                    row[valueMember] = DBNull.Value; 
                    row[displayMember] = defaultText;
                    dataTable.Rows.InsertAt(row, 0);

                    comboBox.DataSource = dataTable;
                    comboBox.DisplayMember = displayMember;
                    comboBox.ValueMember = valueMember;
                    if (comboBox.Items.Count > 0) comboBox.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных для {comboBox.Name}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadCategoriesComboBox()
        {
            LoadComboBoxData(cmbSearchCategory, "SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName", "CategoryName", "CategoryID", "Все категории");
        }

        private void LoadGenresComboBox()
        {
            LoadComboBoxData(cmbSearchGenre, "SELECT GenreID, GenreName FROM Genres ORDER BY GenreName", "GenreName", "GenreID", "Все жанры");
        }

        private void LoadLocationsComboBox()
        {
            LoadComboBoxData(cmbSearchLocation, "SELECT LocationID, LocationName FROM Locations ORDER BY LocationName", "LocationName", "LocationID", "Все местоположения");
        }
        
        private void UpdateStatusLabel()
        {
            if (this.discsBindingSource != null)
            {
                lblDiscCount.Text = $"Отображено дисков: {this.discsBindingSource.Count}";
            }
            else
            {
                lblDiscCount.Text = "Отображено дисков: 0";
            }
        }

        private void txtSearchTitle_TextChanged(object sender, EventArgs e)
        {
            if (isLoadingData) return;

            string searchText = txtSearchTitle.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                try
                {
                    isLoadingData = true;
                    this.discsBindingSource.Filter = string.Empty;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при сбросе фильтра: " + ex.Message);
                }
                finally
                {
                    isLoadingData = false;
                }
                SelectFirstRowInGridAfterFilter();
            }
            else
            {
                SearchDiscsByTitle(searchText);
            }
            UpdateStatusLabel();
        }

        private void SearchDiscsByTitle(string searchText)
        {
            if (isLoadingData) return;

            try
            {
                isLoadingData = true;
                string filterExpression = $"Title LIKE '%{searchText.Replace("'", "''").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%")}%'";
                this.discsBindingSource.Filter = filterExpression;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске по названию: {ex.Message}", "Ошибка поиска", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.discsBindingSource.Filter = string.Empty;
            }
            finally
            {
                isLoadingData = false;
            }
            SelectFirstRowInGridAfterFilter();
        }
        
        private void SelectFirstRowInGridAfterFilter()
        {
            if (dgvDiscs.Rows.Count > 0)
            {
                if (!dgvDiscs.Rows[0].IsNewRow)
                {
                    dgvDiscs.ClearSelection();
                    dgvDiscs.Rows[0].Selected = true;
                    if (dgvDiscs.Columns.Contains("Title") && dgvDiscs.Columns["Title"].Visible)
                    {
                        dgvDiscs.CurrentCell = dgvDiscs.Rows[0].Cells["Title"];
                    }
                    else if (dgvDiscs.ColumnCount > 0)
                    {
                        foreach (DataGridViewColumn col in dgvDiscs.Columns)
                        {
                            if (col.Visible)
                            {
                                dgvDiscs.CurrentCell = dgvDiscs.Rows[0].Cells[col.Index];
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                dgvDiscs.ClearSelection();
            }
        }

        private void btnSearch_Click_1(object sender, EventArgs e) // Предполагается, что кнопка называется btnSearch
        {
            ApplyAllFilters();
        }

        private void btnResetSearch_Click(object sender, EventArgs e) // Предполагается, что кнопка называется btnResetSearch
        {
            isLoadingData = true;
            txtSearchTitle.Clear();
            if (cmbSearchCategory.Items.Count > 0) cmbSearchCategory.SelectedIndex = 0;
            if (cmbSearchGenre.Items.Count > 0) cmbSearchGenre.SelectedIndex = 0;
            if (cmbSearchLocation.Items.Count > 0) cmbSearchLocation.SelectedIndex = 0;
            isLoadingData = false;
            
            ApplyAllFilters(); 
        }

        // В Form1.cs

        private void ApplyAllFilters()
        {
            if (isLoadingData) return;

            List<string> conditions = new List<string>(); // Для RowFilter, если жанр не выбран
            string sqlQuery;
            List<SqlParameter> sqlParameters = new List<SqlParameter>();

            // Проверяем, выбран ли жанр
            bool genreFilterActive = (cmbSearchGenre.SelectedValue != null && cmbSearchGenre.SelectedValue != DBNull.Value);

            if (genreFilterActive)
            {
                // Если жанр выбран, формируем SQL-запрос для перезагрузки данных
                StringBuilder queryBuilder = new StringBuilder(@"
            SELECT DISTINCT d.DiscID, d.Title, d.CategoryID, c.CategoryName, 
                           d.LocationID, l.LocationName, d.Notes, d.DateAdded 
            FROM Discs d
            LEFT JOIN Categories c ON d.CategoryID = c.CategoryID
            LEFT JOIN Locations l ON d.LocationID = l.LocationID
            INNER JOIN DiscGenres dg ON d.DiscID = dg.DiscID 
            WHERE 1=1 "); // 1=1 для удобства добавления AND

                // Добавляем условие по жанру
                queryBuilder.Append("AND dg.GenreID = @GenreID ");
                sqlParameters.Add(new SqlParameter("@GenreID", Convert.ToInt32(cmbSearchGenre.SelectedValue)));

                // Добавляем другие фильтры к SQL-запросу
                if (!string.IsNullOrWhiteSpace(txtSearchTitle.Text))
                {
                    queryBuilder.Append("AND d.Title LIKE @Title ");
                    sqlParameters.Add(new SqlParameter("@Title", "%" + txtSearchTitle.Text + "%")); // Для SQL LIKE
                }
                if (cmbSearchCategory.SelectedValue != null && cmbSearchCategory.SelectedValue != DBNull.Value)
                {
                    queryBuilder.Append("AND d.CategoryID = @CategoryID ");
                    sqlParameters.Add(new SqlParameter("@CategoryID", Convert.ToInt32(cmbSearchCategory.SelectedValue)));
                }
                if (cmbSearchLocation.SelectedValue != null && cmbSearchLocation.SelectedValue != DBNull.Value)
                {
                    queryBuilder.Append("AND d.LocationID = @LocationID ");
                    sqlParameters.Add(new SqlParameter("@LocationID", Convert.ToInt32(cmbSearchLocation.SelectedValue)));
                }
                queryBuilder.Append("ORDER BY d.Title;"); // Или d.DiscID
                sqlQuery = queryBuilder.ToString();

                // Перезагружаем данные с новым запросом
                ReloadDataWithCustomQuery(sqlQuery, sqlParameters);
            }
            else
            {
                // Если жанр НЕ выбран, используем RowFilter для остальных полей
                // (как делали раньше)
                // Сначала перезагрузим все данные, чтобы RowFilter работал на полном наборе
                this.discsTableAdapter.Fill(this.itrunovDataSet.Discs);


                if (!string.IsNullOrWhiteSpace(txtSearchTitle.Text))
                {
                    string titleFilter = txtSearchTitle.Text.Replace("'", "''").Replace("%", "[%]").Replace("_", "[_]").Replace("*", "%");
                    conditions.Add($"Title LIKE '%{titleFilter}%'");
                }
                if (cmbSearchCategory.SelectedValue != null && cmbSearchCategory.SelectedValue != DBNull.Value)
                {
                    conditions.Add($"CategoryID = {Convert.ToInt32(cmbSearchCategory.SelectedValue)}");
                }
                if (cmbSearchLocation.SelectedValue != null && cmbSearchLocation.SelectedValue != DBNull.Value)
                {
                    conditions.Add($"LocationID = {Convert.ToInt32(cmbSearchLocation.SelectedValue)}");
                }

                try
                {
                    isLoadingData = true;
                    this.discsBindingSource.Filter = string.Join(" AND ", conditions);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка применения фильтра: " + ex.Message +
                                    "\n\nВыражение фильтра: " + string.Join(" AND ", conditions),
                                    "Ошибка фильтра", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.discsBindingSource.Filter = string.Empty;
                }
                finally
                {
                    isLoadingData = false;
                }
            }
            UpdateStatusLabel();
            SelectFirstRowInGridAfterFilter(); // Ваш метод для выбора первой строки
        }

        // Новый метод для перезагрузки данных с пользовательским SQL-запросом
        private void ReloadDataWithCustomQuery(string query, List<SqlParameter> parameters)
        {
            isLoadingData = true;
            try
            {
                this.itrunovDataSet.Discs.Clear(); // Очищаем текущие данные в DataTable
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters.ToArray());
                        }
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(this.itrunovDataSet.Discs); // Заполняем DataTable Discs новыми данными
                    }
                }
                // BindingSource должен автоматически подхватить изменения в DataTable,
                // к которой он привязан (itrunovDataSet.Discs).
                // Если нет, можно попробовать:
                // this.discsBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при перезагрузке данных с фильтром по жанру: " + ex.Message, "Ошибка данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                isLoadingData = false;
            }
        }

        // --- Обработчики событий для Меню ---
        private void menuFileExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuDisksAdd_Click(object sender, EventArgs e)
        {
            DiscForm addDiscForm = new DiscForm();
            if (addDiscForm.ShowDialog(this) == DialogResult.OK)
            {
                this.discsTableAdapter.Fill(this.itrunovDataSet.Discs);
                ApplyAllFilters(); 
                UpdateStatusLabel();
            }
        }

        private void menuDisksEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiscs.CurrentRow != null && dgvDiscs.CurrentRow.DataBoundItem is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("DiscID"))
                {
                    try
                    {
                        int discId = Convert.ToInt32(drv.Row["DiscID"]);
                        DiscForm editDiscForm = new DiscForm(discId);
                        if (editDiscForm.ShowDialog(this) == DialogResult.OK)
                        {
                            this.discsTableAdapter.Fill(this.itrunovDataSet.Discs);
                            ApplyAllFilters();
                            UpdateStatusLabel();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при получении ID диска для редактирования: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите диск для редактирования.", "Нет выбора", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void menuDisksDelete_Click(object sender, EventArgs e)
        {
             if (dgvDiscs.CurrentRow != null && dgvDiscs.CurrentRow.DataBoundItem is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("DiscID") && drv.Row.Table.Columns.Contains("Title"))
                {
                    try
                    {
                        int discIdToDelete = Convert.ToInt32(drv.Row["DiscID"]);
                        string discTitle = Convert.ToString(drv.Row["Title"]);

                        DialogResult confirmation = MessageBox.Show($"Вы уверены, что хотите удалить диск \"{discTitle}\" (ID: {discIdToDelete})?",
                                                                "Подтверждение удаления",
                                                                MessageBoxButtons.YesNo,
                                                                MessageBoxIcon.Warning);
                        if (confirmation == DialogResult.Yes)
                        {
                            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                            {
                                string deleteSpecificQueryBase = "DELETE FROM {0} WHERE DiscID = @DiscID";
                                string[] specificTables = { "MovieSpecifics", "AudioSpecifics", "SoftwareSpecifics", "AudiobookSpecifics", "DocumentationSpecifics" };
                                string deleteDiscGenresQuery = "DELETE FROM DiscGenres WHERE DiscID = @DiscID";
                                string deleteDiscQuery = "DELETE FROM Discs WHERE DiscID = @DiscID";
                                
                                conn.Open();
                                SqlTransaction transaction = conn.BeginTransaction();
                                try
                                {
                                    // Удаляем из специфичных таблиц
                                    foreach(string tableName in specificTables)
                                    {
                                        using (SqlCommand cmd = new SqlCommand(string.Format(deleteSpecificQueryBase, tableName), conn, transaction))
                                        {
                                            cmd.Parameters.AddWithValue("@DiscID", discIdToDelete);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    // Удаляем из DiscGenres
                                    using (SqlCommand cmd = new SqlCommand(deleteDiscGenresQuery, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@DiscID", discIdToDelete);
                                        cmd.ExecuteNonQuery();
                                    }
                                    // Удаляем из основной таблицы Discs
                                    using (SqlCommand cmd = new SqlCommand(deleteDiscQuery, conn, transaction))
                                    {
                                        cmd.Parameters.AddWithValue("@DiscID", discIdToDelete);
                                        int rowsAffected = cmd.ExecuteNonQuery();
                                        if (rowsAffected > 0)
                                        {
                                            transaction.Commit();
                                            MessageBox.Show("Диск успешно удален.", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            this.discsTableAdapter.Fill(this.itrunovDataSet.Discs);
                                            ApplyAllFilters();
                                            UpdateStatusLabel();
                                        }
                                        else
                                        {
                                            transaction.Rollback();
                                            MessageBox.Show("Диск не найден или не удалось удалить.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    MessageBox.Show("Ошибка при удалении диска: " + ex.Message, "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при получении ID диска для удаления: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите диск для удаления.", "Нет выбора", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void menuViewStatistics_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Функция статистики пока не реализована.", "В разработке", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuViewDuplicates_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Функция поиска дубликатов пока не реализована.", "В разработке", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuReferencesCategories_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Управление категориями пока не реализовано.", "В разработке", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuReferencesLocations_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Управление местоположениями пока не реализовано.", "В разработке", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuReferencesGenres_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Управление жанрами пока не реализовано.", "В разработке", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void menuHelpAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Домашняя дискотека v1.0\nРазработчик: itrunov", "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Пустые обработчики, которые были в вашем Designer.cs, но могут быть не нужны
        // Если они не используются, их можно удалить из Form1.cs,
        // а затем удалить привязки к ним из событий элементов в дизайнере форм.
        private void dgvDiscs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Console.WriteLine($"CellContentClick: Row {e.RowIndex}, Column {e.ColumnIndex}");
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e) // Это было привязано к menuDisksDelete
        {
            if (dgvDiscs.CurrentRow == null)
            {
                MessageBox.Show("Пожалуйста, выберите диск для удаления.", "Нет выбора", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvDiscs.CurrentRow.DataBoundItem is DataRowView drv)
            {
                if (drv.Row.Table.Columns.Contains("DiscID") && drv.Row.Table.Columns.Contains("Title"))
                {
                    try
                    {
                        int discIdToDelete = Convert.ToInt32(drv.Row["DiscID"]);
                        string discTitle = drv.Row["Title"].ToString();

                        DialogResult confirmation = MessageBox.Show(
                            $"Вы уверены, что хотите удалить диск:\n\"{discTitle}\" (ID: {discIdToDelete})?\n\nЭто действие необратимо!",
                            "Подтверждение удаления",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (confirmation == DialogResult.Yes)
                        {
                            bool deleteSuccessful = PerformDeleteDisc(discIdToDelete);
                            if (deleteSuccessful)
                            {
                                MessageBox.Show("Диск успешно удален.", "Удаление завершено", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.discsTableAdapter.Fill(this.itrunovDataSet.Discs); // Перезаполняем DataSet
                                                                                        // ApplyAllFilters(); // Повторно применить текущие фильтры
                                UpdateStatusLabel();
                            }
                            // Сообщения об ошибках обрабатываются внутри PerformDeleteDisc
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при подготовке к удалению диска: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось получить DiscID или Title из выбранной строки.", "Ошибка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Не удалось получить данные выбранной строки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }

        private void menuDisksAdd_Click_1(object sender, EventArgs e)
        {
            DiscForm addDiscForm = new DiscForm();
            if (addDiscForm.ShowDialog(this) == DialogResult.OK)
            {
                this.discsTableAdapter.Fill(this.itrunovDataSet.Discs); // Перезаполняем DataSet
                                                                        // ApplyAllFilters(); // Повторно применить текущие фильтры, если они были
                UpdateStatusLabel();
            }
        }

        // В Form1.cs
        // В Form1.cs

        // В Form1.cs
        // Новый метод для выполнения операций удаления в БД
        private bool PerformDeleteDisc(int discId)
        {
            using (SqlConnection conn = new SqlConnection(GetConnectionString())) // GetConnectionString() - ваш метод
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(); // Начинаем транзакцию

                try
                {
                    // 1. Удаляем из связующей таблицы DiscGenres
                    string queryDeleteDiscGenres = "DELETE FROM DiscGenres WHERE DiscID = @DiscID";
                    using (SqlCommand cmd = new SqlCommand(queryDeleteDiscGenres, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@DiscID", discId);
                        cmd.ExecuteNonQuery();
                    }

                    // 2. Удаляем из всех специфичных таблиц
                    // (Если нет каскадного удаления в БД, настроенного на FK от Discs)
                    string[] specificTables = { "MovieSpecifics", "AudioSpecifics", "SoftwareSpecifics", "AudiobookSpecifics", "DocumentationSpecifics" };
                    foreach (string tableName in specificTables)
                    {
                        string queryDeleteSpecific = $"DELETE FROM {tableName} WHERE DiscID = @DiscID";
                        using (SqlCommand cmd = new SqlCommand(queryDeleteSpecific, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@DiscID", discId);
                            cmd.ExecuteNonQuery(); // Может не затронуть ни одной строки, если для этого диска нет специфики
                        }
                    }

                    // 3. Удаляем из основной таблицы Discs
                    string queryDeleteDisc = "DELETE FROM Discs WHERE DiscID = @DiscID";
                    using (SqlCommand cmd = new SqlCommand(queryDeleteDisc, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@DiscID", discId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            transaction.Commit(); // Фиксируем транзакцию, если все успешно
                            return true;
                        }
                        else
                        {
                            transaction.Rollback(); // Откатываем, если сам диск не найден (маловероятно, если выбран из грида)
                            MessageBox.Show("Диск не найден в базе данных для удаления.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Откатываем транзакцию в случае любой ошибки
                    MessageBox.Show("Ошибка при удалении диска из базы данных: " + ex.Message, "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        private void menuDisksEdit_Click_1(object sender, EventArgs e) // Убедитесь, что имя метода ваше
        {
            if (dgvDiscs.CurrentRow == null)
            {
                MessageBox.Show("Пожалуйста, выберите диск для редактирования.", "Нет выбора", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Получаем DataRowView из выбранной строки
            if (dgvDiscs.CurrentRow.DataBoundItem is DataRowView drv)
            {
                // Убедимся, что колонка DiscID существует в DataTable, к которой привязан BindingSource
                // (это должно быть так, если TableAdapter ее возвращает)
                if (drv.Row.Table.Columns.Contains("DiscID"))
                {
                    try
                    {
                        int discIdToEdit = Convert.ToInt32(drv.Row["DiscID"]);

                        // Создаем и показываем форму редактирования
                        DiscForm editDiscForm = new DiscForm(discIdToEdit); // Вызываем конструктор для редактирования
                        if (editDiscForm.ShowDialog(this) == DialogResult.OK)
                        {
                            // Если пользователь сохранил изменения, перезагружаем данные в главной форме
                            int currentRowIndex = dgvDiscs.CurrentRow.Index; // Запоминаем индекс текущей строки

                            this.discsTableAdapter.Fill(this.itrunovDataSet.Discs); // Перезаполняем DataSet
                                                                                    // this.discsBindingSource.ResetBindings(false); // Может помочь обновить грид

                            // Попытка восстановить выбор на отредактированной строке
                            // Это может быть не всегда точно, если порядок сортировки изменился
                            // или если RowFilter активен и отредактированная строка больше не соответствует ему.
                            if (dgvDiscs.Rows.Count > currentRowIndex)
                            {
                                dgvDiscs.ClearSelection();
                                dgvDiscs.Rows[currentRowIndex].Selected = true;
                                dgvDiscs.CurrentCell = dgvDiscs.Rows[currentRowIndex].Cells[dgvDiscs.FirstDisplayedCell?.ColumnIndex ?? 0];
                            }

                            ApplyAllFilters(); // Повторно применяем фильтры, так как данные могли измениться
                            UpdateStatusLabel();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при подготовке к редактированию диска: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось получить данные о DiscID из выбранной строки.", "Ошибка данных", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Не удалось получить данные выбранной строки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void menuReferencesCategories_Click_1(object sender, EventArgs e)
        {
            CategoriesForm catForm = new CategoriesForm();
            // Показываем как диалог, чтобы главная форма ждала его закрытия
            DialogResult result = catForm.ShowDialog(this);

            // Если в форме справочника были сделаны изменения (нажали Сохранить или Удалить),
            // то catForm.DialogResult будет DialogResult.OK (мы это установили в CategoriesForm).
            if (result == DialogResult.OK)
            {
                // Обновляем ComboBox категорий на главной форме
                LoadCategoriesComboBox();

                // Также нужно перезагрузить основной DataGridView, так как названия категорий могли измениться
                // или удаленная категория могла использоваться (хотя мы это запретили).
                // Самый простой способ - полный рефреш.
                this.discsTableAdapter.Fill(this.itrunovDataSet.Discs);
                ApplyAllFilters(); // Если у вас есть фильтры, примените их заново
                UpdateStatusLabel();
            }
        }

        private void menuFileExit_Click_2(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuReferencesLocations_Click_1(object sender, EventArgs e)
        {
            LocationsForm locForm = new LocationsForm();
            DialogResult result = locForm.ShowDialog(this);

            if (result == DialogResult.OK) // Если были изменения в справочнике местоположений
            {
                // Обновляем ComboBox местоположений на главной форме
                LoadLocationsComboBox(); // Ваш метод для загрузки cmbSearchLocation

                // Также нужно перезагрузить основной DataGridView, так как названия могли измениться
                this.discsTableAdapter.Fill(this.itrunovDataSet.Discs);
                ApplyAllFilters(); // Применить текущие фильтры заново
                UpdateStatusLabel();
            }
        }

        private void menuReferencesGenres_Click_1(object sender, EventArgs e)
        {
            GenresForm genreForm = new GenresForm();
            DialogResult result = genreForm.ShowDialog(this);

            if (result == DialogResult.OK) // Если были изменения в справочнике жанров
            {
                // Обновляем ComboBox жанров на главной форме
                LoadGenresComboBox(); // Ваш метод для загрузки cmbSearchGenre

                // Перезагрузка основного DataGridView обычно не требуется, если только
                // вы не отображаете жанры прямо в гриде Form1 (что мы пока не делали)
                // или если удаление жанра не повлияло на какие-либо диски (что мы предотвращаем).
                // this.discsTableAdapter.Fill(this.itrunovDataSet.Discs);
                // ApplyAllFilters(); 
                // UpdateStatusLabel(); // Обновление статуса может быть полезно, если изменилось общее кол-во дисков, но здесь вряд ли
            }
        }
    }
}