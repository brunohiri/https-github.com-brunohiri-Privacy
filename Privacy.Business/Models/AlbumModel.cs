using Microsoft.EntityFrameworkCore;
using Privacy.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Privacy.Business.Models
{
    public class AlbumModel
    {
        #region Propriedades
        #endregion

        #region Construtores
        #endregion

        #region Métodos
        public static List<Album> ObterAlbumUsuario(long IdUsuario)
        {
            List<Album> Entity = null;
            using (PrivacyContext context = new PrivacyContext())
            {
                Entity = context.Album
                    .Include(y => y.Foto)
                    .Include(y => y.Video)
                    .Where(x => x.IdUsuario == IdUsuario).ToList();
            }
            return Entity;
        }
        #endregion
    }
}
