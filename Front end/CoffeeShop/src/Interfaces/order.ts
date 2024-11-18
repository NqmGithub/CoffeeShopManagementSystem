import { UserOrderDetails } from "./userOrderDetails"

export interface Order {
    id: string,
    userId: string,
    status: number,
    orderDate: string,
    totalPrice: number,
    product:UserOrderDetails[]
}
