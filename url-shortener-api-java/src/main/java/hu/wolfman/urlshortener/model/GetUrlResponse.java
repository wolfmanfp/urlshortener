package hu.wolfman.urlshortener.model;

public class GetUrlResponse {
    private String url;

    public GetUrlResponse(String url) {
        this.url = url;
    }

    public String getUrl() {
        return url;
    }
}
