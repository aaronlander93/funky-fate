package networking.request;

import core.GameServer;
import core.NetworkManager;
import model.Player;
import model.RegisteredUser;
import networking.response.ResponseName;
import networking.response.ResponseRegistration;
import utility.DataReader;
import utility.Log;

import java.io.IOException;
import java.util.Map;

public class RequestRegistration extends GameRequest {
    // Data
    private String username;
    private String password;

    // Response
    private ResponseRegistration responseRegistration;

    public RequestRegistration(){ responses.add(responseRegistration = new ResponseRegistration());}

    @Override
    public void parse() throws IOException {
        username = DataReader.readString(dataInput).trim();
        password = DataReader.readString(dataInput).trim();
    }

    @Override
    public void doBusiness() throws Exception {
        RegisteredUser registeredUser = client.getRegisteredUser();
        GameServer gs = GameServer.getInstance();

        Map<String, String> registeredUsers = gs.getRegisteredUsers();

        if(!registeredUsers.containsKey(username)){
            registeredUser.setUserName(username);
            registeredUser.setPassword(password);

            registeredUsers.put(username, password);

            responseRegistration.setRegistered(true);
            responseRegistration.setRegisteredUser(registeredUser);
        }
        else {
            responseRegistration.setRegistered(false);
        }

    }
}
