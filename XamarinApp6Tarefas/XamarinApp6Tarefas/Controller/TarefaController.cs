using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Plugin.LocalNotifications;
using XamarinApp6Tarefas.Enums;
using XamarinApp6Tarefas.Models.Tarefas;

namespace XamarinApp6Tarefas.Controller
{
    public static class TarefaController
    {
        public static List<DataTarefaEntity> DataTarefas { get; set; }
        private static int LastIdNotification { get; set; }

        static TarefaController()
        {
            DataTarefas = new List<DataTarefaEntity>();

            if (App.Current.Properties.ContainsKey("Tarefas"))
            {
                var tarefasJson = (String) App.Current.Properties["Tarefas"];

                DataTarefas = JsonConvert.DeserializeObject<List<DataTarefaEntity>>(tarefasJson);

                DataTarefas.RemoveAll(df => (df.Dia - DateTime.Today).TotalDays < -7);
            }

            if (App.Current.Properties.ContainsKey("TarefasIdNotification"))
            {
                var strJson = (String)App.Current.Properties["TarefasIdNotification"];

                LastIdNotification = JsonConvert.DeserializeObject<int>(strJson);
            }
            else
            {
                LastIdNotification = 0;
            }
        }

        public static void SaveChanges()
        {
            if (App.Current.Properties.ContainsKey("Tarefas"))
            {
                App.Current.Properties.Remove("Tarefas");
            }

            var tarefasJson = JsonConvert.SerializeObject(DataTarefas);

            App.Current.Properties.Add("Tarefas", tarefasJson);
        }

        public static bool Cadastrar(DateTime dia, string titulo, PrioridadeEnum prioridade, TimeSpan hora, string descricao)
        {
            try
            {
                var tarefa = new TarefaEntity(titulo, prioridade, hora, descricao, false, NextIdNotification());
                
                if (DataTarefas.Any(df => df.Dia == dia))
                {
                    DataTarefas.Find(df => df.Dia == dia).AddTarefa(tarefa);
                }
                else
                {
                    var dataTarefa = new DataTarefaEntity(dia, new List<TarefaEntity>());
                    dataTarefa.AddTarefa(tarefa);
                    DataTarefas.Add(dataTarefa);
                }

                CrossLocalNotifications.Current.Show(titulo, descricao, tarefa.IdNotificacao, DataTarefas.Find(df => df.Dia == dia).NotificacaoBaseDateTime(tarefa.Id).AddMinutes(-10));
            }
            catch (Exception e)
            {
                return false;
            }

            SaveChanges();
            return true;
        }

        public static bool Alterar(Guid idDataTarefa, DateTime dia, Guid idTarefa, string titulo, PrioridadeEnum prioridade, TimeSpan hora, string descricao, bool realizado)
        {
            try
            {
                var tarefa = TarefaEntity.TarefaFactory.NewTarefa(idTarefa, titulo, prioridade, hora, descricao, realizado);
                if (DataTarefas.Any(df => df.Dia == dia && df.Id == idDataTarefa))
                {
                    var index = DataTarefas.Find(df => df.Id == idDataTarefa).Tarefas.IndexOf(DataTarefas.Find(df => df.Id == idDataTarefa).Tarefas.Find(t => t.Id == idTarefa));
                    DataTarefas.Find(df => df.Id == idDataTarefa).Tarefas[index] = tarefa;
                }
                else
                {
                    if (DataTarefas.Any(df => df.Dia == dia))
                    {
                        DataTarefas.Find(df => df.Dia == dia).AddTarefa(tarefa);
                    }
                    else
                    {
                        var dataTarefa = new DataTarefaEntity(dia, new List<TarefaEntity>());
                        dataTarefa.AddTarefa(tarefa);
                        DataTarefas.Add(dataTarefa);
                    }

                    DataTarefas.Find(df => df.Id == idDataTarefa).RemoveTarefa(tarefa);

                    if (!DataTarefas.Find(df => df.Id == idDataTarefa).Tarefas.Any())
                    {
                        DataTarefas.Remove(DataTarefas.Find(df => df.Id == idDataTarefa));
                    }
                }

                CrossLocalNotifications.Current.Cancel(tarefa.IdNotificacao);
                CrossLocalNotifications.Current.Show(titulo, descricao, tarefa.IdNotificacao, DataTarefas.Find(df => df.Dia == dia).NotificacaoBaseDateTime(tarefa.Id).AddMinutes(-10));
            }
            catch (Exception e)
            {
                return false;
            }

            SaveChanges();
            return true;
        }

        public static bool Remover(Guid idTarefa)
        {
            try
            {
                var idDataTarefa = DataTarefas.Find(df => df.Tarefas.Any(t => t.Id == idTarefa)).Id;

                CrossLocalNotifications.Current.Cancel(DataTarefas.Find(df => df.Id == idDataTarefa).Tarefas.Find(t => t.Id == idTarefa).IdNotificacao);

                DataTarefas.Find(df => df.Id == idDataTarefa).Tarefas.Remove(DataTarefas.Find(df => df.Id == idDataTarefa)
                    .Tarefas.Find(t => t.Id == idTarefa));
                if (!DataTarefas.Find(df => df.Id == idDataTarefa).Tarefas.Any())
                {
                    DataTarefas.Remove(DataTarefas.Find(df => df.Id == idDataTarefa));
                }
            }
            catch (Exception e)
            {
                return false;
            }

            SaveChanges();
            return true;
        }


        private static int NextIdNotification()
        {
            LastIdNotification++;
            return LastIdNotification;
        }
    }
}
