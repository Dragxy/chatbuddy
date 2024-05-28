package at.julian.chatbuddyauth.models;

import lombok.*;

import java.nio.file.FileStore;
import java.time.LocalDateTime;

/**
 * Represents a chat message in the chat application.
 */
@Getter
@Setter
@AllArgsConstructor
@NoArgsConstructor
@Builder
public class ChatMessage {
    private String content;
    private String username;
    private LocalDateTime publish_time;
    private MessageType type;

    public ChatMessage(String username, MessageType type) {
        this.username = username;
        this.type = type;
    }

    public ChatMessage(MessageType type, LocalDateTime publish_time, String username) {
        this.type = type;
        this.publish_time = publish_time;
        this.username = username;
    }

    public enum MessageType {
        CHAT, LEAVE, JOIN
    }
}
