package at.julian.chatbuddyauth.controllers;

import at.julian.chatbuddyauth.models.ChatMessage;
import at.julian.chatbuddyauth.models.Chatroom;
import at.julian.chatbuddyauth.models.User;
import at.julian.chatbuddyauth.payload.request.InviteUserRequest;
import at.julian.chatbuddyauth.payload.response.MessageResponse;
import at.julian.chatbuddyauth.repository.ChatRepository;
import at.julian.chatbuddyauth.repository.UserRepository;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.handler.annotation.DestinationVariable;
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.handler.annotation.Payload;
import org.springframework.messaging.handler.annotation.SendTo;
import org.springframework.messaging.simp.SimpMessageHeaderAccessor;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.Set;

@Slf4j
@Controller
public class ChatController {
    @Autowired
    ChatRepository chatRepository;
    @Autowired
    UserRepository userRepository;

    @MessageMapping("/chat.send/{chatroomId}")
    @SendTo("/topic/{chatroomId}")
    public ChatMessage sendMessage(@Payload ChatMessage chatMessage, @DestinationVariable String chatroomId, SimpMessageHeaderAccessor headerAccessor) {
        if (chatMessage == null) return null;
        if (!chatRepository.existsById(chatroomId)) return null;

        Chatroom chatroom = chatRepository.findById(chatroomId).get();
        chatroom.getMessages().add(chatMessage);
        chatRepository.save(chatroom);
        return chatMessage;
    }
}
