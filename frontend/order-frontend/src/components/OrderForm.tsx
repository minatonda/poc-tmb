import { useState } from "react";
import { OrderCreateRequest } from "../types/order";
import { OrderApiService } from "../api/order.api-service";

interface Props {
  service: OrderApiService;
  onCreated: () => void;
}

export default function OrderForm({ service, onCreated }: Props) {
  const [form, setForm] = useState<OrderCreateRequest>({
    cliente: "",
    produto: "",
    valor: 0,
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await service.createOrder(form);
    setForm({ cliente: "", produto: "", valor: 0 });
    onCreated();
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="mb-2">
        <label>Cliente</label>
        <input name="cliente" value={form.cliente} onChange={handleChange} className="form-control" required />
      </div>
      <div className="mb-2">
        <label>Produto</label>
        <input name="produto" value={form.produto} onChange={handleChange} className="form-control" required />
      </div>
      <div className="mb-2">
        <label>Valor</label>
        <input type="number" name="valor" value={form.valor} onChange={handleChange} className="form-control" required />
      </div>
      <button className="btn btn-dark" type="submit">Criar Pedido</button>
    </form>
  );
}
