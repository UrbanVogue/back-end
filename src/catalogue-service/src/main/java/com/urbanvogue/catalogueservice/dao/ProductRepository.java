package com.urbanvogue.catalogueservice.dao;

import com.urbanvogue.catalogueservice.entity.Product;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

import java.util.List;

public interface ProductRepository extends JpaRepository<Product, Long> {
    @Query("select p from Product p join fetch p.images i where i.isForCatalogue=true")
    List<Product> findAllCatalogueProducts();
}
