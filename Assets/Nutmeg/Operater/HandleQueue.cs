using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nutmeg
{
	public class HandleQueue
	{
		private readonly Queue<IHandle> _queue = new();

		public void Enqueue(IHandle handle) => _queue.Enqueue(handle);

		public async Task RunAll()
		{
			while (_queue.Count > 0)
			{
				var handle = _queue.Dequeue();
				await handle.Run();
			}
		}
	}
}
