using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TravelPro 
{
    public partial class Requests : Form
    {
        private string connectionString = @"Data Source=adclg1; Initial catalog=!!!Бронин_УП; Integrated Security=True";
        private SqlConnection connection;
        public Requests()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
        }
        private void Requests_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dateAdded = dateTimePicker1.Value;
            DateTime tourStartDate = dateTimePicker2.Value;
            DateTime tourEndDate = dateTimePicker3.Value;

            string tourType = textBox1.Text;
            int numberOfPeople = Convert.ToInt32(textBox2.Text);
            string clientName = textBox3.Text;
            string clientPhone = textBox4.Text;
            string status = textBox5.Text;
            
            string query = "INSERT INTO Requests (DateAdded, TourType, TourStartDate, TourEndDate, NumberOfPeople, ClientName, ClientPhone, Status) VALUES (@DateAdded, @TourType, @TourStartDate, @TourEndDate, @NumberOfPeople, @ClientName, @ClientPhone, @Status)";
            ExecuteNonQuery(query, ("@DateAdded", dateAdded), ("@TourType", tourType), ("@TourStartDate", tourStartDate), ("@TourEndDate", tourEndDate), ("@NumberOfPeople", numberOfPeople), ("@ClientName", clientName), ("@ClientPhone", clientPhone), ("@Status", status));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int requestsID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["RequestsID"].Value);

                string query1 = "DELETE FROM Requests WHЕRE RequestsID = @RequestsID";
                string query2 = "DELETE FROM Bookings WHERE RequestsID = @RequestsID";
                string query3 = "DELETE FROM Comments WHERE RequestsID = @RequestsID";
                ExecuteNonQuery(query1, ("@RequestsID", requestsID));
                ExecuteNonQuery(query2, ("@RequestsID", requestsID));
                ExecuteNonQuery(query3, ("@RequestsID", requestsID));

            }
            else
            {
                MessageBox.Show("Выберите заказ для удаления.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int requestsID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["RequestsID"].Value);
                DateTime dateAdded = dateTimePicker1.Value;
                DateTime tourStartDate = dateTimePicker2.Value;
                DateTime tourEndDate = dateTimePicker3.Value;

                string tourType = textBox1.Text;
                int numberOfPeople = Convert.ToInt32(textBox2.Text);
                string clientName = textBox3.Text;
                string clientPhone = textBox4.Text;
                string status = textBox5.Text;

                string query = "UPDATE Requests SET DateAdded = @DateAdded, TourType = @TourType, TourStartDate = @TourStartDate, TourEndDate = @TourEndDate, NumberOfPeople = @NumberOfPeople, ClientName = @ClientName, ClientPhone = @ClientPhone, Status = @Status WHERE RequestsID = @RequestsID";
                ExecuteNonQuery(query, ("@DateAdded", dateAdded), ("@TourType", tourType), ("@TourStartDate", tourStartDate), ("@TourEndDate", tourEndDate), ("@NumberOfPeople", numberOfPeople), ("@ClientName", clientName), ("@ClientPhone", clientPhone), ("@Status", status), ("@RequestsID", requestsID));
            }
            else
            {
                MessageBox.Show("Выберите заказ для обновления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadRequests();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            DateTime dateAdded = DateTime.Today;
            DateTime tourStartDate = DateTime.Today;
            DateTime tourEndDate = DateTime.Today;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string dateString1 = dataGridView1.SelectedRows[0].Cells["DateAdded"].Value.ToString();
                DateTime date1;
                string dateString2 = dataGridView1.SelectedRows[0].Cells["TourStartDate"].Value.ToString();
                DateTime date2;
                string dateString3 = dataGridView1.SelectedRows[0].Cells["TourEndDate"].Value.ToString();
                DateTime date3;

                if (DateTime.TryParseExact(dateString1, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date1))
                {
                    dateTimePicker1.Value = date1;
                }
                if (DateTime.TryParseExact(dateString2, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date2))
                {
                    dateTimePicker2.Value = date2;
                }
                if (DateTime.TryParseExact(dateString3, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date3))
                {
                    dateTimePicker3.Value = date3;
                }

                textBox1.Text = dataGridView1.SelectedRows[0].Cells["TourType"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["NumberOfPeople"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["ClientName"].Value.ToString();
                textBox4.Text = dataGridView1.SelectedRows[0].Cells["ClientPhone"].Value.ToString();
                textBox5.Text = dataGridView1.SelectedRows[0].Cells["Status"].Value.ToString();
                
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }
        private void ExecuteNonQuery(string query, params (string, object)[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
                }

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Операция успешно выполнена.");
                    LoadRequests(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void LoadRequests()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT DateAdded, TourType, TourStartDate, TourEndDate, NumberOfPeople, ClientName, ClientPhone, Status FROM Requests";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }
        private void Requests_Load(object sender, EventArgs e)
        {
            LoadRequests();
        }
    }
}
