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

    public enum CacheType
    {
        InMemory,
        CacheAside,
        CacheThrough,
        Distributed,
        Redis,
        Valkey,
        KeyDB,
        Dragonfly,
        Memcached,
        Hazelcast,
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
        Server, // unnexpcted | unhandled server error
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
        Protected, // only can be accessed via specific links / urls / qrcs / ids
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
        Role,
        User,
        Admin,
        Agent,
        Agency,
        Editor,
        Manager, 
        Moderator,
        Group,
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
        WebHook,
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

    public enum ResourceType 
    {
        DB,
        Blob,
        Cache,
        Queue,
        Cloud,
        Collection,
        Table,
        Document,
        File,
        PDF,
        Image,
        Video,
        Audio,
        Text,
        HTML,
        XML,
        Json,
        DOC,
        XLS,
        PPT,
        CSV,
        Bson,
        Binary,
        Base64,
        Url,
        Path,
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

    public enum PaymentType
    {
        OneTime,
        Upgrade,
        Renewal,
        Recurring, 
        Installment,
        Deferred,
        Split,
        Partial,
        Full,
        Advance,
        Credit,
        Debit,
        Deposit,
        Prepaid,
        Postpaid,
        Automatic,
        Manual,
        Instant,
        Scheduled,
        Conditional,
    }

    public enum PaymentMethod 
    {
        CreditCard,
        DebitCard,
        BankTransfer,
        PayPal,
        Stripe,
        Square,
        ApplePay,
        GooglePay,
        Crypto,
        Bitcoin,
        Ethereum,
        StableCoin,
        Cash,
        Check,
        MobilePayment,
        EWallet,
        GiftCard,
        Voucher,
        LoyaltyPoints,
        Barter,
    }

    public enum PurchaseType 
    {
        Order, 
        Subscription, 
        Membership,
        Enrollment,
        Ticket,
        Donation,
        Tip,
        Gift,
        GiftCard,
        Reservation,
        Booking,
        Rental,
        Lease,
        Auction,
        Bid,
        Service,
        Product,
        Package,
        AddOn,
        Trial,        
    }

    public enum OfferType 
    {
        Bundle,
        Promotion,
        Voucher,
        Coupon,
        Discount,
        EarlyAccess,
        Deal,
        Sale,
        Special,
        Limited,
        Exclusive,
        Seasonal,
        FlashSale,
        PreOrder,
        BackOrder,
        Wholesale,
        Retail,
        Daily,
        Weekly,
        Monthly,
        Holiday,
        NineNine,
        ElevenEleven,
        TwelveTwelve,
        Eid,
        BlackFriday,
        Christmas,
        NewYear,
        Summer,
        Winter,
        Festival,
        Clearance,
        Loyalty,
        Referral,
        Cashback,
        BOGO, // Buy One Get One
    }

    public enum ContextType 
    {
        Any,
        Env,
        Event,
        Entity,
        Class,
        Record,
        Struct,
        This,
        JWT,
        Session,
        Cookie,
        LocalStorage,
        QueryParams,
        RequestBody,
        Payload,
        Header,
        Controller,
        Method,
        EndPoint,
        Repository,
        Response,
        DB,
        Collection,
        Table,
        Document,
        File,
        PDF,
        Image,
        Video,
        Audio,
        Text,
        HTML,
        XML,
        Json,
        Bson,
        Binary,
        Base64,
        Hex,
        Url,
        Path,
        Id,
        Name,
        Role,
        Group,
        Tags,
        Identifiers,
        Email,
        Phone,
    }
}
