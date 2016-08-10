using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Qunau.NetFrameWork.Infrastructure;

namespace Idal
{
    /// <summary>
    /// 设备表接口
    /// </summary>
    public interface IDeviceDal : IAddRepository<Device>
    {
        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>设备信息</returns>
        Device QueryDeviceById(int id);

        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        /// <returns>设备信息</returns>
        Device QueryDeviceByDeviceNo(int deviceNo);

        /// <summary>
        /// 更新Ip地址
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="ip">IP地址信息</param>
        /// <returns>成功= true</returns>
        bool UpdateIp(int id, string ip);

        /// <summary>
        /// 更新设备编号
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="deviceNo">设备编号</param>
        /// <returns>成功= true</returns>
        bool UpdateDeviceNo(int id, int deviceNo);

        /// <summary>
        /// 根据设备编号查询病人主键
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        /// <returns></returns>
        int QueryPatientIdByDeviceNo(int deviceNo);

        /// <summary>
        /// 判断设备是否存在
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        bool IsExistByDeviceNo(int deviceNo);
    }
}
