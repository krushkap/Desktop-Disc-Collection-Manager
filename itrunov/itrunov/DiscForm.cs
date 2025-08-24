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
using System.Configuration; // Для ConfigurationManager

namespace itrunov // Ваше пространство имен
{
    public partial class DiscForm : Form
    {
        private int currentDiscId = 0; // 0 для нового диска, >0 для редактируемого
        private string connectionString;

        // Конструктор для нового диска
        public DiscForm()
        {
            InitializeComponent();
            connectionString = GetConnectionString();
            this.Text = "Добавить новый диск";
            dtpDateAdded.Value = DateTime.Today; // Дата по умолчанию

            LoadComboBoxes();
            UpdateSpecificFieldsPanel(null); // Скрыть все специфичные панели
            btnSave.Click += BtnSave_Click; // Привязка события сохранения
            btnCancel.Click += BtnCancel_Click; // Привязка события отмены

            // Опционально: Скрыть поле DiscID, если оно есть на форме
        }

        // Конструктор для редактирования существующего диска
        public DiscForm(int discId)
        {
            InitializeComponent();
            connectionString = GetConnectionString();
            currentDiscId = discId;
            this.Text = $"Редактировать диск (ID: {currentDiscId})";

            LoadComboBoxes();
            LoadDiscData(); // Загрузка данных диска для редактирования
            btnSave.Click += BtnSave_Click; // Привязка события сохранения
            btnCancel.Click += BtnCancel_Click; // Привязка события отмены

            // Опционально: Отобразить ID, если оно есть на форме

        }

        // Обработчик Form_Load (если он нужен, но большая часть логики уже в конструкторах)
        private void DiscForm_Load(object sender, EventArgs e)
        {
            // В принципе, вся загрузка уже в конструкторах.
            // Этот метод может использоваться для финальной настройки после создания всех контролов.
        }


