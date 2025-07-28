import { AxiosInstance } from "axios";
import { Order, OrderCreateRequest } from "../types/order";

export class OrderApiService {
  constructor(
    private readonly axios: AxiosInstance,
    private readonly baseUrl: string = "/api/orders"
  ) {}

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
