using FiveDayBibleReading.CoreLibrary.Extensions;
using FiveDayBibleReading.CoreLibrary.Services;
using Microsoft.Extensions.DependencyInjection;
namespace FiveDayBibleReading.ReaderLibrary.Core.Extensions;
public static class ServiceExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection RegisterFileStorageBasicBibleServices<R, C>()
            where R : class, IReadingPath
            where C : class, IBibleCategoryService
        {
            services.RegisterBasicBibleServices<C, BibleFileStorage>()
                .AddScoped<IReadingPath, R>();
            return services;
        }

        public IServiceCollection RegisterFileStorageBasicBibleServices<T, C, R>()
            where T : class, ITranslationPath
            where R : class, IReadingPath
            where C : class, IBibleCategoryService
        {
            services.RegisterBasicBibleServices<TranslationFileStorage, C, BibleFileStorage>()
                .AddScoped<IReadingPath, R>()
                .AddScoped<ITranslationPath, T>();
            return services;
        }
        public IServiceCollection RegisterBasicBibleServices<C, B>()
            where B : class, IBibleStorage
            where C : class, IBibleCategoryService
        {
            services.RegisterBlazorBeginningClasses()
                .RegisterRealDatePicker();
            services.RegisterBibleReadingServices<C>()
                .AddTransient<BibleReaderService>()
                .AddTransient<IBibleContent, BibleContent>()
                .AddScoped<IBibleStorage, B>();

            return services;
        }
        public IServiceCollection RegisterBasicBibleServices<T, C, B>()
            where T : class, ITranslationStorage
            where B : class, IBibleStorage
            where C : class, IBibleCategoryService
        {
            services.RegisterBasicBibleServices<C, B>()
                .AddScoped<ITranslationStorage, T>();
            return services;
        }
        public IServiceCollection RegisterLocalStorageBasicBibleServices<C>()
            where C : class, IBibleCategoryService
        {
            return services.RegisterBasicBibleServices<LocalStorageTranslationStorage, C, LocalStorageBibleStorage>();
        }

    }
}
