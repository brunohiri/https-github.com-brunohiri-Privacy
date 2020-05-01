using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Curtida
    {
        public long IdCurtida { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public long? IdPost { get; set; }
        public long? IdUsuario { get; set; }

        public virtual Post IdPostNavigation { get; set; }
    }
}
