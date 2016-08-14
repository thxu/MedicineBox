using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Idal;
using Model;
using Qunau.NetFrameWork.Common.Exception;
using Qunau.NetFrameWork.Common.Extension;
using Qunau.NetFrameWork.DbCommon;
using Qunau.NetFrameWork.Infrastructure;

namespace Dal
{
    public class MedicineDal : BaseRepository, IMedicineDal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Qunau.NetFrameWork.DbCommon.BaseRepository"/> class.
        ///             构造函数
        /// </summary>
        /// <param name="unit">工作单元</param><param name="name">链接名称</param>
        public MedicineDal(IUnitOfWork unit, string name) : base(unit, name)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>
        /// 返回结果
        /// </returns>
        public long Add(Medicine entity)
        {
            this.ClearParameters();
            StringBuilder sql = new StringBuilder();
            sql.Append("INSERT INTO Medicine (");
            sql.Append(" MedicineNo, ");
            sql.Append(" MedicineName, ");
            sql.Append(" Deleted ");
            sql.Append(") VALUES(");
            sql.Append(" @MedicineNo, ");
            sql.Append(" @MedicineName, ");
            sql.Append(" @Deleted ");
            sql.Append("); ");
            sql.Append("SELECT @Id:= LAST_INSERT_ID(); ");

            this.AddParameter("@MedicineNo", entity.MedicineNo);
            this.AddParameter("@MedicineName", entity.MedicineName);
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
        /// 查询药品信息
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>药品信息</returns>
        public Medicine QueryMedicineById(int id)
        {
            this.ClearParameters();
            string sql = "SELECT * FROM Medicine WHERE Id = @Id AND Deleted = 0";
            this.AddParameter("@Id", id);
            return this.ExecuteReader(sql).ToModel<Medicine>();
        }
    }
}
