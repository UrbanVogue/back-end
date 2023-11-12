package com.urbanvogue.catalogueservice;

import com.urbanvogue.catalogueservice.Util.ImagesDataSeeder;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;

@SpringBootApplication
public class CatalogueServiceApplication {

	public static void main(String[] args) {
		SpringApplication.run(CatalogueServiceApplication.class, args);
	}

	@Bean
	public CommandLineRunner run(ImagesDataSeeder imagesDataSeeder) {
		return args -> imagesDataSeeder.run();
	}

}
