package networking.request;

import core.NetworkManager;
import model.Player;
import networking.response.ResponseMax;

import java.io.IOException;

public class RequestMax extends GameRequest {

    ResponseMax responseMax;

    public RequestMax(){ responses.add(responseMax = new ResponseMax()); }

    @Override
    public void parse() throws IOException {
        // No parsing
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        NetworkManager.addResponseForUser(player.getID(), responseMax);
    }
}
