package com.urbanvogue.catalogueservice.service;

import com.urbanvogue.catalogueservice.dao.ProductRepository;
import com.urbanvogue.catalogueservice.dto.GetAllProductsResponse;
import com.urbanvogue.catalogueservice.entity.Product;
import com.urbanvogue.catalogueservice.mapper.ProductToProductDTOMapper;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
public class ProductService {

    private final ProductRepository productRepository;
    private final ProductToProductDTOMapper productToProductDTOMapper;

    public GetAllProductsResponse getAllProducts() {

        return new GetAllProductsResponse(
                productRepository.findAll().stream()
                        .map(productToProductDTOMapper)
                        .collect(Collectors.toList()));
    }

}
