using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Qunau.NetFrameWork.Infrastructure;

namespace Idal
{
    public interface IMedicineDal : IAddRepository<Medicine>
    {
        /// <summary>
        /// 查询药品信息
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>药品信息</returns>
        Medicine QueryMedicineById(int id);
    }
}
