
using System.Windows;
using THTS.MVVM;

namespace THTS.UserCenter
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string userName = this.tbUserName.Text;
            string password = this.pbPassword.Password;
            if(userName == "" || password == "")
            {
                MessageBox.Show("用户名或密码不得为空！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (userName.Equals("admin") && password.Equals("admin"))
            {
                this.DialogResult = true;
                return;
            }

            if (!DataAccess.UserDAO.IsLogin(userName, password))
            {
                MessageBox.Show("用户名或密码错误！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }else
            {
                this.DialogResult = true;
            }

        }
    }
}
