package hu.wolfman.urlshortener.service;

import hu.wolfman.urlshortener.model.Link;
import jakarta.inject.Inject;
import jakarta.nosql.Template;

import java.util.Optional;

public class RedisService {
    @Inject
    private Template template;

    public void set(String id, String url) {
        template.insert(new Link(id, url));
    }

    public Optional<Link> get(String id) {
        return template.find(Link.class, id);
    }
}
