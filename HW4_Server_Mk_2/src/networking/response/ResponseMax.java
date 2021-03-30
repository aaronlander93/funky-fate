package networking.response;

import metadata.Constants;
import model.Player;
import utility.GamePacket;

public class ResponseMax extends GameResponse {

    public ResponseMax() {
        responseCode = Constants.SMSG_MAX;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);

        packet.addInt32(Constants.MAX_SCORE);

        return packet.getBytes();
    }

}
