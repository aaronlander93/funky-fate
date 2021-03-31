package networking.response;

// Other Imports
import metadata.Constants;
import model.Player;
import utility.GamePacket;
import utility.Log;
/**
 * The ResponseLogin class contains information about the authentication
 * process.
 */
public class ResponseMove extends GameResponse {
    private Player player;
    private int moveIndex;

    public ResponseMove() {
        responseCode = Constants.SMSG_MOVE;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addInt32(moveIndex);

        Log.printf("Player with id %d clicked on %d", player.getID(), moveIndex);
 
        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(int moveIndex) {
        this.moveIndex = moveIndex;
    }
}