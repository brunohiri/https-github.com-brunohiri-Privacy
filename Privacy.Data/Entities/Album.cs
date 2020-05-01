using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Album
    {
        public Album()
        {
            Foto = new HashSet<Foto>();
            Video = new HashSet<Video>();
        }

        public long IdAlbum { get; set; }
        public long? IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Localizacao { get; set; }
        public DateTime DataAlbum { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Foto> Foto { get; set; }
        public virtual ICollection<Video> Video { get; set; }
    }
}
