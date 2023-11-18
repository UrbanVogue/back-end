package com.urbanvogue.catalogueservice.mapper;

import com.urbanvogue.catalogueservice.dto.CatalogueProductResponse;
import com.urbanvogue.catalogueservice.entity.Product;
import org.springframework.stereotype.Service;

import java.util.function.Function;

@Service
public class ProductToCatalogueProductResponseMapper implements Function<Product, CatalogueProductResponse> {
    @Override
    public CatalogueProductResponse apply(Product product) {
        return new CatalogueProductResponse(
                product.getId(),
                product.getName(),
                product.getBasePrice(),
                product.getDiscountPrice(),
                product.getRating(),
                product.getImages().get(0));
    }
}
