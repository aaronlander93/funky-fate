package networking.request;

import core.GameServer;
import core.NetworkManager;
import model.Player;
import networking.response.ResponseName;
import networking.response.ResponseRegistration;
import utility.DataReader;
import utility.Log;

import java.io.IOException;

public class RequestRegistration extends GameRequest {
    // Data
    private String username;
    private String password;

    // Response
    private ResponseRegistration responseRegistration;

    @Override
    public void parse() throws IOException {
        username = DataReader.readString(dataInput).trim();
        password = DataReader.readString(dataInput).trim();
    }

    @Override
    public void doBusiness() throws Exception {
        GameServer gs = GameServer.getInstance();


    }
}
