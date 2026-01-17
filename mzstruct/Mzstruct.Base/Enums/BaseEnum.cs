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

    public enum LogicalOperator
    {
        equals,
        not_equal,
        contains,
        doesnt_contain,
        starts_with,
        doesnt_start_with,
        ends_with,
        doesnt_end_with,
        is_null_or_empty,
        isnt_null_or_empty,
        exists,
        doesnt_exist,
        is_in,
        is_not_in,
        has_type,
        doesnt_has_type,
        greater_than,
        greater_than_or_equal,
        less_than,
        less_than_or_equal,
    }
}
