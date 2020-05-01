using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Usuario
    {
        public Usuario()
        {
            Album = new HashSet<Album>();
            AmizadeIdUsuarioAprovacaoNavigation = new HashSet<Amizade>();
            AmizadeIdUsuarioSolicitanteNavigation = new HashSet<Amizade>();
            Comentario = new HashSet<Comentario>();
            Post = new HashSet<Post>();
            Transacao = new HashSet<Transacao>();
            Valor = new HashSet<Valor>();
        }

        public long IdUsuario { get; set; }
        public long? IdEtnia { get; set; }
        public long? IdGenero { get; set; }
        public long? IdInteresse { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string CPF { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string FotoPerfil { get; set; }
        public string FotoCapa { get; set; }
        public bool PerfilPublico { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
        public decimal? QuantoQuer { get; set; }
        public string SobreMim { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Etnia IdEtniaNavigation { get; set; }
        public virtual Genero IdGeneroNavigation { get; set; }
        public virtual Interesse IdInteresseNavigation { get; set; }
        public virtual ICollection<Album> Album { get; set; }
        public virtual ICollection<Amizade> AmizadeIdUsuarioAprovacaoNavigation { get; set; }
        public virtual ICollection<Amizade> AmizadeIdUsuarioSolicitanteNavigation { get; set; }
        public virtual ICollection<Comentario> Comentario { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Transacao> Transacao { get; set; }
        public virtual ICollection<Valor> Valor { get; set; }
    }
}
