using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Orion.Api;
using System.Threading;
using System.Threading.Tasks;

namespace Orion.Api.Tests
{

	public class NotifierTests
	{
		[Fact]
		public void RegisterListen_Test() 
		{
			var logFactory = new OrionNLogLoggerFactory();
			var notifier = new Notifier(logFactory);

			notifier.RegisterListen(new NotifierHandle());

			notifier.TriggerCycle();
			notifier.TriggerChange(1);
			notifier.TriggerFailure(1);
			notifier.TriggerTimeout(1);
		}



		[Fact]
		public async void WaitListen_Test()
		{
			var logFactory = new OrionNLogLoggerFactory();
			var notifier = new Notifier(logFactory);

			ThreadPool.QueueUserWorkItem(_ =>
			{
				Thread.Sleep(100);
				notifier.TriggerChange(2);

				Thread.Sleep(100);
				notifier.TriggerChange(1);
			});


			int a =	await notifier.Wait<int>(x => x == 1);
			
			Assert.Equal(1, a);
		}

		[Fact]
		public async void WaitListen_Test2()
		{
			var logFactory = new OrionNLogLoggerFactory();
			var notifier = new Notifier(logFactory);

			var tt = notifier.Wait<int>(3, x => x == 1);
			notifier.TriggerChange(1);

			int a = await tt;

			Assert.Equal(1, a);
		}



		[Fact]
		public async void WaitListen_TimeoutTest()
		{
			var logFactory = new OrionNLogLoggerFactory();
			var notifier = new Notifier(logFactory);

			try
			{
				int a = await notifier.Wait<int>(1);
				Assert.True(false);
			}
			catch (TimeoutException)
			{
				Assert.True(true);
			}
		}



		[Fact]
		public void WaitListen_ThreadTest()
		{
			var logFactory = new OrionNLogLoggerFactory();
			var notifier = new Notifier(logFactory);


			var taskA = Task.Run(async () =>
			{
				for (int i = 0; i < 100; i++)
				{
					var tt = notifier.Wait<int>(2);
					var tt2 = notifier.Wait<int>(2);
					await Task.Delay(99);
					notifier.TriggerChange(1);

					int a = await tt;
				}
			});

			var taskB = Task.Run(async () =>
			{
				for (int i = 0; i < 100; i++)
				{
					var tt = notifier.Wait<double>(2);
					var tt2 = notifier.Wait<double>(2);
					await Task.Delay(99);
					notifier.TriggerChange(1.1);

					double a = await tt;
				}
			});
			var taskC = Task.Run(async () =>
			{
				for (int i = 0; i < 50; i++)
				{
					await Task.Delay(200);
					notifier.TriggerChange(1m);
					string a = await notifier.Wait<string>(2);
				}
			});
			var taskD = Task.Run(async () =>
			{
				for (int i = 0; i < 50; i++)
				{
					decimal a = await notifier.Wait<decimal>(60);
					Thread.Sleep(200);
					notifier.TriggerChange("");
				}
			});

			Task.WaitAll(taskA, taskB, taskC, taskD);
		}




		[Fact]
		public void WaitListen_DelayTest()
		{
			var logFactory = new OrionNLogLoggerFactory();
			var notifier = new Notifier(logFactory);

			Task<int> a = notifier.Wait<int>(1);
			notifier.TriggerChange(1);
			a.Wait();

			Assert.Equal(1, a.Result);
		}



		[Fact]
		public void RegisterListen_AsyncCheckTest()
		{
			var logFactory = new OrionNLogLoggerFactory();
			var notifier = new Notifier(logFactory);

			Assert.Throws<ArgumentException>(() =>
			{
				notifier.RegisterListen(new NotifierHandle2());
			});
			 
		}
	}



	public class NotifierHandle
	{

		[OnCycle]
		public void Listen() { throw new Exception(); }

		[OnChange]
		[OnTimeout]
		[OnFailure]
		public void Listen(int a) { }

		[OnCycle(Async = true)]
		public void Listen2() { throw new Exception(); }

		[OnChange(Async = true)]
		[OnTimeout(Async = true)]
		[OnFailure(Async = true)]
		public void Listen2(int a) { }

 
		[OnChange]
		public Task Listen4(int a)
		{
			return Task.Run(() => { });
		}

		[OnChange]
		public async Task Listen5(int a)
		{
			await Task.Run(() => { });
			Thread.Sleep(8000);
		}

	}


	public class NotifierHandle2
	{ 
		[OnChange]
		public async void Listen(int a)
		{
			await Task.Run(() => { });
		}

	}
}
