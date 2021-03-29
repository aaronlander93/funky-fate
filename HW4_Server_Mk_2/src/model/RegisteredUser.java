package model;

public class RegisteredUser {
    private String username;
    private String password;

    public RegisteredUser(String username, String password){
        this.username = username;
        this.password = password;
    }

    public String getUsername(){ return this.username;}
    public void setUserName(String username){ this.username = username;}

    public String getPassword(){ return this.password; }
    public void setPassword(String password) { this.password = password; }


}
