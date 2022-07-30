using BlogProject.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Models.Entities.Abstract
{
    public abstract class BaseEntity
    {
        
        // ID required
        public int ID { get; set; }

        
        private DateTime _createDate = DateTime.Now;

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        // ModefiedDate Nullable
        public DateTime? ModifiedDate { get; set; }

        // RemovedTime Nullable
        public DateTime? RemovedDate { get; set; }

       
        private Statu _statu = Statu.Passive;

        public Statu Statu
        {
            get { return _statu; }
            set { _statu = value; }
        }
               
        private AdminCheck _adminChecked = AdminCheck.Waiting;
        public AdminCheck AdminCheck
        {
            get { return _adminChecked; }
            set { _adminChecked = value; }
        }

    }
}
