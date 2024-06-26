package at.julian.chatbuddyauth;

import at.julian.chatbuddyauth.models.ERole;
import at.julian.chatbuddyauth.models.Role;
import at.julian.chatbuddyauth.repository.RoleRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public class ChatBuddyAuthApplication {

	public static void main(String[] args) {
		SpringApplication.run(ChatBuddyAuthApplication.class, args);
	}

}
