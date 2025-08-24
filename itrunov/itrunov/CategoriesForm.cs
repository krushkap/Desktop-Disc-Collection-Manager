// CategoriesForm.cs
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration; // Для строки подключения

namespace itrunov
{
    public partial class CategoriesForm : Form
    {
        private string connectionString;
        private int selectedCategoryId = 0; // Для хранения ID выбранной для редактирования категории

        public CategoriesForm()
        {
            InitializeComponent();
            connectionString = GetConnectionStringFromAppConfig(); // Используем тот же метод получения строки
            // Привязка событий к кнопкам (если не сделано в дизайнере)
            this.Load += CategoriesForm_Load;
            dgvCategories.SelectionChanged += dgvCategories_SelectionChanged;
            btnAddNewCategory.Click += btnAddNewCategory_Click;
            btnSaveCategory.Click += btnSaveCategory_Click;
            btnDeleteCategory.Click += btnDeleteCategory_Click;
            btnCloseCategory.Click += (s, e) => this.Close(); // Кнопка Закрыть
        }

        private string GetConnectionStringFromAppConfig()
        {
            string csName = "itrunov.Properties.Settings.itrunovConnectionString";
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[csName];
            if (settings != null) return settings.ConnectionString;
            // Аварийный вариант
            return @"Data Source=KATA\SQLEXPRESS;Initial Catalog=itrunov;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        }

        private void CategoriesForm_Load(object sender, EventArgs e)
        {
            LoadCategories();
            ClearInputFields(); // Очищаем поля при первой загрузке
        }

        // В CategoriesForm.cs

        private void LoadCategories()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvCategories.DataSource = dt;

                    // Убедимся, что DataGridView имеет колонки для настройки
                    if (dgvCategories.Columns.Count > 0)
                    {
                        // Скрываем колонку CategoryID
                        if (dgvCategories.Columns.Contains("CategoryID"))
                        {
                            dgvCategories.Columns["CategoryID"].Visible = false;
                        }

                        // Настраиваем видимую колонку CategoryName
                        if (dgvCategories.Columns.Contains("CategoryName"))
                        {
                            dgvCategories.Columns["CategoryName"].HeaderText = "Название категории";
                            dgvCategories.Columns["CategoryName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки категорий: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvCategories_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCategories.CurrentRow != null && dgvCategories.CurrentRow.DataBoundItem is DataRowView drv)
            {
                selectedCategoryId = Convert.ToInt32(drv.Row["CategoryID"]);
                txtCategoryName.Text = drv.Row["CategoryName"].ToString();
                btnSaveCategory.Text = "Сохранить изменения";
            }
            else
            {
                ClearInputFields();
            }
        }

        private void ClearInputFields()
        {
            selectedCategoryId = 0; // Сбрасываем ID, указывая, что это новая запись
            txtCategoryName.Clear();
            dgvCategories.ClearSelection(); // Снимаем выделение в гриде
        }

        private void btnAddNewCategory_Click(object sender, EventArgs e)
        {
            ClearInputFields();
            txtCategoryName.Focus();
        }

        private void btnSaveCategory_Click(object sender, EventArgs e)
        {
            string categoryName = txtCategoryName.Text.Trim();
            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Название категории не может быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategoryName.Focus();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query;
                    if (selectedCategoryId == 0) // Добавление новой категории
                    {
                        query = "INSERT INTO Categories (CategoryName) VALUES (@CategoryName)";
                    }
                    else // Обновление существующей
                    {
                        query = "UPDATE Categories SET CategoryName = @CategoryName WHERE CategoryID = @CategoryID";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", categoryName);
                        if (selectedCategoryId != 0)
                        {
                            cmd.Parameters.AddWithValue("@CategoryID", selectedCategoryId);
                        }
                        cmd.ExecuteNonQuery();
                    }
                    LoadCategories(); // Обновляем список в гриде
                    ClearInputFields(); // Очищаем поля для следующего ввода
                    this.DialogResult = DialogResult.OK; // Сигнализируем главной форме об изменениях
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 2627 || sqlEx.Number == 2601) // Ошибка уникальности
                    {
                        MessageBox.Show("Категория с таким названием уже существует.", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка SQL при сохранении категории: " + sqlEx.Message, "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении категории: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (selectedCategoryId == 0 || dgvCategories.CurrentRow == null)
            {
                MessageBox.Show("Пожалуйста, выберите категорию для удаления.", "Нет выбора", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Проверка, используется ли категория дисками
            bool isUsed = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string checkQuery = "SELECT COUNT(*) FROM Discs WHERE CategoryID = @CategoryID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@CategoryID", selectedCategoryId);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        isUsed = true;
                    }
                }
            }

            if (isUsed)
            {
                MessageBox.Show($"Категория \"{txtCategoryName.Text}\" используется одним или несколькими дисками и не может быть удалена. Сначала измените категорию у этих дисков.",
                                "Удаление невозможно", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            DialogResult confirmation = MessageBox.Show($"Вы уверены, что хотите удалить категорию \"{txtCategoryName.Text}\"?",
                                                       "Подтверждение удаления",
                                                       MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Warning);
            if (confirmation == DialogResult.Yes)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "DELETE FROM Categories WHERE CategoryID = @CategoryID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@CategoryID", selectedCategoryId);
                            cmd.ExecuteNonQuery();
                        }
                        LoadCategories();
                        ClearInputFields();
                        this.DialogResult = DialogResult.OK; // Сигнализируем главной форме об изменениях
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении категории: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void dgvCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}