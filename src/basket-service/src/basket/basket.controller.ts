import { Body, Controller, Get, Param, Post, Delete, Req, BadRequestException, NotFoundException, HttpCode } from '@nestjs/common';
import { BasketService } from './basket.service';
import { Basket } from 'src/basket/interfaces/Basket';
import { UpdateBasketDto } from './dto/update-basket.dto';
import { Client, ClientProxy, Transport } from '@nestjs/microservices';
import axios from 'axios';

@Controller('api/v1/basket')
export class BasketController {



  constructor(private readonly basketService: BasketService) {}

  @Get(':username')
  async getBasket(@Param('username') username: string): Promise<Basket | null> {
    return this.basketService.getBasket(username);
  }

  @Post()
  async updateBasket(@Body() basket: UpdateBasketDto): Promise<Basket> {
    return this.basketService.updateBasket(basket);
  }

  @Delete(':username')
  async deleteBasket(@Param('username') username: string): Promise<void> {
    return this.basketService.deleteBasket(username);
  }

  @Post('checkout/:username')
  @HttpCode(202)
  async checkout(@Param('username') username: string): Promise<object> {
    const basket = await this.basketService.getBasket(username); 
    if (!basket) {
      throw new NotFoundException("Basket not found");
    }
    
    await axios.post('http://ordering.service:80/api/v1/Order', basket);
    await this.basketService.deleteBasket(username);
    return { 
      message: 'Checkout successful',
      successful: true
    };
  }
}
