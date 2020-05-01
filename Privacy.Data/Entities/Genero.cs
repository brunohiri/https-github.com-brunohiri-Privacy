using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Genero
    {
        public Genero()
        {
            Usuario = new HashSet<Usuario>();
        }

        public long IdGenero { get; set; }
        public string Descricao { get; set; }
        public bool? Ativo { get; set; }

        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
