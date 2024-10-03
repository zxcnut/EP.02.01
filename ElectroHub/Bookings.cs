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

namespace TravelPro 
{
    public partial class Bookings : Form
    {
        private string connectionString = @"Data Source=adclg1; Initial catalog=!!!Бронин_УП; Integrated Security=True";
        private SqlConnection connection;
        public Bookings()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);

        }

        private void Bookings_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string dateString1 = dataGridView1.SelectedRows[0].Cells["BookingDate"].Value.ToString();
                DateTime date1;
                if (DateTime.TryParseExact(dateString1, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date1))
                {
                    dateTimePicker1.Value = date1;
                }
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["RequestID"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["EmployeeID"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["BookingStatus"].Value.ToString();

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int requestID = Convert.ToInt32(textBox1.Text);
            int employeeID = Convert.ToInt32(textBox2.Text);
            DateTime bookingDate = dateTimePicker1.Value;
            string bookingStatus = textBox3.Text;
            string query = "INSERT INTO Bookings (RequestID, EmployeeID, BookingDate, BookingStatus) VALUES (@RequestID, @EmployeeID, @BookingDate, @BookingStatus)";
            ExecuteNonQuery(query, ("@RequestID ", requestID), ("@EmployeeID", employeeID), ("@BookingDate", bookingDate), ("@BookingStatus", bookingStatus));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int bookingID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["BookingID"].Value);

                string query1 = "DELETE FROM Bookings WHERE BookingID = @BookingID";
                ExecuteNonQuery(query1, ("@BookingID", bookingID));

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
                int bookingID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["BookingID"].Value);

                int requestID = Convert.ToInt32(textBox1.Text);
                int employeeID = Convert.ToInt32(textBox2.Text);
                DateTime bookingDate = dateTimePicker1.Value;
                string bookingStatus = textBox3.Text;

                string query = "UPDATE Bookings SET RequestID = @RequestID, EmployeeID = @EmployeeID, BookingDate = @BookingDate, BookingStatus = @BookingStatus WHERE BookingID = @BookingID";
                ExecuteNonQuery(query, ("@RequestID", requestID), ("@EmployeeID", employeeID), ("@BookingDate", bookingDate), ("@BookingStatus", bookingStatus), ("@BookingID", bookingID));
            }
            else
            {
                MessageBox.Show("Выберите заказ для обновления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadBookings();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Bookings_Load(object sender, EventArgs e)
        {
            LoadBookings();
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
                    LoadBookings(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void LoadBookings()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT BookingID, RequestID, EmployeeID, BookingDate, BookingStatus FROM Bookings";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }
    }
}
