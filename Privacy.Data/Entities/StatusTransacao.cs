using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class StatusTransacao
    {
        public StatusTransacao()
        {
            Transacao = new HashSet<Transacao>();
        }

        public byte IdStatusTransacao { get; set; }
        public string Descricao { get; set; }
        public string CssClass { get; set; }
        public string CssIcon { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Transacao> Transacao { get; set; }
    }
}
