using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TravelPro 
{
    public partial class Main : Form
    {
        public static Main Instance { get; private set; }
        public Main()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            string currentRole = UserManager.CurrentUser.Role;
            string userName = UserManager.CurrentUser.Username;
            label1.Text = $"Здравстуйте {userName}, вы {currentRole} <3";
            if (currentRole != "Администратор")
            {
                пользователиToolStripMenuItem.Enabled = false;
            }
        }

        private void заявкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Requests category = new Requests();
            this.Hide();
            category.ShowDialog();
        }

        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Employees category = new Employees();
            this.Hide();
            category.ShowDialog();
        }

        private void бронированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bookings category = new Bookings();
            this.Hide();
            category.ShowDialog();
        }

        private void комментарииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Comments category = new Comments();
            this.Hide();
            category.ShowDialog();
        }

        private void статистикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Statistic category = new Statistic();
            this.Hide();
            category.ShowDialog();
        }
        private void пользователиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users category = new Users();
            this.Hide();
            category.ShowDialog();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
