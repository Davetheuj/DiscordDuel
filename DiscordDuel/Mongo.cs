
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using MongoDB.Driver.Builders;
using System.Collections.Generic;

using System.Threading;


public class Mongo 

{
    private const string MONGO_URI = "mongodb://newuser:password456@cluster0-shard-00-00-lrmyx.azure.mongodb.net:27017,cluster0-shard-00-01-lrmyx.azure.mongodb.net:27017,cluster0-shard-00-02-lrmyx.azure.mongodb.net:27017/test?ssl=true&replicaSet=Cluster0-shard-0&authSource=admin&retryWrites=true";
    private string RealmDatabase = "DaVDiscordDuel"; 
    //private const string ROUNDDB = "RoundSettings";
    private MongoClient client;
    private MongoDatabase database;
    //private MongoDatabase roundDB;
    private MongoServer server;
    private MongoCollection<Model_Account> userInformationCollection;
   // private MongoCollection<RoundSettings> serverSettingsCollection;
    private string serverPassword;
   


    



    public void Init()
    {
         client = new MongoClient(MONGO_URI);
        server = client.GetServer();
        database = server.GetDatabase(RealmDatabase);

         userInformationCollection = database.GetCollection<Model_Account>("PlayerInformation");
        
        
       
    }

    public void InsertNewAccount(Model_Account accountToInsert)
    {
        userInformationCollection.Insert(accountToInsert);
    }

    public MongoCursor<Model_Account> FindAllUsers()
    {
        return userInformationCollection.FindAll();
    }

    public Model_Account FindAccountInDatabase(string username)
    {
        Model_Account accountToReturn = null;

        accountToReturn = userInformationCollection.FindOne(Query<Model_Account>.EQ(u => u.Username, username));

        return accountToReturn;


    }
    public void UpdateAccount(Model_Account account, string username)
   {
       userInformationCollection.Update(Query<Model_Account>.EQ(u => u.Username,username), Update<Model_Account>.Replace(account));
   }





    void Update()
    {
        while (server.State == MongoServerState.Disconnected)
        {
          
            Thread.Sleep(1000);
            try
            {
                server.Reconnect();
            }
            catch (Exception ex)
            {
            
            }

        }

    }


    public void Shutdown()
    {
        
        client = null;
       
       database = null;
    }
    #region Insert
   // public bool InsertAccount(string username, string password, string email, int racialInt)
   // {
       
   //     //Check if the email is valid.
       

   //     //Check if the username is valid.
   //     if (!Utility.IsUsername(username))
   //     {
   //         errorText.GetComponent<ErrorScript>().SetErrorText("Username Isn't Valid.");
   //         return false;
   //     }

   //     //Check if the username already exists.
   //     if (FindAccountByRealmName(username) != null)
   //     {
   //         errorText.GetComponent<ErrorScript>().SetErrorText("Username Already Exists.");
   //         return false;
   //     }
   //     //Check if the email already exists.
   //   /*  if (FindAccountByEmail(email) != null)
   //     {
   //         Debug.Log(email + " is not appropriate");
   //         return false;
   //     }*/
     

   //     ////Check if the password is valid
   //     //if (!Utility.IsPassword(password))
   //     //{
   //     //    Debug.Log(password + " is not appropriate");
   //     //    return false;
   //     //}

   //     Model_Account newAccount = new Model_Account();
   //     newAccount.RealmName = username;
   //     newAccount.ShaPassword = Utility.Sha256FromString(password);
   //     newAccount.Email = email;
   //     //newAccount.Discriminator = "#0000";
   //     newAccount.GoldBanked = 10000000;
   //     newAccount.CreateOn = System.DateTime.UtcNow;
   //     newAccount.LastUpdated = System.DateTime.UtcNow;
   //     newAccount.LastLogin = System.DateTime.UtcNow;
   //     newAccount.AttackTurns = 25;
   //     newAccount.Race = racialInt;
   //     newAccount.CivilianSize = 100000;
   //     newAccount.Resources = 10000000;
   //     newAccount.SpySize = 1;
   //     newAccount.MedalToken = 1;
        

   //     userInformationCollection.Insert(newAccount);
   //     return true; 
   // }

   // public Model_Account LoginAccount(string username, string password, string token)
   // {
   //     Model_Account myAccount = null;
   //     IMongoQuery query = null;

   //     //Find my account
   //     if (Utility.IsUsername(username))
   //     {
   //         query = Query.And(
   //             Query<Model_Account>.EQ(u => u.RealmName, username),
   //              Query<Model_Account>.EQ(u => u.ShaPassword, password));

   //         myAccount = userInformationCollection.FindOne(query);
   //     }

   //     if(myAccount != null)
   //     {
   //         //Account exists login.

   //         myAccount.Token = token;
   //         myAccount.Status = 1;
   //         myAccount.LastLogin = System.DateTime.UtcNow;

   //         userInformationCollection.Update(query, Update<Model_Account>.Replace(myAccount));
   //     }
   //     else
   //     {
   //         //didnt find anything, logged in with wrong information

   //     }
   //     return myAccount;


   // }

   // public void UpdateAccount(Model_Account account, string realmName)
   // {
   //     userInformationCollection.Update(Query<Model_Account>.EQ(u => u.RealmName,realmName), Update<Model_Account>.Replace(account));
   // }

   // #endregion
   // #region Fetch
   // public Model_Account FindAccountByEmail(string email)
   // {
   //     return userInformationCollection.FindOne(Query<Model_Account>.EQ(u => u.Email, email));
   // }

   // public Model_Account FindAccountByRealmName(string username)
   // {
   //     return userInformationCollection.FindOne(Query<Model_Account>.EQ(u => u.RealmName, username));
   // }

   //public MongoCursor<Model_Account> FindAllAccounts()
   // {
   //    return userInformationCollection.FindAll();
   //    /*foreach(Model_Account account in cursor)
   //     {
   //         Debug.Log("Realm Name: " +account.RealmName);
   //     }*/
   // }
   // public MongoCursor<RoundSettings> GetServerSettings()
   // {
   //     return serverSettingsCollection.FindAll();
   // }
    #endregion

    #region Update
    #endregion

    #region Delete
    #endregion
}
