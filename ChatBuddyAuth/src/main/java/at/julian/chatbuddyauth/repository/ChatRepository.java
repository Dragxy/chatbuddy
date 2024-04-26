package at.julian.chatbuddyauth.repository;

import at.julian.chatbuddyauth.models.Chatroom;
import org.springframework.data.mongodb.repository.MongoRepository;

public interface ChatRepository extends MongoRepository<Chatroom, String> {
    Chatroom findByName(String name);
}
