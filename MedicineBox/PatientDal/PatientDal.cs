using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idal;
using Model;
using Model.Enum;
using Qunau.NetFrameWork.Common.Exception;
using Qunau.NetFrameWork.Common.Extension;
using Qunau.NetFrameWork.Common.Write;
using Qunau.NetFrameWork.DbCommon;
using Qunau.NetFrameWork.Infrastructure;

namespace Dal
{
    public class PatientDal : BaseRepository, IPatientDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Qunau.NetFrameWork.DbCommon.BaseRepository"/> class.
        ///             构造函数
        /// </summary>
        /// <param name="unit">工作单元</param><param name="name">链接名称</param>
        public PatientDal(IUnitOfWork unit, string name) : base(unit, name)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>
        /// 返回结果
        /// </returns>
        public long Add(Patient entity)
        {
            this.ClearParameters();
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO Patient (");
            sql.Append(" Name, ");
            sql.Append(" Sex, ");
            sql.Append(" Age, ");
            sql.Append(" Nation, ");
            sql.Append(" Height, ");
            sql.Append(" Weight, ");
            sql.Append(" Illness, ");
            sql.Append(" MorningHour, ");
            sql.Append(" MorningMinute, ");
            sql.Append(" NoonHour, ");
            sql.Append(" NoonMinute, ");
            sql.Append(" EveningHour, ");
            sql.Append(" EveningMinute, ");
            sql.Append(" AdditionalHour, ");
            sql.Append(" AdditionalMinute, ");
            sql.Append(" MorningStatus, ");
            sql.Append(" NoonStatus, ");
            sql.Append(" EveningStatus, ");
            sql.Append(" AdditionalStatus, ");
            sql.Append(" DeviceId, ");
            sql.Append(" WardNo, ");
            sql.Append(" BedNo, ");
            sql.Append(" Deleted ");
            sql.Append(") VALUES(");
            sql.Append(" @Name, ");
            sql.Append(" @Sex, ");
            sql.Append(" @Age, ");
            sql.Append(" @Nation, ");
            sql.Append(" @Height, ");
            sql.Append(" @Weight, ");
            sql.Append(" @Illness, ");
            sql.Append(" @MorningHour, ");
            sql.Append(" @MorningMinute, ");
            sql.Append(" @NoonHour, ");
            sql.Append(" @NoonMinute, ");
            sql.Append(" @EveningHour, ");
            sql.Append(" @EveningMinute, ");
            sql.Append(" @AdditionalHour, ");
            sql.Append(" @AdditionalMinute, ");
            sql.Append(" @MorningStatus, ");
            sql.Append(" @NoonStatus, ");
            sql.Append(" @EveningStatus, ");
            sql.Append(" @AdditionalStatus, ");
            sql.Append(" @DeviceId, ");
            sql.Append(" @WardNo, ");
            sql.Append(" @BedNo, ");
            sql.Append(" @Deleted ");
            sql.Append("); ");
            sql.Append("SELECT @Id:= LAST_INSERT_ID(); ");

            this.AddParameter("@Name", entity.Name);
            this.AddParameter("@Sex", entity.Sex);
            this.AddParameter("@Age", entity.Age);
            this.AddParameter("@Nation", entity.Nation);
            this.AddParameter("@Height", entity.Height);
            this.AddParameter("@Weight", entity.Weight);
            this.AddParameter("@Illness", entity.Illness);
            this.AddParameter("@MorningHour", entity.MorningHour);
            this.AddParameter("@MorningMinute", entity.MorningMinute);
            this.AddParameter("@NoonHour", entity.NoonHour);
            this.AddParameter("@NoonMinute", entity.NoonMinute);
            this.AddParameter("@EveningHour", entity.EveningHour);
            this.AddParameter("@EveningMinute", entity.EveningMinute);
            this.AddParameter("@AdditionalHour", entity.AdditionalHour);
            this.AddParameter("@AdditionalMinute", entity.AdditionalMinute);
            this.AddParameter("@MorningStatus", entity.MorningStatus);
            this.AddParameter("@NoonStatus", entity.NoonStatus);
            this.AddParameter("@EveningStatus", entity.EveningStatus);
            this.AddParameter("@AdditionalStatus", entity.AdditionalStatus);
            this.AddParameter("@DeviceId", entity.DeviceId);
            this.AddParameter("@WardNo", entity.WardNo);
            this.AddParameter("@BedNo", entity.BedNo);
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
        /// 更新服药时间信息
        /// </summary>
        /// <param name="info">服药时间信息</param>
        /// <returns>成功= true</returns>
        public bool UpdateTakeTimeVal(TakeTimeInfo info)
        {
            this.ClearParameters();
            StringBuilder sql = new StringBuilder();
            sql.Append("UPDATE Patient SET ");
            sql.Append("MorningHour = @MorningHour,");
            sql.Append("MorningMinute = @MorningMinute,");
            sql.Append("NoonHour = @NoonHour,");
            sql.Append("NoonMinute = @NoonMinute,");
            sql.Append("EveningHour = @EveningHour,");
            sql.Append("EveningMinute = @EveningMinute,");
            sql.Append("AdditionalHour = @AdditionalHour,");
            sql.Append("AdditionalMinute = @AdditionalMinute ");
            sql.Append("WHERE Id = @Id LIMIT 1");

            this.AddParameter("@MorningHour", info.MorningHour);
            this.AddParameter("@MorningMinute", info.MorningMinute);
            this.AddParameter("@NoonHour", info.NoonHour);
            this.AddParameter("@NoonMinute", info.NoonMinute);
            this.AddParameter("@EveningHour", info.EveningHour);
            this.AddParameter("@EveningMinute", info.EveningMinute);
            this.AddParameter("@AdditionalHour", info.AdditionalHour);
            this.AddParameter("@AdditionalMinute", info.AdditionalMinute);
            this.AddParameter("@Id", info.Id);

            int res = this.ExecuteNonQuery(sql.ToString());
            return res == 1;
        }

        /// <summary>
        /// 查询病人信息
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>病人信息</returns>
        public Patient QueryPatientById(int id)
        {
            this.ClearParameters();
            string sql = "SELECT * FROM Patient WHERE Id = @Id AND Deleted = 0";
            this.AddParameter("@Id", id);
            return this.ExecuteReader(sql).ToModel<Patient>();
        }

        /// <summary>
        /// 查询所有病人信息
        /// </summary>
        /// <returns>病人信息集合</returns>
        public List<Patient> QueryAllPatients()
        {
            this.ClearParameters();
            string sql = "SELECT * FROM Patient WHERE Deleted = 0";
            return this.ExecuteReader(sql).ToList<Patient>();
        }

        /// <summary>
        /// 更新服药状态
        /// </summary>
        /// <param name="prop">要更新的字段（早上还是中午还是..）</param>
        /// <param name="status">状态</param>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public bool UpdateTakeTimeStatus(string prop, TakeStatus status, int id)
        {
            this.ClearParameters();
            string sql = $"UPDATE Patient SET {prop}=@{prop} WHERE Id = @Id";
            this.AddParameter($"@{prop}", status);
            this.AddParameter("@Id", id);
            int res = this.ExecuteNonQuery(sql);
            if (res != 1)
            {
                LogService.WriteLog($"更新服药状态失败，更新字段：{prop},状态：{status},主键：{id}");
                return false;
            }
            return true;
        }
    }
}
