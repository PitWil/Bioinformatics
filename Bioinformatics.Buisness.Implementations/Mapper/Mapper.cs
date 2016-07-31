using System.Collections.Generic;
using Bioinformatics.Buisness.Models;

namespace Bioinformatics.Buisness.Implementations.Mapper
{
    public class Mapper
    {
        public static void Initialize()
        {
            AutoMapper.Mapper
                .CreateMap<ProteinNode.ProteinWeightNode, Persistence.Entities.ProteinNode.ProteinWeigthNode>()
                .ForMember(a => a.Weigth, b => b.MapFrom(c => c.Weight))
                .ForMember(a => a.Neighbors, b => b.MapFrom(c => c.Node));
            AutoMapper.Mapper
                .CreateMap<Persistence.Entities.ProteinNode.ProteinWeigthNode, ProteinNode.ProteinWeightNode>()
                .ForMember(a => a.Weight, b => b.MapFrom(c => c.Weigth))
                .ForMember(a => a.Node, b => b.MapFrom(c => c.Neighbors));

            AutoMapper.Mapper.CreateMap<ProteinNode, Persistence.Entities.ProteinNode>()
                .ForMember(a => a.Value, b => b.MapFrom(c => c.Value))
                .ForMember(a => a.Neighbors, b => b.MapFrom(c => c.Neighbors));

            AutoMapper.Mapper.CreateMap<Persistence.Entities.ProteinNode, ProteinNode>()
                .ForMember(a => a.Value, b => b.MapFrom(c => c.Value))
                .ForMember(a => a.Neighbors, b => b.MapFrom(c => c.Neighbors));


            AutoMapper.Mapper.CreateMap<List<ProteinNode>, List<Persistence.Entities.ProteinNode>>().ConstructUsing(q);
            AutoMapper.Mapper.CreateMap<List<Persistence.Entities.ProteinNode>, List<ProteinNode>>().ConstructUsing(p);
            //    AutoMapper.Mapper.AssertConfigurationIsValid();
        }

        private static List<ProteinNode> p(List<Persistence.Entities.ProteinNode> nodes)
        {
            var result = new List<ProteinNode>();
            nodes.ForEach(z => result.Add(AutoMapper.Mapper.Map<ProteinNode>(z)));
            return result;
        }

        private static List<Persistence.Entities.ProteinNode> q(List<ProteinNode> nodes)
        {
            var result = new List<Persistence.Entities.ProteinNode>();
            nodes.ForEach(z => result.Add(AutoMapper.Mapper.Map<Persistence.Entities.ProteinNode>(z)));
            return result;
        }
    }
}