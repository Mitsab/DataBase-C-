using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DB1
{
    public partial class edit : Form
    {
        private readonly string _server;
        private readonly string _database;
        private readonly string _username;
        private readonly string _password;
        private readonly int _port;
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }
        public class ApplicationContext : DbContext
        {
            public DbSet<User> Users { get; set; }

            public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
            {

            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<User>().ToTable("Users");
                base.OnModelCreating(modelBuilder);
            }
        }
        public edit (string server, string database, string username, string password, int port)
        {
            InitializeComponent();
        _server = server;
            _database = database;
            _username = username;
            _password = password;
            _port = port;

            // Настройка DataGridView
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
        }

        private async void edit_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                EnableControls(false);

                var connectionString = $"Host={_server};Port={_port};Database={_database};Username={_username};Password={_password}";

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
                optionsBuilder.UseNpgsql(connectionString);

                await using var context = new ApplicationContext(optionsBuilder.Options);

                if (!await context.Database.CanConnectAsync())
                {
                    MessageBox.Show("Не удалось подключиться к базе данных", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var users = await context.Users.AsNoTracking().ToListAsync();
                dataGridView1.DataSource = users;
            }
            catch (Npgsql.PostgresException ex) when (ex.SqlState == "28P01")
            {
                MessageBox.Show("Неверное имя пользователя или пароль", "Ошибка аутентификации",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            catch (Npgsql.PostgresException ex)
            {
                MessageBox.Show($"Ошибка PostgreSQL: {ex.Message}", "Ошибка базы данных",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                EnableControls(true);
            }
        }
        private void EnableControls(bool enable)
        {
            button1.Enabled = enable;
            button2.Enabled = enable;
            button3.Enabled = enable;
            button4.Enabled = enable;
            button5.Enabled = enable;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }


    }
}
