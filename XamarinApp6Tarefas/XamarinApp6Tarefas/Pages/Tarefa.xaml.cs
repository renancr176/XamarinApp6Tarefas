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
        private PrioridadeEnum _prioridade;
        private bool _definirHorario;

        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        public Tarefa (Guid? idTarefa)
        {
            _idTarefa = idTarefa;

            InitializeComponent ();
            Title = "Nova Tarefa";

            var prioridades = PrioridadeEnum.GetAll<PrioridadeEnum>().ToList();

            ListaPrioridades.ItemsSource = prioridades;

            _definirHorario = false;
            DataTarefa.MinimumDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            BtnAlterar.IsVisible = false;

            if (_idTarefa.HasValue 
            && TarefaController.DataTarefas.Any(dt => dt.Tarefas.Any(t => t.Id == _idTarefa)))
		    {
		        _dataTarefa = TarefaController.DataTarefas.Where(dt => dt.Tarefas.Any(t => t.Id == _idTarefa)).FirstOrDefault();
                Title = "Alterar Tarefa";
                DataTarefa.Date = _dataTarefa.Dia;
                DataTarefa.MinimumDate = _dataTarefa.Dia;
                SwHorario.IsToggled = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Hora.HasValue;
                if(_dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Hora.HasValue)
                { 
                    HoraTarefa.Time = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Hora.Value;
                }
                TituloTarefa.Text = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Titulo;
                ListaPrioridades.SelectedItem = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Prioridade;
                _prioridade = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Prioridade;
                DescricaoTarefa.Text = _dataTarefa.Tarefas.Find(tf => tf.Id == _idTarefa).Descricao;
                BtnCadastrar.IsVisible = false;
                BtnAlterar.IsVisible = true;
            }

            DefinirHorario();
        }

        private void ListaPrioridadesItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            _prioridade = (PrioridadeEnum) args.SelectedItem;
        }

        private void DefinirHorarioSw(object sender, ToggledEventArgs args)
        {
            _definirHorario = args.Value;
            DefinirHorario();
        }

        private void DefinirHorario()
        {
            switch (_definirHorario)
            {
                case true:
                    MainGrid.Children[4].IsVisible = true;
                    MainGrid.Children[5].IsVisible = true;
                break;
                default:
                    MainGrid.Children[4].IsVisible = false;
                    MainGrid.Children[5].IsVisible = false;
                break;
            }
        }

        private void BtnCadastrarClicked(object sender, EventArgs e)
        {
            if (IsValid())
            {
                TimeSpan? hora = null;

                if (_definirHorario)
                {
                    hora = HoraTarefa.Time;
                }

                if (TarefaController.Cadastrar(DataTarefa.Date, TituloTarefa.Text, _prioridade, hora,
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

                if (_definirHorario)
                {
                    hora = HoraTarefa.Time;
                }

                if(TarefaController.Alterar(_dataTarefa.Id, _idTarefa.Value, TituloTarefa.Text, _prioridade, hora, DescricaoTarefa.Text, _dataTarefa.Tarefas.Find(t => t.Id == _idTarefa).Realizado))
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
            if (!_definirHorario
            || (_definirHorario && HoraTarefa.Time != null))
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