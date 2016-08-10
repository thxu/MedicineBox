using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Enum;
using Qunau.NetFrameWork.Infrastructure;

namespace Idal
{
    public interface IPatientDal : IAddRepository<Patient>
    {
        /// <summary>
        /// 更新服药时间信息
        /// </summary>
        /// <param name="info">服药时间信息</param>
        /// <returns>成功= true</returns>
        bool UpdateTakeTimeVal(TakeTimeInfo info);

        /// <summary>
        /// 查询病人信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>病人信息</returns>
        Patient QueryPatientById(int id);

        /// <summary>
        /// 查询所有病人信息
        /// </summary>
        /// <returns></returns>
        List<Patient> QueryAllPatients();

        /// <summary>
        /// 更新服药状态
        /// </summary>
        /// <param name="prop">要更新的字段（早上还是中午还是..）</param>
        /// <param name="status">状态</param>
        /// <param name="id">主键</param>
        /// <returns></returns>
        bool UpdateTakeTimeStatus(string prop, TakeStatus status,int id);
    }
}
