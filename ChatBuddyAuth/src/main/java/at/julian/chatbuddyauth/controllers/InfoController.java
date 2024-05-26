package at.julian.chatbuddyauth.controllers;

import at.julian.chatbuddyauth.models.Chatroom;
import at.julian.chatbuddyauth.models.User;
import at.julian.chatbuddyauth.payload.request.CreateChatRequest;
import at.julian.chatbuddyauth.payload.response.MessageResponse;
import at.julian.chatbuddyauth.repository.ChatRepository;
import at.julian.chatbuddyauth.repository.UserRepository;
import at.julian.chatbuddyauth.security.jwt.JwtUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;

import java.util.Optional;

@CrossOrigin(origins = "*", maxAge = 3600)
@RestController
@RequestMapping("/api/info")
public class InfoController {
    @Autowired
    UserRepository userRepository;
    @Autowired
    ChatRepository chatRepository;
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
        chatRepository.save(chatroom);
        userRepository.save(user);
        return new MessageResponse("Successfully created new chatroom.");
    }

    @PutMapping("/inviteUser/{chatroomId}")
    @PreAuthorize("hasRole('USER') or hasRole('MODERATOR') or hasRole('ADMIN')")
    public MessageResponse addPlayer (@PathVariable String chatroomId, @RequestBody User u) {
        User user=userRepository.findByUsername(u.getUsername()).get();
        Chatroom chatroom = chatRepository.findById(chatroomId).get();
        if(!chatroom.getUsers().contains(user)){
            chatroom.getUsers().add(user);
            user.getChatrooms().add(chatroom);
            chatRepository.save(chatroom);
            userRepository.save(user);
            return new MessageResponse("User was successfully added to chatroom!");
        }
        return new MessageResponse("User is already in chatroom!");
    }
}

