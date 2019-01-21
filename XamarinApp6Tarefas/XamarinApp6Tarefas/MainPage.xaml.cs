using System;
using Xamarin.Forms;
using XamarinApp6Tarefas.Pages;

namespace XamarinApp6Tarefas
{
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            GoHome();
        }

        public void GoHome()
        {
            GoHome(null);
        }

        public void GoHome(Guid? idDataTarefa)
        {
            Detail = new NavigationPage(new Home(idDataTarefa))
            {
                BarBackgroundColor = Color.FromHex("0D1F2D"),
                BarTextColor = Color.White
            };
            IsPresented = false;
        }

        public void GoToTarefa(Guid? id)
        {
            Detail = new NavigationPage(new Tarefa(id))
            {
                BarBackgroundColor = Color.FromHex("0D1F2D"),
                BarTextColor = Color.White
            };
            IsPresented = false;
        }

        private void GoToHome(object sender, EventArgs e)
        {
            GoHome();
        }

        private void NovaTarefa(object sender, EventArgs e)
        {
            GoToTarefa(null);
        }
    }
}
