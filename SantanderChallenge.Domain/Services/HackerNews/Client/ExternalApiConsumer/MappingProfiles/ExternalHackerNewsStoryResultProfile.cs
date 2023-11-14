using AutoMapper;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;

namespace SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.MappingProfiles;

public class ExternalHackerNewsStoryResultProfile : Profile
{
    public ExternalHackerNewsStoryResultProfile()
    {
        CreateMap<ExternalHackerNewsStoryResult, HackerNewsStory>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.title))
            .ForMember(dest => dest.Uri, opt => opt.MapFrom(src => src.url))
            .ForMember(dest => dest.PostedBy, opt => opt.MapFrom(src => src.by))
            .ForMember(dest => dest.Time, opt => opt.MapFrom(src => GetTimeFromEpochTimeInt(src.time)))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.score))
            .ConstructUsing((source, context) => new HackerNewsStory(
                source.title,
                source.url,
                source.by,
                GetTimeFromEpochTimeInt(source.time),
                source.score,
                source.descendants // can't find comment count .. so just mapping to this.
            ));
        ;
    }

    public DateTime GetTimeFromEpochTimeInt(int epochTime)
    {
        var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(epochTime);
        return dateTimeOffset.UtcDateTime;
    }
}