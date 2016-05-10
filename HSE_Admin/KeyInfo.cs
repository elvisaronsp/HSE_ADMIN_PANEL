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
    public partial class KeyInfo : Form
    {
        public KeyInfo(string Id, SecureKey Key)
        {
            this.Id = Id;
            this.Key = Key;
            InitializeComponent();
        }
        public string Id;
        public SecureKey Key;

        private async void KeyInfo_Load(object sender, EventArgs e)
        {
            Text = Key.Key;
            if (Key.Date == 0)
            {
                labelInfo.Text = "Ключ еще не активирован";
                labelUser.Text = "";
                buttonDelete.Text = "Удалить";
                labelDate.Text = "";

            }
            if (Key.Date > 0)
            {
                labelInfo.Text = "Ключ активирован";
                buttonDelete.Text = "Заблокировать";
                labelDate.Text = ConvertFromUnixTimestamp(Key.Date).ToString("MM/dd/yy HH:mm:ss zzz");
            }
            if (Key.Date < 0)
            {
                labelInfo.Text = "Ключ заблокирован";
                buttonDelete.Text = "Разблокировать";
                labelDate.Text = "";
            }
            if (Key.Date != 0)
            {
                labelUser.Text = "Загрузка...";
                await DisplayUserInfo();
            }
        }


        async Task DisplayUserInfo()
        {
            var user = await GetUser(Key.UsedBy);
            labelUser.Text = user.ToString();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Key.Key);
        }

        private async void buttonDelete_Click(object sender, EventArgs e)
        {
            Enabled = false;
            string st = "";
            if (Key.Date == 0)
            {
                st = await DeleteKey(Id);
            }else
            if (Key.Date > 0)
            {
                Key.Date = -1;
                st = await SetKey(Id,Key);
            }else
            if (Key.Date < 0)
            {
                Key.Date = ConvertToUnixTimestamp(DateTime.Now);
                st = await SetKey(Id, Key);
            }
            if (st != "")
            {
                Close();
            }
        }
    }
}
