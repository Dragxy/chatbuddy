package at.julian.chatbuddyauth.controllers;

import at.julian.chatbuddyauth.models.Chatroom;
import at.julian.chatbuddyauth.models.User;
import at.julian.chatbuddyauth.payload.SuggestionWrapper;
import at.julian.chatbuddyauth.repository.ChatRepository;
import at.julian.chatbuddyauth.repository.UserRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

@Controller
public class AutocompleteController {
    @Autowired
    UserRepository userRepository;
    @Autowired
    ChatRepository chatRepository;

    @RequestMapping(value = "/suggestion", method = RequestMethod.GET, produces = "application/json")
    @ResponseBody
    public SuggestionWrapper autocompleteSuggestions(@RequestParam("searchstr") String searchstr, @RequestParam("chattarget") String chattarget, Locale locale, Model model) {
        //System.out.println("searchstr: " + searchstr);
        List<String> suggestions = new ArrayList<>();
        Chatroom chatroom = chatRepository.findById(chattarget).get();
        for(User user : userRepository.findAll()) {
                if(user.getUsername().toLowerCase().contains(searchstr.toLowerCase())) {
                    if(!chatroom.getUsers().contains(user)) {
                        suggestions.add(user.getUsername());
                    }
                }
        }
        int n = suggestions.size() > 20 ? 20 : suggestions.size();
        List<String> sulb = new ArrayList<>(suggestions.subList(0, n));
        return new SuggestionWrapper(sulb);
    }
}

