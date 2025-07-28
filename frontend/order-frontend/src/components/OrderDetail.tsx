import { Order } from "../types/order";

interface Props {
  order: Order | null;
}

export default function OrderDetail({ order }: Props) {
  if (!order) return <div>Selecione um pedido para ver os detalhes</div>;

  return (
    <div className="card mt-3">
      <div className="card-body">
        <h5 className="card-title">Detalhes do Pedido</h5>
        <p><strong>ID:</strong> {order.id}</p>
        <p><strong>Cliente:</strong> {order.cliente}</p>
        <p><strong>Produto:</strong> {order.produto}</p>
        <p><strong>Valor:</strong> R$ {order.valor.toFixed(2)}</p>
        <p><strong>Status:</strong> {order.status}</p>
        <p><strong>Data de Criação:</strong> {new Date(order.dataCriacao).toLocaleString()}</p>
      </div>
    </div>
  );
}
