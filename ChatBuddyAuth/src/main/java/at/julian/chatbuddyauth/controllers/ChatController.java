package at.julian.chatbuddyauth.controllers;

import at.julian.chatbuddyauth.models.ChatMessage;
import org.springframework.messaging.handler.annotation.DestinationVariable;
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.handler.annotation.Payload;
import org.springframework.messaging.handler.annotation.SendTo;
import org.springframework.messaging.simp.SimpMessageHeaderAccessor;

public class ChatController {
    @MessageMapping("/chat.register")
    @SendTo("/topic/{chatroomId}")
    public ChatMessage register(@Payload ChatMessage chatMessage, SimpMessageHeaderAccessor headerAccessor) {
        headerAccessor.getSessionAttributes().put("username", chatMessage.getUsername());
        return chatMessage;
    }

    @MessageMapping("/chat.send/{chatroomId}")
    @SendTo("/topic/{chatroomId}")
    public ChatMessage sendMessage(@Payload ChatMessage chatMessage, @DestinationVariable String chatroomId, SimpMessageHeaderAccessor headerAccessor) {
        return chatMessage;
    }
}
