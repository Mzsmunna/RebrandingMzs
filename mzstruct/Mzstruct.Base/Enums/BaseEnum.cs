using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Enums
{
    public enum DBType
    {
        //Default, // SqlClient | SqlServer
        InMemory,
        SQL,
        NoSQL,
        KeyValue,
        Vector,

        //sql
        SqlServer,
        PostgreSql,
        MySQL,
        SQLite,
        Superbase,

        //nosql
        MongoDB,
        AppWrite,
        Cassandra,
        DynamoDB,
        CosmosDB,

        //kv
        Redis,
    }

    public enum CasingType
    {
        ToLowerCase,
        ToUpperCase,
        ToTitleCase
    }

    public enum EventType
    {
        Unknown,
        Request,
        Response,
        Command,
        Query,
        Notify,
        Scheduler,
        Action,
        Create,
        Edit,
        Update, 
        Delete,
        Invite,  
        Accept, 
        Reject, 
        Remove,
        Revoke,
        Block, 
        Unblock, 
        Promote,
        Demote
    }

    public enum ErrorType
    {
        None,
        Null,
        Bad,
        Unauthorized,
        Forbidden,
        NotFound,
        Missing,
        Conflict,
        Validation,
        RateLimit,
        Server,
        Network
    }

    public enum TimeUnit
    {
        Seconds = 1,
        Minutes = 1 * 60,
        Hours = 1 * 60 * 60,
        Days = 1 * 60 * 60 * 24,
        Weeks = 1 * 60 * 60 * 24 * 7,
        Months = 1 * 60 * 60 * 24 * 30,
        Years = 1 * 60 * 60 * 24 * 365
    }

    public enum LogicalOperator
    {
        NE, // not_equal
        EQ, // equals
        DO, //contains
        DONT, //doesnt_contains
        SW, // starts_with
        DSW, // doesnt_start_with
        EW, // ends_with
        DEW, //doesnt_end_with
        NULL, // is_null
        EMT, // is_empty
        NOE, //null_or_empty
        VAL, // has_value | not_null_not_empty
        EX, // exists
        DEX, // doesnt_exist
        IN, // includes
        NIN, // not_in
        TYP, // has_type
        NTYP, // hasnt_type
        GT, // greater_than
        GTE, // greater_than_or_equal
        LT, // less_than
        LTE //less_than_or_equal
    }

    public enum PrivacyType 
    {
        Public,
        Private,
        Protected,
        Custom,
        OnlyMe,
        OnlyFans,
        Friend,
        FriendsOfFriend,
        Followers,
        Following,
        Suscribers,
        Subscribed,
    }

    public enum AccessType 
    {
        X, // no permission
        A, // all permissions
        R, // read
        C, // create
        W, // write -> create + update + edit + delete
        U, // update -> put
        E, // edit -> patch
        F, // filter | search | query
        D, // delete
        S, // share
        // Comb -> multiple permissions: CR, RW, ... etc
    }

    public enum PermissionType 
    {
        User,
        Admin,
        Editor,
        Manager, 
        Moderator,
        Member,
        Advertiser, 
        Analyst,
        Player,
        Client,
        Sponsor,
        Vendor,
        System,
        Platform,
        App,
        Api,
        Controller,
        EndPoint,
        Method,
        Service,
        Resource,
        UI,
        Module,
        Page,
        Conntent,
        Component,
        Section,
        Field
    }

    public enum ViolationType 
    {
        Spam, 
        Fraud, 
        Abuse,
        Rule,
        Harrasment,
        Rights, 
        Policies,
        Political,
        Religious,
    }
}
