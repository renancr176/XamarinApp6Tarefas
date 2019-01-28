using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private ObservableCollection<DiaSemanaEnum> _diasSemana;
        private PrioridadeEnum _prioridade;
        private bool _definirDiaFinal;

        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        public Tarefa (Guid? idTarefa)
        {
            _idTarefa = idTarefa;

            InitializeComponent ();
            Title = "Nova Tarefa";

            var prioridades = PrioridadeEnum.GetAll<PrioridadeEnum>().ToList();
            _diasSemana = new ObservableCollection<DiaSemanaEnum>(DiaSemanaEnum.GetAll<DiaSemanaEnum>().ToList());

            ListaPrioridades.ItemsSource = prioridades;
            NotificacaoTempo.ItemsSource = NotificacaoTempoEnum.GetAll<NotificacaoTempoEnum>().ToList();
            NotificacaoTempo.SelectedIndex = 0;
            _definirDiaFinal = false;
            DataTarefa.MinimumDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            DataFinalTarefa.Date = (DataTarefa.Date).AddDays(1);
            DataFinalTarefa.MinimumDate = (DataTarefa.Date).AddDays(1);
            //ControleListaDiaSemanaSwitch(DataTarefa.Date, DataFinalTarefa.Date);

            if (_idTarefa.HasValue 
            && TarefaController.DataTarefas.Any(dt => dt.Tarefas.Any(t => t.Id == _idTarefa)))
		    {
                MainGrid.Children[4].IsVisible = false;
                MainGrid.Children[5].IsVisible = false;
                _dataTarefa = TarefaController.DataTarefas.Where(dt => dt.Tarefas.Any(t => t.Id == _idTarefa)).FirstOrDefault();
                Title = "Alterar Tarefa";
                DataTarefa.Date = _dataTarefa.Dia;
                DataTarefa.MinimumDate = _dataTarefa.Dia;
                HoraTarefa.Time = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Hora;
                TituloTarefa.Text = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Titulo;
                ListaPrioridades.SelectedItem = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Prioridade;
                _prioridade = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Prioridade;
                NotificacaoTempo.SelectedItem = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).NotificacaoTempo;
                DescricaoTarefa.Text = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Descricao;
                BtnCadastrar.IsVisible = false;
                BtnAlterar.IsVisible = true;
                ExcluirSwitch.IsVisible = true;
            }


            ControleDefinirDiaFinal();
        }

        #region Events Handlers
        
        private void DataTarefaSelected(object sender, DateChangedEventArgs args)
        {
            if (DataFinalTarefa.Date <= args.NewDate)
            {
                DataFinalTarefa.Date = (args.NewDate).AddDays(1);
            }
            else
            {
                ControleListaDiaSemanaSwitch(args.NewDate, DataFinalTarefa.Date);
            }

            DataFinalTarefa.MinimumDate = (args.NewDate).AddDays(1);
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

        private void BtnCadastrarClicked(object sender, EventArgs args)
        {
            if (IsValid())
            {
                TimeSpan? hora = null;
                DateTime? dataFinal = DataFinalTarefa.Date;
                if (!_definirDiaFinal)
                {
                    dataFinal = null;
                }

                if (TarefaController.Cadastrar(DataTarefa.Date, dataFinal, _diasSemana.ToList(), TituloTarefa.Text, _prioridade, HoraTarefa.Time,
                    DescricaoTarefa.Text, (NotificacaoTempoEnum) NotificacaoTempo.SelectedItem))
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

        private void BtnCancelarClicked(object sender, EventArgs args)
        {
            RootPage.GoHome();
        }

        private void BtnAlterarClicked(object sender, EventArgs args)
        {
            if (IsValid())
            {
                TimeSpan? hora = null;

                if(TarefaController.Alterar(_dataTarefa.Id, DataTarefa.Date, _idTarefa.Value, TituloTarefa.Text, _prioridade, HoraTarefa.Time, DescricaoTarefa.Text, 
                    _dataTarefa.Tarefas.Find(t => t.Id == _idTarefa).Realizado, (NotificacaoTempoEnum)NotificacaoTempo.SelectedItem))
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

        private void SwExcluirToggled(object sender, ToggledEventArgs args)
        {
            switch (args.Value)
            {
                case true:
                    BtnAlterar.IsVisible = false;
                    BtnExcluir.IsVisible = true;
                break;
                default:
                    BtnAlterar.IsVisible = true;
                    BtnExcluir.IsVisible = false;
                break;
            }
        }

        private void BtnExcluirClicked(object sender, EventArgs e)
        {
            if (_idTarefa != null)
            {
                TarefaController.Remover(_idTarefa.Value);
                DisplayAlert("Sucesso", "Tarefa excluida com sucesso.", "Ok");
                RootPage.GoHome();
            }
        }

        #endregion

        #region Controls

        private void ControleDefinirDiaFinal()
        {
            MainGrid.Children[6].IsVisible = _definirDiaFinal;
            MainGrid.Children[7].IsVisible = _definirDiaFinal;
            MainGrid.Children[8].IsVisible = _definirDiaFinal;
            MainGrid.Children[9].IsVisible = _definirDiaFinal;
        }

        private void ControleListaDiaSemanaSwitch(DateTime dataInicial, DateTime dataFinal)
        {
            var diasSemanaToAtivo = Enumerable.Range(0, 1 + dataFinal.Subtract(dataInicial).Days)
                .Select(offset => dataInicial.AddDays(offset))
                .ToList()
                .Select(d => d.DayOfWeek)
                .Distinct()
                .ToList();

            var temp = new ObservableCollection<DiaSemanaEnum>();
            foreach (var diaSemana in _diasSemana)
            {
                temp.Add(new DiaSemanaEnum(diaSemana.Id, diaSemana.Descricao, diaSemana.Sigla));
            }

            foreach (var diaSemanaEnum in temp.ToList().FindAll(ds => diasSemanaToAtivo.Contains(ds.Id)))
            {
                diaSemanaEnum.Ativo = true;
            }

            foreach (var diaSemanaEnum in temp.ToList().FindAll(ds => !diasSemanaToAtivo.Contains(ds.Id)))
            {
                diaSemanaEnum.Aplicar = true;
                diaSemanaEnum.Ativo = false;
            }

            _diasSemana.Clear();
            _diasSemana = temp;

            ListaDiaSemana.ItemsSource = _diasSemana;
        }

        #endregion

        #region Validations

        private bool IsValid()
        {
            var listaValidacoes = new List<bool>();

            listaValidacoes.Add(DataValida());
            listaValidacoes.Add(HoraValida());
            listaValidacoes.Add(DataFinalValida());
            listaValidacoes.Add(DiaSemanaValida());
            listaValidacoes.Add(TituloValido());
            listaValidacoes.Add(PrioridadeValida());
            listaValidacoes.Add(NotificacaoTempoValida());

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

        private bool DataFinalValida()
        {
            if (!_definirDiaFinal
            || (_definirDiaFinal && DataFinalTarefa.Date != null && DataFinalTarefa.Date > DataTarefa.Date))
            {
                return true;
            }

            return false;
        }

        private bool DiaSemanaValida()
        {
            if (!_definirDiaFinal
            || (_definirDiaFinal && _diasSemana.ToList().Count(ds => ds.Ativo && ds.Aplicar) >= 1))
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

        private bool NotificacaoTempoValida()
        {
            if (NotificacaoTempo.SelectedItem != null)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}