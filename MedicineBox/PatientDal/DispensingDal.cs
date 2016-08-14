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
    public class DispensingDal : BaseRepository, IDispensingDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Qunau.NetFrameWork.DbCommon.BaseRepository"/> class.
        ///             构造函数
        /// </summary>
        /// <param name="unit">工作单元</param><param name="name">链接名称</param>
        public DispensingDal(IUnitOfWork unit, string name) : base(unit, name)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>
        /// 返回结果
        /// </returns>
        public long Add(Dispensing entity)
        {
            this.ClearParameters();
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO Dispensing (");
            sql.Append(" PatientId, ");
            sql.Append(" MedicineId, ");
            sql.Append(" MedicineNumber, ");
            sql.Append(" TakeTime, ");
            sql.Append(" IsTake, ");
            sql.Append(" Deleted ");
            sql.Append(") VALUES(");
            sql.Append(" @PatientId, ");
            sql.Append(" @MedicineId, ");
            sql.Append(" @MedicineNumber, ");
            sql.Append(" @TakeTime, ");
            sql.Append(" @IsTake, ");
            sql.Append(" @Deleted ");
            sql.Append("); ");
            sql.Append("SELECT @Id:= LAST_INSERT_ID(); ");

            this.AddParameter("@PatientId", entity.PatientId);
            this.AddParameter("@MedicineId", entity.MedicineId);
            this.AddParameter("@MedicineNumber", entity.MedicineNumber);
            this.AddParameter("@TakeTime", entity.TakeTime);
            this.AddParameter("@IsTake", entity.IsTake);
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
        /// 更新是否配药完成
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="isTake">是否配药完成</param>
        /// <returns>成功=true</returns>
        public bool UpdateIsTake(int id, int isTake)
        {
            this.ClearParameters();
            string sql = "UPDATE Dispensing SET IsTake = @IsTake WHERE Id = @Id";
            this.AddParameter("@Id", id);
            this.AddParameter("@IsTake", isTake);
            int res = this.ExecuteNonQuery(sql);
            if (res != 1)
            {
                LogService.WriteLog($"更新配药信息异常，主键:{id},isTake:{isTake}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 重置配药完成情况（每日凌晨用）
        /// </summary>
        /// <returns></returns>
        public bool ResetIsTake()
        {
            this.ClearParameters();
            string sql = "UPDATE Dispensing SET IsTake = @IsTake ";
            int res = this.ExecuteNonQuery(sql);
            if (res != 1)
            {
                LogService.WriteLog($"重置配药完成状态异常");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 删除配药信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>成功=true</returns>
        public bool DeleteDispensing(int id)
        {
            this.ClearParameters();
            string sql = "UPDATE Dispensing SET Deleted = 1 WHERE Id = @Id";
            this.AddParameter("@Id", id);
            int res = this.ExecuteNonQuery(sql);
            if (res != 1)
            {
                LogService.WriteLog($"删除配药信息异常，主键:{id}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 查询配药信息
        /// </summary>
        /// <param name="patientId">病人ID</param>
        /// <returns></returns>
        public List<Dispensing> QueryDispensingsByPatientId(int patientId)
        {
            this.ClearParameters();
            string sql = "SELECT * FROM Dispensing WHERE PatientId = @PatientId AND Deleted = 0";
            this.AddParameter("@PatientId", patientId);
            return this.ExecuteReader(sql).ToList<Dispensing>();
        }

        /// <summary>
        /// 查询第一个配药信息
        /// </summary>
        /// <param name="patientId">病人ID</param>
        /// <returns></returns>
        public Dispensing QueryFirstDispensingByPatientId(int patientId)
        {
            this.ClearParameters();
            string sql = "SELECT * FROM Dispensing WHERE PatientId = @PatientId AND Deleted = 0 AND IsTake = 0 LIMIT 1";
            this.AddParameter("@PatientId", patientId);
            return this.ExecuteReader(sql).ToModel<Dispensing>();
        }
    }
}
