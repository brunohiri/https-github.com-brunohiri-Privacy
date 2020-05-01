using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Etnia
    {
        public Etnia()
        {
            Usuario = new HashSet<Usuario>();
        }

        public long IdEtnia { get; set; }
        public string Descricao { get; set; }
        public bool? Ativo { get; set; }

        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
