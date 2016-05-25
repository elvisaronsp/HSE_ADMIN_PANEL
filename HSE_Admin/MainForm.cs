using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HSE_Admin
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        Dictionary<string, FirebaseMethods.SecureKey> keys = new Dictionary<string, FirebaseMethods.SecureKey>();
        private async Task DisplayAll()
        {
            keys = await FirebaseMethods.GetKeys();
            listBoxKeys.Items.Clear();
            listBoxKeys.Items.AddRange(keys.Values.ToArray());
        }

        private async void buttonAddKey_Click(object sender, EventArgs e)
        {
            Enabled = false;
            string status = await FirebaseMethods.AddKey();
            Enabled = true;
            if (status == "OK")
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

        private async void listBoxKeys_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (listBoxKeys.SelectedIndex < 0) return;
                var key = listBoxKeys.SelectedItem as FirebaseMethods.SecureKey;
                var id = keys.First(x => x.Value.Key == key.Key).Key;
                var f = new KeyInfo(id, key);
                var res = f.ShowDialog();
                await DisplayAll();
            }
            catch { }
        }

        private void listBoxKeys_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            var sk = listBoxKeys.Items[e.Index] as FirebaseMethods.SecureKey;

            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            if (sk.Date == 0)
            {
                e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds);
            }
            if (sk.Date > 0)
            {
                e.Graphics.FillRectangle(Brushes.Green, e.Bounds);
            }
            if (sk.Date < 0)
            {
                e.Graphics.FillRectangle(Brushes.Red, e.Bounds);
            }

            if (selected)
            {
                //e.Graphics.FillRectangle(Brushes.Gray, e.Bounds);
            }


            using (Brush textBrush = new SolidBrush(e.ForeColor))
            {

                e.Graphics.DrawString(listBoxKeys.Items[e.Index].ToString(), e.Font, textBrush, e.Bounds.Location);
            }

            e.DrawFocusRectangle();
        }

        private void aboutItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Properties.Resources.About,"О программе");
        }

        private void exitItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
