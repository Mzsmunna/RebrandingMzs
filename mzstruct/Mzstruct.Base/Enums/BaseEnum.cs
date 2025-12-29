using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Enums
{
    public enum DBType
    {
        Default, // SqlClient | SqlServer
        InMemory,

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
        Redis,
        CosmosDB,
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
        Create,
        Update
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
}
