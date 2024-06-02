package at.julian.chatbuddyauth.config;

import at.julian.chatbuddyauth.models.ChatMessage;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.event.EventListener;
import org.springframework.messaging.simp.SimpMessageSendingOperations;
import org.springframework.messaging.simp.stomp.StompHeaderAccessor;
import org.springframework.stereotype.Component;
import org.springframework.web.socket.messaging.SessionDisconnectEvent;
@Component
@Slf4j
@RequiredArgsConstructor
public class WebSocketEventListener {
    @Autowired
    private SimpMessageSendingOperations messagingTemplate;

    @EventListener
    public void handleWebSocketDisconnectListener(SessionDisconnectEvent event) {
        StompHeaderAccessor headerAccessor = StompHeaderAccessor.wrap(event.getMessage());
        String username = (String) headerAccessor.getSessionAttributes().get("username");
        String chatroomId = (String) headerAccessor.getSessionAttributes().get("chatroomId");
        if (username != null && chatroomId != null) {
            log.info("User disconnected: {}", username);
            var chatMessage = ChatMessage.builder()
                    .type(ChatMessage.MessageType.LEAVE)
                    .username(username)
                    .build();
            String topic = "/topic/" + chatroomId; // Construct topic dynamically
            messagingTemplate.convertAndSend(topic, chatMessage); // Send message to chatroom-specific topic
        }
    }

}

