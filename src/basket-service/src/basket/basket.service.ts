import { Injectable, NotFoundException } from '@nestjs/common';
import Redis from 'ioredis';
import { Basket } from 'src/basket/interfaces/Basket';
import { countTotalPrice } from 'src/util';
import { UpdateBasketDto } from './dto/update-basket.dto';

@Injectable()
export class BasketService {
  private readonly redis: Redis = new Redis({
    port: parseInt(process.env.REDIS_PORT) || 6379,
    host: process.env.REDIS_HOST || 'localhost',
    password: process.env.REDIS_PASSWORD,
  });

  async getBasket(userName: string): Promise<Basket | null> {
    const basket = await this.redis.get(userName);

    if (!basket) {
      throw new NotFoundException('Basket not found');
    }

    const basketJson = JSON.parse(basket);
    basketJson.totalPrice = countTotalPrice(basketJson.items);
    return basketJson;
  }

  async updateBasket(basket: UpdateBasketDto): Promise<Basket> {
    await this.redis.set(
      basket.username,
      JSON.stringify(basket),
      'EX',
      60 * 60 * 24 * 30,
    );
    return this.getBasket(basket.username);
  }

  async deleteBasket(userName: string): Promise<void> {
    await this.redis.del(userName);
  }
}
