package at.julian.chatbuddyauth.controllers;

import at.julian.chatbuddyauth.models.ChatMessage;
import at.julian.chatbuddyauth.models.Chatroom;
import at.julian.chatbuddyauth.models.User;
import at.julian.chatbuddyauth.repository.ChatRepository;
import at.julian.chatbuddyauth.repository.UserRepository;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.handler.annotation.DestinationVariable;
import org.springframework.messaging.handler.annotation.MessageMapping;
import org.springframework.messaging.handler.annotation.Payload;
import org.springframework.messaging.handler.annotation.SendTo;
import org.springframework.messaging.simp.SimpMessageHeaderAccessor;
import org.springframework.stereotype.Controller;

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

    @MessageMapping("/chat.register/{chatroomId}")
    @SendTo("/topic/{chatroomId}")
    public ChatMessage register(@Payload ChatMessage chatMessage, @DestinationVariable String chatroomId, SimpMessageHeaderAccessor headerAccessor) {
        if (chatMessage == null) return null;
        if (!chatRepository.existsById(chatroomId)) return null;

        chatMessage.setPublish_time(LocalDateTime.now());
        Chatroom chatroom = chatRepository.findById(chatroomId).get();
        if (chatroom.getMessages() == null)
            chatroom.setMessages(new ArrayList<ChatMessage>());
        chatroom.getMessages().add(chatMessage);

        if (chatMessage.getType() == ChatMessage.MessageType.JOIN) {
            if (chatroom.getUsers() == null)
                chatroom.setUsers(new HashSet<User>());
            chatroom.getUsers().add(userRepository.findByUsername(chatMessage.getUsername()).get());
        }
        chatRepository.save(chatroom);
        return chatMessage;
    }

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
