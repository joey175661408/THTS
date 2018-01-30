using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace THTS.UserCenter
{
    /// <summary>
    /// DeviceNew.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePassword
    {
        DataAccess.User _newUser = null;

        public ChangePassword(DataAccess.User user)
        {
            InitializeComponent();
            _newUser = user;
            this.tbUserName.Text = _newUser.UserName;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!this.pbPassword.Password.Equals(_newUser.Password))
            {
                MessageBox.Show("原密码错误，请重新输入！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!this.pbPasswordOk.Password.Equals(this.pbPasswordNew.Password))
            {
                MessageBox.Show("两次密码不一致，请重新输入！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _newUser.Password = this.pbPasswordNew.Password;
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
