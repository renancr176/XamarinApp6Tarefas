namespace XamarinApp6Tarefas.Enums
{
    public class NotificacaoTempoEnum : BaseNotificacaoTempoEnum
    {
        public static NotificacaoTempoEnum NaHora = new NaHoraVal();
        public static NotificacaoTempoEnum CincoMinutos = new CincoMinutosVal();
        public static NotificacaoTempoEnum DezMinutos = new DezMinutosVal();
        public static NotificacaoTempoEnum QuizeMinutos = new QuizeMinutosVal();
        public static NotificacaoTempoEnum VinteMinutos = new VinteMinutosVal();
        public static NotificacaoTempoEnum VinteCincoMinutos = new VinteCincoMinutosVal();
        public static NotificacaoTempoEnum TrintaMinutos = new TrintaMinutosVal();
        public static NotificacaoTempoEnum QuarentaCincoMinutos = new QuarentaCincoMinutosVal();
        public static NotificacaoTempoEnum UmaHora = new UmaHoraVal();
        public static NotificacaoTempoEnum UmaHoraMeia = new UmaHoraMeiaVal();
        public static NotificacaoTempoEnum DuasHoras = new DuasHorasVal();
        public static NotificacaoTempoEnum DuasHorasMeia = new DuasHorasMeiaVal();
        public static NotificacaoTempoEnum TresHoras = new TresHorasVal();

        public NotificacaoTempoEnum(int id, int minutos, string descricao)
            : base(id, minutos, descricao)
        {
        }

        private class NaHoraVal : NotificacaoTempoEnum
        {
            public NaHoraVal()
                : base(1, 0, "Na hora")
            { }
        }

        private class CincoMinutosVal : NotificacaoTempoEnum
        {
            public CincoMinutosVal()
                : base(2, -5, "5 minutos antes")
            { }
        }

        private class DezMinutosVal : NotificacaoTempoEnum
        {
            public DezMinutosVal()
                : base(3, -10, "10 minutos antes")
            { }
        }

        private class QuizeMinutosVal : NotificacaoTempoEnum
        {
            public QuizeMinutosVal()
                : base(4, -15, "15 minutos antes")
            { }
        }

        private class VinteMinutosVal : NotificacaoTempoEnum
        {
            public VinteMinutosVal()
                : base(5, -20, "20 minutos antes")
            { }
        }

        private class VinteCincoMinutosVal : NotificacaoTempoEnum
        {
            public VinteCincoMinutosVal()
                : base(6, -25, "25 minutos antes")
            { }
        }

        private class TrintaMinutosVal : NotificacaoTempoEnum
        {
            public TrintaMinutosVal()
                : base(7, -30, "30 minutos antes")
            { }
        }

        private class QuarentaCincoMinutosVal : NotificacaoTempoEnum
        {
            public QuarentaCincoMinutosVal()
                : base(8, -45, "45 minutos antes")
            { }
        }

        private class UmaHoraVal : NotificacaoTempoEnum
        {
            public UmaHoraVal()
                : base(9, -60, "1 hora antes")
            { }
        }

        private class UmaHoraMeiaVal : NotificacaoTempoEnum
        {
            public UmaHoraMeiaVal()
                : base(9, -90, "1 hora e 30 minutos antes")
            { }
        }

        private class DuasHorasVal : NotificacaoTempoEnum
        {
            public DuasHorasVal()
                : base(10, -120, "2 horas antes")
            { }
        }

        private class DuasHorasMeiaVal : NotificacaoTempoEnum
        {
            public DuasHorasMeiaVal()
                : base(11, -150, "2 horas e 30 minutos antes")
            { }
        }

        private class TresHorasVal : NotificacaoTempoEnum
        {
            public TresHorasVal()
                : base(12, -180, "3 horas antes")
            { }
        }
    }
}
