using System.Collections.Generic;
using System.Drawing;

namespace XamarinApp6Tarefas.Enums
{
    public class PrioridadeEnum : Enumeration
    {
        public static PrioridadeEnum UrgenteImportante = new UrgenteImportanteVal();
        public static PrioridadeEnum UrgenteNaoImportante = new UrgenteNaoImportanteVal();
        public static PrioridadeEnum NaoUrgenteImportante = new NaoUrgenteImportanteVal();
        public static PrioridadeEnum NaoUrgenteNaoImportante = new NaoUrgenteNaoImportanteVal();

        public PrioridadeEnum(int id, string descricao, Color cor)
            : base(id, descricao, cor)
        {
        }

        private class UrgenteImportanteVal : PrioridadeEnum
        {
            public UrgenteImportanteVal() 
                : base(1, "Urgente e importante", Color.Red)
            { }
        }

        private class UrgenteNaoImportanteVal : PrioridadeEnum
        {
            public UrgenteNaoImportanteVal()
                : base(2, "Urgente mas não importante", Color.OrangeRed)
            { }
        }

        private class NaoUrgenteImportanteVal : PrioridadeEnum
        {
            public NaoUrgenteImportanteVal()
                : base(3, "Não urgente mas importante", Color.Yellow)
            { }
        }

        private class NaoUrgenteNaoImportanteVal : PrioridadeEnum
        {
            public NaoUrgenteNaoImportanteVal()
                : base(4, "Não urgente e não importante", Color.Blue)
            { }
        }
    }
}
