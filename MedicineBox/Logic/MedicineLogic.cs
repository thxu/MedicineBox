using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idal;
using Model;
using Model.Enum;
using Qunau.NetFrameWork.Common.Exception;

namespace Logic
{
    public class MedicineLogic
    {
        /// <summary>
        /// 查询所有病人信息
        /// </summary>
        /// <returns></returns>
        public static List<Patient> QueryAllPatients()
        {
            using (IPatientDal dal = Factory.CreatePatientDalRead())
            {
                return dal.QueryAllPatients();
            }
        }

        /// <summary>
        /// 查询病人信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>病人信息</returns>
        public static Patient QueryPatientById(int id)
        {
            using (IPatientDal dal = Factory.CreatePatientDalRead())
            {
                return dal.QueryPatientById(id);
            }
        }

        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>设备信息</returns>
        public static Device QueryDeviceById(int id)
        {
            using (IDeviceDal dal = Factory.CreateDeviceDalDalRead())
            {
                return dal.QueryDeviceById(id);
            }
        }

        /// <summary>
        /// 查询设备信息
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        /// <returns>设备信息</returns>
        public static Device QueryDeviceByDeviceNo(int deviceNo)
        {
            using (IDeviceDal dal = Factory.CreateDeviceDalDalRead())
            {
                return dal.QueryDeviceByDeviceNo(deviceNo);
            }
        }

        /// <summary>
        /// 判断设备是否存在
        /// </summary>
        /// <param name="deviceNo">设备号</param>
        /// <returns></returns>
        public static bool IsDeviceExistbyDeviceNo(int deviceNo)
        {
            using (IDeviceDal dal = Factory.CreateDeviceDalDalRead())
            {
                return dal.IsExistByDeviceNo(deviceNo);
            }
        }

        /// <summary>
        /// 更新设备IP
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool UpdateDeviceIp(int id, string ip)
        {
            using (IDeviceDal dal = Factory.CreateDeviceDalDalWrite())
            {
                return dal.UpdateIp(id, ip);
            }
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool AddDevice(Device info)
        {
            using (IDeviceDal dal = Factory.CreateDeviceDalDalWrite())
            {
                try
                {
                    dal.Add(info);
                }
                catch (CustomException ex)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 更新设备号
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deviceNo"></param>
        /// <returns></returns>
        public static bool UpdateDeviceNo(int id, int deviceNo)
        {
            using (IDeviceDal dal = Factory.CreateDeviceDalDalWrite())
            {
                return dal.UpdateDeviceNo(id, deviceNo);
            }
        }

        /// <summary>
        /// 更新服药时间
        /// </summary>
        /// <param name="info">服药时间信息</param>
        /// <returns>成功=true</returns>
        public static bool UpdateTakeTime(TakeTimeInfo info)
        {
            using (IPatientDal dal = Factory.CreateMedicineDalWrite())
            {
                return dal.UpdateTakeTimeVal(info);
            }
        }

        /// <summary>
        /// 查询配药信息
        /// </summary>
        /// <param name="patientId">病人ID</param>
        /// <returns></returns>
        public static List<Dispensing> QueryDispensingByPatientId(int patientId)
        {
            using (IDispensingDal dal = Factory.CreateDispensingDalRead())
            {
                return dal.QueryDispensingsByPatientId(patientId);
            }
        }

        /// <summary>
        /// 查询第一个配药信息
        /// </summary>
        /// <param name="patientId">病人主键</param>
        /// <returns></returns>
        public static Dispensing QueryFirstDispensingByPatientId(int patientId)
        {
            using (IDispensingDal dal = Factory.CreateDispensingDalRead())
            {
                return dal.QueryFirstDispensingByPatientId(patientId);
            }
        }


        /// <summary>
        /// 重置配药完成情况
        /// </summary>
        /// <returns></returns>
        public static bool ResetIsTake()
        {
            using (IDispensingDal dal = Factory.CreateDispensingDalWrite())
            {
                return dal.ResetIsTake();
            }
        }

        /// <summary>
        /// 查询药品
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public static Medicine QueryMedicineById(int id)
        {
            using (IMedicineDal dal = Factory.CreateMedicineDalRead())
            {
                return dal.QueryMedicineById(id);
            }
        }

        /// <summary>
        /// 查询病人主键
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        /// <returns></returns>
        public static int QueryPatientIdByDeviceNo(int deviceNo)
        {
            using (IDeviceDal dal = Factory.CreateDeviceDalDalRead())
            {
                return dal.QueryPatientIdByDeviceNo(deviceNo);
            }
        }

        /// <summary>
        /// 更新是否配药完成为 已完成
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public static bool UpdateIsTake(int id)
        {
            using (IDispensingDal dal = Factory.CreateDispensingDalWrite())
            {
                return dal.UpdateIsTake(id, 1);
            }
        }

        /// <summary>
        /// 更新服药状态
        /// </summary>
        /// <param name="prop">要更新的字段（早上还是中午还是..）</param>
        /// <param name="status">状态</param>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public static bool UpdateTakeTimeStatus(string prop, TakeStatus status, int id)
        {
            using (IPatientDal dal = Factory.CreateMedicineDalWrite())
            {
                return dal.UpdateTakeTimeStatus(prop, status, id);
            }
        }
    }
}
