using AutoMapper;
using SantanderChallenge.Domain.Services.HackerNews.Client.ExternalApiConsumer.Models;
using SantanderChallenge.WebApi.ApiResponseModels;

namespace SantanderChallenge.WebApi.MappingProfiles;

public class HackerNewsStoryMappingProfile : Profile // AutoMapper Profile
{
    public HackerNewsStoryMappingProfile()
    {
        CreateMap<HackerNewsStory, HackerNewsArticleResponse>()
            .ConstructUsing((source, context) => new HackerNewsArticleResponse(
                source.Title,
                source.Uri,
                source.PostedBy,
                source.Time,
                source.Score,
                source.CommentCount
            ));
    }
}