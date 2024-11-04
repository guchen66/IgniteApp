using IgniteShared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDb.IRepositorys
{
    public interface IStaticticRepository
    {
        List<StaticticDto> GetAll();
    }
}
