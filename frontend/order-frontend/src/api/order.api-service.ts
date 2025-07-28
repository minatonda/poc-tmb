import { AxiosInstance } from "axios";
import { Order, OrderCreateRequest } from "../types/order";
import * as signalR from "@microsoft/signalr";

export class OrderApiService {
  private connection: signalR.HubConnection;
  private readonly hubUrl: string;

  constructor(
    private readonly axios: AxiosInstance,
    private readonly baseUrl: string = "/api/orders",
    hubUrl: string = "http://localhost:5000/hub/order" // ajuste conforme necessário
  ) {
    this.hubUrl = hubUrl;

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(this.hubUrl)
      .withAutomaticReconnect()
      .build();

    this.startConnection();
  }

  private async startConnection() {
    try {
      await this.connection.start();
      console.log("SignalR conectado");
    } catch (err) {
      console.error("Erro ao conectar SignalR:", err);
      setTimeout(() => this.startConnection(), 5000); // tenta reconectar depois de 5s
    }
  }

  // Registrar callback para quando uma ordem mudar
  onOrderStatusChanged(callback: (order: Order) => void) {
    this.connection.on("OrderStatusChanged", callback);
  }

  offOrderStatusChanged() {
    this.connection.off("OrderStatusChanged");
  }

  // Métodos REST para Orders
  async getOrders(): Promise<Order[]> {
    const response = await this.axios.get(this.baseUrl);
    return response.data;
  }

  async getOrderById(id: string): Promise<Order> {
    const response = await this.axios.get(`${this.baseUrl}/${id}`);
    return response.data;
  }

  async createOrder(data: OrderCreateRequest): Promise<Order> {
    const response = await this.axios.post(this.baseUrl, data);
    return response.data;
  }
}
