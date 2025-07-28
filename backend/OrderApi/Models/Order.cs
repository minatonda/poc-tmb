using System;

namespace OrderApi.Models
{
    public enum OrderStatus
    {
        Pendente,
        Processando,
        Finalizado
    }

    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Cliente { get; set; }
        public string Produto { get; set; }
        public decimal Valor { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pendente;
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    }
}
