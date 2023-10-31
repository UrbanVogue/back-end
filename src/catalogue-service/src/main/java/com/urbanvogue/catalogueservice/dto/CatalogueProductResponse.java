package com.urbanvogue.catalogueservice.dto;

import com.urbanvogue.catalogueservice.entity.Image;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.math.BigDecimal;
import java.util.List;

@Data
@Builder
@AllArgsConstructor
@NoArgsConstructor
public class CatalogueProductResponse {

    private String name;

    private BigDecimal basePrice;

    private BigDecimal discountPrice;

    private Float rating;

    private Image image;
}
