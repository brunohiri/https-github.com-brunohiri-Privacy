using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Amizade
    {
        public long IdAmizade { get; set; }
        public long? IdUsuarioSolicitante { get; set; }
        public long? IdUsuarioAprovacao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Aprovado { get; set; }
        public DateTime? DataAprovacao { get; set; }

        public virtual Usuario IdUsuarioAprovacaoNavigation { get; set; }
        public virtual Usuario IdUsuarioSolicitanteNavigation { get; set; }
    }
}
