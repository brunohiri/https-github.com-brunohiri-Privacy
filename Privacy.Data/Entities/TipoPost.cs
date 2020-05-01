using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class TipoPost
    {
        public TipoPost()
        {
            Post = new HashSet<Post>();
        }

        public byte IdTipoPost { get; set; }
        public string Descricao { get; set; }
        public string CssIcon { get; set; }
        public string CssClass { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<Post> Post { get; set; }
    }
}
