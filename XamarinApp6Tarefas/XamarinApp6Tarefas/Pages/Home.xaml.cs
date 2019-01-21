using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp6Tarefas.Controller;
using XamarinApp6Tarefas.Models.Tarefas;

namespace XamarinApp6Tarefas.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Home : TabbedPage
    {
        private List<DataTarefaEntity> _datasTarefas;

		public Home ()
		{
			InitializeComponent ();

            _datasTarefas = TarefaController.DataTarefas.OrderBy(df => df.Dia).ToList();

            foreach (var dataTarefa in _datasTarefas)
            {
                this.Children.Add(new DataTarefasPage(dataTarefa));
            }
        }
    }
}