﻿using System;
using System.Threading.Tasks;
using HelpfulThings.Connect.Cryptowatch.DataModel;
using HelpfulThings.Connect.Cryptowatch.Ioc;
using HelpfulThings.Connect.Cryptowatch.Result;

namespace HelpfulThings.Connect.Cryptowatch.Metering
{
    public interface IRequestMeteringMonitor
    {
        long MeterMaxValue { get; }
        long MeterCurrentValue { get; }
        MeteringResult CheckMeter();
        long GetSerial();
        void RegisterResult(long requestSerial, Allowance requestAllowance);
    }

    public class RequestMeteringMonitor : IRequestMeteringMonitor
    {
        private readonly long _meterMaxValue;
        private readonly long _stopThreshold;

        private readonly object _serializerLock;
        private readonly object _resultsProcessingLock;
        
        private long _requestSerial;
        private long _processedSerial;
        private long _currentMeterValue;

        public long MeterMaxValue => _meterMaxValue;
        public long MeterCurrentValue
        {
            get
            {
                lock (_resultsProcessingLock)
                {
                    return _currentMeterValue;
                }
            }
        }

        public RequestMeteringMonitor(CryptowatchApiOptions configuration)
        {
            _meterMaxValue = configuration.RequestMeterMaximum;
            
            var stopThresholdPercentage = configuration.StopThresholdPercentage;
            _stopThreshold = (long) Math.Round(_meterMaxValue * stopThresholdPercentage);

            _serializerLock = new object();
            _resultsProcessingLock = new object();
            
            _requestSerial = 0;
            _processedSerial = 0;
            _currentMeterValue = _meterMaxValue;

            Task.Run(MeterResetWorker);
        }

        private void MeterResetWorker()
        {
            while (true)
            {
                var timeOfDay = DateTime.UtcNow.TimeOfDay;
                var nextFullHour = TimeSpan.FromHours(Math.Ceiling(timeOfDay.TotalHours));
                var delta = nextFullHour - timeOfDay;

                Task.Delay(delta);
                
                _currentMeterValue = _meterMaxValue;
            }
        }

        public MeteringResult CheckMeter()
        {
            lock (_resultsProcessingLock)
            {
                if (_currentMeterValue > _stopThreshold)
                {
                    return MeteringResult.Proceeded;
                }
            }

            return MeteringResult.Reject;
        }

        public long GetSerial()
        {
            long returnSerial;

            lock (_serializerLock)
            {
                returnSerial = _requestSerial;
                _requestSerial++;
            }

            return returnSerial;
        }

        public void RegisterResult(long requestSerial, Allowance requestAllowance)
        {
            lock (_resultsProcessingLock)
            {
                if (requestSerial > _processedSerial)
                {
                    _processedSerial = requestSerial;
                    _currentMeterValue = requestAllowance.Remaining;
                }
            }
        }
    }
}
