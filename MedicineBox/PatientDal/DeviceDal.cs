using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idal;
using Model;
using Qunau.NetFrameWork.Common.Exception;
using Qunau.NetFrameWork.Common.Extension;
using Qunau.NetFrameWork.Common.Write;
using Qunau.NetFrameWork.DbCommon;
using Qunau.NetFrameWork.Infrastructure;

namespace Dal
{
    public class DeviceDal : BaseRepository, IDeviceDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Qunau.NetFrameWork.DbCommon.BaseRepository"/> class.
        ///             构造函数
        /// </summary>
        /// <param name="unit">工作单元</param><param name="name">链接名称</param>
        public DeviceDal(IUnitOfWork unit, string name) : base(unit, name)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>
        /// 返回结果
        /// </returns>
        public long Add(Device entity)
        {
            this.ClearParameters();
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO Device (");
            sql.Append(" DeviceNo, ");
            sql.Append(" DeviceIP, ");
            sql.Append(" Deleted ");
            sql.Append(") VALUES(");
            sql.Append(" @DeviceNo, ");
            sql.Append(" @DeviceIP, ");
            sql.Append(" @Deleted ");
            sql.Append("); ");
            sql.Append("SELECT @Id:= LAST_INSERT_ID(); ");

            this.AddParameter("@DeviceNo", entity.DeviceNo);
            this.AddParameter("@DeviceIP", entity.DeviceIP);
            this.AddParameter("@Deleted", entity.Deleted);

            object obj = this.ExecuteScalar(sql.ToString());
            var result = obj == null ? 0 : Convert.ToInt64(obj);
            if (result <= 0)
            {
                throw new CustomException("添加失败");
            }
            return result;
        }

        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>设备信息</returns>
        public Device QueryDeviceById(int id)
        {
            this.ClearParameters();
            string sql = "SELECT * FROM Device WHERE Id = @Id AND Deleted = 0";
            this.AddParameter("@Id", id);
            return this.ExecuteReader(sql).ToModel<Device>();
        }

        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        /// <returns>设备信息</returns>
        public Device QueryDeviceByDeviceNo(int deviceNo)
        {
            this.ClearParameters();
            string sql = "SELECT * FROM Device WHERE DeviceNo = @DeviceNo AND Deleted = 0";
            this.AddParameter("@DeviceNo", deviceNo);
            return this.ExecuteReader(sql).ToModel<Device>();
        }

        /// <summary>
        /// 更新Ip地址
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="ip">IP地址信息</param>
        /// <returns>成功= true</returns>
        public bool UpdateIp(int id, string ip)
        {
            this.ClearParameters();
            string sql = "UPDATE Device SET DeviceIP = @DeviceIP WHERE Id = @Id";
            this.AddParameter("@DeviceIP", ip);
            this.AddParameter("@Id", id);
            int res = this.ExecuteNonQuery(sql);
            if (res != 1)
            {
                LogService.WriteLog($"更新IP地址异常,主键：{id},IP:{ip}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新设备编号
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="deviceNo">设备编号</param>
        /// <returns>成功= true</returns>
        public bool UpdateDeviceNo(int id, int deviceNo)
        {
            this.ClearParameters();
            string sql = "UPDATE Device SET DeviceNo = @DeviceNo WHERE Id = @Id";
            this.AddParameter("@DeviceNo", deviceNo);
            this.AddParameter("@Id", id);
            int res = this.ExecuteNonQuery(sql);
            if (res != 1)
            {
                LogService.WriteLog($"更新设备编号异常,主键：{id},DeviceNo:{deviceNo}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据设备编号查询病人主键
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        /// <returns></returns>
        public int QueryPatientIdByDeviceNo(int deviceNo)
        {
            this.ClearParameters();
            string sql =
                "SELECT p.Id FROM Device as d LEFT JOIN Patient as p ON d.Id = p.DeviceId WHERE d.Deleted = 0 AND p.Deleted = 0 AND d.DeviceNo = @DeviceNo";
            this.AddParameter("@DeviceNo", deviceNo);
            object obj = this.ExecuteScalar(sql);
            int res = obj == null ? 0 : Convert.ToInt32(obj);
            return res;
        }

        /// <summary>
        /// 判断设备是否存在
        /// </summary>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        public bool IsExistByDeviceNo(int deviceNo)
        {
            this.ClearParameters();
            string sql = "SELECT COUNT(0) FROM Device WHERE DeviceNo = @DeviceNo AND Deleted = 0";
            this.AddParameter("@DeviceNo", deviceNo);
            object obj = this.ExecuteScalar(sql);
            int res = obj == null ? 0 : Convert.ToInt32(obj);
            return res != 0;
        }
    }
}
