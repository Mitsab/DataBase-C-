using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.ApplicationServices;
using Npgsql.Internal;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Npgsql;

namespace DB1
{
    public partial class connect_edit : Form
    {
        private string serv = "localhost";
        private string data = "postgres";
        private string user = "postgres";
        private string pas = ""; 
        private int port = 5432;

        public class ApplicationContext : DbContext
        {
            public DbSet<User> Users { get; set; } = null!;

            public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
            {
                Database.EnsureCreated(); 
            }
            public class User
            {
                public int Id { get; set; }
                public string Name { get; set; } = string.Empty;
                public string Email { get; set; } = string.Empty;
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<User>().ToTable("Users");
            }
        }

        private Form1 _form1;

        public connect_edit(Form1 form1)
        {
            InitializeComponent();
            _form1 = form1;
             textBox5.PasswordChar = '*';
        }

        private void InitializeDefaultValues()
        {
            textBox1.Text = serv;
            textBox3.Text = port.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            serv = textBox1.Text; 
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            data = textBox2.Text; 
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox3.Text, out port))
            {
                MessageBox.Show("Введите корректный номер порта.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Text = port.ToString();
                textBox3.Focus();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            user = textBox4.Text; 
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            pas = textBox5.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(pas))
            {
                MessageBox.Show("Введите пароль", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox5.Focus();
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor; 
                button1.Enabled = false;

                var connectionString = $"Host={serv};Port={port};Database={data};Username={user};Password={pas}";

                using (var connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Close();
                }

                MessageBox.Show("Соединение успешно установлено!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                var editForm = new edit(serv, data, user, pas, port);
                editForm.Show();
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка:\n{ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            _form1.Show();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void connect_edit_Load(object sender, EventArgs e)
        {
            InitializeDefaultValues();
        }
    }
}
