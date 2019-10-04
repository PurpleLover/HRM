using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonHelper.ObjectExtention
{
    public class CompareListDataSave
    {
        /// <summary>
        /// Item1 danh sách listNew khác biệt
        /// Item2 danh sách listOld khác biệt
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listNew"></param>
        /// <param name="lstOld"></param>
        /// <returns></returns>
        public static Tuple<List<T>, List<T>> GetDifferent<T>(List<T> listNew, List<T> lstOld)
        {
            if (listNew == null && lstOld == null)
            {
                return new Tuple<List<T>, List<T>>(null, null);
            }
            if (listNew == null && lstOld != null)
            {
                return new Tuple<List<T>, List<T>>(null, lstOld);
            }
            if (listNew != null && lstOld == null)
            {
                return new Tuple<List<T>, List<T>>(null, lstOld);
            }
            if (listNew != null && lstOld != null)
            {
                var difnew = listNew.Where(x => !lstOld.Contains(x)).ToList();
                var difOld = lstOld.Where(x => !listNew.Contains(x)).ToList();
                return new Tuple<List<T>, List<T>>(difnew, difOld);
            }
            return new Tuple<List<T>, List<T>>(null, null);
        }
    }
}
