using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Comentario
    {
        public Comentario()
        {
            Foto = new HashSet<Foto>();
            InverseIdComentarioReferenciaNavigation = new HashSet<Comentario>();
        }

        public long IdComentario { get; set; }
        public long? IdComentarioReferencia { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdPost { get; set; }
        public string Texto { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Comentario IdComentarioReferenciaNavigation { get; set; }
        public virtual Post IdPostNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Foto> Foto { get; set; }
        public virtual ICollection<Comentario> InverseIdComentarioReferenciaNavigation { get; set; }
    }
}
