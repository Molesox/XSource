using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XSource.Services
{
    public class GridService : ServiceBase, IGridService
    {
        public void ExpandFirstLevel()
        {
            var grid = (GridControl)AssociatedObject;
            int groupRowHandle = -1;
            while (grid.IsValidRowHandle(groupRowHandle))
            {
                if (grid.GetRowLevelByRowHandle(groupRowHandle) <= 0)
                {
                    grid.ExpandGroupRow(groupRowHandle);
                }
                groupRowHandle--;
            }
        }

      
    }
}
