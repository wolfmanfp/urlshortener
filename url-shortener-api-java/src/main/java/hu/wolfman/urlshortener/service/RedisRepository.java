package hu.wolfman.urlshortener.service;

import hu.wolfman.urlshortener.model.Link;
import jakarta.nosql.mapping.Repository;

public interface RedisRepository extends Repository<Link, String> {
}
