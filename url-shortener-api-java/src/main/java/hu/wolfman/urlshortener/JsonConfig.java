package hu.wolfman.urlshortener;

import jakarta.json.bind.Jsonb;
import jakarta.json.bind.JsonbBuilder;
import jakarta.json.bind.JsonbConfig;
import jakarta.ws.rs.ext.ContextResolver;
import jakarta.ws.rs.ext.Provider;

@Provider
public class JsonConfig implements ContextResolver<Jsonb> {
    @Override
    public Jsonb getContext(Class<?> aClass) {
        JsonbConfig config = new JsonbConfig().withFormatting(true);
        return JsonbBuilder.create(config);
    }
}
