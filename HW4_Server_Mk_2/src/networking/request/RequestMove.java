package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseMove;
import utility.DataReader;
import core.NetworkManager;

public class RequestMove extends GameRequest {
    private int moveIndex;
    // Responses
    private ResponseMove responseMove;

    public RequestMove() {
        responses.add(responseMove = new ResponseMove());
    }

    @Override
    public void parse() throws IOException {
        moveIndex = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseMove.setPlayer(player);
        responseMove.setData(moveIndex);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseMove);
    }
}