using System.Collections.Generic;
using AutoMapper;
using LearnEnglish.XN.Core.Definitions.DalModels;
using LearnEnglish.XN.Core.Definitions.Enums;
using LearnEnglish.XN.Core.Definitions.Extensions;
using LearnEnglish.XN.Core.ViewModels.Items;
using Newtonsoft.Json;

namespace LearnEnglish.XN.Core;

public static class Mapper
{
    internal static IMapper Build() => GetMapperConfiguration().CreateMapper();

    private static MapperConfiguration GetMapperConfiguration() => new MapperConfiguration(cfg =>
    {
        cfg.CreateMap<MessageViewModel, MessageDalModel>()
            .ForMember(d => d.Id, s => s.Ignore())
            .ForMember(d => d.Variants, s => s.MapFrom((m, _) => m.Variants?.Then<IEnumerable<VariantViewModel>, string>(JsonConvert.SerializeObject)))
            .ValidateMemberList(MemberList.Destination);

        cfg.CreateMap<MessageDalModel, MessageViewModel>()
            .ForMember(d => d.Variants, s => s.MapFrom((m, _) => m.Variants?.Then<string, IEnumerable<VariantViewModel>>(JsonConvert.DeserializeObject<IEnumerable<VariantViewModel>>)))
            .ForMember(d => d.SelectVariantCommand, s => s.Ignore())
            .ValidateMemberList(MemberList.Destination);

        cfg.CreateMap<VariantViewModel, MessageViewModel>()
            .ForMember(d => d.MessageType, s => s.MapFrom(m => MessageTypes.Mine))
            .ForMember(d => d.Variants, s => s.Ignore())
            .ValidateMemberList(MemberList.Destination);
    });
}
