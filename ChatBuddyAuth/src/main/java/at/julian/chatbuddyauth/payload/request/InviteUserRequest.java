package at.julian.chatbuddyauth.payload.request;

import jakarta.validation.constraints.NotBlank;

public class InviteUserRequest {
    @NotBlank
    private String username;

    public @NotBlank String getUsername() {
        return username;
    }

    public void setUsername(@NotBlank String username) {
        this.username = username;
    }
}
