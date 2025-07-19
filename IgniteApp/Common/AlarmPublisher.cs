using IgniteDevices.PLC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IgniteApp.Common
{
    public class AlarmPublisher : IObservable<AlarmMessage>
    {
        private readonly List<IObserver<AlarmMessage>> _observers = new List<IObserver<AlarmMessage>>();

        public IDisposable Subscribe(IObserver<AlarmMessage> observer)
        {
            _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<AlarmMessage>> _observers;
            private readonly IObserver<AlarmMessage> _observer;

            public Unsubscriber(List<IObserver<AlarmMessage>> observers, IObserver<AlarmMessage> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        // 触发报警通知
        public void NotifyAlarm(string alarmName)
        {
            var message = new AlarmMessage { Name = alarmName };
            foreach (var observer in _observers.ToArray())
            {
                observer.OnNext(message);
            }
        }

        // 结束传输（可选）
        public void EndTransmission()
        {
            foreach (var observer in _observers.ToArray())
            {
                observer.OnCompleted();
            }
            _observers.Clear();
        }
    }
}