﻿using System;
using XamarinApp6Tarefas.Enums;

namespace XamarinApp6Tarefas.Models.Tarefas
{
    public class TarefaEntity 
        : Entity<TarefaEntity>, IComparable
    {
        public string Titulo { get; protected set; }
        public PrioridadeEnum Prioridade { get; protected set; }
        public TimeSpan Hora { get; protected set; }
        public string Descricao { get; protected set; }
        public bool Realizado { get; protected set; }
        public NotificacaoTempoEnum NotificacaoTempo { get; protected set; }
        public int IdNotificacao { get; protected set; }

        protected TarefaEntity()
        {
        }

        public TarefaEntity(string titulo, PrioridadeEnum prioridade, TimeSpan hora, string descricao, bool realizado, NotificacaoTempoEnum notificacaoTempo, int idNotificacao)
        {
            Id = Guid.NewGuid();
            Titulo = titulo;
            Prioridade = prioridade;
            Hora = hora;
            Descricao = descricao;
            Realizado = realizado;
            NotificacaoTempo = notificacaoTempo;
            IdNotificacao = idNotificacao;
        }

        public override bool IsValid()
        {
            if (Titulo != null && Prioridade != null)
            {
                return true;
            }

            return false;
        }

        public static class TarefaFactory
        {
            public static TarefaEntity NewTarefa(Guid id, string titulo, PrioridadeEnum prioridade, TimeSpan hora, string descricao, bool realizado, NotificacaoTempoEnum notificacaoTempo, int idNotificacao)
            {
                var tarefa = new TarefaEntity()
                {
                    Id = id,
                    Titulo = titulo,
                    Prioridade = prioridade,
                    Hora = hora,
                    Descricao = descricao,
                    Realizado = realizado,
                    NotificacaoTempo = notificacaoTempo,
                    IdNotificacao = idNotificacao
            };

                return tarefa;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            var tarefa = (TarefaEntity) obj;
            return Id.CompareTo(tarefa.Id);
        }
    }
}
