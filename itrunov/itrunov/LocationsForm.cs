// LocationsForm.cs
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Configuration; // Для строки подключения

namespace itrunov // Ваше пространство имен
{
    public partial class LocationsForm : Form
    {
        private string connectionString;
        private int selectedLocationId = 0; // Для хранения ID выбранного местоположения

        public LocationsForm()
        {
            InitializeComponent();
            connectionString = GetConnectionStringFromAppConfig();
            // Привязка событий к кнопкам и событиям формы
            this.Load += LocationsForm_Load;
            dgvLocations.SelectionChanged += dgvLocations_SelectionChanged; // Имя вашего DataGridView для местоположений
            btnAddNewLocation.Click += btnAddNewLocation_Click;     // Имя вашей кнопки
            btnSaveLocation.Click += btnSaveLocation_Click;       // Имя вашей кнопки
            btnDeleteLocation.Click += btnDeleteLocation_Click;   // Имя вашей кнопки
            btnCloseLocation.Click += (s, e) => this.Close();    // Имя вашей кнопки
        }

        private string GetConnectionStringFromAppConfig()
        {
            string csName = "itrunov.Properties.Settings.itrunovConnectionString";
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[csName];
            if (settings != null) return settings.ConnectionString;
            // Аварийный вариант
            return @"Data Source=KATA\SQLEXPRESS;Initial Catalog=itrunov;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        }

        private void LocationsForm_Load(object sender, EventArgs e)
        {
            LoadLocations();
            ClearInputFields(); // Очищаем поля при первой загрузке
        }

        private void LoadLocations()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    // Заменяем запрос на таблицу Locations
                    string query = "SELECT LocationID, LocationName FROM Locations ORDER BY LocationName";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgvLocations.DataSource = dt; // Имя вашего DataGridView

                    // Настройка колонок
                    if (dgvLocations.Columns.Count > 0)
                    {
                        if (dgvLocations.Columns.Contains("LocationID"))
                        {
                            dgvLocations.Columns["LocationID"].Visible = false; // Скрываем ID
                        }
                        if (dgvLocations.Columns.Contains("LocationName"))
                        {
                            // Заменяем текст заголовка
                            dgvLocations.Columns["LocationName"].HeaderText = "Название местоположения";
                            dgvLocations.Columns["LocationName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Заменяем текст сообщения
                    MessageBox.Show("Ошибка загрузки местоположений: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvLocations_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLocations.CurrentRow != null && dgvLocations.CurrentRow.DataBoundItem is DataRowView drv)
            {
                // Заменяем на LocationID и LocationName
                selectedLocationId = Convert.ToInt32(drv.Row["LocationID"]);
                txtLocationName.Text = drv.Row["LocationName"].ToString(); // txtLocationName - ваш TextBox
                btnSaveLocation.Text = "Сохранить изменения";      // btnSaveLocation - ваша кнопка
            }
            else
            {
                ClearInputFields();
            }
        }

        private void ClearInputFields()
        {
            selectedLocationId = 0;
            txtLocationName.Clear();        // txtLocationName - ваш TextBox
            dgvLocations.ClearSelection(); // dgvLocations - ваш DataGridView
            // btnSaveLocation.Text = "Добавить"; // Опционально, если кнопка одна и текст меняется
        }

        private void btnAddNewLocation_Click(object sender, EventArgs e) // Имя вашей кнопки
        {
            ClearInputFields();
            txtLocationName.Focus();        // txtLocationName - ваш TextBox
        }

        private void btnSaveLocation_Click(object sender, EventArgs e) // Имя вашей кнопки
        {
            string locationName = txtLocationName.Text.Trim(); // txtLocationName - ваш TextBox
            if (string.IsNullOrEmpty(locationName))
            {
                // Заменяем текст сообщения
                MessageBox.Show("Название местоположения не может быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLocationName.Focus();
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query;
                    if (selectedLocationId == 0) // Добавление нового
                    {
                        // Заменяем на таблицу Locations и поле LocationName
                        query = "INSERT INTO Locations (LocationName) VALUES (@LocationName)";
                    }
                    else // Обновление существующего
                    {
                        // Заменяем на таблицу Locations, поле LocationName и LocationID
                        query = "UPDATE Locations SET LocationName = @LocationName WHERE LocationID = @LocationID";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LocationName", locationName);
                        if (selectedLocationId != 0)
                        {
                            cmd.Parameters.AddWithValue("@LocationID", selectedLocationId);
                        }
                        cmd.ExecuteNonQuery();
                    }
                    LoadLocations();
                    ClearInputFields();
                    btnSaveLocation.Text = "Добавить"; // Сбрасываем текст кнопки после сохранения
                    this.DialogResult = DialogResult.OK;
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 2627 || sqlEx.Number == 2601) // Ошибка уникальности
                    {
                        // Заменяем текст сообщения
                        MessageBox.Show("Местоположение с таким названием уже существует.", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        // Заменяем текст сообщения
                        MessageBox.Show("Ошибка SQL при сохранении местоположения: " + sqlEx.Message, "Ошибка БД", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    // Заменяем текст сообщения
                    MessageBox.Show("Ошибка при сохранении местоположения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteLocation_Click(object sender, EventArgs e) // Имя вашей кнопки
        {
            if (selectedLocationId == 0 || dgvLocations.CurrentRow == null)
            {
                // Заменяем текст сообщения
                MessageBox.Show("Пожалуйста, выберите местоположение для удаления.", "Нет выбора", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool isUsed = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Проверяем, используется ли местоположение в таблице Discs
                string checkQuery = "SELECT COUNT(*) FROM Discs WHERE LocationID = @LocationID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@LocationID", selectedLocationId);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        isUsed = true;
                    }
                }
            }

            if (isUsed)
            {
                // Заменяем текст сообщения
                MessageBox.Show($"Местоположение \"{txtLocationName.Text}\" используется одним или несколькими дисками и не может быть удалено. Сначала измените местоположение у этих дисков.",
                                "Удаление невозможно", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Заменяем текст сообщения
            DialogResult confirmation = MessageBox.Show($"Вы уверены, что хотите удалить местоположение \"{txtLocationName.Text}\"?",
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
                        // Заменяем на таблицу Locations и LocationID
                        string query = "DELETE FROM Locations WHERE LocationID = @LocationID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@LocationID", selectedLocationId);
                            cmd.ExecuteNonQuery();
                        }
                        LoadLocations();
                        ClearInputFields();
                        btnSaveLocation.Text = "Добавить"; // Сбрасываем текст кнопки
                        this.DialogResult = DialogResult.OK;
                    }
                    catch (Exception ex)
                    {
                        // Заменяем текст сообщения
                        MessageBox.Show("Ошибка при удалении местоположения: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Обработчик для кнопки закрытия (если он не привязан в дизайнере к свойству CancelButton формы)
        // private void btnCloseLocation_Click(object sender, EventArgs e)
        // {
        //     this.Close();
        // }
    }
}