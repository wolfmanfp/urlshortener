package hu.wolfman.urlshortener.resource;

import hu.wolfman.urlshortener.model.GetUrlResponse;
import hu.wolfman.urlshortener.model.Link;
import hu.wolfman.urlshortener.model.PostUrlRequest;
import hu.wolfman.urlshortener.model.PostUrlResponse;
import hu.wolfman.urlshortener.service.RedisService;
import io.seruco.encoding.base62.Base62;
import org.apache.commons.codec.digest.DigestUtils;

import javax.ejb.Stateless;
import javax.inject.Inject;
import javax.ws.rs.*;
import javax.ws.rs.core.Response;
import javax.ws.rs.core.UriBuilder;
import java.net.URI;
import java.util.Optional;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

@Path("/url")
@Consumes(APPLICATION_JSON)
@Produces(APPLICATION_JSON)
@Stateless
public class UrlResource {
    @Inject
    private RedisService redisService;

    @GET
    @Path("/{hash}")
    public Response getUrl(@PathParam("hash") String hash) {
        Optional<Link> link = redisService.get(hash);
        if (link.isEmpty()) {
            return Response.status(Response.Status.NOT_FOUND).build();
        }
        return Response.ok(new GetUrlResponse(link.get().getUrl())).build();
    }

    @POST
    public Response postUrl(PostUrlRequest request) {
        String url = request.getUrl();
        String hash = getHash(url);
        redisService.set(hash, url);

        final URI uri = UriBuilder.fromResource(UrlResource.class).path("/{hash}").build(hash);
        return Response.created(uri).entity(new PostUrlResponse(hash)).build();
    }

    private String getHash(String url) {
        Base62 base62 = Base62.createInstance();
        String hash = new String(base62.encode(getMd5Hash(url)));
        return hash.substring(0, 6);
    }

    private byte[] getMd5Hash(String url) {
        return DigestUtils.md5Hex(url).toLowerCase().getBytes();
    }
}
