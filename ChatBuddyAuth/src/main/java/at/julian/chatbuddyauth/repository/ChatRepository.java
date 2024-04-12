package at.julian.chatbuddyauth.repository;

import at.julian.chatbuddyauth.models.Chatroom;
import at.julian.chatbuddyauth.models.User;
import org.springframework.data.mongodb.repository.MongoRepository;

import java.util.Optional;

public interface ChatRepository extends MongoRepository<Chatroom, String> {
    Optional<Chatroom> findById(String id);
}
