package at.julian.chatbuddyauth.security.jwt;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.Message;
import org.springframework.messaging.MessageChannel;
import org.springframework.messaging.support.ChannelInterceptor;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

@Service
public class JwtChannelInterceptor implements ChannelInterceptor {

    @Autowired
    JwtUtils jwtUtils;

    @Override
    public Message<?> preSend(Message<?> message, MessageChannel channel) {

        String jwtToken = extractJwtToken(message);

        if (isValidToken(jwtToken)) {
            return message;
        } else {
            throw new AccessDeniedException("Unauthorized access");
        }
    }
    private String extractJwtToken(Message<?> message) {
        return (String) message.getHeaders().get("Authorization");
    }

    private boolean isValidToken(String jwtToken) {
        return jwtUtils.validateJwtToken(jwtToken);
    }
}
