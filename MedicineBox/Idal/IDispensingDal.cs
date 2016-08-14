using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Qunau.NetFrameWork.Infrastructure;

namespace Idal
{
    public interface IDispensingDal : IAddRepository<Dispensing>
    {
        /// <summary>
        /// 更新是否配药完成
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="isTake">是否配药完成</param>
        /// <returns>成功=true</returns>
        bool UpdateIsTake(int id, int isTake);

        /// <summary>
        /// 重置配药完成情况（每日凌晨用）
        /// </summary>
        /// <returns></returns>
        bool ResetIsTake();

        /// <summary>
        /// 删除配药信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>成功=true</returns>
        bool DeleteDispensing(int id);

        /// <summary>
        /// 查询配药信息
        /// </summary>
        /// <param name="patientId">病人ID</param>
        /// <returns></returns>
        List<Dispensing> QueryDispensingsByPatientId(int patientId);

        /// <summary>
        /// 查询第一个配药信息
        /// </summary>
        /// <param name="patientId">病人ID</param>
        /// <returns></returns>
        Dispensing QueryFirstDispensingByPatientId(int patientId);
    }
}
