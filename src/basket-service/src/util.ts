import { BasketItem } from './basket/interfaces/BasketItem';

export function countTotalPrice(basketItems: BasketItem[]): number {
  let totalPrice = 0;
  basketItems.forEach((basketItem) => {
    totalPrice += basketItem.price * basketItem.quantity;
  });
  return totalPrice;
}
