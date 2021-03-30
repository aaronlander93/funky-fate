package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseMove;
import utility.DataReader;
import core.NetworkManager;

public class RequestMove extends GameRequest {
    private int playerId;
    private String move;
    private ResponseMove responseMove;

    public RequestMove() {
        responses.add(responseMove = new ResponseMove());
    }

    @Override
    public void parse() throws IOException {
        playerId = DataReader.readInt(dataInput);
        move = DataReader.readString(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        if(playerId == player.getID()) {
            responseMove.setPlayer(player);
            responseMove.setData(move);

            NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseMove);
        }
    }
}