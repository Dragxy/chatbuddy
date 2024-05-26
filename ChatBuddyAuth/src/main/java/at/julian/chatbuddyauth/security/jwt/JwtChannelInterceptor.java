package at.julian.chatbuddyauth.security.jwt;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.Message;
import org.springframework.messaging.MessageChannel;
import org.springframework.messaging.support.ChannelInterceptor;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.stereotype.Service;

import java.sql.SQLOutput;
import java.util.List;
import java.util.Map;

@Service
public class JwtChannelInterceptor implements ChannelInterceptor {

    @Autowired
    JwtUtils jwtUtils;

    @Override
    public Message<?> preSend(Message<?> message, MessageChannel channel) {
        String jwtToken = extractJwtToken(message);

        try{
            if (isValidToken(jwtToken)) {
                return message;
            } else {
                throw new AccessDeniedException("Unauthorized access");
            }
        } catch (AccessDeniedException ex){
            System.err.println("Access denied!");
            return null;
        }
    }
    private String extractJwtToken(Message<?> message) {
        Map<String, List<String>> nativeHeaders = (Map<String, List<String>>) message.getHeaders().get("nativeHeaders");
        if (nativeHeaders != null) {
            List<String> authorizationHeaderList = nativeHeaders.get("Authorization");
            if (authorizationHeaderList != null && !authorizationHeaderList.isEmpty()) {
                String authorizationHeader = authorizationHeaderList.get(0).split(" ")[1];
                return authorizationHeader;
            }
        }
        return null;
    }


    private boolean isValidToken(String jwtToken) {
        return jwtUtils.validateJwtToken(jwtToken);
    }
}
