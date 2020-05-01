using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Video
    {
        public Video()
        {
            Post = new HashSet<Post>();
        }

        public long IdVideo { get; set; }
        public long? IdAlbum { get; set; }
        public string Descricao { get; set; }
        public string Caminho { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Album IdAlbumNavigation { get; set; }
        public virtual ICollection<Post> Post { get; set; }
    }
}
