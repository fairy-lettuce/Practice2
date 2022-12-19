using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice2
{
	public class EpsilonGreedy : Bandit
	{
		public EpsilonGreedy(double eps)
		{
			base.eps = eps;
		}

		public override int SelectAction(double[] q, int t)
		{
			var r = rand.NextDouble();
			if (r < eps)
			{
				return rand.Next(q.Length);
			}
			else
			{
				return ArgMax(q);
			}
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
