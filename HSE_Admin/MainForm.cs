using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static HSE_Admin.FirebaseMethods;

namespace HSE_Admin
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async Task DisplayAll()
        {
            var array = await GetKeys();
            listBoxKeys.Items.Clear();
            listBoxKeys.Items.AddRange(array.Values.ToArray());
        }

        private async void buttonAddKey_Click(object sender, EventArgs e)
        {
            string status = await AddKey();
            if(status == "OK")
            {
                MessageBox.Show("Новый ключ добавлен", "Успешно!");
                await DisplayAll();
            }
        }

        private async void buttonRefresh_Click(object sender, EventArgs e)
        {
            await DisplayAll();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await DisplayAll();
        }
    }
}
