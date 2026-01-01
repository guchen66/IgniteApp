using IgniteShared.Entitys;
using System.Collections.Generic;

namespace IgniteDb.IRepositorys
{
    public interface IMaterialRepository
    {
        void AddMaterial(MaterialInfo materialInfo);

        List<MaterialInfo> GetAllMaterialInfo();
    }
}
