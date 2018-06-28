﻿using System;
using System.Collections.Generic;

using CryptoExchange.Net.Converters;

using Newtonsoft.Json;

/// <summary>Order book type</summary>
public enum OrderBookType
{
    /// <summary>Only show buy orders</summary>
    Buy,

    /// <summary>Only show sell orders</summary>
    Sell,

    /// <summary>Show all orders</summary>
    Both
}

/// <summary>Whether the order is partially or fully filled</summary>
public enum FillType
{
    Fill,

    PartialFill
}

public enum OrderSide
{
    Buy,

    Sell
}

public enum OrderType
{
    Limit,

    Market
}

public enum OrderSideExtended
{
    LimitBuy,

    LimitSell
}

public enum TickInterval
{
    OneMinute,

    FiveMinutes,

    HalfHour,

    OneHour,

    OneDay
}

public enum TimeInEffect
{
    GoodTillCancelled,

    ImmediateOrCancel
}

public enum ConditionType
{
    None,

    GreaterThan,

    LessThan,

    StopLossFixed,

    StopLossPercentage
}

public enum OrderUpdateType
{
    Open,

    PartialFill,

    Fill,

    Cancel
}

public class BittrexStreamOrderBookUpdateEntry : BittrexStreamOrderBookEntry
{
    /// <summary>how to handle data (used by stream)</summary>
    [JsonProperty("TY")]
    public OrderBookEntryType Type { get; set; }
}

public class BittrexStreamOrderBookEntry
{
    /// <summary>Total quantity of order at this price</summary>
    [JsonProperty("Q")]
    public decimal Quantity { get; set; }

    /// <summary>Price of the orders</summary>
    [JsonProperty("R")]
    public decimal Rate { get; set; }
}

public enum OrderBookEntryType
{
    NewEntry = 0,

    RemoveEntry = 1,

    UpdateEntry = 2
}

public class BittrexStreamUpdateExchangeState
{
    [JsonProperty("N")]
    public long Nonce { get; set; }

    /// <summary>Name of the market</summary>
    [JsonProperty("M")]
    public string MarketName { get; set; }

    /// <summary>Buys in the order book</summary>
    [JsonProperty("Z")]
    public List<BittrexStreamOrderBookUpdateEntry> Buys { get; set; }

    /// <summary>Sells in the order book</summary>
    [JsonProperty("S")]
    public List<BittrexStreamOrderBookUpdateEntry> Sells { get; set; }

    /// <summary>Market history</summary>
    [JsonProperty("f")]
    public List<BittrexStreamFill> Fills { get; set; }
}

public class BittrexStreamFill
{
    /// <summary>Timestamp of the fill</summary>
    [JsonProperty("T")]
    [JsonConverter(typeof(TimestampConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>Quantity of the fill</summary>
    [JsonProperty("Q")]
    public decimal Quantity { get; set; }

    /// <summary>Rate of the fill</summary>
    [JsonProperty("R")]
    public decimal Rate { get; set; }

    /// <summary>The side of the order</summary>
    [JsonConverter(typeof(OrderSideConverter))]
    [JsonProperty("OT")]
    public OrderSide OrderType { get; set; }
}

public class BittrexStreamQueryExchangeState
{
    [JsonProperty("N")]
    public long Nonce { get; set; }

    /// <summary>Name of the market</summary>
    [JsonProperty("M")]
    public string MarketName { get; set; }

    /// <summary>Buys in the order book</summary>
    [JsonProperty("Z")]
    public List<BittrexStreamOrderBookEntry> Buys { get; set; }

    /// <summary>Sells in the order book</summary>
    [JsonProperty("S")]
    public List<BittrexStreamOrderBookEntry> Sells { get; set; }

    /// <summary>Market history</summary>
    [JsonProperty("f")]
    public List<BittrexStreamMarketHistory> Fills { get; set; }
}

public class OrderSideConverter : BaseConverter<OrderSide>
{
    public OrderSideConverter()
        : this(true)
    {
    }

    public OrderSideConverter(bool quotes)
        : base(quotes)
    {
    }

    protected override Dictionary<OrderSide, string> Mapping => new Dictionary<OrderSide, string> { { OrderSide.Buy, "BUY" }, { OrderSide.Sell, "SELL" } };
}

public class BittrexStreamMarketHistory
{
    /// <summary>The order id</summary>
    [JsonProperty("I")]
    public long Id { get; set; }

    /// <summary>Timestamp of the order</summary>
    [JsonConverter(typeof(TimestampConverter))]
    [JsonProperty("T")]
    public DateTime Timestamp { get; set; }

    /// <summary>Quantity of the order</summary>
    [JsonProperty("Q")]
    public decimal Quantity { get; set; }

    /// <summary>Price of the order</summary>
    [JsonProperty("P")]
    public decimal Price { get; set; }

    /// <summary>Total price of the order</summary>
    [JsonProperty("t")]
    public decimal Total { get; set; }

    /// <summary>Whether the order was fully filled</summary>
    [JsonConverter(typeof(FillTypeConverter))]
    [JsonProperty("F")]
    public FillType FillType { get; set; }

    /// <summary>The side of the order</summary>
    [JsonConverter(typeof(OrderSideConverter))]
    [JsonProperty("OT")]
    public OrderSide OrderType { get; set; }

    public class FillTypeConverter : BaseConverter<FillType>
    {
        public FillTypeConverter()
            : this(true)
        {
        }

        public FillTypeConverter(bool quotes)
            : base(quotes)
        {
        }

        protected override Dictionary<FillType, string> Mapping => new Dictionary<FillType, string> { { FillType.Fill, "FILL" }, { FillType.PartialFill, "PARTIAL_FILL" } };
    }
}