package networking.request;

import core.NetworkManager;
import model.Player;
import networking.response.ResponseResult;
import utility.DataReader;

import java.io.Console;
import java.io.IOException;

public class RequestResult extends GameRequest {

    private ResponseResult responseResult;
    private int playerId;
    private String playerMove;
    private String oppMove;

    public RequestResult(){ responses.add(responseResult = new ResponseResult());}
    @Override
    public void parse() throws IOException {
        playerId = DataReader.readInt(dataInput);
        playerMove = DataReader.readString(dataInput);
        oppMove = DataReader.readString(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        if(playerId == player.getID()){
            responseResult.setPlayerId(playerId);

            if(playerMove.equals(oppMove)){
                responseResult.setResult(2);
            }
            else if(playerMove.equals("Rock") && oppMove.equals("Scissors")){
                responseResult.setResult(0);
            }
            else if(playerMove.equals("Scissors") && oppMove.equals("Paper")){
                responseResult.setResult(0);
            }
            else if(playerMove.equals("Paper") && oppMove.equals("Rock")){
                responseResult.setResult(0);
            }
            else {
                responseResult.setResult(1);
            }

            NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseResult);
        }

    }
}
