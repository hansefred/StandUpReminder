using System.Diagnostics;
using System.Timers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StandUP
{
    public class PausableTimer : System.Timers.Timer , INotifyPropertyChanged
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double RemainingAfterPause
        {
            get => _remainingAfterPause;
            private set
            {
                if (value.Equals(_remainingAfterPause)) return;
                _remainingAfterPause = value;
                OnPropertyChanged();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsResumed
        {
            get => _isResumed;
            set
            {
                if (value == _isResumed) return;
                _isResumed = value;
                OnPropertyChanged();
                OnPropertyChanged();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsPaused
        {
            get => _isPaused;
            set
            {
                if (value == _isPaused) return;
                _isPaused = value;
                OnPropertyChanged();
            }
        }

        private readonly Stopwatch _stopwatch;
        private readonly double _initialInterval;
        private bool _isResumed;
        private double _remainingAfterPause;
        private bool _isPaused;


        public PausableTimer(double interval) : base(interval)
        {
            _initialInterval = interval;
            Elapsed += OnElapsed;
            _stopwatch = new Stopwatch();
        }

        public new void Start()
        {
            IsPaused = false;
            ResetStopwatch();
            base.Start();
        }

        private void OnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (IsResumed)
            {
                IsResumed = false;
                Stop();
                Interval = _initialInterval;
                Start();
            }

            ResetStopwatch();
        }

        private void ResetStopwatch()
        {
            IsPaused = false;
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public void Pause()
        {
            IsPaused = true;
            Stop();
            _stopwatch.Stop();
            RemainingAfterPause = Interval - _stopwatch.Elapsed.TotalMilliseconds;
        }

        public void Resume()
        {
            IsPaused = false;
            IsResumed = true;
            Interval = RemainingAfterPause;
            RemainingAfterPause = 0;
            Start();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
