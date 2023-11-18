package com.urbanvogue.catalogueservice.controller;

import com.urbanvogue.catalogueservice.dto.CatalogueProductResponse;
import com.urbanvogue.catalogueservice.dto.DetailedProductResponse;
import com.urbanvogue.catalogueservice.service.ProductService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;


//@CrossOrigin(origins = "http://localhost:4200")
@CrossOrigin(maxAge = 3600)
@RestController
@RequestMapping("/api/v1/catalogue")
@RequiredArgsConstructor
@Slf4j
public class ProductController {

    private final ProductService productService;

    @GetMapping
    @ResponseStatus(HttpStatus.OK)
    public ResponseEntity<List<CatalogueProductResponse>> getProducts() {
        log.info("Request for all products");

        return new ResponseEntity<>(productService.getCatalogueProducts(), HttpStatus.OK);
    }

    @GetMapping("{id}")
    public ResponseEntity<DetailedProductResponse> getDetailedProductById(@PathVariable Long id) {
        log.info("Request for product with id: {}", id);

        return new ResponseEntity<>(productService.getDetailedProductById(id), HttpStatus.OK);
    }
}
