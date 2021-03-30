package networking.response;

import model.Player;
import utility.GamePacket;
import utility.Log;

public class ResponseScore extends GameResponse {

    private Player player;

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addInt32(player.getScore());

        Log.printf("Player %s now has a score of: %d", player.getName(), player.getScore());

        return packet.getBytes();
    }

    public void setPlayer(Player player) { this.player = player; }
}
