import { BasketItem } from './BasketItem';

export interface Basket {
  username: string;
  items: BasketItem[];
  totalPrice: number;
}
