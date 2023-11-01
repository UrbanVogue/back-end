package com.urbanvogue.catalogueservice.service;

import com.urbanvogue.catalogueservice.dao.ProductRepository;
import com.urbanvogue.catalogueservice.dto.CatalogueProductResponse;
import com.urbanvogue.catalogueservice.dto.DetailedProductResponse;
import com.urbanvogue.catalogueservice.entity.Product;
import com.urbanvogue.catalogueservice.mapper.ProductToCatalogueProductResponseMapper;
import com.urbanvogue.catalogueservice.mapper.ProductToDetailedProductResponseMapper;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
@Slf4j
public class ProductService {

    private final ProductRepository productRepository;
    private final ProductToCatalogueProductResponseMapper productToCatalogueProductResponseMapper;
    private final ProductToDetailedProductResponseMapper productToDetailedProductResponseMapper;

    public List<CatalogueProductResponse> getCatalogueProducts() {

        log.info("Getting all products");

        List<Product> products = productRepository.findAllCatalogueProducts();

        List<CatalogueProductResponse> catalogueProductResponses = products
                .stream()
                .map(productToCatalogueProductResponseMapper)
                .collect(Collectors.toList());

        return catalogueProductResponses;
    }

    public DetailedProductResponse getDetailedProductById(Long id) {

        log.info("Getting product with id: {}", id);

        return productRepository.findById(id)
                .map(productToDetailedProductResponseMapper)
                .orElseThrow(() -> new RuntimeException("Product not found"));
    }
}
