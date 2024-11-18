import { UserOrderDetails } from "./userOrderDetails"

export interface Order {
    id: string;
    orderNumber: string;
    customerName: string;
    orderDate: string;
    totalAmount: number;
    status: number;
}
export interface OrdersResponse {
    totalRecords: number;
    totalPages: number;
    data: Order[];
   
}
export interface OrderDTO{
    id: string,
    userId: string,
    status: number,
    orderDate: string,
    totalPrice: number,
    product:UserOrderDetails[]
}