        private string GetConnectionString()
        {
            // Используем тот же метод, что и в Form1, или напрямую строку из app.config
            // Проверьте app.config на имя строки
            string csName = "itrunov.Properties.Settings.itrunovConnectionString";
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[csName];
            if (settings != null) return settings.ConnectionString;

            // Аварийный вариант (должен соответствовать реальной строке)
            MessageBox.Show($"Строка подключения с именем '{csName}' не найдена в app.config. Используется строка по умолчанию.",
                                "Ошибка конфигурации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return @"Data Source=KATA\SQLEXPRESS;Initial Catalog=itrunov;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
        }

        private void LoadComboBoxes()
        {
            // Загрузка категорий в cmbCategory
            LoadGenericComboBox(cmbCategory, "SELECT CategoryID, CategoryName FROM Categories ORDER BY CategoryName", "CategoryName", "CategoryID", "Выберите категорию...");
            // Загрузка местоположений в cmbLocation
            LoadGenericComboBox(cmbLocation, "SELECT LocationID, LocationName FROM Locations ORDER BY LocationName", "LocationName", "LocationID", "Выберите место...");
            // Загрузка жанров в clbGenres
            LoadGenresCheckedListBox();

            // Привязка обработчика изменения категории для отображения нужной панели
            // Убедитесь, что у вас есть элемент cmbCategory на форме
            if (cmbCategory != null)
            {
                cmbCategory.SelectedIndexChanged -= CmbCategory_SelectedIndexChanged; // Отписываемся, если уже подписаны
                cmbCategory.SelectedIndexChanged += CmbCategory_SelectedIndexChanged;
            }

            // Добавьте элементы в cmbAudioType и cmbSoftwareType вручную или загрузите из справочника
            if (cmbAudioType != null && cmbAudioType.Items.Count == 0)
            {
                cmbAudioType.Items.Add("CD-Audio");
                cmbAudioType.Items.Add("MP3");
            }
            if (cmbSoftwareType != null && cmbSoftwareType.Items.Count == 0)
            {
                // Пример типов ПО, адаптируйте под свои нужды
                cmbSoftwareType.Items.Add("ОС");
                cmbSoftwareType.Items.Add("Приложение");
                cmbSoftwareType.Items.Add("Игра");
                cmbSoftwareType.Items.Add("Драйверы");
            }
        }

        private void LoadGenericComboBox(ComboBox comboBox, string query, string displayMember, string valueMember, string defaultText)
        {
            if (comboBox == null) return; // Проверка на существование элемента

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    DataRow dr = dt.NewRow();
                    dr[valueMember] = DBNull.Value;
                    dr[displayMember] = defaultText;
                    dt.Rows.InsertAt(dr, 0);

                    comboBox.DataSource = dt;
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

        private void LoadGenresCheckedListBox()
        {
            if (clbGenres == null) return; // Проверка на существование элемента

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    string query = "SELECT GenreID, GenreName FROM Genres ORDER BY GenreName";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dtGenres = new DataTable();
                    adapter.Fill(dtGenres);

                    clbGenres.DataSource = dtGenres;
                    clbGenres.DisplayMember = "GenreName";
                    clbGenres.ValueMember = "GenreID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки жанров: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Обработчик изменения выбранного элемента в ComboBox Категории
        private void CmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategoryName = null;
            if (cmbCategory.SelectedItem is DataRowView drv)
            {
                selectedCategoryName = drv["CategoryName"].ToString();
            }
            UpdateSpecificFieldsPanel(selectedCategoryName);
        }

        // Метод для управления видимостью панелей специфичных полей
        private void UpdateSpecificFieldsPanel(string categoryName)
        {
            // Сначала скрываем все панели
            if (panelMovie != null) panelMovie.Visible = false;
            if (panelAudio != null) panelAudio.Visible = false;
            if (panelSoftware != null) panelSoftware.Visible = false;
            if (panelAudiobook != null) panelAudiobook.Visible = false;
            if (panelDocumentation != null) panelDocumentation.Visible = false;

            // Затем показываем нужную панель в зависимости от категории
            if (categoryName == "Фильм")
            {
                if (panelMovie != null) panelMovie.Visible = true;
            }
            else if (categoryName == "Аудио")
            {
                if (panelAudio != null) panelAudio.Visible = true;
            }
            else if (categoryName == "ПО") // Убедитесь, что имена категорий соответствуют вашим
            {
                if (panelSoftware != null) panelSoftware.Visible = true;
            }
            else if (categoryName == "Аудиокнига")
            {
                if (panelAudiobook != null) panelAudiobook.Visible = true;
            }
            else if (categoryName == "Документация")
            {
                if (panelDocumentation != null) panelDocumentation.Visible = true;
            }

            // Опционально: Перемещение панелей на одно и то же место, если они накладываются
            Point specificPanelLocation = new Point(10, 200); // Примерное место под общими полями
            if (panelMovie != null) panelMovie.Location = specificPanelLocation;
            if (panelAudio != null) panelAudio.Location = specificPanelLocation;
            if (panelSoftware != null) panelSoftware.Location = specificPanelLocation;
            if (panelAudiobook != null) panelAudiobook.Location = specificPanelLocation;
            if (panelDocumentation != null) panelDocumentation.Location = specificPanelLocation;

            // Опционально: Изменение размера формы, чтобы вместить видимую панель и кнопки
            // this.AutoSize = true; // Или ручная настройка высоты формы
        }

        // Метод для загрузки данных существующего диска (вызывается конструктором для редактирования)
        private void LoadDiscData()
        {
            if (currentDiscId == 0) return; // Нечего загружать, если это новый диск

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // 1. Загрузка основной информации о диске
                    string queryDisc = "SELECT Title, CategoryID, LocationID, Notes, DateAdded FROM Discs WHERE DiscID = @DiscID";
                    using (SqlCommand cmdDisc = new SqlCommand(queryDisc, conn))
                    {
                        cmdDisc.Parameters.AddWithValue("@DiscID", currentDiscId);
                        using (SqlDataReader readerDisc = cmdDisc.ExecuteReader())
                        {
                            if (readerDisc.Read())
                            {
                                if (txtTitle != null) txtTitle.Text = readerDisc["Title"].ToString();
                                if (cmbCategory != null) cmbCategory.SelectedValue = readerDisc["CategoryID"]; // Это вызовет CmbCategory_SelectedIndexChanged
                                if (cmbLocation != null)
                                {
                                    if (readerDisc["LocationID"] != DBNull.Value) cmbLocation.SelectedValue = readerDisc["LocationID"];
                                    else cmbLocation.SelectedIndex = 0; // или -1
                                }
                                if (txtNotes != null) txtNotes.Text = readerDisc["Notes"].ToString();
                                if (dtpDateAdded != null) dtpDateAdded.Value = Convert.ToDateTime(readerDisc["DateAdded"]);
                            }
                        }
                    }

                    // 2. Загрузка специфичных данных
                    string selectedCategoryNameOnLoad = null;
                    if (cmbCategory.SelectedItem is DataRowView drv)
                    {
                        selectedCategoryNameOnLoad = drv["CategoryName"].ToString();
                    }
                    // UpdateSpecificFieldsPanel(selectedCategoryNameOnLoad); // Уже вызвано при установке cmbCategory.SelectedValue

                    // Вызываем загрузку специфичных данных для текущего соединения
                    LoadSpecificData(conn, selectedCategoryNameOnLoad);


                    // 3. Загрузка выбранных жанров
                    if (clbGenres != null)
                    {
                        string queryGenres = "SELECT GenreID FROM DiscGenres WHERE DiscID = @DiscID";
                        using (SqlCommand cmdGenres = new SqlCommand(queryGenres, conn))
                        {
                            cmdGenres.Parameters.AddWithValue("@DiscID", currentDiscId);
                            using (SqlDataReader readerGenres = cmdGenres.ExecuteReader())
                            {
                                List<int> selectedGenreIds = new List<int>();
                                while (readerGenres.Read())
                                {
                                    selectedGenreIds.Add(Convert.ToInt32(readerGenres["GenreID"]));
                                }

                                for (int i = 0; i < clbGenres.Items.Count; i++)
                                {
                                    if (clbGenres.Items[i] is DataRowView item)
                                    {
                                        int genreId = Convert.ToInt32(item["GenreID"]);
                                        if (selectedGenreIds.Contains(genreId))
                                        {
                                            clbGenres.SetItemChecked(i, true);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных диска: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Метод для загрузки специфичных данных в зависимости от категории (нужно дописать для всех категорий)
        private void LoadSpecificData(SqlConnection conn, string categoryName)
        {
            // Проверяем видимость панели перед загрузкой данных
            if (categoryName == "Фильм" && panelMovie != null && panelMovie.Visible)
            {
                string qMovie = "SELECT Director, MainActors, ReleaseYear, DurationMinutes, Rating FROM MovieSpecifics WHERE DiscID = @DiscID";
                using (SqlCommand cmd = new SqlCommand(qMovie, conn))
                {
                    cmd.Parameters.AddWithValue("@DiscID", currentDiscId);
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            if (txtDirector != null) txtDirector.Text = r["Director"].ToString();
                            if (txtMainActors != null) txtMainActors.Text = r["MainActors"].ToString();
                            if (nudMovieReleaseYear != null && r["ReleaseYear"] != DBNull.Value) nudMovieReleaseYear.Value = Convert.ToDecimal(r["ReleaseYear"]); else if (nudMovieReleaseYear != null) nudMovieReleaseYear.Value = nudMovieReleaseYear.Minimum;
                            if (nudMovieDuration != null && r["DurationMinutes"] != DBNull.Value) nudMovieDuration.Value = Convert.ToDecimal(r["DurationMinutes"]); else if (nudMovieDuration != null) nudMovieDuration.Value = nudMovieDuration.Minimum;
                            if (nudMovieRating != null && r["Rating"] != DBNull.Value) nudMovieRating.Value = Convert.ToDecimal(r["Rating"]); else if (nudMovieRating != null) nudMovieRating.Value = nudMovieRating.Minimum;
                        }
                    }
                }
            }
            // TODO: Дописать для других категорий
            // Например, для Аудио:
            else if ((categoryName == "Аудио CD" || categoryName == "MP3-диск") && panelAudio != null && panelAudio.Visible)
            {
                string qAudio = "SELECT Artist, Album, ReleaseYear, AudioType, Tracklist FROM AudioSpecifics WHERE DiscID = @DiscID";
                using (SqlCommand cmd = new SqlCommand(qAudio, conn))
                {
                    cmd.Parameters.AddWithValue("@DiscID", currentDiscId);
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            if (txtArtist != null) txtArtist.Text = r["Artist"].ToString();
                            if (txtAlbum != null) txtAlbum.Text = r["Album"].ToString();
                            if (nudAudioReleaseYear != null && r["ReleaseYear"] != DBNull.Value) nudAudioReleaseYear.Value = Convert.ToDecimal(r["ReleaseYear"]); else if (nudAudioReleaseYear != null) nudAudioReleaseYear.Value = nudAudioReleaseYear.Minimum;
                            if (cmbAudioType != null && r["AudioType"] != DBNull.Value) cmbAudioType.SelectedItem = r["AudioType"].ToString();
                            if (txtTracklist != null) txtTracklist.Text = r["Tracklist"].ToString();
                        }
                    }
                }
            }
            // TODO: Дописать для Software, Audiobook, Documentation
        }


        // Обработчик кнопки "Сохранить"
        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 1. Валидация общих полей
            if (txtTitle == null || string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Название диска не может быть пустым.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (txtTitle != null) txtTitle.Focus();
                return;
            }
            if (cmbCategory == null || cmbCategory.SelectedValue == null || cmbCategory.SelectedValue == DBNull.Value)
            {
                MessageBox.Show("Выберите категорию диска.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (cmbCategory != null) cmbCategory.Focus();
                return;
            }
            // TODO: Добавить валидацию специфичных полей, если необходимо

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    int categoryId = Convert.ToInt32(cmbCategory.SelectedValue);
                    object locationId = (cmbLocation == null || cmbLocation.SelectedValue == null || cmbLocation.SelectedValue == DBNull.Value) ? DBNull.Value : cmbLocation.SelectedValue;

                    // 2. Сохранение основной информации (INSERT или UPDATE)
                    if (currentDiscId == 0) // Новый диск (INSERT)
                    {
                        string queryInsertDisc = "INSERT INTO Discs (Title, CategoryID, LocationID, Notes, DateAdded) OUTPUT INSERTED.DiscID VALUES (@Title, @CategoryID, @LocationID, @Notes, @DateAdded)";
                        using (SqlCommand cmd = new SqlCommand(queryInsertDisc, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                            cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                            cmd.Parameters.AddWithValue("@LocationID", locationId);
                            cmd.Parameters.AddWithValue("@Notes", txtNotes?.Text ?? ""); // Используем ?? "" на случай, если txtNotes null
                            cmd.Parameters.AddWithValue("@DateAdded", dtpDateAdded?.Value ?? DateTime.Today); // Используем ?? для dtpDateAdded
                            currentDiscId = (int)cmd.ExecuteScalar(); // Получаем ID нового диска
                        }
                    }
                    else // Редактирование (UPDATE)
                    {
                        string queryUpdateDisc = "UPDATE Discs SET Title = @Title, CategoryID = @CategoryID, LocationID = @LocationID, Notes = @Notes, DateAdded = @DateAdded WHERE DiscID = @DiscID";
                        using (SqlCommand cmd = new SqlCommand(queryUpdateDisc, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@DiscID", currentDiscId);
                            cmd.Parameters.AddWithValue("@Title", txtTitle.Text);
                            cmd.Parameters.AddWithValue("@CategoryID", categoryId);
                            cmd.Parameters.AddWithValue("@LocationID", locationId);
                            cmd.Parameters.AddWithValue("@Notes", txtNotes?.Text ?? "");
                            cmd.Parameters.AddWithValue("@DateAdded", dtpDateAdded?.Value ?? DateTime.Today);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // 3. Сохранение специфичных данных (удалить старые, если есть, и вставить новые)
                    DataRowView drv = cmbCategory.SelectedItem as DataRowView;
                    string selectedCategoryNameOnSave = "";
                    if (drv != null) selectedCategoryNameOnSave = drv["CategoryName"].ToString();

                    SaveSpecificData(conn, transaction, selectedCategoryNameOnSave);


                    // 4. Сохранение жанров (удалить старые, вставить новые выбранные)
                    if (clbGenres != null)
                    {
                        string queryDeleteGenres = "DELETE FROM DiscGenres WHERE DiscID = @DiscID";
                        using (SqlCommand cmdDelGenres = new SqlCommand(queryDeleteGenres, conn, transaction))
                        {
                            cmdDelGenres.Parameters.AddWithValue("@DiscID", currentDiscId);
                            cmdDelGenres.ExecuteNonQuery();
                        }

                        string queryInsertGenre = "INSERT INTO DiscGenres (DiscID, GenreID) VALUES (@DiscID, @GenreID)";
                        foreach (var item in clbGenres.CheckedItems)
                        {
                            if (item is DataRowView checkedDrv)
                            {
                                if (checkedDrv["GenreID"] != DBNull.Value)
                                {
                                    int genreId = Convert.ToInt32(checkedDrv["GenreID"]);
                                    using (SqlCommand cmdInsGenre = new SqlCommand(queryInsertGenre, conn, transaction))
                                    {
                                        cmdInsGenre.Parameters.AddWithValue("@DiscID", currentDiscId);
                                        cmdInsGenre.Parameters.AddWithValue("@GenreID", genreId);
                                        cmdInsGenre.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }

                    transaction.Commit();
                    MessageBox.Show("Данные диска успешно сохранены!", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK; // Сигнализируем главной форме об успехе
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show($"Ошибка сохранения данных диска: {ex.Message}", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Метод для сохранения специфичных данных (нужно дописать для всех категорий)
        private void SaveSpecificData(SqlConnection conn, SqlTransaction transaction, string categoryName)
        {
            // Сначала удаляем старые специфичные данные для этого диска (для всех типов)
            // Убедитесь, что currentDiscId > 0 (что диск уже вставлен)
            if (currentDiscId <= 0) return;

            string[] specificTables = { "MovieSpecifics", "AudioSpecifics", "SoftwareSpecifics", "AudiobookSpecifics", "DocumentationSpecifics" };
            foreach (string tableName in specificTables)
            {
                string qDelete = $"DELETE FROM {tableName} WHERE DiscID = @DiscID";
                using (SqlCommand cmdDel = new SqlCommand(qDelete, conn, transaction))
                {
                    cmdDel.Parameters.AddWithValue("@DiscID", currentDiscId);
                    cmdDel.ExecuteNonQuery();
                }
            }

            // Затем вставляем новые данные в нужную таблицу
            if (categoryName == "Фильм" && panelMovie != null && panelMovie.Visible)
            {
                string qInsert = "INSERT INTO MovieSpecifics (DiscID, Director, MainActors, ReleaseYear, DurationMinutes, Rating) VALUES (@DiscID, @Director, @MainActors, @ReleaseYear, @DurationMinutes, @Rating)";
                using (SqlCommand cmd = new SqlCommand(qInsert, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@DiscID", currentDiscId);
                    cmd.Parameters.AddWithValue("@Director", txtDirector?.Text ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MainActors", txtMainActors?.Text ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReleaseYear", (nudMovieReleaseYear != null && nudMovieReleaseYear.Value > 0) ? (object)Convert.ToInt32(nudMovieReleaseYear.Value) : DBNull.Value);
                    cmd.Parameters.AddWithValue("@DurationMinutes", (nudMovieDuration != null && nudMovieDuration.Value > 0) ? (object)Convert.ToInt32(nudMovieDuration.Value) : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Rating", (nudMovieRating != null && nudMovieRating.Value >= 0) ? (object)nudMovieRating.Value : DBNull.Value); // Rating может быть 0
                    cmd.ExecuteNonQuery();
                }
            }
            // TODO: Дописать для других категорий
            // Пример для Аудио:
            else if ((categoryName == "Аудио CD" || categoryName == "MP3-диск") && panelAudio != null && panelAudio.Visible)
            {
                string qInsertAudio = "INSERT INTO AudioSpecifics (DiscID, Artist, Album, ReleaseYear, AudioType, Tracklist) VALUES (@DiscID, @Artist, @Album, @ReleaseYear, @AudioType, @Tracklist)";
                using (SqlCommand cmd = new SqlCommand(qInsertAudio, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@DiscID", currentDiscId);
                    cmd.Parameters.AddWithValue("@Artist", txtArtist?.Text ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Album", txtAlbum?.Text ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ReleaseYear", (nudAudioReleaseYear != null && nudAudioReleaseYear.Value > 0) ? (object)Convert.ToInt32(nudAudioReleaseYear.Value) : DBNull.Value);
                    cmd.Parameters.AddWithValue("@AudioType", cmbAudioType?.SelectedItem?.ToString() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tracklist", txtTracklist?.Text ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            // TODO: Дописать для Software (ПО/Игры), Audiobook, Documentation
            else if ((categoryName == "Программное обеспечение" || categoryName == "Игры") && panelSoftware != null && panelSoftware.Visible)
            {
                string qInsertSoftware = "INSERT INTO SoftwareSpecifics (DiscID, SoftwareName, Version, Developer, SoftwareType, LicenseKey) VALUES (@DiscID, @SoftwareName, @Version, @Developer, @SoftwareType, @LicenseKey)";
                using (SqlCommand cmd = new SqlCommand(qInsertSoftware, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@DiscID", currentDiscId);
                    cmd.Parameters.AddWithValue("@SoftwareName", txtSoftwareName?.Text ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Version", txtSoftwareVersion?.Text ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Developer", txtSoftwareDeveloper?.Text ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SoftwareType", cmbSoftwareType?.SelectedItem?.ToString() ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LicenseKey", txtLicenseKey?.Text ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            else if (categoryName == "Аудиокнига" && panelAudiobook != null && panelAudiobook.Visible)
            {
                string qInsertAudiobook = "INSERT INTO AudiobookSpecifics (DiscID, BookAuthor, Reader, TotalDurationMinutes) VALUES (@DiscID, @BookAuthor, @Reader, @TotalDurationMinutes)";
                using (SqlCommand cmd = new SqlCommand(qInsertAudiobook, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@DiscID", currentDiscId);
                    cmd.Parameters.AddWithValue("@BookAuthor", txtBookAuthor?.Text ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Reader", txtReader?.Text ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TotalDurationMinutes", (nudAudiobookDuration != null && nudAudiobookDuration.Value > 0) ? (object)Convert.ToInt32(nudAudiobookDuration.Value) : DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
            else if (categoryName == "Документация" && panelDocumentation != null && panelDocumentation.Visible)
            {
                string qInsertDocumentation = "INSERT INTO DocumentationSpecifics (DiscID, Topic, DocumentType, DocumentDate) VALUES (@DiscID, @Topic, @DocumentType, @DocumentDate)";
                using (SqlCommand cmd = new SqlCommand(qInsertDocumentation, conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@DiscID", currentDiscId);
                    cmd.Parameters.AddWithValue("@Topic", txtDocTopic?.Text ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DocumentType", txtDocType?.Text ?? (object)DBNull.Value); // Или cmbDocType.SelectedItem?.ToString()
                    cmd.Parameters.AddWithValue("@DocumentDate", (dtpDocDate != null && dtpDocDate.Value.Year > 1753) ? (object)dtpDocDate.Value : DBNull.Value); // Проверка на минимальную дату SQL Server
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Обработчик кнопки "Отмена"
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void panelDocumentation_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}