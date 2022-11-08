using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading; 

namespace ET
{
    [ObjectSystem]
    public class TimeControlComponentAwakeSystem : AwakeSystem<TimeControlComponent>
    {
        public override void Awake(TimeControlComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class TimeControlComponentUpdateSystem : UpdateSystem<TimeControlComponent>
    { 
        public override void Update(TimeControlComponent self)
        {
            self.Update();
        }
    }
     
    public class TimeControlComponent :Entity
    { 
        public static TimeControlComponent Instance { get; set; }

        /// <summary>
        /// 战斗速度
        /// </summary>
        public float combatSpeed = 1;

        long _minActivityTime { get; set;}

        Queue<Timer> _timerPool = new Queue<Timer>();
        Queue<List<Timer>> _listPool = new Queue<List<Timer>>();
        Dictionary<long, List<Timer>> _timesDic = new Dictionary<long, List<Timer>>();
        //key: timer.id value:暂停时的时间
        Dictionary<long, long> _pauseDic = new Dictionary<long, long>();

          
        public void Awake()
        {
            Instance = this;
            _minActivityTime = long.MaxValue;
        } 

        public void Update()
        {
            if (_minActivityTime == 0 || _minActivityTime > TimeHelper.ServerNow()) return; 

            List<Timer> list = _timesDic[_minActivityTime];
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].tcs.IsCompleted)
                    continue;
                list[i].tcs.SetResult();
            }

            _timesDic.Remove(_minActivityTime);

            this.GetMinTime();
            this.RecycleList(list);
        }
        
        /// <summary>
        /// 暂停
        /// </summary> 
        public void Pause(Timer t)
        {
            if (t.Id == default) return;

            long nowTime = TimeHelper.ServerNow();
            this.timesDicRemoveTimer(t, false);

            if(!_pauseDic.ContainsKey(t.Id))
                _pauseDic[t.Id] = nowTime;
        }
        /// <summary>
        /// 继续(取消暂停)
        /// </summary> 
        public void KeepOn(Timer t)
        {
            if(_pauseDic.ContainsKey(t.Id))
            {
                t.Time += TimeHelper.ServerNow() - _pauseDic[t.Id];
                this.Wait(t, t.token);
                _pauseDic.Remove(t.Id);
            }
        }  
        /// <summary>
        /// 获取减免后的时间
        /// </summary> 
        public long GetDerateTime(long time,bool isCombat=true)
        {
            if (isCombat)
                time = (long)(time * (2f - combatSpeed));
            else
                ;
            return time;
        }
        /// <summary>
        /// 战斗中等待时间
        /// </summary> 
        public ETTask WaitCombatTime(long time)
        {
            time = this.GetDerateTime(time);
            return WaitTime(time); 
        }

        public Timer GetWaitTimer(long time, bool IsCombat = true)
        {
            time = IsCombat ? this.GetDerateTime(time) : time;

            Timer t = this.CreatTimerToken(time);

            this.Wait(t, CancellationToken.None);

            return t;
        }
        public Timer GetWaitTimer(long time, CancellationToken token, bool IsCombat = true)
        {
            time = IsCombat ? this.GetDerateTime(time): time;

            Timer t = this.CreatTimerToken(time);

            this.Wait(t, t.token);

            return t;
        }


        public ETTask WaitTime(long time)
        { 
            return WaitTime(this.CreatTimerToken(time));
        }
        public ETTask WaitTime(long time, CancellationToken token)
        {
            return WaitTime(this.CreatTimerToken(time), token);
        }
        public ETTask WaitTime(Timer timer)
        {
            return WaitTime(timer, CancellationToken.None);
        }
        public ETTask WaitTime(Timer timer ,CancellationToken token)
        {
            return Wait(timer, token);
        }

        public async ETTask WaitTimeDoSomething(long time, Action action)
        {
            await WaitTime(time);
            action?.Invoke();
        }

        public Timer CreatTimerToken(long time ,bool IsLater = true)
        {
            ETTask tcs = ETTask.Create();

            time = IsLater ? time + TimeHelper.ServerNow() : time;

            Timer t = this.GetTimer();
            t.Id = IdGenerater.Instance.GenerateId();
            t.Time = time;
            t.tcs = tcs; 

            return t;
        }

         
        ETTask Wait(Timer timer, CancellationToken token)
        {
            if (!_timesDic.ContainsKey(timer.Time))
                _timesDic[timer.Time] = this.GetList();

            if (timer.Time < _minActivityTime)
                _minActivityTime = timer.Time;

            _timesDic[timer.Time].Add(timer);

            timer.token = token;
            token.Register(() => {
                this.timesDicRemoveTimer(timer);
                if (_pauseDic.ContainsKey(timer.Id))
                    _pauseDic.Remove(timer.Id);
            });

            return timer.tcs;
        }
        /// <summary>
        /// 删除Timer
        /// </summary> 
        public void timesDicRemoveTimer(Timer timer,bool isRecycle = true)
        {
            if (_timesDic.ContainsKey(timer.Time) && _timesDic[timer.Time].Contains(timer))
                _timesDic[timer.Time].Remove(timer);
            else
                return;

            if (_timesDic[timer.Time].Count == 0)
            {
                _listPool.Enqueue(_timesDic[timer.Time]);
                _timesDic.Remove(timer.Time);
                this.GetMinTime();
            }
           
            if(isRecycle) this.RecycleTimer(timer);
        }
        List<Timer> GetList()
        {
            return _listPool.Count != 0 ? _listPool.Dequeue(): new List<Timer>();
        }
        Timer GetTimer()
        {
            return _timerPool.Count != 0 ? _timerPool.Dequeue() : new Timer(); 
        } 
        void RecycleList(List<Timer> list)
        {
            list.Clear();
            _listPool.Enqueue(list);
        }
        void RecycleTimer(Timer t)
        {
            _timerPool.Enqueue(t);
        }
        void GetMinTime()
        {
            List<long> list = _timesDic.Keys.ToList();
            if (list == null || list.Count == 0) {
                _minActivityTime = long.MaxValue;
                return;
            }
            list.Sort();
            _minActivityTime = list[0];
        } 
    }
}
