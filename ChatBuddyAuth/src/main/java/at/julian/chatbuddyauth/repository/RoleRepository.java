package at.julian.chatbuddyauth.repository;

import at.julian.chatbuddyauth.models.ERole;
import at.julian.chatbuddyauth.models.Role;
import org.springframework.data.mongodb.repository.MongoRepository;

import java.util.Optional;

public interface RoleRepository extends MongoRepository<Role, String> {
    Optional<Role> findByName(ERole name);
}
