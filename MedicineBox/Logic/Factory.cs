using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Idal;
using Qunau.NetFrameWork.DbCommon;
using Qunau.NetFrameWork.Infrastructure;

namespace Logic
{
    public class Factory
    {
        /// <summary>
        /// 数据库用户信息读连接
        /// </summary>
        private static string strMedicineBoxRead = "MedicineBoxRead";

        /// <summary>
        /// 数据库用户信息写连接
        /// </summary>
        private static string strMedicineBoxWrite = "MedicineBoxWrite";

        #region 工作单元
        /// <summary>
        /// 创建工作单元
        /// </summary>
        /// <param name="isolationLevel">事物级别</param>
        /// <returns>工作单元</returns>
        public static IUnitOfWork CreateIUnitOfWork(IsolationLevel isolationLevel)
        {
            return new UnitOfWork(isolationLevel, strMedicineBoxRead);
        }

        /// <summary>
        /// 创建工作单元
        /// </summary>
        /// <returns>工作单元</returns>
        public static IUnitOfWork CreateIUnitOfWork()
        {
            return new UnitOfWork(strMedicineBoxWrite);
        }
        #endregion



        #region 病人
        public static IPatientDal CreatePatientDalRead()
        {
            return new PatientDal(null, strMedicineBoxRead);
        }

        public static IPatientDal CreatePatientDalWrite()
        {
            return new PatientDal(null, strMedicineBoxWrite);
        }

        public static IPatientDal CreatePatientDalWrite(IUnitOfWork iUnitOfWork)
        {
            return new PatientDal(iUnitOfWork, null);
        }

        #endregion

        #region 药品
        public static IMedicineDal CreateMedicineDalRead()
        {
            return new MedicineDal(null, strMedicineBoxRead);
        }

        public static IPatientDal CreateMedicineDalWrite()
        {
            return new PatientDal(null, strMedicineBoxWrite);
        }

        public static IPatientDal CreateMedicineDalWrite(IUnitOfWork iUnitOfWork)
        {
            return new PatientDal(iUnitOfWork, null);
        }
        #endregion

        #region 配药
        public static IDispensingDal CreateDispensingDalRead()
        {
            return new DispensingDal(null, strMedicineBoxRead);
        }

        public static IDispensingDal CreateDispensingDalWrite()
        {
            return new DispensingDal(null, strMedicineBoxWrite);
        }

        public static IDispensingDal CreateDispensingDalWrite(IUnitOfWork iUnitOfWork)
        {
            return new DispensingDal(iUnitOfWork, null);
        }
        #endregion

        #region 设备
        public static IDeviceDal CreateDeviceDalDalRead()
        {
            return new DeviceDal(null, strMedicineBoxRead);
        }

        public static IDeviceDal CreateDeviceDalDalWrite()
        {
            return new DeviceDal(null, strMedicineBoxWrite);
        }

        public static IDeviceDal CreateDeviceDalDalWrite(IUnitOfWork iUnitOfWork)
        {
            return new DeviceDal(iUnitOfWork, null);
        }
        #endregion
    }
}
