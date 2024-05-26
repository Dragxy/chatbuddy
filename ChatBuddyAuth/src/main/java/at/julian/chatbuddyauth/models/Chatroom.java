package at.julian.chatbuddyauth.models;

import com.fasterxml.jackson.annotation.JsonIgnore;
import lombok.AllArgsConstructor;
import lombok.NoArgsConstructor;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.DBRef;
import org.springframework.data.mongodb.core.mapping.Document;

import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.Set;
@NoArgsConstructor
@Document(collection = "chatrooms")
public class Chatroom {
    @Id
    private String id;
    private String name;
    @DBRef(lazy = true)
    @JsonIgnore
    private Set<User> users;
    private List<ChatMessage> messages = new ArrayList<>();

    public Chatroom(String id, String name, Set<User> users, List<ChatMessage> messages) {
        this.id = id;
        this.name = name;
        this.users = users;
        this.messages = messages;
    }

    public Chatroom(String name) {
        this.name = name;
        users = new HashSet<>();
        messages=new ArrayList<>();
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public Set<User> getUsers() {
        return users;
    }

    public void setUsers(Set<User> users) {
        this.users = users;
    }

    public List<ChatMessage> getMessages() {
        return messages;
    }

    public void setMessages(List<ChatMessage> messages) {
        this.messages = messages;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
