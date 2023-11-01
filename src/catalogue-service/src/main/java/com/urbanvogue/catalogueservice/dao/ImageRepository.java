package com.urbanvogue.catalogueservice.dao;

import com.urbanvogue.catalogueservice.entity.Image;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

public interface ImageRepository extends JpaRepository<Image, Long> {

    Image findImageByName(String name);

}
