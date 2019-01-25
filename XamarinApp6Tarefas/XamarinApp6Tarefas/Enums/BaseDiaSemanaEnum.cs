using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XamarinApp6Tarefas.Enums
{
    public abstract class BaseDiaSemanaEnum : IComparable
    {
        public DayOfWeek Id { get; private set; }
        public string Descricao { get; private set; }
        public string Sigla { get; private set; }
        public bool Ativo { get; set; }

        protected BaseDiaSemanaEnum()
        { }

        protected BaseDiaSemanaEnum(DayOfWeek id, string descricao, string sigla)
        {
            Id = id;
            Descricao = descricao;
            Sigla = sigla;
            Ativo = true;
        }

        public override string ToString() => Descricao;

        public static IEnumerable<T> GetAll<T>() where T : BaseDiaSemanaEnum
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as BaseDiaSemanaEnum;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Id.CompareTo(((BaseDiaSemanaEnum)other).Id);
    }
}
