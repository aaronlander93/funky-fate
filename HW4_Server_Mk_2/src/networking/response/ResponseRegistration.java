package networking.response;

import core.GameServer;
import metadata.Constants;
import model.Player;
import model.RegisteredUser;
import utility.GamePacket;

import java.util.List;

public class ResponseRegistration extends GameResponse {

    private RegisteredUser registeredUser;
    private boolean registered;

    public ResponseRegistration() { responseCode = Constants.SMSG_REGISTRATION; }
    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);

        packet.addBoolean(registered);

        if(registered){
            packet.addString(registeredUser.getUsername());
            packet.addString(registeredUser.getPassword());
        }

        return packet.getBytes();
    }

    public void setRegisteredUser(RegisteredUser registeredUser) { this.registeredUser = registeredUser; }

    public void setRegistered(boolean registered){ this.registered = registered; }
}
