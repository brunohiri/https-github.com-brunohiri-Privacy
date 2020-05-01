using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class TipoTransacao
    {
        public TipoTransacao()
        {
            Transacao = new HashSet<Transacao>();
        }

        public byte IdTipoTransacao { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Transacao> Transacao { get; set; }
    }
}
