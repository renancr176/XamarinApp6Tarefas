using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace XamarinApp6Tarefas.Enums
{
    public abstract class BasePrioridadeEnum : IComparable
    {
        public int Id { get; private set; }
        public string Descricao { get; private set; }
        public Color Cor { get; private set; }

        protected BasePrioridadeEnum()
        { }

        protected BasePrioridadeEnum(int id, string descricao, Color cor)
        {
            Id = id;
            Descricao = descricao;
            Cor = cor;
        }

        public override string ToString() => Descricao;

        public static IEnumerable<T> GetAll<T>() where T : BasePrioridadeEnum
        {
            var fields = typeof(T).GetFields(BindingFlags.Public |
                                             BindingFlags.Static |
                                             BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as BasePrioridadeEnum;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public int CompareTo(object other) => Id.CompareTo(((BasePrioridadeEnum)other).Id);
    }
}
