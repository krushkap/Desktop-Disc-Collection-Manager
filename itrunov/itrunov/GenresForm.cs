// GenresForm.cs
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration;

namespace itrunov // Ваше пространство имен
{
    public partial class GenresForm : Form
    {
        private string connectionString;
        private int selectedGenreId = 0; // Для хранения ID выбранного жанра

        public GenresForm()
        {
            InitializeComponent();
            connectionString = GetConnectionStringFromAppConfig();
            // Привязка событий к кнопкам и событиям формы
            this.Load += GenresForm_Load;
            dgvGenres.SelectionChanged += dgvGenres_SelectionChanged;
            btnAddNewGenre.Click += btnAddNewGenre_Click;
            btnSaveGenre.Click += btnSaveGenre_Click;
            btnDeleteGenre.Click += btnDeleteGenre_Click;
            btnCloseGenre.Click += (s, e) => this.Close();
        }

        private string GetConnectionStringFromAppConfig()
        {
            string csName = "itrunov.Properties.Settings.itrunovConnectionString";
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[csName];
            if (settings != null) return settings.ConnectionString;
            MessageBox.Show($"Строка подключения с именем '{csName}' не найдена в app.config. Используется строка по умолчанию.",
                            "Ошибка конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return @"Data Source=KATA\SQLEXPRESS;Initial Catalog=itrunov;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        }

        private void GenresForm_Load(object sender, EventArgs e)
        {
            LoadGenres();
            ClearInputFields();
        }

        private void LoadGenres()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT GenreID, GenreName FROM Genres ORDER BY GenreName"; // Таблица Genres
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvGenres.DataSource = dt;

                    if (dgvGenres.Columns.Count > 0)
                    {
                        if (dgvGenres.Columns.Contains("GenreID"))
                        {
                            dgvGenres.Columns["GenreID"].Visible = false; // Скрываем ID
                        }
                        if (dgvGenres.Columns.Contains("GenreName"))
                        {
                            dgvGenres.Columns["GenreName"].HeaderText = "Название жанра"; // Заголовок
                            dgvGenres.Columns["GenreName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки жанров: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvGenres_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvGenres.CurrentRow != null && dgvGenres.CurrentRow.DataBoundItem is DataRowView drv)
            {
                if (drv.Row["GenreID"] != DBNull.Value && drv.Row["GenreName"] != DBNull.Value)
                {
                    selectedGenreId = Convert.ToInt32(drv.Row["GenreID"]); // GenreID
                   
                    txtGenreName.Text = drv.Row["GenreName"].ToString(); // txtGenreName
                    btnSaveGenre.Text = "Сохранить изменения";      // btnSaveGenre
                }
            }
            // else
            // {
            //     ClearInputFields(); // Решаем, нужно ли сбрасывать, если выделение снято
            // }
        }

        private void ClearInputFields()
        {
            selectedGenreId = 0;
            txtGenreName.Clear();        // txtGenreName
            // btnSaveGenre.Text = "Добавить"; // Текст кнопки будет меняться в btnAddNewGenre_Click
            dgvGenres.ClearSelection();
        }

        private void btnAddNewGenre_Click(object sender, EventArgs e)
        {
            ClearInputFields();
            txtGenreName.Focus();        // txtGenreName
            btnSaveGenre.Text = "Добавить"; // Устанавливаем текст кнопки в режим добавления
        }

        private void btnSaveGenre_Click(object sender, EventArgs e)
        {
            string genreName = txtGenreName.Text.Trim(); // txtGenreName
            if (string.IsNullOrEmpty(genreName))
            {
                MessageBox.Show("Название жанра не может быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGenreName.Focus();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query;
                    bool isNewRecord = (selectedGenreId == 0);

                    if (isNewRecord)
                    {
                        query = "INSERT INTO Genres (GenreName) VALUES (@GenreName)"; // Таблица Genres, поле GenreName
                    }
                    else
                    {
                        query = "UPDATE Genres SET GenreName = @GenreName WHERE GenreID = @GenreID"; // Таблица Genres, поля GenreName, GenreID
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@GenreName", genreName);
                        if (!isNewRecord)
                        {
                            cmd.Parameters.AddWithValue("@GenreID", selectedGenreId);
                        }
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show(isNewRecord ? "Жанр успешно добавлен!" : "Изменения успешно сохранены!",
                                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadGenres();
                    ClearInputFields();
                    btnSaveGenre.Text = "Добавить"; // Сбрасываем текст кнопки
                    this.DialogResult = DialogResult.OK;
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 2627 || sqlEx.Number == 2601) // Ошибка уникальности
                    {
                        MessageBox.Show("Жанр с таким названием уже существует.", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка SQL при сохранении жанра: " + sqlEx.Message, "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении жанра: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteGenre_Click(object sender, EventArgs e)
        {
            if (selectedGenreId == 0 || dgvGenres.CurrentRow == null)
            {
                MessageBox.Show("Пожалуйста, выберите жанр для удаления.", "Нет выбора", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool isUsed = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Проверяем, используется ли жанр в таблице DiscGenres
                string checkQuery = "SELECT COUNT(*) FROM DiscGenres WHERE GenreID = @GenreID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@GenreID", selectedGenreId);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        isUsed = true;
                    }
                }
            }

            if (isUsed)
            {
                MessageBox.Show($"Жанр \"{txtGenreName.Text}\" используется одним или несколькими дисками и не может быть удален.\n" +
                                "Сначала удалите этот жанр у дисков.",
                                "Удаление невозможно", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmation = MessageBox.Show($"Вы уверены, что хотите удалить жанр \"{txtGenreName.Text}\" (ID: {selectedGenreId})?",
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
                        string query = "DELETE FROM Genres WHERE GenreID = @GenreID"; // Таблица Genres, поле GenreID
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@GenreID", selectedGenreId);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Жанр успешно удален!", "Удаление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadGenres();
                        ClearInputFields();
                        btnSaveGenre.Text = "Добавить"; // Сбрасываем текст кнопки
                        this.DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при удалении жанра: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // btnCloseGenre_Click привязан лямбдой в конструкторе
        // private void btnCloseGenre_Click(object sender, EventArgs e)
        // {
        //     this.DialogResult = DialogResult.Cancel;
        //     this.Close();
        // }
    }
}