using System.Collections.Generic;
using System.Collections;
using System;

public class Pointer < T >
{
    private Func<T> getter;
    private Action<T> setter;
	
    public Pointer ( Func<T> getter, Action<T> setter )
    {
        this.getter = getter;
        this.setter = setter;
    }
	
    public T Value
    {
        get { return getter(); }
        set { setter(value); }
    }
}
