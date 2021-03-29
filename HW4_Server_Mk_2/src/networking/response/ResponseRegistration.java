package networking.response;

import core.GameServer;
import model.Player;
import utility.GamePacket;

import java.util.List;

public class ResponseRegistration extends GameResponse {

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);


    }
}
