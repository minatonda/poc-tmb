import { useEffect, useState } from "react";
import { Order } from "../types/order";
import { OrderApiService } from "../api/order.api-service";

interface Props {
  service: OrderApiService;
  onSelect: (order: Order) => void;
}

export default function OrderList({ service, onSelect }: Props) {
  const [orders, setOrders] = useState<Order[]>([]);

  useEffect(() => {
    // Busca inicial das ordens
    service.getOrders().then(setOrders);

    // Callback para atualizar a ordem específica na lista quando receber notificação
    const handleOrderUpdate = (updatedOrder: Order) => {
      setOrders((currentOrders) => {
        const index = currentOrders.findIndex(o => o.id === updatedOrder.id);
        if (index === -1) {
          // Se a ordem não existir, adiciona no topo
          return [updatedOrder, ...currentOrders];
        } else {
          // Se existir, atualiza o item na lista
          const newOrders = [...currentOrders];
          newOrders[index] = updatedOrder;
          return newOrders;
        }
      });
    };

    // Assina o evento no SignalR
    service.onOrderStatusChanged(handleOrderUpdate);

    // Cleanup: remove o listener na desmontagem
    return () => {
      service.offOrderStatusChanged();
    };
  }, [service]);

  return (
    <div className="table-responsive">
      <table className="table table-bordered table-striped">
        <thead className="table-dark">
          <tr>
            <th>Cliente</th>
            <th>Produto</th>
            <th>Valor</th>
            <th>Status</th>
            <th>Data</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr
              key={order.id}
              onClick={() => onSelect(order)}
              style={{ cursor: "pointer" }}
            >
              <td>{order.cliente}</td>
              <td>{order.produto}</td>
              <td>R$ {order.valor.toFixed(2)}</td>
              <td>{order.status}</td>
              <td>{new Date(order.dataCriacao).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
