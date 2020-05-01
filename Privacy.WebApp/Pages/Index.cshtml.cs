using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Privacy.Business.Models;
using Privacy.Business.Util;
using Privacy.Data.Entities;
using Privacy.WebApp.Models;

namespace Privacy.Pages
{
    public class IndexModel : PageModel
    {
        #region Propriedades

        [BindProperty]
        public Usuario UsuarioLogado { get; set; }

        [BindProperty]
        public List<Usuario> ListSugestoes { get; set; }

        [BindProperty]
        public List<Post> ListPost { get; set; }

        [BindProperty]
        public Post Postagem { get; set; }

        [BindProperty]
        public string TextoMural { get; set; }

        [BindProperty]
        public string Upload { get; set; }

        [BindProperty]
        public string[] Fotos { get; set; }

        public Foto Foto { get; set; }
        public Video Video { get; set; }

        //public Curtida Curtida { get; set; }
        //public Comentario Comentario { get; set; }

        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        #endregion

        #region Construtores

        public IndexModel(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        #endregion


        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            UsuarioLogado = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

            ListPost = PostModel.ObterPosts();


            ListSugestoes = UsuarioModel.ObterSugestoes();

            return Page();
        }



        public IActionResult OnPostMural()
        {
            int salvou = 0;
            long idFoto = 0;
            long idVideo = 0;

            string fileName = string.Empty;
            string filePath = string.Empty;
            try
            {
                var data = Request.Form;

                if (data.Files.Count > 0)
                {
                    int i = 1;
                    foreach (var formFile in data.Files)
                    {
                        if (formFile.ContentType.Contains("video"))
                            filePath = _environment.WebRootPath + _configuration["CaminhoVideoPostagem"].ToString();

                        else
                            filePath = _environment.WebRootPath + _configuration["CaminhoFotoPostagem"].ToString();

                        if (formFile.Length > 0)
                        {
                            if (formFile.ContentType.Contains("video"))
                            {
                                fileName = string.Format("Video_Postagem_{0:dd_MM_yyyy_HH_mm_ss}{1}.{2}", DateTime.Now, i, formFile.FileName.Split('.')[1]);

                                using (var stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                                {
                                    formFile.CopyTo(stream);

                                    Video = new Video();

                                    Video.DataCadastro = DateTime.Now;
                                    Video.NomeArquivo = fileName;
                                    Video.Caminho = ".." + _configuration["CaminhoVideoPostagem"].ToString().Replace("\\", "/");
                                    Video.Descricao = TextoMural;
                                    Video.Ativo = true;

                                    using (PrivacyContext context = new PrivacyContext())
                                    {
                                        context.Video.Add(Video);

                                        salvou = context.SaveChanges();

                                        if (salvou == 1)
                                            idVideo = Video.IdVideo;
                                    }
                                }
                            }
                            else
                            {
                                fileName = string.Format("Foto_Postagem_{0:dd_MM_yyyy_HH_mm_ss}{1}.{2}", DateTime.Now, i, formFile.FileName.Split('.')[1]);

                                using (var stream = new FileStream(Path.Combine(filePath, fileName), FileMode.Create))
                                {
                                    formFile.CopyTo(stream);

                                    Foto = new Foto();

                                    Foto.DataCadastro = DateTime.Now;
                                    Foto.NomeArquivo = fileName;
                                    Foto.Caminho = ".." + _configuration["CaminhoFotoPostagem"].ToString().Replace("\\", "/");
                                    Foto.Descricao = TextoMural;
                                    Foto.Ativo = true;

                                    using (PrivacyContext context = new PrivacyContext())
                                    {
                                        context.Foto.Add(Foto);

                                        salvou = context.SaveChanges();

                                        if (salvou == 1)
                                            idFoto = Foto.IdFoto;
                                    }
                                }
                            }
                        }
                    }

                    if (idFoto > 0)
                    {
                        Postagem.Texto = TextoMural;
                        Postagem.IdUsuario = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO").IdUsuario;
                        Postagem.Data = DateTime.Now;
                        Postagem.Ativo = true;
                        Postagem.IdFoto = idFoto;
                    }
                    else
                    {
                        Postagem.Texto = TextoMural;
                        Postagem.IdUsuario = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO").IdUsuario;
                        Postagem.Data = DateTime.Now;
                        Postagem.Ativo = true;
                        Postagem.IdVideo = idVideo;
                    }

                    salvou = PostModel.Postar(Postagem);

                    if (salvou == 1)
                        return RedirectToAction("OnGet");
                }
                else if (!string.IsNullOrEmpty(TextoMural))
                {
                    Postagem.Texto = TextoMural;
                    Postagem.IdUsuario = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO").IdUsuario;
                    Postagem.Data = DateTime.Now;
                    Postagem.Ativo = true;
                }
            }
            catch (Exception e)
            {

                throw;
            }
            finally
            {
                ListPost = PostModel.ObterPosts();

                ListSugestoes = UsuarioModel.ObterSugestoes();
            }

            return RedirectToAction("OnGet");
        }


        public IActionResult OnGetLike(long IdUsuario, long IdPost)
        {
            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            if (IdPost > 0)
            {
                int likes = 0;

                using (PrivacyContext context = new PrivacyContext())
                {
                    bool curtido = PostModel.IsCurtido(IdUsuario, IdPost);

                    if (curtido)
                    {
                        likes = PostModel.GetLikesDestePost(IdPost);
                        return new JsonResult(new { OK = false, Likes = likes }, new Newtonsoft.Json.JsonSerializerSettings() { });
                    }
                    else
                    {
                        bool curtiu = PostModel.Curtir(IdUsuario, IdPost);

                        if (curtiu)
                        {
                            likes = PostModel.GetLikesDestePost(IdPost);
                            return new JsonResult(new { OK = true, Likes = likes }, new Newtonsoft.Json.JsonSerializerSettings() { });
                        }
                    }


                    return new JsonResult(new { OK = false, Likes = likes }, new Newtonsoft.Json.JsonSerializerSettings() { });
                }
            }

            return null;

        }

        public IActionResult OnGetComentar(long IdUsuario, long IdPost, string Texto, long IdComentario)
        {
            if (string.IsNullOrEmpty(Texto))
                return new JsonResult(new { OK = false }, new Newtonsoft.Json.JsonSerializerSettings() { });

            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            if (IdPost > 0)
            {
                using (PrivacyContext context = new PrivacyContext())
                {
                    bool comentou = PostModel.Comentar(IdUsuario, IdPost, Texto, IdComentario);

                    if (comentou)
                        return new JsonResult(new { OK = true }, new Newtonsoft.Json.JsonSerializerSettings() { });

                    else
                        return new JsonResult(new { OK = false }, new Newtonsoft.Json.JsonSerializerSettings() { });
                }
            }

            return null;

        }

        public IActionResult OnGetOcultarMostrarComentario(long IdComentario)
        {
            if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                return RedirectToPage("/Login");

            using (PrivacyContext context = new PrivacyContext())
            {
                bool ocultou = PostModel.OcultarMostrarComentario(IdComentario);

                if (ocultou)
                    return new JsonResult(new { OK = true }, new Newtonsoft.Json.JsonSerializerSettings() { });

                else
                    return new JsonResult(new { OK = false }, new Newtonsoft.Json.JsonSerializerSettings() { });
            }
        }

        public IActionResult OnPostCriarAlbum(string[] HiddenFotosAlbum, string[] DescricaoFotoAlbum, string NomeAlbum, string DescricaoAlbum, string LocalizacaoAlbum, string DataAlbum)
        {
            try
            {
                if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
                    return RedirectToPage("/Login");

                UsuarioLogado = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

                string filePath = _configuration["CaminhoFotoPostagem"].ToString();
                if (HiddenFotosAlbum != null)
                {
                    long IdAlbum = 0;
                    using (PrivacyContext context = new PrivacyContext())
                    {
                        context.Album.Add(new Album()
                        {
                            IdUsuario = UsuarioLogado.IdUsuario,
                            Nome = NomeAlbum,
                            Descricao = DescricaoAlbum,
                            Localizacao = LocalizacaoAlbum,
                            DataAlbum = DataAlbum.ExtractDateTime(),
                            DataCadastro = DateTime.Now,
                            Ativo = true
                        });
                        context.SaveChanges();
                        IdAlbum = context.Album.Max(x => x.IdAlbum);
                    }

                    if (IdAlbum > 0)
                    {
                        int i = 0;
                        foreach (var ItemFotoVideo in HiddenFotosAlbum)
                        {
                            byte[] bytes = Convert.FromBase64String(HttpUtility.UrlDecode(ItemFotoVideo).Split(',')[1]);
                            string fileName = string.Format("Foto_{0:dd_MM_yyyy_HH_mm_ss}.{1}", DateTime.Now, HttpUtility.UrlDecode(ItemFotoVideo).Contains("image/png") ? "png" : "jpg");

                            if (bytes != null)
                            {
                                System.IO.File.WriteAllBytes(Path.Combine(_environment.WebRootPath + filePath, fileName), bytes);

                                using (PrivacyContext context = new PrivacyContext())
                                {

                                    context.Foto.Add(new Foto()
                                    {
                                        IdAlbum = IdAlbum,
                                        Descricao = DescricaoFotoAlbum[i],
                                        Caminho = filePath,
                                        NomeArquivo = fileName,
                                        DataCadastro = DateTime.Now,
                                        Ativo = true
                                    });

                                    context.SaveChanges();
                                }
                            }
                            i++;
                        }
                    }
                }
                return Page();
            }
            catch (Exception e)
            {

                throw;
            }
            finally
            {
                ListPost = PostModel.ObterPosts();

                ListSugestoes = UsuarioModel.ObterSugestoes();
            }
        }

    }
}
