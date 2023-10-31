package com.urbanvogue.catalogueservice;

import com.urbanvogue.catalogueservice.Util.ImagesDataSeeder;
import com.urbanvogue.catalogueservice.dao.ImageRepository;
import lombok.RequiredArgsConstructor;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;
import org.springframework.core.io.Resource;
import org.springframework.core.io.ResourceLoader;

import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;

@SpringBootApplication
public class CatalogueServiceApplication {

	public static void main(String[] args) {
		SpringApplication.run(CatalogueServiceApplication.class, args);
	}

	@Bean
	public CommandLineRunner run(ImagesDataSeeder imagesDataSeeder) {
		return args -> {
			imagesDataSeeder.run();
		};
	}

}
