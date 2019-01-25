using System;
using System.Collections.Generic;
using System.Linq;

namespace XamarinApp6Tarefas.Models.Tarefas
{
    public class DataTarefaEntity : Entity<DataTarefaEntity>
    {
        public DateTime Dia { get; private set; }
        public List<TarefaEntity> Tarefas { get; private set; }

        protected DataTarefaEntity()
        {
            Tarefas = new  List<TarefaEntity>();
        }

        public DataTarefaEntity(DateTime dia, List<TarefaEntity> tarefas)
        {
            Id = Guid.NewGuid();
            Dia = dia;
            Tarefas = tarefas;  
        }

        public DateTime NotificacaoBaseDateTime(Guid idTarefa)
        {
            var notificar = Dia;

            notificar.Add(Tarefas.Find(t => t.Id == idTarefa).Hora);

            return notificar;
        }

        public void AddTarefa(TarefaEntity tarefa)
        {
            Tarefas.Add(tarefa);
        }

        public void RemoveTarefa(TarefaEntity tarefa)
        {
            Tarefas.Remove(tarefa);
        }

        public override bool IsValid()
        {
            if (Dia != null 
            && Tarefas.Any()
            && !Tarefas.Any(t => t.IsValid())
            )
            {
                return true;
            }

            return false;
        }

        public static class DataTarefaFactory
        {
            public static DataTarefaEntity NewDataTarefa(Guid id, DateTime dia, List<TarefaEntity> tarefas)
            {
                var dataTarefa = new DataTarefaEntity()
                {
                    Id = id,
                    Dia = dia,
                    Tarefas = tarefas
                };

                return dataTarefa;
            }
        }
    }
}
