using System;

namespace XamarinApp6Tarefas.Enums
{
    public class DiaSemanaEnum : BaseDiaSemanaEnum
    {
        public static DiaSemanaEnum Domingo = new DomingoVal();
        public static DiaSemanaEnum Segunda = new SegundaVal();
        public static DiaSemanaEnum Terca = new TercaVal();
        public static DiaSemanaEnum Quarta = new QuartaVal();
        public static DiaSemanaEnum Quinta = new QuintaVal();
        public static DiaSemanaEnum Sexta = new SextaVal();
        public static DiaSemanaEnum Sabado = new SabadoVal();

        public DiaSemanaEnum(DayOfWeek id, string descricao, string sigla)
            : base(id, descricao, sigla)
        {
        }

        private class DomingoVal : DiaSemanaEnum
        {
            public DomingoVal()
                : base(DayOfWeek.Sunday, "Domingo", "D")
            { }
        }

        private class SegundaVal : DiaSemanaEnum
        {
            public SegundaVal()
                : base(DayOfWeek.Monday, "Segunda-Feira", "S")
            { }
        }

        private class TercaVal : DiaSemanaEnum
        {
            public TercaVal()
                : base(DayOfWeek.Tuesday, "Terça-Feira", "T")
            { }
        }

        private class QuartaVal : DiaSemanaEnum
        {
            public QuartaVal()
                : base(DayOfWeek.Wednesday, "Quarta-Feira", "Q")
            { }
        }

        private class QuintaVal : DiaSemanaEnum
        {
            public QuintaVal()
                : base(DayOfWeek.Thursday, "Quinta-Feira", "Q")
            { }
        }

        private class SextaVal : DiaSemanaEnum
        {
            public SextaVal()
                : base(DayOfWeek.Friday, "Sexta-Feira", "S")
            { }
        }

        private class SabadoVal : DiaSemanaEnum
        {
            public SabadoVal()
                : base(DayOfWeek.Saturday, "Sábado", "S")
            { }
        }
    }
}
