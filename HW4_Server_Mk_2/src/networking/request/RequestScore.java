package networking.request;

import core.NetworkManager;
import model.Player;
import networking.response.ResponseScore;
import utility.DataReader;

import java.io.IOException;

public class RequestScore extends GameRequest {

    private ResponseScore responseScore;
    private int playerId;
    private int score;

    public RequestScore(){ responses.add(responseScore = new ResponseScore()); }

    @Override
    public void parse() throws IOException {
        playerId = DataReader.readInt(dataInput);
        score = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        if(playerId == player.getID()){
            responseScore.setPlayer(player);
            player.setScore(score);

            NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseScore);
        }
    }
}
