using IgniteShared.Dtos;
using System.Collections.Generic;

namespace IgniteDb.IRepositorys
{
    public interface IStaticticRepository
    {
        List<StaticticDto> GetAll();
    }
}
