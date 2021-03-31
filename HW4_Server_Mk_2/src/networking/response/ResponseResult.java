package networking.response;

import metadata.Constants;
import utility.GamePacket;

public class ResponseResult extends GameResponse {
    private int playerId;
    private int result;
    private boolean set = false;

    public ResponseResult() {
        responseCode = Constants.SMSG_RESULT;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);

        if(set){
            packet.addInt32(playerId);
        }
        else{
            packet.addInt32(-1);
        }

        packet.addInt32(result);
        return packet.getBytes();
    }

    public void setPlayerId(int id) {
        this.playerId = id;
        set = true;
    }

    public void setResult(int result){ this.result = result; }
}
