using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice2
{
	public class GradientBandit : Bandit
	{
		public GradientBandit(double alpha)
		{
			base.alpha = alpha;
		}

		public override int SelectAction(double[] q, int t)
		{
			var prob = SoftMaxProbability(q);
			var accum = new double[q.Length + 1];
			for (int i = 0; i < q.Length; i++)
			{
				accum[i + 1] = accum[i] + prob[i];
			}
			var r = rand.NextDouble();
			for (int i = 0; i < q.Length; i++)
			{
				if (r < accum[i + 1]) return i;
			}
			return q.Length - 1;
		}

		public override double[] UpdateInternal(double[] q, int a, double r, double b)
		{
			var prob = SoftMaxProbability(q);
			for (int i = 0; i < q.Length; i++)
			{
				if (i == a)
				{
					q[i] = q[i] + alpha * (r - b) * (1 - prob[i]);
				}
				else
				{
					q[i] = q[i] - alpha * (r - b) * prob[i];
				}
			}
			return q;
		}

		public double[] SoftMaxProbability(double[] q)
		{
			var exp = q.Select(p => Math.Exp(p)).ToArray();
			var sum = exp.Sum();
			exp = exp.Select(p => p / sum).ToArray();
			return exp;
		}

		public override (double[] qTrue, double sdTrue) UpdateExternal(double[] qTrue, double sdTrue)
		{
			return base.UpdateExternal(qTrue, sdTrue);
		}
	}
}
