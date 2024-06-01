package at.julian.chatbuddyauth.controllers;

import at.julian.chatbuddyauth.models.ChatMessage;
import at.julian.chatbuddyauth.models.Chatroom;
import at.julian.chatbuddyauth.models.User;
import at.julian.chatbuddyauth.payload.request.CreateChatRequest;
import at.julian.chatbuddyauth.payload.request.InviteUserRequest;
import at.julian.chatbuddyauth.payload.request.LeaveChatRequest;
import at.julian.chatbuddyauth.payload.response.MessageResponse;
import at.julian.chatbuddyauth.repository.ChatRepository;
import at.julian.chatbuddyauth.repository.UserRepository;
import at.julian.chatbuddyauth.security.jwt.JwtUtils;
import jakarta.servlet.ServletOutputStream;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.messaging.simp.SimpMessagingTemplate;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.time.LocalDateTime;
import java.util.Optional;

@CrossOrigin(origins = "*", maxAge = 3600)
@RestController
@RequestMapping("/api/info")
public class InfoController {
    @Autowired
    UserRepository userRepository;
    @Autowired
    ChatRepository chatRepository;
    @Autowired
    private SimpMessagingTemplate messagingTemplate;
    JwtUtils jwtUtils;

    @GetMapping("/user/{username}")
    @PreAuthorize("hasRole('USER') or hasRole('MODERATOR') or hasRole('ADMIN')")
    public Optional<User> getUserByUsername(@PathVariable String username) {
        return userRepository.findByUsername(username);
    }
    @GetMapping("/chat/{chatroomId}")
    @PreAuthorize("hasRole('USER') or hasRole('MODERATOR') or hasRole('ADMIN')")
    public Optional<Chatroom> getChatroomById(@PathVariable String chatroomId) {
        return chatRepository.findById(chatroomId);
    }
    @PostMapping("/chats/add")
    @PreAuthorize("hasRole('USER') or hasRole('MODERATOR') or hasRole('ADMIN')")
    public MessageResponse postChatroom(@RequestBody CreateChatRequest createChatRequest) {
        Chatroom chatroom = new Chatroom(createChatRequest.getChatName());
        User user = userRepository.findByUsername(createChatRequest.getUsername()).get();
        chatroom.getUsers().add(user);
        user.getChatrooms().add(chatroom);
        chatroom.getMessages().add(new ChatMessage(ChatMessage.MessageType.JOIN, LocalDateTime.now(), user.getUsername()));
        chatRepository.save(chatroom);
        userRepository.save(user);

        ChatMessage joinMessage = new ChatMessage(ChatMessage.MessageType.JOIN, LocalDateTime.now(), user.getUsername());
        messagingTemplate.convertAndSend("/topic/" + chatroom.getId(), joinMessage);
        return new MessageResponse("Successfully created new chatroom.");
    }

    @PutMapping("/inviteUser/{chatroomId}")
    @PreAuthorize("hasRole('USER') or hasRole('MODERATOR') or hasRole('ADMIN')")
    public MessageResponse addPlayer (@PathVariable String chatroomId, @RequestBody InviteUserRequest r) {
        User user=userRepository.findByUsername(r.getUsername()).get();
        Chatroom chatroom = chatRepository.findById(chatroomId).get();
        if(!chatroom.getUsers().contains(user)){
            chatroom.getUsers().add(user);
            user.getChatrooms().add(chatroom);
            chatroom.getMessages().add(new ChatMessage(ChatMessage.MessageType.JOIN, LocalDateTime.now(), user.getUsername()));
            chatRepository.save(chatroom);
            userRepository.save(user);

            ChatMessage joinMessage = new ChatMessage(ChatMessage.MessageType.JOIN, LocalDateTime.now(), user.getUsername());
            messagingTemplate.convertAndSend("/topic/" + chatroom.getId(), joinMessage);
            return new MessageResponse("User was successfully added to chatroom!");
        }
        return new MessageResponse("User is already in chatroom!");
    }

    @PutMapping("/leaveChat/{chatroomId}")
    @PreAuthorize("hasRole('USER') or hasRole('MODERATOR') or hasRole('ADMIN')")
    public MessageResponse leaveChat (@PathVariable String chatroomId, @RequestBody LeaveChatRequest r) {
        User user=userRepository.findByUsername(r.getUsername()).get();
        Chatroom chatroom = chatRepository.findById(chatroomId).get();
        if(chatroom.getUsers().contains(user)){
            chatroom.getUsers().remove(user);
            user.getChatrooms().remove(chatroom);
            chatroom.getMessages().add(new ChatMessage(ChatMessage.MessageType.LEAVE, LocalDateTime.now(), user.getUsername()));
            chatRepository.save(chatroom);
            userRepository.save(user);

            ChatMessage leaveMessage = new ChatMessage(ChatMessage.MessageType.LEAVE, LocalDateTime.now(), user.getUsername());
            messagingTemplate.convertAndSend("/topic/" + chatroom.getId(), leaveMessage);
            return new MessageResponse("User was successfully removed from chatroom!");
        }
        return new MessageResponse("User is not in chatroom!");
    }
}

