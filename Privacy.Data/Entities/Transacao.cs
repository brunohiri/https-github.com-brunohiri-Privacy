using System;
using System.Collections.Generic;

namespace Privacy.Data.Entities
{
    public partial class Transacao
    {
        public long IdTransacao { get; set; }
        public byte? IdStatusTransacao { get; set; }
        public byte? IdTipoTransacao { get; set; }
        public long? IdUsuario { get; set; }
        public long? IdCupom { get; set; }
        public long? IdPost { get; set; }
        public long? IdPerfil { get; set; }
        public decimal? Valor { get; set; }
        public DateTime DataTransacao { get; set; }
        public string Ip { get; set; }
        public string TokenPayPal { get; set; }
        public string TransactionIdPayPal { get; set; }
        public string PaymentStatusPayPal { get; set; }
        public string PendingReasonPayPal { get; set; }
        public string ReasonCodePayPal { get; set; }
        public string PayerIdPayPal { get; set; }
        public DateTime? OrderTimePayPal { get; set; }
        public string ResultPayPal { get; set; }

        public virtual Cupom IdCupomNavigation { get; set; }
        public virtual Post IdPostNavigation { get; set; }
        public virtual StatusTransacao IdStatusTransacaoNavigation { get; set; }
        public virtual TipoTransacao IdTipoTransacaoNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
