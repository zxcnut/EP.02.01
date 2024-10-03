using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
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
    public partial class Comments : Form
    {
        private string connectionString = @"Data Source=adclg1; Initial catalog=!!!Бронин_УП; Integrated Security=True";
        private SqlConnection connection;
        public Comments()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
        }

        private void Comments_Load(object sender, EventArgs e)
        {
            LoadComments();
        }

        private void Comments_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string dateString1 = dataGridView1.SelectedRows[0].Cells["CommentDate"].Value.ToString();
                DateTime date1;
                if (DateTime.TryParseExact(dateString1, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date1))
                {
                    dateTimePicker1.Value = date1;
                }
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["RequestID"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["EmployeeID"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["CommentText"].Value.ToString();

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
            string commentText = textBox3.Text;
            DateTime commentDate = dateTimePicker1.Value;
            string query = "INSERT INTO Comments (RequestID, EmployeeID, CommentText, CommentDate) VALUES (@RequestID, @EmployeeID, @CommentText, @CommentDate)";
            ExecuteNonQuery(query, ("@RequestID ", requestID), ("@EmployeeID", employeeID), ("@CommentText", commentText), ("@CommentDate", commentDate));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int commentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CommentID"].Value);

                string query1 = "DELETE FROM Comments WHERE CommentID = @CommentID";
                ExecuteNonQuery(query1, ("@CommentID", commentID));

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
                int commentID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["CommentID"].Value);

                int requestID = Convert.ToInt32(textBox1.Text);
                int employeeID = Convert.ToInt32(textBox2.Text);
                string commentText = textBox3.Text;
                DateTime commentDate = dateTimePicker1.Value;

                string query = "UPDATE Bookings SET RequestID = @RequestID, EmployeeID = @EmployeeID, CommentText = @CommentText, CommentDate = @CommentDate WHERE CommentID = @CommentID";
                ExecuteNonQuery(query, ("@RequestID", requestID), ("@EmployeeID", employeeID), ("@CommentText", commentText), ("@CommentDate", commentDate), ("@CommentID", commentID));
            }
            else
            {
                MessageBox.Show("Выберите заказ для обновления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadComments();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    LoadComments(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void LoadComments()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT CommentID, RequestID, EmployeeID, CommentText, CommentDate FROM Comments";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }
    }
}
