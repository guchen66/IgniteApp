using IgniteDb.IRepositorys;
using IgniteShared.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteDb.Repositorys
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AccessDbContext _context;
        public MaterialRepository(AccessDbContext context)
        {
            _context = context;
        }
        public void AddMaterial(MaterialInfo materialInfo)
        {
            _context.MaterialInfos.Add(materialInfo);
            _context.SaveChanges();
        }

        public List<MaterialInfo> GetAllMaterialInfo()
        {
            return _context.MaterialInfos.ToList();
        }
    }
}
