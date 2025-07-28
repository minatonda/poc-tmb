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
    service.getOrders().then(setOrders);
  }, []);

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
            <tr key={order.id} onClick={() => onSelect(order)} style={{ cursor: "pointer" }}>
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
