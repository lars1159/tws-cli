﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IBApi;
using TradeBot.Events;
using TradeBot.Extensions;
using TradeBot.TwsAbstractions;
using static TradeBot.AppProperties;

namespace TradeBot
{
    public class TradeBotHeader
    {
        private TradeBotConsole console;
        private TradeBotService service;

        public TradeBotHeader(TradeBotConsole console, TradeBotService service)
        {
            this.console = console;
            this.service = service;

            service.PropertyChanged += OnPropertyChanged;
            service.TickUpdated += OnTickUpdated;
            service.PositionUpdated += OnPositionUpdated;
        }

        private void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            switch (eventArgs.PropertyName)
            {
                case nameof(service.TickerSymbol):
                    UpdateHeaderAsync();
                    break;
            }
        }

        private void OnTickUpdated(int tickType, double value)
        {
            UpdateHeaderAsync();
        }

        private void OnPositionUpdated(Position position)
        {
            UpdateHeaderAsync();
        }

        private async Task UpdateHeaderAsync()
        {
            IList<string> infoStrings = new List<string>();

            string appName = Messages.AppName;
            if (!string.IsNullOrWhiteSpace(appName))
            {
                infoStrings.Add(appName);
            }

            bool hasTickerSymbol = service.HasTickerSymbol;
            string tickerSymbol = service.TickerSymbol;
            string tickerDisplayValue = hasTickerSymbol ? tickerSymbol : Messages.TitleUnavailable;
            infoStrings.Add(string.Format(Messages.TitleTickerSymbol, tickerDisplayValue));

            infoStrings.Add(string.Format(Messages.TitleShares, console.Shares));

            if (hasTickerSymbol)
            {
                Position currentPosition = await service.RequestCurrentPositionAsync();
                double positionSize = currentPosition?.PositionSize ?? 0;
                infoStrings.Add(string.Format(Messages.TitlePositionSize, positionSize));

                infoStrings.Add(string.Format(Messages.TitleLastFormat, GetTickAsCurrencyString(TickType.LAST)));
                infoStrings.Add(string.Format(Messages.TitleBidAskFormat, GetTickAsCurrencyString(TickType.BID), GetTickAsCurrencyString(TickType.ASK)));
                infoStrings.Add(string.Format(Messages.TitleVolumeFormat, GetTickAsString(TickType.VOLUME)));
                infoStrings.Add(string.Format(Messages.TitleCloseFormat, GetTickAsCurrencyString(TickType.CLOSE)));
                infoStrings.Add(string.Format(Messages.TitleOpenFormat, GetTickAsCurrencyString(TickType.OPEN)));
            }

            Console.Title = string.Join(Messages.TitleDivider, infoStrings);
        }

        private string GetTickAsString(int tickType)
        {
            return GetTickAsFormattedString(tickType, v => v.ToString());
        }

        private string GetTickAsCurrencyString(int tickType)
        {
            return GetTickAsFormattedString(tickType, v => v.ToCurrencyString());
        }

        private string GetTickAsFormattedString(int tickType, Func<double, string> messageFormatter)
        {
            double? tick = service.GetTick(tickType);
            return tick.HasValue && tick.Value >= 0
                ? messageFormatter(tick.Value)
                : Messages.TitleUnavailable;
        }
    }
}