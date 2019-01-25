using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp6Tarefas.Controller;
using XamarinApp6Tarefas.Enums;
using XamarinApp6Tarefas.Models.Tarefas;

namespace XamarinApp6Tarefas.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DataTarefasPage : ContentPage
    {
        private DataTarefaEntity _dataTarefa;
        private MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        public DataTarefasPage (DataTarefaEntity dataTarefa)
        {
            InitializeComponent ();

            _dataTarefa = dataTarefa;
            Title = _dataTarefa.Dia.ToString("dd/MM/yy");
            ListaTarefas.ItemsSource = dataTarefa.Tarefas.OrderBy(t => t.Realizado).ThenBy(t => t.Hora).ThenBy(t => t.Prioridade)
                .Select(t => new TarefaView(t.Id, t.Titulo, t.Prioridade, t.Hora, t.Descricao, t.Realizado, t.IdNotificacao));
        }

        private void BtnAlterarTarefaClicked(object sender, EventArgs args)
        {
            var btn = (ImageButton) sender;
            var tarefa = (TarefaEntity) btn.CommandParameter;
            RootPage.GoToTarefa(tarefa.Id);
        }

        private void BtnDescricaoClicked(object sender, EventArgs e)
        {
            var btn = (ImageButton) sender;
            var tarefa = (TarefaEntity) btn.CommandParameter;
            DisplayAlert("Descrição", tarefa.Descricao ?? "Não há descrição cadastrada", "Ok");
        }

        private void BtnCheckTarefaClicked(object sender, EventArgs e)
        {
            var btn = (ImageButton) sender;
            var tarefa = (TarefaEntity) btn.CommandParameter;
            TarefaController.Alterar(_dataTarefa.Id, _dataTarefa.Dia, tarefa.Id, tarefa.Titulo, tarefa.Prioridade, tarefa.Hora,
                tarefa.Descricao, !tarefa.Realizado);
            RootPage.GoHome(_dataTarefa.Id);
        }

        internal class TarefaView
            : TarefaEntity
        {
            public Style CheckButtonStyle { get; set; }
            public string StrHorario { get; protected set; }

            public TarefaView(Guid id, string titulo, PrioridadeEnum prioridade, TimeSpan hora, string descricao, bool realizado, int idNotificacao)
                : base(titulo, prioridade, hora, descricao, realizado, idNotificacao)
            {
                Id = id;
                CheckButtonStyle = (Style) ((Realizado) ? Application.Current.Resources["CheckBoxOn"] : Application.Current.Resources["CheckBoxOff"]);
                StrHorario = Hora.ToString(@"hh\:mm");
            }
        }

        
    }
}