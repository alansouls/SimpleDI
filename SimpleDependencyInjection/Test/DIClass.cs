using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleDependencyInjection.Test
{
    public class DIClass
    {
        private readonly Service1 _service1;
        private readonly Service2 _service2;
        private readonly Service3 _service3;

        public DIClass(Service1 service1, Service2 service2, Service3 service3)
        {
            _service1 = service1;
            _service2 = service2;
            _service3 = service3;
        }

        public string GetInfoServices()
        {
            return $"{_service1.ServiceInfo}\n {_service2.ServiceInfo} \n {_service3.ServiceInfo}";
        }

        public void ChangeServices()
        {
            _service1.ServiceInfo = "Info changed!";
            _service2.ServiceInfo = "Info changed!";
            _service3.ServiceInfo = "Info changed!";
        }
    }
    public class DIClass2
    {
        private readonly DIClass _diClass;

        public DIClass2(DIClass diClass)
        {
            _diClass = diClass;
        }

        public string GetInfoServices()
        {
            return _diClass.GetInfoServices();
        }

        public void ChangeServices()
        {
            _diClass.ChangeServices();
        }
    }
    public class DIClass3
    {
        private readonly DIClass2 _diClass;

        public DIClass3(DIClass2 diClass)
        {
            _diClass = diClass;
        }

        public string GetInfoServices()
        {
            return _diClass.GetInfoServices();
        }

        public void ChangeServices()
        {
            _diClass.ChangeServices();
        }
    }
    public class DIClass4
    {
        private readonly DIClass3 _diClass;

        public DIClass4(DIClass3 diClass)
        {
            _diClass = diClass;
        }

        public string GetInfoServices()
        {
            return _diClass.GetInfoServices();
        }

        public void ChangeServices()
        {
            _diClass.ChangeServices();
        }
    }
}
