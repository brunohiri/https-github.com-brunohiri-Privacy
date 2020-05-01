using Microsoft.EntityFrameworkCore;
using Privacy.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Privacy.Business.Models
{
    public class PostModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos

        #region Método para obter a listagem de posts
        public static List<Post> ObterPosts()
        {
            List<Post> ListEntity = null;
            using (PrivacyContext context = new PrivacyContext())
            {
                ListEntity = context.Post
                    .Include(y => y.IdUsuarioNavigation)
                    .Include(y => y.IdVideoNavigation)
                    .Include(y => y.IdFotoNavigation)
                    .Include(y => y.Curtida)
                    .Include(y => y.Comentario)
                        .ThenInclude(z => z.IdUsuarioNavigation)
                    .Where(x => x.Ativo).ToList().OrderByDescending(k => k.Data).ToList();
            }
            return ListEntity;
        }
        #endregion

        #region Método para verificar se este usuário já curtiu este post
        public static bool IsCurtido(long IdUsuario, long IdPost)
        {
            bool curtiu = false;
            
            using (PrivacyContext context = new PrivacyContext())
            {
                var curtida = context.Curtida.Where(c => c.IdPost == IdPost && c.IdUsuario == IdUsuario).FirstOrDefault();

                if (curtida != null)
                {
                    context.Curtida.Remove(curtida);

                    int salvou = context.SaveChanges();

                    if (salvou == 1)
                        curtiu = true;
                }

                return curtiu;
            }
        }

        #endregion

        #region Método para obter a quantidade de likes deste post
        public static int GetLikesDestePost(long IdPost)
        {
            using (PrivacyContext context = new PrivacyContext())
            {
                int likes = context.Curtida.Where(c => c.IdPost == IdPost).Count();

                return likes;
            }
        }
        #endregion

        #region Método para postar
        public static int Postar(Post Postagem)
        {
            int salvou = 0;

            using (PrivacyContext context = new PrivacyContext())
            {
                var post = context.Post.Add(Postagem);

                salvou = context.SaveChanges();

                return salvou;
            }
        }
        #endregion

        #region Método para curtir
        public static bool Curtir(long IdUsuario, long IdPost)
        {
            bool curtiu = false;
            Curtida curtida = new Curtida();
            using (PrivacyContext context = new PrivacyContext())
            {

                curtida.IdPost = IdPost;
                curtida.IdUsuario = IdUsuario;
                curtida.DataCadastro = DateTime.Now;
                curtida.Ativo = true;

                context.Curtida.Add(curtida);

                int salvou = context.SaveChanges();

                if (salvou == 1)
                    curtiu = true;

                return curtiu;
            }

        }
        #endregion

        #region Método para comentar
        public static bool Comentar(long IdUsuario, long IdPost, string texto, long IdComentario)
        {
            bool comentou = false;
            int salvou = 0;

            using (PrivacyContext context = new PrivacyContext())
            {
                if (IdComentario > 0)
                {
                    var objetoComentario = context.Comentario.Where(c => c.IdComentario == IdComentario).FirstOrDefault();

                    if (objetoComentario != null)
                    {
                        objetoComentario.Texto = texto;
                        objetoComentario.DataCadastro = DateTime.Now;
                        objetoComentario.Ativo = true;

                        context.Update(objetoComentario);

                        salvou = context.SaveChanges();

                        if (salvou == 1)
                            comentou = true;

                        return comentou;
                    }
                }
                
                Comentario comentario = new Comentario();

                comentario.IdUsuario = IdUsuario;
                comentario.IdPost = IdPost;
                comentario.DataCadastro = DateTime.Now;
                comentario.Texto = texto;
                comentario.Ativo = true;

                context.Add(comentario);

                salvou = context.SaveChanges();

                if (salvou == 1)
                    comentou = true;

                return comentou;
            }
        }
        #endregion

        #region Método para ocultar/mostrar o comentário
        public static bool OcultarMostrarComentario(long IdComentario)
        {
            bool ocultou = false;

            using (PrivacyContext context = new PrivacyContext())
            {
               var objeto = context.Comentario.Where(c => c.IdComentario == IdComentario).FirstOrDefault();

                if (objeto != null)
                {
                    if (objeto.Ativo)
                        objeto.Ativo = false;
                    else
                        objeto.Ativo = true;

                    context.Update(objeto);

                    int salvou = context.SaveChanges();

                    if (salvou == 1)
                        ocultou = true;
                }

                return ocultou;
            }
        }
        #endregion

        #endregion
    }
}
