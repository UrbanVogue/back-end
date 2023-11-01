package com.urbanvogue.catalogueservice.mapper;

import com.urbanvogue.catalogueservice.dto.DetailedProductResponse;
import com.urbanvogue.catalogueservice.entity.Product;
import org.springframework.stereotype.Service;

import java.util.function.Function;

@Service
public class ProductToDetailedProductResponseMapper implements Function<Product, DetailedProductResponse> {

    @Override
    public DetailedProductResponse apply(Product product) {
        return new DetailedProductResponse(
                product.getName(),
                product.getBasePrice(),
                product.getDiscountPrice(),
                product.getRating(),
                product.getImages());
    }
}
