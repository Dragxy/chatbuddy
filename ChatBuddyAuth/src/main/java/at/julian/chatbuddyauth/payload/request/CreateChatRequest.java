package at.julian.chatbuddyauth.payload.request;

import jakarta.validation.constraints.NotBlank;

public class CreateChatRequest {
    @NotBlank
    private String chatName;
    @NotBlank
    private String username;

    public @NotBlank String getChatName() {
        return chatName;
    }

    public void setChatName(@NotBlank String chatName) {
        this.chatName = chatName;
    }

    public @NotBlank String getUsername() {
        return username;
    }

    public void setUsername(@NotBlank String username) {
        this.username = username;
    }
}
