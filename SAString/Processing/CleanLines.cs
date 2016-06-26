using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAString;

namespace SAString
{
    public static class CleanLines
    {
        public static List<HesseForm> Clean(List<HesseForm> data, double thresholdRho, double thresholdTheta)
        {
            List<List<HesseForm>> FirstSorted = new List<List<HesseForm>>();
            List<List<HesseForm>> Sorted = new List<List<HesseForm>>();
            List<HesseForm> res = new List<HesseForm>();
            data.Sort();
            FirstSorted.Add(new List<HesseForm>());
            foreach (HesseForm hf in data)
            {
                if (FirstSorted[FirstSorted.Count - 1].Count != 0 && hf.Theta - FirstSorted[FirstSorted.Count - 1][0].Theta > thresholdTheta) FirstSorted.Add(new List<HesseForm>());
                FirstSorted[FirstSorted.Count - 1].Add(hf);
            }
            Sorted.Add(new List<HesseForm>());
            for (int i = 0; i < FirstSorted.Count; i++)
            {
                FirstSorted[i].Sort(new HesseRhoCompare());
                Sorted.Add(new List<HesseForm>());
                foreach (HesseForm hf in FirstSorted[i])
                {
                    if (Sorted[Sorted.Count - 1].Count != 0 && hf.Rho - Sorted[Sorted.Count - 1][0].Rho > thresholdRho) Sorted.Add(new List<HesseForm>());
                    Sorted[Sorted.Count - 1].Add(hf);
                }
            }
            for (int i = 0; i < Sorted.Count; i++)
                if (Sorted[i].Count < 1) Sorted.RemoveAt(i);
            for (int i = 0; i < Sorted.Count; i++)
            {
                double a = 0, b = 0;
                foreach (HesseForm hf in Sorted[i])
                {
                    a += hf.Theta; b += hf.Rho;
                }
                res.Add(new HesseForm(a / Sorted[i].Count, b / Sorted[i].Count));
            }
            if (Settings.SaveAsCSV)
            {
                StringBuilder sb = new StringBuilder(String.Format("Rho,Theta\r\n"));
                foreach (HesseForm hf in res)
                    sb.Append(String.Format("{0},{1}\r\n", hf.Rho, hf.Theta));
                System.IO.File.WriteAllText("sorted.csv", sb.ToString());
            }
            return res;
        }
    }
}
