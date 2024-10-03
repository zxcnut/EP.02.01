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
    public partial class Employees : Form
    {
        private string connectionString = @"Data Source=adclg1; Initial catalog=!!!Бронин_УП; Integrated Security=True";
        private SqlConnection connection;
        public Employees()
        {
            InitializeComponent();
            connection = new SqlConnection(connectionString);
        }

        private void Employees_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells["EmployeeName"].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells["EmployeePhone"].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells["Role"].Value.ToString();

                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string employeeName = textBox1.Text;
            string employeePhone = textBox2.Text;
            string role = textBox3.Text;
            string query = "INSERT INTO Employees (EmployeeName, EmployeePhone, Role) VALUES (@EmployeeName, @EmployeePhone, @Role)";
            ExecuteNonQuery(query, ("@EmployeeName", employeeName), ("@EmployeePhone", employeePhone), ("@Role", role));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int employeesID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["RequestsID"].Value);

                string query1 = "DELETE FROM Employees WHERE EmployeesID = @EmployeesID";
                string query2 = "DELETE FROM Bookings WHERE EmployeesID = @EmployeesID";
                string query3 = "DELETE FROM Comments WHERE EmployeesID = @EmployeesID";
                ExecuteNonQuery(query1, ("@EmployeesID", employeesID));
                ExecuteNonQuery(query2, ("@EmployeesID", employeesID));
                ExecuteNonQuery(query3, ("@EmployeesID", employeesID));

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
                int employeesID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["EmployeesID"].Value);

                string employeeName = textBox1.Text;
                string employeePhone = textBox2.Text;
                string role = textBox3.Text;

                string query = "UPDATE Requests SET EmployeeName = @EmployeeName, EmployeePhone = @EmployeePhone, Role = @Role WHERE EmployeesID = @EmployeesID";
                ExecuteNonQuery(query, ("@EmployeeName", employeeName), ("@EmployeePhone", employeePhone), ("@Role", role));
            }
            else
            {
                MessageBox.Show("Выберите заказ для обновления.");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadEmployees();
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
                    LoadEmployees(); // Перезагружаем данные после выполнения операции
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при выполнении операции: " + ex.Message);
                }
            }
        }

        private void LoadEmployees()
        {
            string currentRole = UserManager.CurrentUser.Role;
            if (currentRole != "Администратор")
            {
                button1.Hide();
                button2.Hide();
                button3.Hide();
            }

            string query = "SELECT EmployeesID, EmployeeName, EmployeePhone, Role FROM Employees";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }

        private void Employees_FormClosed(object sender, FormClosedEventArgs e)
        {
            Main.Instance.Show();
        }
    }
}
