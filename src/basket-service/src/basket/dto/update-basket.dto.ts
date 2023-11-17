import { IsNotEmpty, IsArray, ValidateNested, IsString } from 'class-validator';
import { Type } from 'class-transformer';
import { Item } from './basket-item.dto';

export class UpdateBasketDto {
  @IsNotEmpty()
  @IsString()
  username: string;

  @IsArray()
  @ValidateNested({ each: true })
  @Type(() => Item)
  items: Item[];
}
