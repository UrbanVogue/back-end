package com.urbanvogue.catalogueservice.entity;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.Type;

import javax.persistence.*;

@Entity
@Table(name = "image")
@Data
@Builder
@AllArgsConstructor
@NoArgsConstructor
public class Image {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "id")
    private Long id;

    @Column(name ="name")
    private String name;

    @Column(name = "type")
    private String type;

    @Column(name = "is_for_catalogue")
    private boolean isForCatalogue;

    @Lob
    @Column(name = "data")
    @Type(type = "org.hibernate.type.BinaryType")
    private byte[] data;

}
