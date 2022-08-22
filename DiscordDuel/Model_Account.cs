using MongoDB.Bson;
public class Model_Account
{

    public ObjectId _id;
    public string Username { get; set; }
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public System.DateTime JoinedOn { get; set; }
    public int Coins { get; set; }
    
    public float KD { get; set; }


}
