using PRBD_Framework;
using System;
using System.Windows;
using System.Windows.Input;

namespace prbd_1718_presences_g27
{
    /// <summary>
    /// Logique d'interaction pour LoginView.xaml
    /// </summary>
    public partial class LoginView : WindowBase
    {
        public ICommand Login { get; set; }
        public ICommand Cancel { get; set; }

        private string pseudo;
        public string Pseudo { get { return pseudo; } set { pseudo = value; Validate(); } }

        private string password;
        public string Password { get { return password; } set { password = value; Validate(); } }
        public int  PseudoPass;

        public LoginView()
        {
            InitializeComponent();
            PseudoPass = 0;
            Login = new RelayCommand(LoginAction, () => { return pseudo != null && password != null && !HasErrors; });
            Cancel = new RelayCommand(() => Close());
            
            DataContext = this;

            Pseudo = "stephanie"; /*connection avec stephanie*/
            //Pseudo = "boris"; /*connection avec boris*/
            Password = "password";
        }

        private User Validate()
        {
            ClearErrors();

            foreach (var u in App.Model.user)
            {
                if (u.Pseudo == Pseudo)
                {
                    PseudoPass = u.Id;
                }

            }
            
            var user = App.Model.user.Find(PseudoPass);
           

            if (string.IsNullOrEmpty(Pseudo))
            {
                AddError("Pseudo", Properties.Resources.Error_Required);
            }
            if (Pseudo != null)
            {
                if (Pseudo.Length < 3)
                    AddError("Pseudo", Properties.Resources.Error_LengthGreaterEqual3);
                else
                {
                    if (user == null)
                        AddError("Pseudo", Properties.Resources.Error_DoesNotExist);
                }
            }

            if (string.IsNullOrEmpty(Password))
                AddError("Password", Properties.Resources.Error_Required);
            if (Password != null)
            {
                if (Password.Length < 3)
                    AddError("Password", Properties.Resources.Error_LengthGreaterEqual3);
                else if (user != null && user.Password != Password)
                    AddError("Password", Properties.Resources.Error_WrongPassword);
            }

            RaiseErrors();

            return user;
        }

        private void LoginAction()
        {
            var user = Validate();
            if (!HasErrors)
            {
                App.CurrentUser = user;
                ShowMainView();
                Close();
            }
        }

        private static void ShowMainView()
        {
            var mainView = new MainView();
            mainView.Show();
            Application.Current.MainWindow = mainView;
        }

    }
}
