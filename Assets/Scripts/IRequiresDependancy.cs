using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRequiresDependancy {

    void SetUpDependancies(ServiceCollection serviceCollection);

}
