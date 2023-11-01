package com.urbanvogue.catalogueservice.Util;

import com.urbanvogue.catalogueservice.dao.ImageRepository;
import com.urbanvogue.catalogueservice.entity.Image;
import lombok.RequiredArgsConstructor;
import org.springframework.core.io.Resource;
import org.springframework.core.io.ResourceLoader;
import org.springframework.stereotype.Service;

import javax.imageio.ImageIO;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.InputStream;

@Service
@RequiredArgsConstructor
public class ImagesDataSeeder {

    private final ResourceLoader resourceLoader;
    private final ImageRepository imageRepository;

    public void run() throws Exception {
        String[] folders = {"Pants", "Shirt", "Shoes", "Socks", "Hat"};

        for (String folder : folders) {

            String secondImageName = folder + "1";

            byte[] firstImage = getImage(folder, folder);
            byte[] secondImage = getImage(folder, secondImageName);

            Image image1 = imageRepository.findImageByName(folder);
            Image image2 = imageRepository.findImageByName(secondImageName);

            image1.setData(firstImage);
            image2.setData(secondImage);

            imageRepository.save(image1);
            imageRepository.save(image2);

        }
    }

    private byte[] getImage(String folder, String name) throws Exception {
        String pathname = "static/images/" + folder + "/" + name + ".jpg";
        Resource resource = resourceLoader.getResource("classpath:" + pathname);

        // Open an input stream for the resource
        try (InputStream inputStream = resource.getInputStream()) {
            return inputStream.readAllBytes();
        }
    }
}
