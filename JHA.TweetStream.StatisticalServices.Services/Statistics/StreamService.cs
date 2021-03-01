#region Namespace
using JHA.TweetStream.StatisticalServices.Models.Settings;
using JHA.TweetStream.StatisticalServices.Models.Statistics;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
#endregion

namespace JHA.TweetStream.StatisticalServices.Services.Statistics
{
    public class StreamService : BackgroundService
    {
        #region Variables
        private readonly ILogger<StreamService> _logger;
        private AppSettings _appSettings;
        private readonly StatisticsService _tweetStatistics;
        private int _batchSize = 20;
        private Action<List<Tweet>> _callBack;
        private ConcurrentQueue<Tweet> _batchTweets = new ConcurrentQueue<Tweet>();
        private CancellationToken _cancelToken;
        private object _statisticsLock = new object();
        private int _actualRetries = 0;
        private HttpClient _httpClient;
        #endregion

        public StreamService(ILogger<StreamService> logger, IOptions<AppSettings> options, StatisticsService tweetStatistics)
        {
            _tweetStatistics = tweetStatistics;
            _appSettings = options?.Value;
            _logger = logger;
        }

        HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null) { _httpClient = buildHttpClient(); }                
                return _httpClient;
            }
        }

        public async Task DownloadSampleStream(Action<List<Tweet>> callback, CancellationToken cancelToken, int? batchSize = null)
        {
            _callBack = callback;
            _cancelToken = cancelToken;

            if (batchSize.HasValue) { _batchSize = batchSize.Value; }                

            StartStatistics(cancelToken);

            while (0 > _appSettings.RetryLimit || _actualRetries <= _appSettings.RetryLimit)
            {
                if (0 < _actualRetries)
                {
                    _logger.LogWarning($"Failed to connect to Sample Twitter API. Re-try attempt {_actualRetries}");
                    Thread.Sleep(1000 * _actualRetries);
                }

                try
                {
                    using (var response = await HttpClient.GetAsync($"{_appSettings.StreamTweetUrl}?tweet.fields=entities", HttpCompletionOption.ResponseHeadersRead))
                    {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                while (!reader.EndOfStream)
                                {
                                    if (_cancelToken.IsCancellationRequested) { break; }                                     
                                    _actualRetries = 0; 
                                    var line = reader.ReadLine();
                                    if (String.IsNullOrEmpty(line)) { continue; }

                                    try
                                    {
                                        var tweet = JsonConvert.DeserializeObject<Tweet>(line);                                      
                                        _batchTweets.Enqueue(tweet);                                       
                                    }
                                    catch
                                    {
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    ++_actualRetries;
                }
            }

            _logger.LogInformation("DownloadSampleStream exiting.");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return DownloadSampleStream(ProcessBatch, stoppingToken);
        }

        private HttpClient buildHttpClient()
        {
            var result = new HttpClient();

            result.BaseAddress = new Uri(_appSettings.StreamTweetUrl);
            result.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _appSettings.AuthorizeToken);
            result.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

            return result;
        }

        private void ProcessLocalList()
        {
            if (_callBack == null) { return; }

            var localList = new List<Tweet>();

            for (int index = 0; index < _batchSize; index++)
            {
                Tweet t;
                if (_batchTweets.TryDequeue(out t)) { localList.Add(t); }
            }

            _callBack(localList);
        }

        private void StartStatistics(CancellationToken cancelToken)
        {
            Task.Run(async () =>
            {
                do
                {
                    if (cancelToken.IsCancellationRequested) break;
                    if (_batchSize <= _batchTweets.Count) { _ = Task.Run(() => ProcessLocalList()); }
                    Thread.Sleep(250);
                } while (true);
            });
        }

        void ProcessBatch(List<Tweet> tweets)
        {
            lock (_statisticsLock)
            {
                Task.Run(() => _tweetStatistics.ProcessTweet(tweets));
            }
        }
           
    }
}
