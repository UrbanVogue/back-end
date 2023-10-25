package com.urbanvogue.catalogueservice.dto;

import com.urbanvogue.catalogueservice.entity.Product;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Data
@Builder
@AllArgsConstructor
@NoArgsConstructor
public class GetAllProductsResponse {
    List<ProductDTO> products;
}
