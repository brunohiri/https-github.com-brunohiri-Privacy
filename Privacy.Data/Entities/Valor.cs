using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Valor
    {
        public long IdValor { get; set; }
        public long? IdUsuario { get; set; }
        public decimal? ValorMensal { get; set; }
        public decimal? ValorTrimestral { get; set; }
        public decimal? ValorSemestral { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
