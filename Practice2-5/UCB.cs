using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice2
{
	public class UCB : Bandit
	{
		public UCB(double c)
		{
			base.c = c;
		}

		public override int SelectAction(double[] q, int t)
		{
			return ArgMax(q.Select((s, a) => n[a] == 0 ? double.MaxValue : s + c * Math.Sqrt(Math.Log(t) / (n[a] + 0.0))).ToArray());
		}

		public override double[] UpdateInternal(double[] q, int a, double r, double b)
		{
			q[a] = q[a] + (r - q[a]) / (n[a] + 0.0);
			return q;
		}

		public override (double[] qTrue, double sdTrue) UpdateExternal(double[] qTrue, double sdTrue)
		{
			return base.UpdateExternal(qTrue, sdTrue);
		}
	}
}
