using Microsoft.EntityFrameworkCore;
using Privacy.Business.Util;
using Privacy.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Privacy.Business.Models
{
    public class UsuarioModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos

        public static Usuario ObterUsuario(long IdUsuario)
        {
            Usuario Entity = null;
            using (PrivacyContext context = new PrivacyContext())
            {
                Entity = context.Usuario
                    .Include(y => y.Comentario)
                    .Include(y => y.Post)
                        .ThenInclude(z => z.Curtida)
                    .Include(y => y.Post)
                        .ThenInclude(z => z.IdFotoNavigation)
                        .Include(y => y.Post)
                        .ThenInclude(z => z.IdVideoNavigation)
                        .Include(v => v.Valor)

                    .Where(x => x.IdUsuario == IdUsuario).FirstOrDefault();
            }
            return Entity;
        }

        public static List<Usuario> ObterSugestoes()
        {
            List<Usuario> ListEntity = null;
            using (PrivacyContext context = new PrivacyContext())
            {
                ListEntity = context.Usuario
                    .Include(y => y.IdEtniaNavigation)
                    .Include(y => y.IdGeneroNavigation)
                    .Where(x => x.Ativo && x.PerfilPublico).ToList();
            }
            return ListEntity;
        }

        public static List<ResultPesquisaModel> ObterPesquisa(string texto)
        {
            List<ResultPesquisaModel> ListEntity = null;
            using (PrivacyContext context = new PrivacyContext())
            {
                ListEntity = context.Usuario
                    .Include(y => y.IdEtniaNavigation)
                    .Include(y => y.IdGeneroNavigation)
                    .Where(x => /*x.Ativo &&*/ x.PerfilPublico
                    &&(
                    x.Nome.ToLower().Contains(texto.ToLower())
                    || x.IdEtniaNavigation.Descricao.ToLower().Contains(texto.ToLower())
                    || x.IdGeneroNavigation.Descricao.ToLower().Contains(texto.ToLower())
                    || x.Cidade.ToLower().Contains(texto.ToLower())
                    || x.Pais.ToLower().Contains(texto.ToLower())
                    || x.Estado.ToLower().Contains(texto.ToLower())
                    )).ToList()
                    .Select(k => new ResultPesquisaModel {
                        IdUsuario = k.IdUsuario,
                        IdUsuarioCriptografado = WebUtility.UrlDecode(Criptography.Encrypt(k.IdUsuario.ToString())),
                        Nome = k.Nome,
                        Cidade = k.Cidade,
                        Pais = k.Pais,
                        FotoPerfil = (k.FotoPerfil.IsNullOrEmpty() ? "/FotoPerfil/default.jpg" : k.FotoPerfil),
                        Etnia = k.IdEtniaNavigation.Descricao,
                        Genero = k.IdGeneroNavigation.Descricao
                    }).ToList();
            }
            return ListEntity;
        }
        #endregion
    }
}
