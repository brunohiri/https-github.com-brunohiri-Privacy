using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Privacy.Data.Entities
{
    public partial class PrivacyContext : DbContext
    {
        public PrivacyContext()
        {
        }

        public PrivacyContext(DbContextOptions<PrivacyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Album> Album { get; set; }
        public virtual DbSet<Amizade> Amizade { get; set; }
        public virtual DbSet<Comentario> Comentario { get; set; }
        public virtual DbSet<Cupom> Cupom { get; set; }
        public virtual DbSet<Curtida> Curtida { get; set; }
        public virtual DbSet<Etnia> Etnia { get; set; }
        public virtual DbSet<Foto> Foto { get; set; }
        public virtual DbSet<Genero> Genero { get; set; }
        public virtual DbSet<Interesse> Interesse { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<StatusTransacao> StatusTransacao { get; set; }
        public virtual DbSet<TipoPost> TipoPost { get; set; }
        public virtual DbSet<TipoTransacao> TipoTransacao { get; set; }
        public virtual DbSet<Transacao> Transacao { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Valor> Valor { get; set; }
        public virtual DbSet<Video> Video { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:webprivacy.database.windows.net,1433;Initial Catalog=Privacy;Persist Security Info=False;User ID=userApp;Password=(#WebPrivacy#2020$);MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=90;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Album>(entity =>
            {
                entity.HasKey(e => e.IdAlbum);

                entity.Property(e => e.DataAlbum).HasColumnType("datetime");

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Localizacao)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Album)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Album");
            });

            modelBuilder.Entity<Amizade>(entity =>
            {
                entity.HasKey(e => e.IdAmizade);

                entity.Property(e => e.DataAprovacao).HasColumnType("datetime");

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.HasOne(d => d.IdUsuarioAprovacaoNavigation)
                    .WithMany(p => p.AmizadeIdUsuarioAprovacaoNavigation)
                    .HasForeignKey(d => d.IdUsuarioAprovacao)
                    .HasConstraintName("Usuario_Amizade");

                entity.HasOne(d => d.IdUsuarioSolicitanteNavigation)
                    .WithMany(p => p.AmizadeIdUsuarioSolicitanteNavigation)
                    .HasForeignKey(d => d.IdUsuarioSolicitante)
                    .HasConstraintName("Usuario_Amizade_Solicitante");
            });

            modelBuilder.Entity<Comentario>(entity =>
            {
                entity.HasKey(e => e.IdComentario);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Texto)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdComentarioReferenciaNavigation)
                    .WithMany(p => p.InverseIdComentarioReferenciaNavigation)
                    .HasForeignKey(d => d.IdComentarioReferencia)
                    .HasConstraintName("Comentario_Comentario");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Comentario)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("FK__Comentari__IdPos__2DE6D218");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Comentario)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Comentario");
            });

            modelBuilder.Entity<Cupom>(entity =>
            {
                entity.HasKey(e => e.IdCupom);

                entity.Property(e => e.Codigo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataValidade).HasColumnType("datetime");
            });

            modelBuilder.Entity<Curtida>(entity =>
            {
                entity.HasKey(e => e.IdCurtida);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Curtida)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("Post_Curtida");
            });

            modelBuilder.Entity<Etnia>(entity =>
            {
                entity.HasKey(e => e.IdEtnia);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Foto>(entity =>
            {
                entity.HasKey(e => e.IdFoto);

                entity.Property(e => e.Caminho)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NomeArquivo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdAlbumNavigation)
                    .WithMany(p => p.Foto)
                    .HasForeignKey(d => d.IdAlbum)
                    .HasConstraintName("Album_Foto");

                entity.HasOne(d => d.IdComentarioNavigation)
                    .WithMany(p => p.Foto)
                    .HasForeignKey(d => d.IdComentario)
                    .HasConstraintName("Comentario_Foto");
            });

            modelBuilder.Entity<Genero>(entity =>
            {
                entity.HasKey(e => e.IdGenero);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Interesse>(entity =>
            {
                entity.HasKey(e => e.IdInteresse);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.IdPost);

                entity.Property(e => e.Data).HasColumnType("datetime");

                entity.Property(e => e.Texto)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdFotoNavigation)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.IdFoto)
                    .HasConstraintName("Foto_Post");

                entity.HasOne(d => d.IdTipoPostNavigation)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.IdTipoPost)
                    .HasConstraintName("TipoPost_Post");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Post");

                entity.HasOne(d => d.IdVideoNavigation)
                    .WithMany(p => p.Post)
                    .HasForeignKey(d => d.IdVideo)
                    .HasConstraintName("Video_Post");
            });

            modelBuilder.Entity<StatusTransacao>(entity =>
            {
                entity.HasKey(e => e.IdStatusTransacao);

                entity.Property(e => e.IdStatusTransacao).ValueGeneratedOnAdd();

                entity.Property(e => e.CssClass)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CssIcon)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoPost>(entity =>
            {
                entity.HasKey(e => e.IdTipoPost);

                entity.Property(e => e.IdTipoPost).ValueGeneratedOnAdd();

                entity.Property(e => e.CssClass)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CssIcon)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TipoTransacao>(entity =>
            {
                entity.HasKey(e => e.IdTipoTransacao);

                entity.Property(e => e.IdTipoTransacao).ValueGeneratedOnAdd();

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(300)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.HasKey(e => e.IdTransacao);

                entity.Property(e => e.DataTransacao).HasColumnType("datetime");

                entity.Property(e => e.Ip)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OrderTimePayPal).HasColumnType("datetime");

                entity.Property(e => e.PayerIdPayPal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentStatusPayPal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PendingReasonPayPal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ReasonCodePayPal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ResultPayPal)
                    .HasMaxLength(8000)
                    .IsUnicode(false);

                entity.Property(e => e.TokenPayPal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TransactionIdPayPal)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdCupomNavigation)
                    .WithMany(p => p.Transacao)
                    .HasForeignKey(d => d.IdCupom)
                    .HasConstraintName("Cupom_Transacao");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Transacao)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("Post_Transacao");

                entity.HasOne(d => d.IdStatusTransacaoNavigation)
                    .WithMany(p => p.Transacao)
                    .HasForeignKey(d => d.IdStatusTransacao)
                    .HasConstraintName("StatusTransacao_Transacao");

                entity.HasOne(d => d.IdTipoTransacaoNavigation)
                    .WithMany(p => p.Transacao)
                    .HasForeignKey(d => d.IdTipoTransacao)
                    .HasConstraintName("TipoTransacao_Transacao");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Transacao)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Transacao");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.Property(e => e.CPF)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cidade)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.DataNascimento).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FotoCapa)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FotoPerfil)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Nome)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Pais)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.QuantoQuer).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Senha)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SobreMim)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdEtniaNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdEtnia)
                    .HasConstraintName("FK__Usuario__IdEtnia__6BE40491");

                entity.HasOne(d => d.IdGeneroNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdGenero)
                    .HasConstraintName("FK__Usuario__IdGener__7A3223E8");

                entity.HasOne(d => d.IdInteresseNavigation)
                    .WithMany(p => p.Usuario)
                    .HasForeignKey(d => d.IdInteresse)
                    .HasConstraintName("FK__Usuario__IdInter__6CD828CA");
            });

            modelBuilder.Entity<Valor>(entity =>
            {
                entity.HasKey(e => e.IdValor);

                entity.Property(e => e.ValorMensal).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ValorSemestral).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ValorTrimestral).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.Valor)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("Usuario_Valor");
            });

            modelBuilder.Entity<Video>(entity =>
            {
                entity.HasKey(e => e.IdVideo);

                entity.Property(e => e.Caminho)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DataCadastro).HasColumnType("datetime");

                entity.Property(e => e.Descricao)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.NomeArquivo)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdAlbumNavigation)
                    .WithMany(p => p.Video)
                    .HasForeignKey(d => d.IdAlbum)
                    .HasConstraintName("Album_Video");
            });
        }
    }
}
