using System;
using System.Collections.Generic;
using System.Text;

namespace Privacy.Business.Models
{
    public class ResultPesquisaModel
    {
        public long IdUsuario { get; set; }
        public string IdUsuarioCriptografado { get; set; }
        public string FotoPerfil { get; set; }
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Pais { get; set; }
        public string Genero { get; set; }
        public string Etnia { get; set; }
    }
}
