using System;

namespace SAString
{
    public class HesseForm : IComparable
    {
        public HesseForm(double theta, double rho)
        {
            Theta = theta; Rho = rho;
        }
        public double EuclidianDist(HesseForm OtherPoint)
        {
            return Math.Sqrt(this.Rho * this.Rho + OtherPoint.Rho * OtherPoint.Rho - 2 * this.Rho * OtherPoint.Rho * Math.Cos(this.Theta - OtherPoint.Theta));
        }
        public double Theta { get; set; }
        public double Rho { get; set; }
        public double Slope { get { return -1 / Math.Tan(Theta); } }
        int IComparable.CompareTo(object obj)
        {
            if (obj is HesseForm)
                return -1 * (((obj as HesseForm).Theta == Theta) ? (obj as HesseForm).Rho.CompareTo(Rho) : (obj as HesseForm).Theta.CompareTo(Theta));
            else throw new NotImplementedException();
        }
    }
}
