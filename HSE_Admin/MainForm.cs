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

        private void listBoxKeys_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(listBoxKeys.SelectedIndex+"");
        }

        private void окноToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listBoxKeys_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            var sk = listBoxKeys.Items[e.Index] as SecureKey;

            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            if (sk.Date == 0)
            {
                e.Graphics.FillRectangle(Brushes.Green, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
            }

            if (selected)
            {
                //e.Graphics.FillRectangle(Brushes.Gray, e.Bounds);
            }

            /*if (e.Index == 0)
                e.Graphics.FillRectangle(Brushes.Green, e.Bounds);
            else if (e.Index == 1)
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
            else
                e.DrawBackground();
                */


            using (Brush textBrush = new SolidBrush(e.ForeColor))
            {

                e.Graphics.DrawString(listBoxKeys.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds.Location);
            }

            e.DrawFocusRectangle();
        }
    }
}
