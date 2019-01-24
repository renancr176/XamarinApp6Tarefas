using System;
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

        public Home()
        {
            InitializeComponent();

            Init();
        }

		public Home (Guid? idDataTarefa)
		{
			InitializeComponent ();

            Init();

            if (idDataTarefa.HasValue)
            {
                SelectedItem = Children[_datasTarefas.IndexOf(_datasTarefas.Find(df => df.Id == idDataTarefa.Value))];
            }
            else if(_datasTarefas.Count > 0)
            {
                idDataTarefa = _datasTarefas.Select(df => new {df.Id, (df.Dia - DateTime.Today).TotalDays })
                    .OrderBy(x => Math.Abs((long) x.TotalDays - 0)).First().Id;

                SelectedItem = Children[_datasTarefas.IndexOf(_datasTarefas.Find(df => df.Id == idDataTarefa.Value))];
            }
        }

        private void Init()
        {
            _datasTarefas = TarefaController.DataTarefas.OrderBy(df => df.Dia).ToList();

            foreach (var dataTarefa in _datasTarefas)
            {
                Children.Add(new DataTarefasPage(dataTarefa));
            }
        }
    }
}