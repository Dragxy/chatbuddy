package at.julian.chatbuddyauth.controllers;

import at.julian.chatbuddyauth.models.Chatroom;
import at.julian.chatbuddyauth.models.User;
import at.julian.chatbuddyauth.payload.response.MessageResponse;
import at.julian.chatbuddyauth.repository.ChatRepository;
import at.julian.chatbuddyauth.repository.UserRepository;
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
    @GetMapping("/all")
    public String allAccess() {
        return "Public Content.";
    }

    @GetMapping("/user")
    @PreAuthorize("hasRole('USER') or hasRole('MODERATOR') or hasRole('ADMIN')")
    public String userAccess() {
        return "User Content.";
    }

    @GetMapping("/mod")
    @PreAuthorize("hasRole('MODERATOR') or hasRole('ADMIN')")
    public String moderatorAccess() {
        return "Moderator Board.";
    }

    @GetMapping("/admin")
    @PreAuthorize("hasRole('ADMIN')")
    public String adminAccess() {
        return "Admin Board.";
    }

    @PostMapping("/debug/")
    public Chatroom postChatroom(@RequestBody Chatroom chatroom) {
        return chatRepository.save(chatroom);
    }

    @PutMapping("/debug/{chatroomId}")
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

