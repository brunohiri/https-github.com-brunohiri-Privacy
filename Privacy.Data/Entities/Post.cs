using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Post
    {
        public Post()
        {
            Comentario = new HashSet<Comentario>();
            Curtida = new HashSet<Curtida>();
            Transacao = new HashSet<Transacao>();
        }

        public long IdPost { get; set; }
        public byte? IdTipoPost { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdVideo { get; set; }
        public long? IdFoto { get; set; }
        public string Texto { get; set; }
        public DateTime Data { get; set; }
        public bool Ativo { get; set; }

        public virtual Foto IdFotoNavigation { get; set; }
        public virtual TipoPost IdTipoPostNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual Video IdVideoNavigation { get; set; }
        public virtual ICollection<Comentario> Comentario { get; set; }
        public virtual ICollection<Curtida> Curtida { get; set; }
        public virtual ICollection<Transacao> Transacao { get; set; }
    }
}
