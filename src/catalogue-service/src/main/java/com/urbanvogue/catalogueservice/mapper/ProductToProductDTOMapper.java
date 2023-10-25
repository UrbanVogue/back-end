package com.urbanvogue.catalogueservice.mapper;

import com.urbanvogue.catalogueservice.dto.ProductDTO;
import com.urbanvogue.catalogueservice.entity.Product;
import org.springframework.stereotype.Service;

import java.util.function.Function;

@Service
public class ProductToProductDTOMapper implements Function<Product, ProductDTO> {
    @Override
    public ProductDTO apply(Product product) {
        return new ProductDTO(product.getName());
    }
}
