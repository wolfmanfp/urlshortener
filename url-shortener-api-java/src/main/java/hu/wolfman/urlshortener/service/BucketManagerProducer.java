package hu.wolfman.urlshortener.service;

import jakarta.nosql.keyvalue.BucketManager;
import jakarta.nosql.keyvalue.BucketManagerFactory;
import jakarta.nosql.keyvalue.KeyValueConfiguration;
import org.eclipse.jnosql.diana.redis.keyvalue.RedisConfiguration;

import javax.annotation.PostConstruct;
import javax.enterprise.inject.Produces;

public class BucketManagerProducer {
    private static final String BUCKET = "";

    private BucketManagerFactory managerFactory;

    @PostConstruct
    public void init() {
        KeyValueConfiguration configuration = new RedisConfiguration();
        managerFactory = configuration.get();
    }

    @Produces
    public BucketManager getManager() {
        return managerFactory.getBucketManager(BUCKET);
    }
}
