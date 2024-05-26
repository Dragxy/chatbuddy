package at.julian.chatbuddyauth;

import at.julian.chatbuddyauth.models.Chatroom;
import at.julian.chatbuddyauth.models.ERole;
import at.julian.chatbuddyauth.models.Role;
import at.julian.chatbuddyauth.repository.ChatRepository;
import at.julian.chatbuddyauth.repository.RoleRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.context.event.ApplicationReadyEvent;
import org.springframework.context.event.EventListener;
import org.springframework.data.mongodb.core.MongoTemplate;
import org.springframework.stereotype.Component;

@Component

public class DataInitilizer {
    @Autowired
    private MongoTemplate mongoTemplate;

    @Autowired
    private RoleRepository roleRepository;
    @Autowired
    private ChatRepository chatRepository;

    @EventListener(ApplicationReadyEvent.class)
    public void initData() {
        if (roleRepository.count() == 0) {
            Role role = new Role(ERole.ROLE_USER);
            roleRepository.save(role);
            role = new Role(ERole.ROLE_MODERATOR);
            roleRepository.save(role);
            role = new Role(ERole.ROLE_ADMIN);
            roleRepository.save(role);
        }
        if (chatRepository.count() == 0) {
            Chatroom chatroom = new Chatroom("global");
            chatRepository.save(chatroom);
        }
    }
}
