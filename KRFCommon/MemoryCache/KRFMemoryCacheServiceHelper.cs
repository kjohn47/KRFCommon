﻿namespace KRFCommon.MemoryCache
{
    using System;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static class KRFMemoryCacheServiceHelper
    {
        public static IServiceCollection AddKRFMemoryCache( this IServiceCollection services, KRFMemoryCacheSettings settings = null )
        {
            if ( services == null )
            {
                throw new ArgumentNullException( nameof( services ) );
            }

            var cacheSettings = settings ?? new KRFMemoryCacheSettings();

            services.AddOptions();

            services.TryAdd( ServiceDescriptor.Singleton<IKRFMemoryCache, KRFMemoryCache>() );

            services.Configure<KRFMemoryCacheOptions>( x => {
                x.ExpirationScanFrequency = new TimeSpan(
                    cacheSettings.CacheCleanupInterval.Hours,
                    cacheSettings.CacheCleanupInterval.Minutes,
                    cacheSettings.CacheCleanupInterval.Seconds );

                if ( cacheSettings.MemoryCacheSize != null )
                {
                    if ( cacheSettings.MemoryCacheSize.MaxSize.HasValue )
                    {
                        x.SizeLimit = cacheSettings.MemoryCacheSize.MaxSize.Value;
                    }

                    if ( cacheSettings.MemoryCacheSize.CompactionPercentage.HasValue )
                    {
                        x.SizeLimit = cacheSettings.MemoryCacheSize.CompactionPercentage.Value;
                    }
                }

                x.CachedKeySettings = cacheSettings.CachedKeySettings;
            } );

            return services;
        }

        public static IServiceCollection AddKRFMemoryCache( this IServiceCollection services, IConfiguration configuration )
        {
            if ( services == null )
            {
                throw new ArgumentNullException( nameof( services ) );
            }

            KRFMemoryCacheSettings cacheSettings;
            if ( configuration != null && configuration.Get<KRFMemoryCacheSettings>() != null )
            {
                cacheSettings = configuration.Get<KRFMemoryCacheSettings>();
            }
            else
            {
                cacheSettings = new KRFMemoryCacheSettings();
            }

            services.AddOptions();

            services.TryAdd( ServiceDescriptor.Singleton<IKRFMemoryCache, KRFMemoryCache>() );

            services.Configure<KRFMemoryCacheOptions>( x => {
                x.ExpirationScanFrequency = new TimeSpan(
                    cacheSettings.CacheCleanupInterval.Hours,
                    cacheSettings.CacheCleanupInterval.Minutes,
                    cacheSettings.CacheCleanupInterval.Seconds );

                if ( cacheSettings.MemoryCacheSize != null )
                {
                    if ( cacheSettings.MemoryCacheSize.MaxSize.HasValue )
                    {
                        x.SizeLimit = cacheSettings.MemoryCacheSize.MaxSize.Value;
                    }

                    if ( cacheSettings.MemoryCacheSize.CompactionPercentage.HasValue )
                    {
                        x.SizeLimit = cacheSettings.MemoryCacheSize.CompactionPercentage.Value;
                    }
                }

                x.CachedKeySettings = cacheSettings.CachedKeySettings;
            } );

            return services;
        }
    }
}
