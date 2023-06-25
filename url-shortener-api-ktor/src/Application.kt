package hu.wolfman.urlshortener

import com.fasterxml.jackson.databind.SerializationFeature
import io.ktor.application.*
import io.ktor.features.*
import io.ktor.jackson.*
import io.ktor.request.*
import io.ktor.response.*
import io.ktor.routing.*

data class GetUrlResponse(var url: String)
data class PostUrlRequest(var url: String)
data class PostUrlResponse(var hash: String)

fun main(args: Array<String>) = io.ktor.server.netty.EngineMain.main(args)

fun Application.module() {
    install(ContentNegotiation) {
        jackson {
            enable(SerializationFeature.INDENT_OUTPUT)
        }
    }

    routing {
        route("/api") {
            route("/url") {
                get("/{hash}") {
                    call.respond(GetUrlResponse(""))
                }

                post {
                    var request = call.receive<PostUrlRequest>()
                    call.respond(PostUrlResponse(""))
                }
            }
        }
    }
}

