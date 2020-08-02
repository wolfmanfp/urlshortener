package hu.wolfman.urlshortener.model;

public class PostUrlResponse {
    private String hash;

    public PostUrlResponse(String hash) {
        this.hash = hash;
    }

    public String getHash() {
        return hash;
    }
}
