package com.urbanvogue.catalogueservice.dao;

import com.urbanvogue.catalogueservice.entity.Product;
import org.springframework.data.jpa.repository.JpaRepository;

public interface ProductRepository extends JpaRepository<Product, Long> {
}
