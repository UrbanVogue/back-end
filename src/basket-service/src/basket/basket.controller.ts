import { Body, Controller, Get, Param, Post, Delete } from '@nestjs/common';
import { BasketService } from './basket.service';
import { Basket } from 'src/basket/interfaces/Basket';

@Controller('api/v1/basket')
export class BasketController {
  constructor(private readonly basketService: BasketService) {}

  @Get(':username')
  async getBasket(@Param('username') username: string): Promise<Basket | null> {
    return this.basketService.getBasket(username);
  }

  @Post()
  async updateBasket(@Body() basket: Basket): Promise<Basket> {
    return this.basketService.updateBasket(basket);
  }

  @Delete(':username')
  async deleteBasket(@Param('username') username: string): Promise<void> {
    return this.basketService.deleteBasket(username);
  }
}
