export enum OrderStatus {
  Pendente = "Pendente",
  Processando = "Processando",
  Finalizado = "Finalizado",
}

export interface Order {
  id: string;
  cliente: string;
  produto: string;
  valor: number;
  status: OrderStatus;
  dataCriacao: string;
}

export interface OrderCreateRequest {
  cliente: string;
  produto: string;
  valor: number;
}
