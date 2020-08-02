package hu.wolfman.urlshortener.service;

import hu.wolfman.urlshortener.model.Link;
import jakarta.nosql.mapping.Database;
import jakarta.nosql.mapping.DatabaseType;

import javax.inject.Inject;
import java.util.Optional;

public class RedisService {
    @Inject
    @Database(DatabaseType.KEY_VALUE)
    private RedisRepository repository;

    public void set(String id, String url) {
        repository.save(new Link(id, url));
    }

    public Optional<Link> get(String id) {
        return repository.findById(id);
    }
}
