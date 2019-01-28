using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XamarinApp6Tarefas.Enums
{
    public abstract class BaseNotificacaoTempoEnum : IComparable
    {
        public int Id { get; private set; }
        public int Minutos { get; private set; }
        public string Descricao { get; private set; }

        protected BaseNotificacaoTempoEnum()
        { }

        protected BaseNotificacaoTempoEnum(int id, int minutos, string descricao)
        {
            Id = id;
            Minutos = minutos;
            Descricao = descricao;
        }

        public override string ToString() => Descricao;

        public static IEnumerable<T> GetAll<T>() where T : BaseNotificacaoTempoEnum
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as BaseNotificacaoTempoEnum;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Id.CompareTo(((BaseNotificacaoTempoEnum)other).Id);
    }
}
