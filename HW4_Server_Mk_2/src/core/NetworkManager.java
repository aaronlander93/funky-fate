package core;

// Other Imports
import java.io.IOException;
import model.Player;
import networking.response.GameResponse;
import utility.Log;

public class NetworkManager {

    private NetworkManager() {
    }

    /**
     * Push a pending response to a user's queue.
     *
     * @param player_id holds the player ID
     * @param response is the instance containing the response information
     */
    public static void addResponseForUser(int player_id, GameResponse response) {
        GameClient client = GameServer.getInstance().getThreadByPlayerID(player_id);

        if (client != null) {
            client.addResponseForUpdate(response);
        } else {
            Log.printf_e("Failed to create response for user, %d.", player_id);
        }
    }

    /**
     * Push a pending response to all users' queue except one user.
     *
     * @param player_id holds the excluding player ID
     * @param response is the instance containing the response information
     */
    public static void addResponseForAllOnlinePlayers(int player_id, GameResponse response) throws IOException {
        for (GameClient client : GameServer.getInstance().getActiveThreads().values()) {
            Player player = client.getPlayer();

            if (player != null && client.getPlayer().getID() != player_id) {
                System.out.println("SENDING RESPONSE TO " + client.getPlayer().getID());
                System.out.println("THIS IS COMPARED TO " + player_id);                
                client.send(response);
            }
        }
    }
}
