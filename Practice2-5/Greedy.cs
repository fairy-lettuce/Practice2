using Practice2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice2
{
	public class Greedy : Bandit
	{
		public Greedy(double q0)
		{
			base.q0 = q0;
		}

		public override int SelectAction(double[] q, int t)
		{
			return ArgMax(q);
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
