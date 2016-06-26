using System.Collections.Generic;

namespace SAString
{
    public class HesseRhoCompare : IComparer<HesseForm>
    {
        int IComparer<HesseForm>.Compare(HesseForm x, HesseForm y)
        {
            return x.Rho.CompareTo(y.Rho);
        }
    }
}
