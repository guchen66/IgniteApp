using AutoMapper;
using IgniteDb.IRepositorys;
using IgniteShared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDb.Repositorys
{
    public class StaticticRepository : IStaticticRepository
    {
        private readonly IMapper _mapper;
        private readonly AccessDbContext _context;

        public StaticticRepository(AccessDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public List<StaticticDto> GetAll()
        {
            var staticticInfos = _context.StaticticInfos.ToList();
            List<StaticticDto> dtos = _mapper.Map<List<StaticticDto>>(staticticInfos);
            return dtos;
        }
    }
}