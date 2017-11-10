using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineWithData<T>
{
    private IEnumerator _target;
    public T result;
    public Coroutine Coroutine { get; private set; }

    public CoroutineWithData(MonoBehaviour owner_, IEnumerator target_)
    {
        _target = target_;
        Coroutine = owner_.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (_target.MoveNext())
        {
            try
            {
                result = (T)Convert.ChangeType(_target.Current, result.GetType());
            }
            catch
            {

            }
            
            //result = (T)_target.Current;
            yield return result;
        }
    }
}
