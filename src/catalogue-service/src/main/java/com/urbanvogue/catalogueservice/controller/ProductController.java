package com.urbanvogue.catalogueservice.controller;

import com.urbanvogue.catalogueservice.dto.GetAllProductsResponse;
import com.urbanvogue.catalogueservice.service.ProductService;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestController;


@RestController
@RequestMapping("/api/v1/catalogue")
@RequiredArgsConstructor
public class ProductController {

    private final ProductService productService;

    @GetMapping
    @ResponseStatus(HttpStatus.OK)
    public GetAllProductsResponse getProducts() {
        return productService.getAllProducts();
    }
}
