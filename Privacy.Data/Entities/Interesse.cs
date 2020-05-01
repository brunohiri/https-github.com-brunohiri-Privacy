using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Interesse
    {
        public Interesse()
        {
            Usuario = new HashSet<Usuario>();
        }

        public long IdInteresse { get; set; }
        public string Descricao { get; set; }
        public bool? Ativo { get; set; }

        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
