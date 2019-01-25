using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp6Tarefas.Controller;
using XamarinApp6Tarefas.Enums;
using XamarinApp6Tarefas.Models.Tarefas;

namespace XamarinApp6Tarefas.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Tarefa : ContentPage
	{
	    private DataTarefaEntity _dataTarefa;
	    private Guid? _idTarefa;
        private List<DiaSemanaEnum> _diasSemana;
        private PrioridadeEnum _prioridade;
        private bool _definirDiaFinal;

        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        public Tarefa (Guid? idTarefa)
        {
            _idTarefa = idTarefa;

            InitializeComponent ();
            Title = "Nova Tarefa";

            var prioridades = PrioridadeEnum.GetAll<PrioridadeEnum>().ToList();
            _diasSemana = DiaSemanaEnum.GetAll<DiaSemanaEnum>().ToList();

            ListaPrioridades.ItemsSource = prioridades;
            _definirDiaFinal = false;
            DataTarefa.MinimumDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DataFinalTarefa.Date = (DataTarefa.Date).AddDays(1);
            DataFinalTarefa.MinimumDate = (DataTarefa.Date).AddDays(1);
            BtnAlterar.IsVisible = false;
            ControleListaDiaSemanaSwitch(DataTarefa.Date, DataFinalTarefa.Date);

            if (_idTarefa.HasValue 
            && TarefaController.DataTarefas.Any(dt => dt.Tarefas.Any(t => t.Id == _idTarefa)))
		    {
		        _dataTarefa = TarefaController.DataTarefas.Where(dt => dt.Tarefas.Any(t => t.Id == _idTarefa)).FirstOrDefault();
                Title = "Alterar Tarefa";
                DataTarefa.Date = _dataTarefa.Dia;
                DataTarefa.MinimumDate = _dataTarefa.Dia;
                HoraTarefa.Time = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Hora;
                TituloTarefa.Text = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Titulo;
                ListaPrioridades.SelectedItem = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Prioridade;
                _prioridade = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Prioridade;
                DescricaoTarefa.Text = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Descricao;
                BtnCadastrar.IsVisible = false;
                BtnAlterar.IsVisible = true;
            }


            ControleDefinirDiaFinal();
        }

        private void DataTarefaSelected(object sender, DateChangedEventArgs args)
        {
            if (DataFinalTarefa.Date <= args.NewDate)
            {
                DataFinalTarefa.Date = (args.NewDate).AddDays(1);
            }
            DataFinalTarefa.MinimumDate = (args.NewDate).AddDays(1);

            ControleListaDiaSemanaSwitch(args.NewDate, DataFinalTarefa.Date);
        }

        private void DiaSemanaTogled(object sender, ToggledEventArgs args)
        {
            var sw = (Switch) sender;
            var diaSemana = (DiaSemanaEnum)sw.BindingContext;
            _diasSemana.Find(ds => ds.Id == diaSemana.Id).Ativo = args.Value;
        }

        private void ListaPrioridadesItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            _prioridade = (PrioridadeEnum) args.SelectedItem;
        }

        private void DefiniDataFinal(object sender, ToggledEventArgs args)
        {
            _definirDiaFinal = args.Value;
            ControleDefinirDiaFinal();
        }

        private void DataTarefaFinalSelected(object sender, DateChangedEventArgs args)
        {
            ControleListaDiaSemanaSwitch(DataTarefa.Date, args.NewDate);
        }

        private void BtnCadastrarClicked(object sender, EventArgs e)
        {
            if (IsValid())
            {
                TimeSpan? hora = null;

                if (TarefaController.Cadastrar(DataTarefa.Date, TituloTarefa.Text, _prioridade, HoraTarefa.Time,
                    DescricaoTarefa.Text))
                {
                    DisplayAlert("Sucesso", "Tarefa cadastrada com sucesso.", "Ok");
                    RootPage.GoHome();
                }
                else
                {
                    DisplayAlert("Erro", "Erro ao tentar cadastrar a nova tarefa.", "Ok");
                }
            }
            else
            {
                DisplayAlert("Erro", "Informe todos os campos obrigatórios", "Ok");
            }
        }

        private void BtnCancelarClicked(object sender, EventArgs e)
        {
            RootPage.GoHome();
        }

        private void BtnAlterarClicked(object sender, EventArgs e)
        {
            if (IsValid())
            {
                TimeSpan? hora = null;

                if(TarefaController.Alterar(_dataTarefa.Id, DataTarefa.Date, _idTarefa.Value, TituloTarefa.Text, _prioridade, HoraTarefa.Time, DescricaoTarefa.Text, _dataTarefa.Tarefas.Find(t => t.Id == _idTarefa).Realizado))
                {
                    DisplayAlert("Sucesso", "Tarefa alterada com sucesso.", "Ok");
                    RootPage.GoHome();
                }
                else
                {
                    DisplayAlert("Erro", "Erro ao tentar alterar a tarefa.", "Ok");
                }
            }
            else
            {
                DisplayAlert("Erro", "Informe todos os campos obrigatórios", "Ok");
            }
        }

        #region Controls

        private void ControleDefinirDiaFinal()
        {
            MainGrid.Children[4].IsVisible = _definirDiaFinal;
            MainGrid.Children[5].IsVisible = _definirDiaFinal;
            MainGrid.Children[6].IsVisible = _definirDiaFinal;
            MainGrid.Children[7].IsVisible = _definirDiaFinal;
        }

        private void ControleListaDiaSemanaSwitch(DateTime dataInicial, DateTime? dataFinal)
        {
            if (dataFinal.HasValue)
            {
                var diasSemanaToAtivo = Enumerable.Range(0, 1 + dataFinal.Value.Subtract(dataInicial).Days)
                    .Select(offset => dataInicial.AddDays(offset))
                    .ToList()
                    .Select(d => d.DayOfWeek)
                    .Distinct()
                    .ToList();

                foreach (var diaSemanaEnum in _diasSemana.FindAll(ds => diasSemanaToAtivo.Contains(ds.Id)))
                {
                    diaSemanaEnum.Ativo = true;
                }
            }
            else
            {
                foreach (var diaSemanaEnum in _diasSemana.FindAll(ds => ds.Id != dataInicial.DayOfWeek))
                {
                    diaSemanaEnum.Ativo = false;
                }
            }

            ListaDiaSemana.ItemsSource = _diasSemana;
        }

        #endregion

        #region Validations

        private bool IsValid()
        {
            var listaValidacoes = new List<bool>();

            listaValidacoes.Add(DataValida());
            listaValidacoes.Add(HoraValida());
            listaValidacoes.Add(TituloValido());
            listaValidacoes.Add(PrioridadeValida());

            return !listaValidacoes.Any(v => v == false);
        }

        private bool DataValida()
        {
            var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            if ((_dataTarefa != null && DataTarefa.Date != null)
            || (_dataTarefa == null && DataTarefa.Date != null && DataTarefa.Date >= hoje)
            )
            {
                return true;
            }
            return false;
        }

        private bool HoraValida()
        {
            if (HoraTarefa.Time != null)
            {
                return true;
            }
            return false;
        }

        private bool TituloValido()
        {
            if (TituloTarefa.Text != null && TituloTarefa.Text.Length > 0)
            {
                return true;
            }

            return false;
        }

        private bool PrioridadeValida()
        {
            if (_prioridade != null)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}