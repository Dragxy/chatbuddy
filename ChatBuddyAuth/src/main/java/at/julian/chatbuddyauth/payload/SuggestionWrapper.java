package at.julian.chatbuddyauth.payload;

import java.util.List;

public class SuggestionWrapper {
    public List<String> suggestions;

    public SuggestionWrapper(List<String> suggestions) {
        this.suggestions = suggestions;
    }

    public List<String> getSuggestions() {
        return suggestions;
    }

    public void setSuggestions(List<String> suggestions) {
        this.suggestions = suggestions;
    }
}
