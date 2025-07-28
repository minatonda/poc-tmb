import { useState } from "react";
import axios from "axios";
import "bootstrap/dist/css/bootstrap.min.css";
import "./index.css";
import OrderList from "./components/OrderList";
import OrderForm from "./components/OrderForm";
import OrderDetail from "./components/OrderDetail";
import { Order } from "./types/order";
import { OrderApiService } from "./api/order.api-service";

const api = axios.create({ baseURL: "http://localhost:5000" });
const service = new OrderApiService(api);

function App() {
  const [selectedOrder, setSelectedOrder] = useState<Order | null>(null);
  const [refreshKey, setRefreshKey] = useState(0);

  return (
    <div className="container mt-4">
      <h1 className="mb-4 text-black">📦 Gestão de Pedidos</h1>
      <OrderForm service={service} onCreated={() => setRefreshKey(k => k + 1)} />
      <hr />
      <OrderList key={refreshKey} service={service} onSelect={setSelectedOrder} />
      <OrderDetail order={selectedOrder} />
    </div>
  );
}

export default App;
