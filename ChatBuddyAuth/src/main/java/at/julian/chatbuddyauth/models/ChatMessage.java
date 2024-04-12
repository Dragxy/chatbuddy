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
    private String userId;
    private String username;
    private LocalDateTime publish_time;
    private MessageType type;

    /**
     * Enum representing the type of the chat message.
     */
    public enum MessageType {
        CHAT, LEAVE, JOIN
    }
}
