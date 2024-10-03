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
    public partial class Statistic : Form
    {
        private string connectionString = @"Data Source=adclg1; Initial catalog=!!!Бронин_УП; Integrated Security=True";
        private SqlConnection connection;
        public Statistic()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
        }

        private void Statistic_Load(object sender, EventArgs e)
        {
            LoadStatistic();
        }

        private void Statistic_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tourType = textBox1.Text;
            int numberOfBookings = Convert.ToInt32(textBox2.Text);
            float averageProcessingTime = Convert.ToSingle(textBox3.Text);
            string query = "INSERT INTO Statistic (TourType, NumberOfBookings, AverageProcessingTime) VALUES (@RequestID, @NumberOfBookings, @AverageProcessingTime)";
            ExecuteNonQuery(query, ("@TourType ", tourType), ("@NumberOfBookings", numberOfBookings), ("@AverageProcessingTime", averageProcessingTime));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int statisticID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["StatisticID"].Value);

                string query1 = "DELETE FROM Statistic WHERE StatisticID = @StatisticID";
                ExecuteNonQuery(query1, ("@StatisticID", statisticID));

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
                int statisticID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["StatisticID"].Value);

                string tourType = textBox1.Text;
                int numberOfBookings = Convert.ToInt32(textBox2.Text);
                float averageProcessingTime = Convert.ToSingle(textBox3.Text);

                string query = "UPDATE Statistic SET TourType = @TourType, NumberOfBookings = @NumberOfBookings, AverageProcessingTime = @AverageProcessingTime WHERE StatisticID = @StatisticID";
                ExecuteNonQuery(query, ("@TourType", tourType), ("@NumberOfBookings", numberOfBookings), ("@AverageProcessingTime", averageProcessingTime), ("@StatisticID", statisticID));
            }
            else
            {
                MessageBox.Show("Выберите заказ для обновления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadStatistic();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["TourType"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["NumberOfBookings"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["AverageProcessingTime"].Value.ToString();

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
                    LoadStatistic(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void LoadStatistic()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT StatisticID, TourType, NumberOfBookings, AverageProcessingTime FROM Statistic";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }
    }
}
