using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Foto
    {
        public Foto()
        {
            Post = new HashSet<Post>();
        }

        public long IdFoto { get; set; }
        public long? IdComentario { get; set; }
        public long? IdAlbum { get; set; }
        public string Descricao { get; set; }
        public string Caminho { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Album IdAlbumNavigation { get; set; }
        public virtual Comentario IdComentarioNavigation { get; set; }
        public virtual ICollection<Post> Post { get; set; }
    }
}
