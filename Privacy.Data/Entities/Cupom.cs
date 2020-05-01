using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Cupom
    {
        public Cupom()
        {
            Transacao = new HashSet<Transacao>();
        }

        public long IdCupom { get; set; }
        public string Codigo { get; set; }
        public int? Percentual { get; set; }
        public int? QuantidadeMaxima { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataValidade { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Transacao> Transacao { get; set; }
    }
}
