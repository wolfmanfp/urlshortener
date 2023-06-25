package hu.wolfman.urlshortener.model;

import jakarta.nosql.Entity;
import jakarta.nosql.Id;

@Entity
public class Link {
    @Id
    private String id;

    private String url;

    public Link() {
    }

    public Link(String id, String url) {
        this.id = id;
        this.url = url;
    }

    public String getUrl() {
        return url;
    }
}
